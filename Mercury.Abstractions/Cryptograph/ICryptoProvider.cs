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
    /// Seals the asynchronous.
    /// </summary>
    /// <param name="sealRequest">The protect request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
    Task<ICryptoProviderResult> SealAsync(ISealRequest sealRequest, IEnvelopeService envelopeService, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the asynchronous.
    /// </summary>
    /// <param name="openRequest">The unprotect request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
    Task<ICryptoProviderResult> OpenAsync(IOpenRequest openRequest, IEnvelopeService envelopeService, CancellationToken cancellationToken = default);
}