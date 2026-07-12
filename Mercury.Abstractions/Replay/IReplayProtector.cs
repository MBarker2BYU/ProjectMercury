// ***********************************************************************
// Assembly     : Mercury.Abstractions
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="IReplayProtector.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;

namespace Mercury.Abstractions.Replay;

/// <summary>
/// Detects previously accepted secure envelopes.
/// </summary>
public interface IReplayProtector
{
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
    /// otherwise, <c>false</c> if it was previously recorded.
    /// </returns>
    Task<bool> TryAcceptAsync(IEnvelopeHeader header, CancellationToken cancellationToken = default);
}