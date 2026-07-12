// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="LoopbackTransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Core.Transport;

/// <summary>
/// Class LoopbackTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
internal sealed class LoopbackTransport : ITransport
{
    /// <summary>
    /// The m queue
    /// </summary>
    private readonly ConcurrentQueue<ReadOnlyMemory> m_Queue = new();
    /// <summary>
    /// The m signal
    /// </summary>
    private readonly SemaphoreSlim m_Signal = new(0);

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendAsync(
        ReadOnlyMemory frame,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (frame.IsEmpty)
        {
            throw new ArgumentException("Sending empty frames is not allowed.",nameof(frame));
        }

        m_Queue.Enqueue(frame.Clone());
        m_Signal.Release();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ReadOnlyMemory&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">The loopback transport was signaled without a frame.</exception>
    public async Task<ReadOnlyMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        await m_Signal
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        if (m_Queue.TryDequeue(out var frame))
        {
            return frame;
        }

        throw new InvalidOperationException(
            "The loopback transport was signaled without a frame.");
    }
}