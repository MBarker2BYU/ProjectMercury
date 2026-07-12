// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="PassThroughCryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class PassThroughCryptoProvider. This class cannot be inherited.
/// Implements the <see cref="ICryptoProvider" />
/// </summary>
/// <param name="name">The name.</param>
/// <seealso cref="ICryptoProvider" />
internal sealed class PassThroughCryptoProvider(string name) : ICryptoProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; } = name;

    /// <summary>
    /// Seals the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
    public Task<ICryptoProviderResult> SealAsync(
        ISealRequest sealRequest,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        //Do Crypto Here
        var header = envelopeService.BuildEnvelopeHeader();
        var footer = envelopeService.BuildEnvelopeFooter();

        return Task.FromResult<ICryptoProviderResult>(envelopeService.PackEnvelope(header, sealRequest.Payload, footer));
    }

    /// <summary>
    /// Opens the asynchronous.
    /// </summary>
    /// <param name="secureEnvelope">The secure envelope.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
    public Task<ICryptoProviderResult> OpenAsync(
        IOpenRequest openRequest,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        //Do Crypto Here

        return Task.FromResult<ICryptoProviderResult>(envelopeService.UnpackEnvelope(openRequest.SecureEnvelope));
    }
}