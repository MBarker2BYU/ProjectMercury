// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="ICryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;

namespace Mercury.Abstractions.Cryptograph;

/// <summary>
/// Interface ICryptoProvider
/// </summary>
public interface ICryptoProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    /// <summary>
    /// Protects the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="envelopeService"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    Task<ICryptoProviderResult> ProtectAsync(ReadOnlyMemory payload, IEnvelopeService envelopeService, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unprotects the asynchronous.
    /// </summary>
    /// <param name="secureEnvelope"></param>
    /// <param name="envelopeService"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    Task<ICryptoProviderResult> UnprotectAsync(ISecureEnvelope secureEnvelope, IEnvelopeService envelopeService, CancellationToken cancellationToken = default);
}