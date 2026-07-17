// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="PassThroughCryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using System.Security.Cryptography;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class PassThroughCryptoProvider. This class cannot be inherited.
/// Implements the <see cref="ICryptoProvider" />
/// </summary>
/// <seealso cref="ICryptoProvider" />
internal sealed class PassThroughCryptoProvider : ICryptoProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name => "pass-through";

    /// <summary>
    /// Seals the asynchronous.
    /// </summary>
    /// <param name="sealRequest">The protect request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>

    public Task<ICryptoProviderResult> SealAsync(ISealRequest sealRequest, IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (sealRequest == null)
            throw new ArgumentNullException(nameof(sealRequest));

        if (envelopeService == null)
            throw new ArgumentNullException(nameof(envelopeService));

        if (sealRequest.Payload.IsEmpty)
        {
            return Task.FromResult(envelopeService.BuildCryptoProviderResult(
                    false, ReadOnlyMemory.Empty, null,
                    FailureReason.Custom, "The payload cannot be empty."));
        }

        if (sealRequest.CryptoContext.IsEmpty)
        {
            return Task.FromResult(envelopeService.BuildCryptoProviderResult(
                    false, ReadOnlyMemory.Empty, null,
                    FailureReason.Custom, "The crypto context cannot be empty."));
        }

        var replayToken = new byte[16];

        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(replayToken);
        }

        var header =
            envelopeService.BuildEnvelopeHeader(new KeyId(Guid.NewGuid().ToString("N")),
                DateTimeOffset.UtcNow, sealRequest.CryptoContext.SenderKeyId, sealRequest.CryptoContext.RecipientKeyId,
                new AlgorithmId(Name), AlgorithmId.None, new ReadOnlyMemory(replayToken),
                sealRequest.HeaderMeta.Clone());

        var footer =
            envelopeService.BuildEnvelopeFooter(sealRequest.FooterMeta.Clone());

        return Task.FromResult<ICryptoProviderResult>(envelopeService.PackEnvelope(header,
                sealRequest.Payload, footer));
    }

    /// <summary>
    /// Opens the asynchronous.
    /// </summary>
    /// <param name="openRequest">The unprotect request.</param>
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