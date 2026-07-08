// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="ICryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;

namespace Mercury.Abstractions.Cryptography;

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
    /// Seals the asynchronous.
    /// </summary>
    /// <param name="sealRequest">The seal request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ISecureEnvelope&gt;.</returns>
    Task<ISecureEnvelope> SealAsync(ISealRequest sealRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unseals the asynchronous.
    /// </summary>
    /// <param name="unsealRequest">The unseal request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IMercuryResult&gt;.</returns>
    Task<IMercuryResult> UnsealAsync(IUnsealRequest unsealRequest, CancellationToken cancellationToken = default);
}