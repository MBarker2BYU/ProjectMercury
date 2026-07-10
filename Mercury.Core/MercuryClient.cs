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

namespace Mercury.Core;

/// <summary>
/// Class MercuryClient. This class cannot be inherited.
/// Implements the <see cref="IMercuryClient" />
/// </summary>
/// <seealso cref="IMercuryClient" />
internal sealed class MercuryClient : IMercuryClient
{
    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SendAsync(ReadOnlyMemory payload, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IMercuryResult&gt;.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IMercuryResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
