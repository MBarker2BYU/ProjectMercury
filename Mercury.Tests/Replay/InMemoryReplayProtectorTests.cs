// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="InMemoryReplayProtectorTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Core.Replay;
using Mercury.Core.Services;
using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Replay;

/// <summary>
/// Class InMemoryReplayProtectorTests. This class cannot be inherited.
/// </summary>
public sealed class InMemoryReplayProtectorTests
{
    /// <summary>
    /// Defines the test method Constructor_NonPositiveWindow_ThrowsArgumentOutOfRangeException.
    /// </summary>
    /// <param name="milliseconds">The milliseconds.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveWindow_ThrowsArgumentOutOfRangeException(int milliseconds)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() 
            => new InMemoryReplayProtector(TimeSpan.FromMilliseconds(milliseconds)));
    }

    /// <summary>
    /// Defines the test method Constructor_NonPositiveMaximumEntries_ThrowsArgumentOutOfRangeException.
    /// </summary>
    /// <param name="maximumEntries">The maximum entries.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveMaximumEntries_ThrowsArgumentOutOfRangeException(int maximumEntries)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() 
            => new InMemoryReplayProtector(maxEntries: maximumEntries));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_FirstHeader_ReturnsTrue.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_FirstHeader_ReturnsTrue()
    {
        var protector = new InMemoryReplayProtector();

        var header = BuildHeader("alpha", [1, 2, 3]);

        var accepted = await protector.TryAcceptAsync(header);

        Assert.True(accepted);
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_SameSenderAndReplayToken_ReturnsFalseOnReplay.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_SameSenderAndReplayToken_ReturnsFalseOnReplay()
    {
        var protector = new InMemoryReplayProtector();
        var header = BuildHeader("alpha", [1, 2, 3]);

        var first = await protector.TryAcceptAsync(header);
        var second = await protector.TryAcceptAsync(header);

        Assert.True(first);
        Assert.False(second);
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_SameReplayTokenDifferentSender_ReturnsTrue.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_SameReplayTokenDifferentSender_ReturnsTrue()
    {
        var protector = new InMemoryReplayProtector();

        var alpha = await protector.TryAcceptAsync(BuildHeader("alpha", [1, 2, 3]));
        
        var bravo = await protector.TryAcceptAsync(BuildHeader("bravo", [1, 2, 3]));

        Assert.True(alpha);
        Assert.True(bravo);
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_SameSenderDifferentReplayToken_ReturnsTrue.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_SameSenderDifferentReplayToken_ReturnsTrue()
    {
        var protector = new InMemoryReplayProtector();

        var first = await protector.TryAcceptAsync(BuildHeader("alpha", [1, 2, 3]));

        var second = await protector.TryAcceptAsync(BuildHeader("alpha", [4, 5, 6]));

        Assert.True(first);
        Assert.True(second);
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_MissingSenderKeyId_ReturnsFalse.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_MissingSenderKeyId_ReturnsFalse()
    {
        var protector = new InMemoryReplayProtector();
        var header = BuildHeader(string.Empty, [1, 2, 3]);

        Assert.False(await protector.TryAcceptAsync(header));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_MissingReplayToken_ReturnsFalse.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_MissingReplayToken_ReturnsFalse()
    {
        var protector = new InMemoryReplayProtector();
        var header = BuildHeader("alpha", []);

        Assert.False(await protector.TryAcceptAsync(header));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_NullHeader_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_NullHeader_ThrowsArgumentNullException()
    {
        var protector = new InMemoryReplayProtector();

        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => protector.TryAcceptAsync(null!));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var protector = new InMemoryReplayProtector();

        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() 
            => protector.TryAcceptAsync(BuildHeader("alpha", [1, 2, 3]), cancellation.Token));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_ConcurrentDuplicateOnlyOneIsAccepted.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_ConcurrentDuplicateOnlyOneIsAccepted()
    {
        var protector = new InMemoryReplayProtector();
        var header = BuildHeader("alpha", [1, 2, 3]);

        var results = await Task.WhenAll(Enumerable.Range(0, 32)
                .Select(_ => protector.TryAcceptAsync(header)));

        Assert.Equal(1, results.Count(accepted => accepted));
        Assert.Equal(31, results.Count(accepted => !accepted));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_MaximumEntriesReached_ReturnsFalse.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_MaximumEntriesReached_ReturnsFalse()
    {
        var protector = new InMemoryReplayProtector(maxEntries: 2);

        var first = await protector.TryAcceptAsync(BuildHeader("alpha", [1, 2, 3]));

        var second = await protector.TryAcceptAsync(BuildHeader("alpha", [4, 5, 6]));

        var third = await protector.TryAcceptAsync(BuildHeader("alpha", [7, 8, 9]));

        Assert.True(first);
        Assert.True(second);
        Assert.False(third);
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_AfterWindowExpires_HeaderCanBeAcceptedAgain.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_AfterWindowExpires_HeaderCanBeAcceptedAgain()
    {
        var protector = new InMemoryReplayProtector(TimeSpan.FromMilliseconds(25));

        var header = BuildHeader("alpha", [1, 2, 3]);

        Assert.True(await protector.TryAcceptAsync(header));
        
        await Task.Delay(100);
        
        Assert.True(await protector.TryAcceptAsync(header));
    }

    /// <summary>
    /// Defines the test method TryAcceptAsync_AfterEntryExpires_CapacityBecomesAvailable.
    /// </summary>
    [Fact]
    public async Task TryAcceptAsync_AfterEntryExpires_CapacityBecomesAvailable()
    {
        var protector = new InMemoryReplayProtector(TimeSpan.FromMilliseconds(25), maxEntries: 1);

        var firstHeader = BuildHeader("alpha", [1, 2, 3]);

        var secondHeader = BuildHeader("alpha", [4, 5, 6]);

        Assert.True(await protector.TryAcceptAsync(firstHeader));

        Assert.False(await protector.TryAcceptAsync(secondHeader));

        await Task.Delay(100);

        Assert.True(await protector.TryAcceptAsync(secondHeader));
    }

    /// <summary>
    /// Builds the header.
    /// </summary>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="replayToken">The replay token.</param>
    /// <returns>Mercury.Abstractions.Envelope.IEnvelopeHeader.</returns>
    private static Mercury.Abstractions.Envelope.IEnvelopeHeader BuildHeader(
        string senderKeyId,
        byte[] replayToken)
        => EnvelopeService.Instance.BuildEnvelopeHeader(new KeyId(Guid.NewGuid().ToString("N")), DateTimeOffset.UtcNow,
            new KeyId(senderKeyId), new KeyId("recipient"), new AlgorithmId("test"), AlgorithmId.Empty,
            new MercuryMemory(replayToken));
}