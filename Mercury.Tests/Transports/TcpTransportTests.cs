// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="TcpTransportTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Net.Sockets;
using Mercury.Transport.Tcp;

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Transports;

/// <summary>
/// Class TcpTransportTests. This class cannot be inherited.
/// </summary>
public sealed class TcpTransportTests
{
    private const int MAXIMUM_FRAME_BYTES = 8 * 1024 * 1024;

    /// <summary>
    /// Defines the test method Constructor_NullClient_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public void Constructor_NullClient_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new TcpTransport(null!));
    }

    /// <summary>
    /// Defines the test method Constructor_UnconnectedClient_ThrowsInvalidOperationException.
    /// </summary>
    [Fact]
    public void Constructor_UnconnectedClient_ThrowsInvalidOperationException()
    {
        using var client = new TcpClient();

        Assert.Throws<InvalidOperationException>(() => new TcpTransport(client));
    }

    /// <summary>
    /// Defines the test method ConnectAsync_BlankHost_ThrowsArgumentException.
    /// </summary>
    [Fact]
    public async Task ConnectAsync_BlankHost_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => TcpTransport.ConnectAsync(" ", 12345));
    }

    /// <summary>
    /// Defines the test method ConnectAsync_InvalidPort_ThrowsArgumentOutOfRangeException.
    /// </summary>
    /// <param name="port">The port.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(65536)]
    public async Task ConnectAsync_InvalidPort_ThrowsArgumentOutOfRangeException(
        int port)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => TcpTransport.ConnectAsync("127.0.0.1", port));
    }

    /// <summary>
    /// Defines the test method AcceptAsync_NullListener_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public async Task AcceptAsync_NullListener_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => TcpTransport.AcceptAsync(null!));
    }

    /// <summary>
    /// Defines the test method ConnectedPair_ReportsConnected.
    /// </summary>
    [Fact]
    public async Task ConnectedPair_ReportsConnected()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        {
            Assert.True(alpha.IsConnected);
            Assert.True(bravo.IsConnected);
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_AlphaToBravo_ReturnsIdenticalFrame.
    /// </summary>
    [Fact]
    public async Task SendAsync_AlphaToBravo_ReturnsIdenticalFrame()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        {
            var expected = Enumerable.Range(0, 256)
                .Select(value => (byte)value)
                .ToArray();

            await alpha.SendAsync(new MercuryMemory(expected));
            var actual = await bravo.ReceiveAsync();

            Assert.Equal(expected, actual.ToArray());
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_BravoToAlpha_ReturnsIdenticalFrame.
    /// </summary>
    [Fact]
    public async Task SendAsync_BravoToAlpha_ReturnsIdenticalFrame()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        {
            var expected = new byte[] { 9, 8, 7, 6, 5 };

            await bravo.SendAsync(new MercuryMemory(expected));
            var actual = await alpha.ReceiveAsync();

            Assert.Equal(expected, actual.ToArray());
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_MultipleFrames_PreservesFrameBoundariesAndOrder.
    /// </summary>
    [Fact]
    public async Task SendAsync_MultipleFrames_PreservesFrameBoundariesAndOrder()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        {
            for (var index = 1; index <= 20; index++)
            {
                await alpha.SendAsync(new MercuryMemory(Enumerable.Repeat((byte)index, index).ToArray()));
            }

            for (var index = 1; index <= 20; index++)
            {
                var frame = await bravo.ReceiveAsync();
                Assert.Equal(Enumerable.Repeat((byte)index, index).ToArray(), frame.ToArray());
            }
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_EmptyFrame_ThrowsArgumentException.
    /// </summary>
    [Fact]
    public async Task SendAsync_EmptyFrame_ThrowsArgumentException()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => alpha.SendAsync(MercuryMemory.Empty));
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_FrameAboveMaximum_ThrowsInvalidOperationException.
    /// </summary>
    [Fact]
    public async Task SendAsync_FrameAboveMaximum_ThrowsInvalidOperationException()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        {
            var oversized = new byte[MAXIMUM_FRAME_BYTES + 1];

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => alpha.SendAsync(new MercuryMemory(oversized)));
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_WritesBigEndianLengthPrefix.
    /// </summary>
    [Fact]
    public async Task SendAsync_WritesBigEndianLengthPrefix()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();

        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        var acceptTask = listener.AcceptTcpClientAsync();
        var transportTask = TcpTransport.ConnectAsync("127.0.0.1", port);
        using var rawServer = await acceptTask;
        await using var transport = await transportTask;
        var expected = new byte[] { 1, 2, 3, 4, 5 };

        await transport.SendAsync(new MercuryMemory(expected));

        var stream = rawServer.GetStream();
        var header = new byte[4];

        await ReadExactAsync(stream, header);

        var frame = new byte[expected.Length];

        await ReadExactAsync(stream, frame);

        Assert.Equal(new byte[] { 0, 0, 0, 5 }, header);
        Assert.Equal(expected, frame);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_HandlesPartialHeaderAndPartialFrameReads.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_HandlesPartialHeaderAndPartialFrameReads()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();

        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        var acceptTask = TcpTransport.AcceptAsync(listener);

        using var rawClient = new TcpClient();
        await rawClient.ConnectAsync(IPAddress.Loopback, port);
        await using var transport = await acceptTask;

        listener.Stop();

        var expected = Enumerable.Range(1, 64)
            .Select(value => (byte)value)
            .ToArray();

        var stream = rawClient.GetStream();
        var receiveTask = transport.ReceiveAsync();

        foreach (var value in new byte[] { 0, 0, 0, 64 })
        {
            await stream.WriteAsync(new[] { value });
            await stream.FlushAsync();
        }

        for (var offset = 0; offset < expected.Length; offset += 3)
        {
            var count = Math.Min(3, expected.Length - offset);
            await stream.WriteAsync(expected.AsMemory(offset, count));
            await stream.FlushAsync();
        }

        var actual = await receiveTask;

        Assert.Equal(expected, actual.ToArray());
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_ZeroLengthPrefix_ThrowsInvalidOperationException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ZeroLengthPrefix_ThrowsInvalidOperationException()
    {
        var (transport, rawClient, listener) = await CreateRawToTransportPairAsync();
        await using (transport)

        using (rawClient)
        using (listener)
        {
            await rawClient.GetStream().WriteAsync(new byte[4]);
            await rawClient.GetStream().FlushAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => transport.ReceiveAsync());
        }
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_OversizedLengthPrefix_ThrowsInvalidOperationException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_OversizedLengthPrefix_ThrowsInvalidOperationException()
    {
        var (transport, rawClient, listener) = await CreateRawToTransportPairAsync();

        await using (transport)
        using (rawClient)
        using (listener)
        {
            var invalidLength = (uint)(MAXIMUM_FRAME_BYTES + 1);
            var header = new byte[]
            {
                (byte)(invalidLength >> 24),
                (byte)(invalidLength >> 16),
                (byte)(invalidLength >> 8),
                (byte)invalidLength
            };

            await rawClient.GetStream().WriteAsync(header);
            await rawClient.GetStream().FlushAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => transport.ReceiveAsync());
        }
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_RemoteClosesDuringHeader_ThrowsIOException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_RemoteClosesDuringHeader_ThrowsIOException()
    {
        var (transport, rawClient, listener) = await CreateRawToTransportPairAsync();

        await using (transport)
        using (listener)
        {
            await rawClient.GetStream().WriteAsync(new byte[] { 0, 0 });
            rawClient.Dispose();

            await Assert.ThrowsAsync<IOException>(
                () => transport.ReceiveAsync());
        }
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_RemoteClosesDuringFrame_ThrowsIOException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_RemoteClosesDuringFrame_ThrowsIOException()
    {
        var (transport, rawClient, listener) = await CreateRawToTransportPairAsync();

        await using (transport)
        using (listener)
        {
            var stream = rawClient.GetStream();
            await stream.WriteAsync(new byte[] { 0, 0, 0, 10, 1, 2, 3 });
            await stream.FlushAsync();
            rawClient.Dispose();

            await Assert.ThrowsAsync<IOException>(() => transport.ReceiveAsync());
        }
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_CanceledWhileWaiting_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_CanceledWhileWaiting_ThrowsOperationCanceledException()
    {
        var (alpha, bravo) = await CreatePairAsync();

        await using (alpha)
        await using (bravo)
        using (var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(250)))
        {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => alpha.ReceiveAsync(cancellation.Token));
        }
    }

    /// <summary>
    /// Defines the test method ConnectAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task ConnectAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => TcpTransport.ConnectAsync(
                "192.0.2.1", 65000, cancellation.Token));
    }

    /// <summary>
    /// Defines the test method AcceptAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task AcceptAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();

        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => TcpTransport.AcceptAsync(
                listener,
                cancellation.Token));
    }

    /// <summary>
    /// Creates the pair asynchronous.
    /// </summary>
    /// <returns>System.Threading.Tasks.Task&lt;(Mercury.Transport.Tcp.TcpTransport Alpha, Mercury.Transport.Tcp.TcpTransport Bravo)&gt;.</returns>
    private static async Task<(TcpTransport Alpha, TcpTransport Bravo)> CreatePairAsync()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();

        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        var acceptTask = TcpTransport.AcceptAsync(listener);
        var connectTask = TcpTransport.ConnectAsync("127.0.0.1", port);

        await Task.WhenAll(acceptTask, connectTask);
        listener.Stop();

        return (await connectTask, await acceptTask);
    }

    /// <summary>
    /// Creates the raw to transport pair asynchronous.
    /// </summary>
    /// <returns>System.Threading.Tasks.Task&lt;(Mercury.Transport.Tcp.TcpTransport Transport, System.Net.Sockets.TcpClient RawClient, System.Net.Sockets.TcpListener Listener)&gt;.</returns>
    private static async Task<(TcpTransport Transport, TcpClient RawClient, TcpListener Listener)> CreateRawToTransportPairAsync()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();

        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        var acceptTask = TcpTransport.AcceptAsync(listener);
        var rawClient = new TcpClient();

        await rawClient.ConnectAsync(IPAddress.Loopback, port);
        var transport = await acceptTask;

        listener.Stop();

        return (transport, rawClient, listener);
    }

    /// <summary>
    /// Reads the exact asynchronous.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    private static async Task ReadExactAsync(Stream stream, byte[] buffer)
    {
        var offset = 0;

        while (offset < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset, buffer.Length - offset));

            if (read == 0)
            {
                throw new EndOfStreamException();
            }

            offset += read;
        }
    }
}
