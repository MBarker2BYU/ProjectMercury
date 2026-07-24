// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="EnvelopeCodecTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Transport.InMemory;
using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Transports;

/// <summary>
/// Class InMemoryDuplexTransportTests. This class cannot be inherited.
/// </summary>
public sealed class InMemoryDuplexTransportTests
{
    /// <summary>
    /// Defines the test method CreateConnectedPair_InvalidCapacity_ThrowsArgumentOutOfRangeException.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateConnectedPair_InvalidCapacity_ThrowsArgumentOutOfRangeException(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => InMemoryDuplexTransport.CreateConnectedPair(capacity));
    }

    /// <summary>
    /// Defines the test method CreateConnectedPair_ReturnsConnectedEndpoints.
    /// </summary>
    [Fact]
    public void CreateConnectedPair_ReturnsConnectedEndpoints()
    {
        var (alpha, bravo) =
            InMemoryDuplexTransport.CreateConnectedPair();

        Assert.True(alpha.IsConnected);
        Assert.True(bravo.IsConnected);
        Assert.NotSame(alpha, bravo);
    }

    /// <summary>
    /// Defines the test method SendAsync_AlphaToBravo_ReturnsIdenticalFrame.
    /// </summary>
    [Fact]
    public async Task SendAsync_AlphaToBravo_ReturnsIdenticalFrame()
    {
        var (alpha, bravo) =
            InMemoryDuplexTransport.CreateConnectedPair();

        var expected = Enumerable.Range(0, 256)
            .Select(value => (byte)value)
            .ToArray();

        await alpha.SendAsync(new MercuryMemory(expected));
        var actual = await bravo.ReceiveAsync();

        Assert.Equal(expected, actual.ToArray());
    }

    /// <summary>
    /// Defines the test method SendAsync_BravoToAlpha_ReturnsIdenticalFrame.
    /// </summary>
    [Fact]
    public async Task SendAsync_BravoToAlpha_ReturnsIdenticalFrame()
    {
        var (alpha, bravo) = InMemoryDuplexTransport.CreateConnectedPair();
        var expected = new byte[] { 9, 8, 7, 6, 5 };

        await bravo.SendAsync(new MercuryMemory(expected));
        var actual = await alpha.ReceiveAsync();

        Assert.Equal(expected, actual.ToArray());
    }

    /// <summary>
    /// Defines the test method SendAsync_MultipleFrames_PreservesFifoOrder.
    /// </summary>
    [Fact]
    public async Task SendAsync_MultipleFrames_PreservesFifoOrder()
    {
        var (alpha, bravo) = InMemoryDuplexTransport.CreateConnectedPair();

        for (var index = 0; index < 32; index++)
        {
            await alpha.SendAsync(
                new MercuryMemory([(byte)index]));
        }

        for (var index = 0; index < 32; index++)
        {
            var frame = await bravo.ReceiveAsync();
            Assert.Equal([(byte)index], frame.ToArray());
        }
    }

    /// <summary>
    /// Defines the test method SendAsync_EmptyFrame_ThrowsArgumentException.
    /// </summary>
    [Fact]
    public async Task SendAsync_EmptyFrame_ThrowsArgumentException()
    {
        var (alpha, _) = InMemoryDuplexTransport.CreateConnectedPair();

        await Assert.ThrowsAsync<ArgumentException>(() => alpha.SendAsync(MercuryMemory.Empty));
    }

    /// <summary>
    /// Defines the test method SendAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task SendAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var (alpha, _) = InMemoryDuplexTransport.CreateConnectedPair();
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => alpha.SendAsync(
                new MercuryMemory([1, 2, 3]), cancellation.Token));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_CanceledWhileWaiting_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_CanceledWhileWaiting_ThrowsOperationCanceledException()
    {
        var (alpha, _) = InMemoryDuplexTransport.CreateConnectedPair();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => alpha.ReceiveAsync(cancellation.Token));
    }

    /// <summary>
    /// Defines the test method SendAsync_WhenBoundedChannelIsFull_HonorsCancellation.
    /// </summary>
    [Fact]
    public async Task SendAsync_WhenBoundedChannelIsFull_HonorsCancellation()
    {
        var (alpha, _) =
            InMemoryDuplexTransport.CreateConnectedPair(capacity: 1);
        await alpha.SendAsync(new MercuryMemory([1]));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => alpha.SendAsync(
                new MercuryMemory([2]), cancellation.Token));
    }

    /// <summary>
    /// Defines the test method Endpoints_DoNotReceiveTheirOwnOutboundFrames.
    /// </summary>
    [Fact]
    public async Task Endpoints_DoNotReceiveTheirOwnOutboundFrames()
    {
        var (alpha, bravo) = InMemoryDuplexTransport.CreateConnectedPair();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        await alpha.SendAsync(new MercuryMemory([1, 2, 3]), cancellation.Token);
        var receivedByBravo = await bravo.ReceiveAsync(cancellation.Token);

        Assert.Equal(new byte[] { 1, 2, 3 }, receivedByBravo.ToArray());

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => alpha.ReceiveAsync(cancellation.Token));
    }

    /// <summary>
    /// Defines the test method BidirectionalExchange_CanRunConcurrently.
    /// </summary>
    [Fact]
    public async Task BidirectionalExchange_CanRunConcurrently()
    {
        var (alpha, bravo) = InMemoryDuplexTransport.CreateConnectedPair();

        var alphaPayload = new byte[] { 1, 3, 5, 7 };
        var bravoPayload = new byte[] { 2, 4, 6, 8 };

        await Task.WhenAll(alpha.SendAsync(new MercuryMemory(alphaPayload)),
            bravo.SendAsync(new MercuryMemory(bravoPayload)));

        var alphaReceive = alpha.ReceiveAsync();
        var bravoReceive = bravo.ReceiveAsync();
        await Task.WhenAll(alphaReceive, bravoReceive);

        Assert.Equal(bravoPayload, alphaReceive.Result.ToArray());
        Assert.Equal(alphaPayload, bravoReceive.Result.ToArray());
    }

    /// <summary>
    /// Defines the test method SeparatePairs_RemainIsolated.
    /// </summary>
    [Fact]
    public async Task SeparatePairs_RemainIsolated()
    {
        var (alphaOne, bravoOne) = InMemoryDuplexTransport.CreateConnectedPair();
        var (alphaTwo, bravoTwo) = InMemoryDuplexTransport.CreateConnectedPair();

        await alphaOne.SendAsync(new MercuryMemory([1]));
        await alphaTwo.SendAsync(new MercuryMemory([2]));

        Assert.Equal(new byte[] { 1 }, (await bravoOne.ReceiveAsync()).ToArray());
        Assert.Equal(new byte[] { 2 }, (await bravoTwo.ReceiveAsync()).ToArray());
    }

    /// <summary>
    /// Defines the test method
    /// SendAsync_FrameExceedsMaximum_ThrowsInvalidOperationException.
    /// </summary>
    [Fact]
    public async Task SendAsync_FrameExceedsMaximum_ThrowsInvalidOperationException()
    {
        var (alpha, _) = InMemoryDuplexTransport.CreateConnectedPair();

        var oversizedFrame = new byte[(8 * 1024 * 1024) + 1];

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => alpha.SendAsync(new MercuryMemory(oversizedFrame)));

        Assert.Contains("Frame is too large", exception.Message);
    }
}
