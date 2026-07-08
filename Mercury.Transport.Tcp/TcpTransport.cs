// ***********************************************************************
// Assembly       : Mercury.Transport.Tcp
// Author           : Matthew D. Barker
// Created          : 07-03-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-03-2026
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
/// TCP transport with simple length-prefixed framing:
/// [4 bytes big-endian length][payload bytes]
/// </summary>
/// <param name="client">The client.</param>
/// <exception cref="ArgumentNullException">client</exception>
/// <remarks>Initializes a new instance of the <see cref="TcpTransport" /> class.</remarks>
public sealed class TcpTransport(TcpClient client) : ITransport, IAsyncDisposable
{
    /// <summary>
    /// The m client
    /// </summary>
    private readonly TcpClient m_Client = client ?? throw new ArgumentNullException(nameof(client));
    /// <summary>
    /// The m stream
    /// </summary>
    private readonly NetworkStream m_Stream = client.GetStream();

    /// <summary>
    /// Connect asynchronously.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TcpTransport&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Host is required. - host</exception>
    /// <exception cref="ArgumentOutOfRangeException">port</exception>
    public static async Task<TcpTransport> ConnectAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("Host is required.", nameof(host));
        if (port is < 1 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(port));

        var client = new TcpClient();
        try
        {
            await client.ConnectAsync(host, port, cancellationToken).ConfigureAwait(false);
            return new TcpTransport(client);
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Accept asynchronously.
    /// </summary>
    /// <param name="listener">The listener.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TcpTransport&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<TcpTransport> AcceptAsync(TcpListener listener, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(listener);

        var client = await listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
        return new TcpTransport(client);
    }

    /// <summary>
    /// Send as an asynchronous operation.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Frame must not be empty. - frame</exception>
    public async Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (frame.IsEmpty)
            throw new ArgumentException("Frame must not be empty.", nameof(frame));

        var header = new byte[4];
        WriteUInt32Be(header, (uint)frame.Length);

        await m_Stream.WriteAsync(header, cancellationToken).ConfigureAwait(false);
        await m_Stream.WriteAsync(frame.ToArray(), cancellationToken).ConfigureAwait(false);
        await m_Stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Receive as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ReadOnlyMemory&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Frame too large: {len} bytes.</exception>
    public async Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var header = new byte[4];
        await ReadExactAsync(m_Stream, header, cancellationToken).ConfigureAwait(false);

        var len = ReadUInt32Be(header);
        if (len == 0)
            return ReadOnlyMemory.Empty;

        const uint maxFrame = 8 * 1024 * 1024; // 8 MiB guard
        if (len > maxFrame)
            throw new InvalidOperationException($"Frame too large: {len} bytes.");

        var payload = new byte[len];
        await ReadExactAsync(m_Stream, payload, cancellationToken).ConfigureAwait(false);

        return new ReadOnlyMemory(payload);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
    /// </summary>
    /// <returns>A Task&lt;ValueTask&gt; representing the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        try { await m_Stream.DisposeAsync().ConfigureAwait(false); } catch { }
        try { m_Client.Dispose(); } catch { }
    }

    /// <summary>
    /// Read exact as an asynchronous operation.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="buffer">The buffer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="IOException">Remote closed connection while reading.</exception>
    private static async Task ReadExactAsync(NetworkStream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;
        while (offset < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset, buffer.Length - offset), cancellationToken).ConfigureAwait(false);
            if (read <= 0)
                throw new IOException("Remote closed connection while reading.");
            offset += read;
        }
    }

    /// <summary>
    /// Writes the u int32 be.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    private static void WriteUInt32Be(byte[] destination, uint value)
    {
        destination[0] = (byte)(value >> 24);
        destination[1] = (byte)(value >> 16);
        destination[2] = (byte)(value >> 8);
        destination[3] = (byte)value;
    }

    /// <summary>
    /// Reads the u int32 be.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>System.UInt32.</returns>
    private static uint ReadUInt32Be(byte[] source)
        => ((uint)source[0] << 24) | ((uint)source[1] << 16) | ((uint)source[2] << 8) | source[3];

    /// <summary>
    /// Safes the close.
    /// </summary>
    /// <param name="tcpClient">The tcpClient.</param>
    private static void SafeClose(TcpClient tcpClient)
    {
        try { tcpClient.Close(); } catch { }
    }
}