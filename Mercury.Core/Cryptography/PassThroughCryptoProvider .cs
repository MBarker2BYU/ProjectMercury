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

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;

namespace Mercury.Core.Cryptography;

internal sealed class PassThroughCryptoProvider(string name) : ICryptoProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; } = name;

    public Task<ICryptoProviderResult> ProtectAsync(
        ReadOnlyMemory payload,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        //Do Crypto Here
        var header = envelopeService.BuildEnvelopeHeader();
        var footer = envelopeService.BuildEnvelopeFooter();

        return Task.FromResult<ICryptoProviderResult>(envelopeService.PackEnvelope(header, payload, footer));
    }

    public Task<ICryptoProviderResult> UnprotectAsync(
        ISecureEnvelope secureEnvelope,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        //Do Crypto Here

        return Task.FromResult<ICryptoProviderResult>(envelopeService.UnpackEnvelope(secureEnvelope));
    }
}