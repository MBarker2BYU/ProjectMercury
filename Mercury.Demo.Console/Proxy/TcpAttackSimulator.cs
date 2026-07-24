// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-21-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="TcpAttackSimulator.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Mercury.Demo.Console.Proxy;

/// <summary>
/// Intercepts framed TCP traffic for controlled replay and tamper
/// demonstrations.
/// </summary>
/// <param name="listenPort">The proxy listen port.</param>
/// <param name="targetPort">The real server port.</param>
/// <seealso cref="IAsyncDisposable" />
internal sealed class TcpAttackSimulator(int listenPort, int targetPort) : IAsyncDisposable
{
    /// <summary>
    /// The maximum permitted Mercury frame size.
    /// </summary>
    private const uint MAX_FRAME_BYTES = 8 * 1024 * 1024;

    /// <summary>
    /// The maximum permitted metadata entry count.
    /// </summary>
    private const uint MAX_METADATA_ENTRIES = 100_000;

    /// <summary>
    /// Synchronizes access to the saved replay frame.
    /// </summary>
    private readonly object m_AttackSync = new();

    /// <summary>
    /// The TCP listener.
    /// </summary>
    private TcpListener? m_Listener;

    /// <summary>
    /// The cancellation token source.
    /// </summary>
    private CancellationTokenSource? m_CancellationTokenSource;

    /// <summary>
    /// The last complete client-to-server frame.
    /// </summary>
    private byte[]? m_LastFrame;

    /// <summary>
    /// Gets or sets a value indicating whether tampering is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> when tampering is enabled; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool TamperEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether replay is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> when replay is enabled; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool ReplayEnabled { get; set; }

    /// <summary>
    /// Starts the attack simulator.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    public Task StartAsync()
    {
        m_CancellationTokenSource = new CancellationTokenSource();

        m_Listener = new TcpListener(IPAddress.Loopback, listenPort);

        m_Listener.Start();

        _ = AcceptLoopAsync(m_CancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Accepts incoming proxy connections.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    private async Task AcceptLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = await m_Listener!
                        .AcceptTcpClientAsync(cancellationToken)
                        .ConfigureAwait(false);

                _ = HandleClientAsync(client, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception)
            {
                // Controlled demo proxy failure; the active operation reports the result.
            }
        }
    }

    /// <summary>
    /// Connects one proxy client to the real server.
    /// </summary>
    /// <param name="client">
    /// The connected proxy client.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using (client)
        using (var server = new TcpClient())
        {
            try
            {
                await server
                    .ConnectAsync(IPAddress.Loopback, targetPort, cancellationToken)
                    .ConfigureAwait(false);

                var clientStream = client.GetStream();

                var serverStream = server.GetStream();

                var clientToServer = ForwardFramedAsync(clientStream, serverStream,
                    true, cancellationToken);

                var serverToClient = ForwardFramedAsync(serverStream, clientStream,
                    false, cancellationToken);

                await Task
                    .WhenAll(clientToServer, serverToClient)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown.
            }
            catch (IOException)
            {
                // Normal connection shutdown or transport disposal.
            }
            catch (SocketException)
            {
                // Normal connection shutdown or transport disposal.
            }
            catch (Exception)
            {
                // The active operation reports the failure through Mercury.
            }
        }
    }

    /// <summary>
    /// Forwards complete length-prefixed frames.
    /// </summary>
    /// <param name="from">
    /// The source stream.
    /// </param>
    /// <param name="to">
    /// The destination stream.
    /// </param>
    /// <param name="isClientToServer">
    /// Indicates whether the frame is traveling toward the server.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    private async Task ForwardFramedAsync(NetworkStream from, NetworkStream to, bool isClientToServer, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var lengthBytes = new byte[4];

            await ReadExactAsync(from, lengthBytes, cancellationToken)
                .ConfigureAwait(false);

            var frameLength = ReadUInt32BigEndian(lengthBytes, 0);

            if (frameLength == 0)
                throw new InvalidDataException("The intercepted frame is empty.");
            

            if (frameLength > MAX_FRAME_BYTES)
                throw new InvalidDataException($"The intercepted frame is too large: {frameLength} bytes.");
            

            var frame = new byte[(int)frameLength];

            await ReadExactAsync(from, frame, cancellationToken)
                .ConfigureAwait(false);

            var dataToSend = frame;

            if (isClientToServer)
            {
                dataToSend = ApplyAttack(frame);
            }

            var outgoingLength = new byte[4];

            WriteUInt32BigEndian(outgoingLength, (uint)dataToSend.Length);

            await to
                .WriteAsync(outgoingLength, cancellationToken)
                .ConfigureAwait(false);

            await to
                .WriteAsync(dataToSend, cancellationToken)
                .ConfigureAwait(false);

            await to
                .FlushAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies the selected attack to a complete client frame.
    /// </summary>
    /// <param name="frame">
    /// The complete Mercury frame without the TCP length prefix.
    /// </param>
    /// <returns>
    /// The frame to forward.
    /// </returns>
    private byte[] ApplyAttack(byte[] frame)
    {
        byte[]? replayFrame = null;

        var tamperEnabled = TamperEnabled;

        lock (m_AttackSync)
        {
            if (ReplayEnabled && m_LastFrame != null)
            {
                replayFrame = (byte[])m_LastFrame.Clone();
            }
            else
            {
                /*
                 * Save the original valid frame for a future
                 * replay demonstration.
                 */
                m_LastFrame = (byte[])frame.Clone();
            }
        }

        if (replayFrame != null)
        {
                        return replayFrame;
        }

        if (!tamperEnabled)
            return frame;
        

        var tamperedFrame = (byte[])frame.Clone();

        TamperProtectedPayload(tamperedFrame);

                return tamperedFrame;
    }

    /// <summary>
    /// Changes one protected byte without changing the frame length
    /// or envelope structure.
    /// </summary>
    /// <param name="frame">
    /// The complete Mercury frame.
    /// </param>
    private static void TamperProtectedPayload(
        byte[] frame)
    {
        if (IsBinaryEnvelope(frame))
        {
            TamperBinaryProtectedPayload(frame);

            return;
        }

        TamperJsonProtectedPayload(frame);
    }

    /// <summary>
    /// Determines whether the frame uses the Mercury binary codec.
    /// </summary>
    /// <param name="frame">
    /// The complete Mercury frame.
    /// </param>
    /// <returns>
    /// <c>true</c> when the binary magic is present.
    /// </returns>
    private static bool IsBinaryEnvelope(byte[] frame)
    {
        return
            frame is [(byte)'M', (byte)'E', (byte)'R', (byte)'1', ..];
    }

    /// <summary>
    /// Locates and changes one byte inside the binary protected payload.
    /// </summary>
    /// <param name="frame">
    /// The Mercury binary envelope.
    /// </param>
    private static void TamperBinaryProtectedPayload(
        byte[] frame)
    {
        var offset = 0;

        RequireAvailable(frame, offset, 6);

        /*
         * Skip:
         * 4-byte magic
         * 1-byte major version
         * 1-byte minor version
         */
        offset += 6;

        // Envelope ID.
        offset =
            SkipLengthPrefixedValue(frame, offset);

        // Timestamp.
        RequireAvailable(frame, offset, 8);

        offset += 8;

        // Sender key ID.
        offset =
            SkipLengthPrefixedValue(frame, offset);

        // Recipient key ID.
        offset =
            SkipLengthPrefixedValue(frame, offset);

        // Encryption algorithm.
        offset =
            SkipLengthPrefixedValue(frame, offset);

        // Signature algorithm.
        offset =
            SkipLengthPrefixedValue(frame, offset);

        // Replay token.
        offset = SkipLengthPrefixedValue(frame, offset);

        // Header metadata.
        offset = SkipMetadata(frame, offset);

        /*
         * The next field is:
         * [4-byte protected payload length]
         * [nonce | authentication tag | ciphertext]
         */
        var protectedPayloadLength = ReadLength(frame, offset);

        offset += 4;

        if (protectedPayloadLength <= 0)
            throw new FormatException("The protected payload is empty.");

        RequireAvailable(frame, offset, protectedPayloadLength);

        /*
         * The final protected-payload byte is ciphertext.
         * Changing it leaves every envelope length unchanged.
         */
        var tamperIndex = offset + protectedPayloadLength - 1;

        frame[tamperIndex] ^= 0x01;
    }

    /// <summary>
    /// Changes one Base64 character inside the JSON protected payload.
    /// </summary>
    /// <param name="frame">
    /// The Mercury JSON envelope.
    /// </param>
    private static void TamperJsonProtectedPayload(byte[] frame)
    {
        var marker = Encoding.UTF8.GetBytes("\"protectedPayloadBase64\":\"");

        var markerIndex = FindSequence(frame, marker);

        if (markerIndex < 0)
            throw new FormatException("The intercepted frame is not a recognized Mercury envelope.");
        
        var valueStart = markerIndex + marker.Length;

        var valueEnd = Array.IndexOf(frame, (byte)'"', valueStart);

        if (valueEnd <= valueStart)
            throw new FormatException("The JSON protected payload is empty or incomplete.");
        

        var tamperIndex = valueStart + ((valueEnd - valueStart) / 2);

        while (tamperIndex < valueEnd && frame[tamperIndex] == (byte)'=')
        {
            tamperIndex++;
        }

        if (tamperIndex >= valueEnd)
            throw new FormatException("The JSON protected payload cannot be altered.");
        

        /*
         * Both A and B are legal Base64 characters and occupy
         * exactly one byte in the JSON frame.
         */
        frame[tamperIndex] = frame[tamperIndex] == (byte)'A'
                ? (byte)'B'
                : (byte)'A';
    }

    /// <summary>
    /// Skips one length-prefixed field.
    /// </summary>
    /// <param name="frame">
    /// The encoded frame.
    /// </param>
    /// <param name="offset">
    /// The current offset.
    /// </param>
    /// <returns>
    /// The offset immediately after the field.
    /// </returns>
    private static int SkipLengthPrefixedValue(byte[] frame, int offset)
    {
        var length = ReadLength(frame, offset);

        offset += 4;

        RequireAvailable(frame, offset, length);

        return offset + length;
    }

    /// <summary>
    /// Skips one metadata collection.
    /// </summary>
    /// <param name="frame">
    /// The encoded frame.
    /// </param>
    /// <param name="offset">
    /// The current offset.
    /// </param>
    /// <returns>
    /// The offset immediately after the metadata.
    /// </returns>
    private static int SkipMetadata(byte[] frame, int offset)
    {
        var entryCount = ReadUInt32BigEndian(frame, offset);

        offset += 4;

        if (entryCount > MAX_METADATA_ENTRIES)
            throw new FormatException("The metadata entry count is invalid.");

        for (uint index = 0; index < entryCount; index++)
        {
            offset = SkipLengthPrefixedValue(frame, offset);

            offset = SkipLengthPrefixedValue(frame, offset);
        }

        return offset;
    }

    /// <summary>
    /// Reads one length-prefixed field length.
    /// </summary>
    /// <param name="frame">
    /// The encoded frame.
    /// </param>
    /// <param name="offset">
    /// The length offset.
    /// </param>
    /// <returns>
    /// The decoded length.
    /// </returns>
    private static int ReadLength(byte[] frame, int offset)
    {
        var length = ReadUInt32BigEndian(frame, offset);

        if (length > int.MaxValue)
            throw new FormatException("The encoded field is too large.");
        
        return (int)length;
    }

    /// <summary>
    /// Finds one byte sequence inside another.
    /// </summary>
    /// <param name="source">
    /// The source bytes.
    /// </param>
    /// <param name="value">
    /// The value to locate.
    /// </param>
    /// <returns>
    /// The starting index, or -1 when not found.
    /// </returns>
    private static int FindSequence(byte[] source, byte[] value)
    {
        if (value.Length == 0 || source.Length < value.Length)
            return -1;
        

        for (var sourceIndex = 0; sourceIndex <= source.Length - value.Length; sourceIndex++)
        {
            var matched = true;

            for (var valueIndex = 0; valueIndex < value.Length; valueIndex++)
            {
                if (source[sourceIndex + valueIndex] == value[valueIndex])
                {
                    continue;
                }

                matched = false;

                break;
            }

            if (matched)
            {
                return sourceIndex;
            }
        }

        return -1;
    }

    /// <summary>
    /// Verifies that a requested byte range exists.
    /// </summary>
    /// <param name="frame">
    /// The frame.
    /// </param>
    /// <param name="offset">
    /// The range offset.
    /// </param>
    /// <param name="length">
    /// The range length.
    /// </param>
    private static void RequireAvailable(byte[] frame, int offset, int length)
    {
        if (offset < 0 || length < 0 || offset > frame.Length - length)
            throw new FormatException("The Mercury frame is incomplete.");
    }

    /// <summary>
    /// Clears the saved replay frame.
    /// </summary>
    public void ClearLastFrame()
    {
        lock (m_AttackSync)
        {
            m_LastFrame = null;
        }

            }

    /// <summary>
    /// Reads exactly the requested number of bytes.
    /// </summary>
    /// <param name="stream">
    /// The network stream.
    /// </param>
    /// <param name="buffer">
    /// The destination buffer.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    private static async Task ReadExactAsync(NetworkStream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;

        while (offset < buffer.Length)
        {
            var bytesRead = await stream
                    .ReadAsync(buffer.AsMemory(offset, buffer.Length - offset), cancellationToken)
                    .ConfigureAwait(false);

            if (bytesRead == 0)
                throw new IOException("The TCP connection was closed.");
            
            offset += bytesRead;
        }
    }

    /// <summary>
    /// Writes an unsigned 32-bit value in big-endian order.
    /// </summary>
    /// <param name="destination">
    /// The four-byte destination.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    private static void WriteUInt32BigEndian(byte[] destination, uint value)
    {
        destination[0] = (byte)(value >> 24);

        destination[1] = (byte)(value >> 16);

        destination[2] = (byte)(value >> 8);

        destination[3] = (byte)value;
    }

    /// <summary>
    /// Reads an unsigned 32-bit value in big-endian order.
    /// </summary>
    /// <param name="source">
    /// The source bytes.
    /// </param>
    /// <param name="offset">
    /// The starting offset.
    /// </param>
    /// <returns>
    /// The decoded value.
    /// </returns>
    private static uint ReadUInt32BigEndian(byte[] source, int offset)
    {
        RequireAvailable(source, offset, 4);

        return ((uint)source[offset] << 24) 
               | ((uint)source[offset + 1] << 16) 
               | ((uint)source[offset + 2] << 8) 
               | source[offset + 3];
    }

    /// <summary>
    /// Stops the attack simulator and releases its listener.
    /// </summary>
    /// <returns>
    /// A value task representing the asynchronous operation.
    /// </returns>
    public async ValueTask DisposeAsync()
    {
        if (m_CancellationTokenSource != null)
        {
            await m_CancellationTokenSource
                .CancelAsync()
                .ConfigureAwait(false);

            m_CancellationTokenSource.Dispose();
            m_CancellationTokenSource = null;
        }

        m_Listener?.Stop();
        m_Listener = null;
    }
}