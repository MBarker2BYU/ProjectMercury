// ***********************************************************************
// Assembly       : Mercury.Transport.Tcp
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="TcpTransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Net.Sockets;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Transport.Tcp;

/// <summary>
/// TCP transport using length-prefixed framing:
/// [4-byte big-endian frame length][frame bytes].
/// </summary>
public sealed class TcpTransport : ITransport, IAsyncDisposable
{
    /// <summary>
    /// The maximum permitted frame size.
    /// </summary>
    private const uint MAX_FRAME_BYTES =
        8 * 1024 * 1024;

    /// <summary>
    /// The TCP client.
    /// </summary>
    private readonly TcpClient m_Client;

    /// <summary>
    /// The network stream.
    /// </summary>
    private readonly NetworkStream m_Stream;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TcpTransport"/> class.
    /// </summary>
    /// <param name="client">The connected TCP client.</param>
    /// <exception cref="ArgumentNullException">
    /// The client is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The TCP client is not connected.
    /// </exception>
    public TcpTransport(TcpClient client)
    {
        m_Client = client
            ?? throw new ArgumentNullException(
                nameof(client));

        if (!m_Client.Connected)
        {
            throw new InvalidOperationException(
                "The TCP client must be connected.");
        }

        m_Stream = m_Client.GetStream();
    }

    /// <summary>
    /// Connects to a remote TCP endpoint.
    /// </summary>
    /// <param name="host">The remote host.</param>
    /// <param name="port">The remote port.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>The connected TCP transport.</returns>
    /// <exception cref="ArgumentException">
    /// The host is missing.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The port is outside the valid TCP port range.
    /// </exception>
    public static async Task<TcpTransport> ConnectAsync(
        string host, int port, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            throw new ArgumentException(
                "Host is required.",
                nameof(host));
        }

        if (port is < 1 or > 65535)
        {
            throw new ArgumentOutOfRangeException(
                nameof(port),
                "Port must be between 1 and 65535.");
        }

        var client = new TcpClient();

        try
        {
            await client
                .ConnectAsync(
                    host,
                    port,
                    cancellationToken)
                .ConfigureAwait(false);

            return new TcpTransport(client);
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Accepts an incoming TCP client.
    /// </summary>
    /// <param name="listener">The TCP listener.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>The accepted TCP transport.</returns>
    /// <exception cref="ArgumentNullException">
    /// The listener is null.
    /// </exception>
    public static async Task<TcpTransport> AcceptAsync(
        TcpListener listener, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(listener);

        var client =
            await listener
                .AcceptTcpClientAsync(cancellationToken)
                .ConfigureAwait(false);

        return new TcpTransport(client);
    }

    /// <summary>
    /// Gets a value indicating whether the underlying socket is connected.
    /// </summary>
    /// <remarks>
    /// This reflects the last known socket state. A remote disconnect may not
    /// be detected until the next send or receive operation.
    /// </remarks>
    public bool IsConnected => m_Client.Connected;

    /// <summary>
    /// Sends one complete frame.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>A task representing the operation.</returns>
    /// <exception cref="ArgumentException">
    /// The frame is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The frame exceeds the maximum permitted size.
    /// </exception>
    public async Task SendAsync(
        ReadOnlyMemory frame,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (frame.IsEmpty)
        {
            throw new ArgumentException(
                "Frame must not be empty.",
                nameof(frame));
        }

        if ((uint)frame.Length > MAX_FRAME_BYTES)
        {
            throw new InvalidOperationException(
                $"Frame is too large: {frame.Length} bytes.");
        }

        var header = new byte[4];

        WriteUInt32BigEndian(
            header,
            (uint)frame.Length);

        var frameBytes =
            frame.ToArray();

        await m_Stream
            .WriteAsync(
                header,
                cancellationToken)
            .ConfigureAwait(false);

        await m_Stream
            .WriteAsync(
                frameBytes,
                cancellationToken)
            .ConfigureAwait(false);

        await m_Stream
            .FlushAsync(
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives one complete frame.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>The received frame.</returns>
    /// <exception cref="InvalidOperationException">
    /// The frame is empty or exceeds the maximum permitted size.
    /// </exception>
    public async Task<ReadOnlyMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var header = new byte[4];

        await ReadExactAsync(
                m_Stream,
                header,
                cancellationToken)
            .ConfigureAwait(false);

        var frameLength =
            ReadUInt32BigEndian(header);

        if (frameLength == 0)
        {
            throw new InvalidOperationException(
                "The remote endpoint sent an empty frame.");
        }

        if (frameLength > MAX_FRAME_BYTES)
        {
            throw new InvalidOperationException(
                $"Frame is too large: {frameLength} bytes.");
        }

        var frame =
            new byte[(int)frameLength];

        await ReadExactAsync(
                m_Stream,
                frame,
                cancellationToken)
            .ConfigureAwait(false);

        return new ReadOnlyMemory(frame);
    }

    /// <summary>
    /// Releases the TCP transport resources.
    /// </summary>
    /// <returns>A value task representing the operation.</returns>
    public async ValueTask DisposeAsync()
    {
        try
        {
            await m_Stream
                .DisposeAsync()
                .ConfigureAwait(false);
        }
        finally
        {
            m_Client.Dispose();
        }
    }

    /// <summary>
    /// Reads exactly the requested number of bytes.
    /// </summary>
    /// <param name="stream">The network stream.</param>
    /// <param name="buffer">The destination buffer.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>A task representing the operation.</returns>
    /// <exception cref="IOException">
    /// The remote endpoint closed the connection before the frame was complete.
    /// </exception>
    private static async Task ReadExactAsync(
        NetworkStream stream,
        byte[] buffer,
        CancellationToken cancellationToken)
    {
        var offset = 0;

        while (offset < buffer.Length)
        {
            var bytesRead =
                await stream
                    .ReadAsync(
                        buffer.AsMemory(
                            offset,
                            buffer.Length - offset),
                        cancellationToken)
                    .ConfigureAwait(false);

            if (bytesRead == 0)
            {
                throw new IOException(
                    "The remote endpoint closed the connection while a frame was being received.");
            }

            offset += bytesRead;
        }
    }

    /// <summary>
    /// Writes an unsigned 32-bit value in big-endian order.
    /// </summary>
    /// <param name="destination">
    /// The four-byte destination.
    /// </param>
    /// <param name="value">The value.</param>
    private static void WriteUInt32BigEndian(
        byte[] destination,
        uint value)
    {
        destination[0] =
            (byte)(value >> 24);

        destination[1] =
            (byte)(value >> 16);

        destination[2] =
            (byte)(value >> 8);

        destination[3] =
            (byte)value;
    }

    /// <summary>
    /// Reads an unsigned 32-bit value in big-endian order.
    /// </summary>
    /// <param name="source">The four-byte source.</param>
    /// <returns>The decoded value.</returns>
    private static uint ReadUInt32BigEndian(
        byte[] source)
    {
        return
            ((uint)source[0] << 24) |
            ((uint)source[1] << 16) |
            ((uint)source[2] << 8) |
            source[3];
    }
}