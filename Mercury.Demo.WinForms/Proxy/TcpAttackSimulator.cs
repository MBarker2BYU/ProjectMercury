// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-21-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-21-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Net.Sockets;

namespace Mercury.Demo.WinForms.Proxy;

/// <summary>
/// Class TcpAttackSimulator. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <param name="listenPort">The listen port.</param>
/// <param name="targetPort">The target port.</param>
/// <seealso cref="System.IAsyncDisposable" />
public sealed class TcpAttackSimulator(int listenPort, int targetPort) : IAsyncDisposable
{
    /// <summary>
    /// The m listener
    /// </summary>
    private TcpListener? m_Listener;
    /// <summary>
    /// The m CTS
    /// </summary>
    private CancellationTokenSource? m_CancellationTokenSource;

    // Demo controls
    /// <summary>
    /// Gets or sets a value indicating whether [tamper enabled].
    /// </summary>
    /// <value><c>true</c> if [tamper enabled]; otherwise, <c>false</c>.</value>
    public bool TamperEnabled { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [replay enabled].
    /// </summary>
    /// <value><c>true</c> if [replay enabled]; otherwise, <c>false</c>.</value>
    public bool ReplayEnabled { get; set; }

    /// <summary>
    /// The m last frame
    /// </summary>
    private byte[]? m_LastFrame;

    /// <summary>
    /// Start as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task StartAsync()
    {
        m_CancellationTokenSource = new CancellationTokenSource();
        m_Listener = new TcpListener(IPAddress.Loopback, listenPort);
        m_Listener.Start();

        Console.WriteLine(@$"[AttackSimulator] Listening on {listenPort} → real server {targetPort}");

        // Fire-and-forget the accept loop
        _ = AcceptLoopAsync(m_CancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Accept loop as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task AcceptLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = await m_Listener!.AcceptTcpClientAsync(cancellationToken);
                _ = HandleClientAsync(client, cancellationToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                Console.WriteLine(@$"[AttackSimulator] Accept error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Handle client as an asynchronous operation.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using var server = new TcpClient();
        await server.ConnectAsync(IPAddress.Loopback, targetPort, cancellationToken);

        var clientStream = client.GetStream();
        var serverStream = server.GetStream();

        var clientToServer = ForwardFramedAsync(clientStream, serverStream, isClientToServer: true, cancellationToken);
        var serverToClient = ForwardFramedAsync(serverStream, clientStream, isClientToServer: false, cancellationToken);

        await Task.WhenAll(clientToServer, serverToClient);
    }

    /// <summary>
    /// Forward framed as an asynchronous operation.
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="isClientToServer">if set to <c>true</c> [is client to server].</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task ForwardFramedAsync(NetworkStream from, NetworkStream to, bool isClientToServer, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // 1. Read length prefix
            var lengthBytes = new byte[4];
            await ReadExactAsync(from, lengthBytes, cancellationToken);

            var frameLength = ReadUInt32BigEndian(lengthBytes);
            if (frameLength == 0 || frameLength > 8 * 1024 * 1024)
                continue;

            // 2. Read the actual frame
            var frame = new byte[frameLength];
            await ReadExactAsync(from, frame, cancellationToken);

            var dataToSend = frame;

            // 3. Apply demo attacks (client → server)
            if (isClientToServer)
            {
                if (ReplayEnabled && m_LastFrame != null)
                {
                    if (ReplayEnabled && m_LastFrame != null)
                    {
                        // Replace the current frame with the previous one
                        dataToSend = m_LastFrame;
                        Console.WriteLine(@"[AttackSimulator] REPLAY – sending previous frame again");
                    }
                    else
                    {
                        // Normal case – remember this frame for a future replay
                        m_LastFrame = (byte[])dataToSend.Clone();
                    }
                }

                //if (TamperEnabled && dataToSend.Length > 40)
                //{
                //    dataToSend = (byte[])dataToSend.Clone();
                //    dataToSend[40] ^= 0xFF;
                //    Console.WriteLine(@"[AttackSimulator] TAMPER");
                //}

                if (TamperEnabled)
                {
                    dataToSend = (byte[])dataToSend.Clone();

                    var offset = dataToSend.Length / 3;

                    if (dataToSend.Length > 0)
                        dataToSend[^offset] ^= 0xFF;

                    Console.WriteLine(@$"[AttackSimulator] TAMPER applied (len={dataToSend.Length})");
                }

                m_LastFrame = (byte[])dataToSend.Clone();
            }

            // 4. Write length + frame
            var outLength = new byte[4];
            WriteUInt32BigEndian(outLength, (uint)dataToSend.Length);

            await to.WriteAsync(outLength, cancellationToken);
            await to.WriteAsync(dataToSend, cancellationToken);
            await to.FlushAsync(cancellationToken);
        }
        
    }

    public void ClearLastFrame()
    {
        m_LastFrame = null;
        Console.WriteLine(@"[AttackSimulator] Last frame cleared");
    }

    // ---------- helpers ----------
    /// <summary>
    /// Read exact as an asynchronous operation.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="buffer">The buffer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="IOException">Connection closed</exception>
    private static async Task ReadExactAsync(NetworkStream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;
        while (offset < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset), cancellationToken);
            if (read == 0) throw new IOException(@"Connection closed");
            offset += read;
        }
    }

    /// <summary>
    /// Writes the u int32 big endian.
    /// </summary>
    /// <param name="dest">The dest.</param>
    /// <param name="value">The value.</param>
    private static void WriteUInt32BigEndian(byte[] dest, uint value)
    {
        dest[0] = (byte)(value >> 24);
        dest[1] = (byte)(value >> 16);
        dest[2] = (byte)(value >> 8);
        dest[3] = (byte)value;
    }

    /// <summary>
    /// Reads the u int32 big endian.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>System.UInt32.</returns>
    private static uint ReadUInt32BigEndian(byte[] source)
    {
        return ((uint)source[0] << 24) |
               ((uint)source[1] << 16) |
               ((uint)source[2] << 8) |
               source[3];
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.
    /// </summary>
    /// <returns>A Task&lt;ValueTask&gt; representing the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await m_CancellationTokenSource?.CancelAsync()!;
        m_Listener?.Stop();
        await Task.CompletedTask;
    }
}