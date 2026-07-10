// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Primitives;
using System.Collections.Concurrent;
using System.Security.Authentication.ExtendedProtection;
using Mercury.Abstractions.Enums;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClient. This class cannot be inherited.
/// Implements the <see cref="IMercuryClient" />
/// </summary>
/// <seealso cref="IMercuryClient" />
internal sealed class MercuryClient : IMercuryClient
{

    private readonly ConcurrentQueue<byte[]> m_Queue = new();
    private readonly SemaphoreSlim m_Signal = new(0);

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SendAsync(ReadOnlyMemory payload, CancellationToken cancellationToken = default)
    {
        var copy = (byte[])payload.Clone();

        m_Queue.Enqueue(copy);
        m_Signal.Release();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IMercuryResult&gt;.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IMercuryResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        await m_Signal.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        if (m_Queue.TryDequeue(out byte[] payload))
        {
            return new MercuryResult(true, new ReadOnlyMemory(payload), FailureReason.None);
        }

        return new MercuryResult(false, null, FailureReason.InternalError,
            "The loopback queue was signaled without a payload.");
    }
}
