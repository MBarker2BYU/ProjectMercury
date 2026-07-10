// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="IMercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions;

/// <summary>
/// Interface IMercuryClient
/// </summary>
public interface IMercuryClient
{
    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SendAsync(ReadOnlyMemory payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IMercuryResult&gt;.</returns>
    Task<IMercuryResult> ReceiveAsync(CancellationToken cancellationToken = default);
}