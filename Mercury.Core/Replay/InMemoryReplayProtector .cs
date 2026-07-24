// ***********************************************************************
// Assembly     : Mercury.Core
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="InMemoryReplayProtector.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Replay;

namespace Mercury.Core.Replay;

/// <summary>
/// Detects repeated sender and replay-token combinations in memory.
/// </summary>
public sealed class InMemoryReplayProtector : IReplayProtector
{
    /// <summary>
    /// The default replay window.
    /// </summary>
    private static readonly TimeSpan sm_DefaultWindow = TimeSpan.FromMinutes(10);

    /// <summary>
    /// The default maximum number of active replay entries.
    /// </summary>
    private const int DEFAULT_MAX_ENTRIES = 10_000;

    /// <summary>
    /// The replay window.
    /// </summary>
    private readonly TimeSpan m_Window;

    /// <summary>
    /// The maximum number of active replay entries.
    /// </summary>
    private readonly int m_MaxEntries;

    /// <summary>
    /// Synchronizes cleanup, capacity checks, and insertion.
    /// </summary>
    private readonly object m_SyncRoot = new();

    /// <summary>
    /// The previously accepted replay entries.
    /// </summary>
    private readonly ConcurrentDictionary<string, long> m_Seen = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="InMemoryReplayProtector"/> class.
    /// </summary>
    /// <param name="window">The replay window.</param>
    /// <param name="maxEntries">
    /// The maximum number of active replay entries.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The replay window or maximum entry count is not greater than zero.
    /// </exception>
    public InMemoryReplayProtector(TimeSpan? window = null, int maxEntries = DEFAULT_MAX_ENTRIES)
    {
        m_Window = window ?? sm_DefaultWindow;

        if (m_Window <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(window), "Replay window must be greater than zero.");
        

        if (maxEntries <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxEntries), "Maximum replay entries must be greater than zero.");
        
        m_MaxEntries = maxEntries;
    }

    /// <summary>
    /// Attempts to accept and record the envelope header.
    /// </summary>
    /// <param name="header">The validated envelope header.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A task containing <c>true</c> if the header was accepted;
    /// otherwise, <c>false</c> if it was previously recorded or the replay
    /// store has reached capacity.
    /// </returns>
    public Task<bool> TryAcceptAsync(IEnvelopeHeader header, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (header == null)
            throw new ArgumentNullException(nameof(header));

        if (header.SenderKeyId.IsEmpty || header.ReplayToken.IsEmpty)
            return Task.FromResult(false);
        
        var nowMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var replayToken = Convert.ToBase64String(header.ReplayToken.ToArray());

        var replayKey = $"{header.SenderKeyId.Value}:{replayToken}";

        lock (m_SyncRoot)
        {
            Cleanup(nowMilliseconds);

            if (m_Seen.Count >= m_MaxEntries)
                return Task.FromResult(false);

            return Task.FromResult(m_Seen.TryAdd(replayKey, nowMilliseconds));
        }
    }

    /// <summary>
    /// Removes replay entries outside the configured window.
    /// </summary>
    /// <param name="nowMilliseconds">
    /// The current Unix time in milliseconds.
    /// </param>
    private void Cleanup(long nowMilliseconds)
    {
        var cutoffMilliseconds = nowMilliseconds - (long)m_Window.TotalMilliseconds;

        foreach (var entry in m_Seen)
        {
            if (entry.Value < cutoffMilliseconds)
            {
                m_Seen.TryRemove(entry.Key, out _);
            }
        }
    }
}