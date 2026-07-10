// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="LoopbackTransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions.Envelope;
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
    private readonly ConcurrentQueue<ISecureEnvelope> m_Queue = new();
    /// <summary>
    /// The m signal
    /// </summary>
    private readonly SemaphoreSlim m_Signal = new(0);

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="secureEnvelope"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendAsync(
        ISecureEnvelope secureEnvelope,
        CancellationToken cancellationToken = default)
    {
        if (secureEnvelope == null)
        {
            throw new ArgumentNullException(nameof(secureEnvelope));
        }

        m_Queue.Enqueue(secureEnvelope);
        m_Signal.Release();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ISecureEnvelope&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">The loopback transport was signaled without a payload.</exception>
    public async Task<ISecureEnvelope> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        await m_Signal
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        if (m_Queue.TryDequeue(out var secureEnvelope))
        {
            return secureEnvelope;
        }

        throw new InvalidOperationException(
            "The loopback transport was signaled without a secure envelope.");
    }
}