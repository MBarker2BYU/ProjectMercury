// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="InMemoryReplayProtector.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions.Detection;
using Mercury.Abstractions.Envelope;

namespace Mercury.Core.Detection;

/// <summary>
/// Class InMemoryReplayProtector.
/// Implements the <see cref="IReplayProtector" />
/// </summary>
/// <seealso cref="IReplayProtector" />
internal class InMemoryReplayProtector : IReplayProtector
{

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryReplayProtector"/> class.
    /// </summary>
    /// <param name="window">The window.</param>
    /// <exception cref="ArgumentOutOfRangeException">window - Replay window must be > 0.</exception>
    public InMemoryReplayProtector(TimeSpan? window = null)
    {
        m_Window = window ?? TimeSpan.FromMinutes(10);

        if (m_Window <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(window), "Replay window must be > 0.");
    }

    /// <summary>
    /// Tries to accept asynchronous.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    /// <exception cref="ArgumentException">header</exception>
    public Task<bool> TryAcceptAsync(IEnvelopeHeader header, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (header is null)
            throw new ArgumentException(nameof(header));

        if (string.IsNullOrWhiteSpace(header.SenderKeyId.Value) || header.ReplayToken.IsEmpty)
            return Task.FromResult(false);

        var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        Cleanup(nowMs);

        var tokenB64 = Convert.ToBase64String(header.ReplayToken.ToArray());
        var key = $"{header.SenderKeyId}:{tokenB64}";

        var added = m_Seen.TryAdd(key, nowMs);
        return Task.FromResult(added);
    }

    /// <summary>
    /// Cleanups the specified now ms.
    /// </summary>
    /// <param name="nowMs">The now ms.</param>
    private void Cleanup(long nowMs)
    {
        var cutoffMs = nowMs - (long)m_Window.TotalMilliseconds;

        foreach (var keyValuePair in m_Seen)
        {
            if (keyValuePair.Value < cutoffMs)
                m_Seen.TryRemove(keyValuePair.Key, out _);
        }
    }

    /// <summary>
    /// The m window
    /// </summary>
    private readonly TimeSpan m_Window;

    /// <summary>
    /// The m seen
    /// </summary>
    private readonly ConcurrentDictionary<string, long> m_Seen = new();

}