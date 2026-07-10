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
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Transport;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClient. This class cannot be inherited.
/// Implements the <see cref="IMercuryClient" />
/// </summary>
/// <seealso cref="IMercuryClient" />
internal sealed class MercuryClient(ITransport transport) : IMercuryClient
{

    private readonly ITransport m_Transport = transport
                                              ?? throw new ArgumentNullException(nameof(transport));

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SendAsync(ReadOnlyMemory payload, CancellationToken cancellationToken = default)
    {
        return m_Transport.SendAsync(
            payload,
            cancellationToken);
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IMercuryResult&gt;.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IMercuryResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = await m_Transport
                .ReceiveAsync(cancellationToken)
                .ConfigureAwait(false);

            return new MercuryResult(
                true,
                payload,
                FailureReason.None);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return new MercuryResult(
                false,
                ReadOnlyMemory.Empty,
                FailureReason.InternalError,
                exception.Message);
        }
    }
}
