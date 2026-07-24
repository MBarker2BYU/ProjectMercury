// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DemoController.Builders.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Core.Factories;
using Mercury.Demo.Console.Wrappers;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using System.Security.Cryptography;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury RC 1.2 console demonstration application.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Applies the current provider, transport, and codec configuration.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ApplyConfigurationAsync()
    {
        await DisposeTransportsAsync().ConfigureAwait(false);

        var keys = new Dictionary<KeyId, byte[]>
        {
            [ALPHA_NODE] = RandomNumberGenerator.GetBytes(32),
            [BRAVO_NODE] = RandomNumberGenerator.GetBytes(32),
            [CHARLIE_NODE] = RandomNumberGenerator.GetBytes(32)
        };

        var keyProvider = new SymmetricKeyProviderDictionary(keys);
        var alphaCryptoProvider = BuildCryptoProvider(keyProvider);
        var bravoCryptoProvider = BuildCryptoProvider(keyProvider);
        var transports = await BuildTransportAsync().ConfigureAwait(false);
        var envelopeCodec = BuildEnvelopeCodec();

        m_AlphaTransport = transports.Alpha;
        m_BravoTransport = transports.Bravo;
        m_AlphaCaptureTransport = new CaptureTransport(transports.Alpha);

        var alphaDependencies = MercuryFactory.Instance.BuildDependencies(ALPHA_NODE,
            alphaCryptoProvider,
            envelopeCodec,
            m_AlphaCaptureTransport);

        var bravoDependencies = MercuryFactory.Instance.BuildDependencies(BRAVO_NODE,
            bravoCryptoProvider,
            envelopeCodec,
            transports.Bravo);

        m_AlphaClient = MercuryFactory.Instance.BuildClient(alphaDependencies);
        m_BravoClient = MercuryFactory.Instance.BuildClient(bravoDependencies);
    }

    /// <summary>
    /// Builds the configured cryptographic provider.
    /// </summary>
    /// <param name="keys">The key provider.</param>
    /// <returns>The configured cryptographic provider.</returns>
    private ICryptoProvider BuildCryptoProvider(SymmetricKeyProviderDictionary keys)
    {
        return m_Configuration.CryptoProvider switch
        {
            AES_GCM => new AesGcmCryptoProvider(keys),
            CHA_CHA_20 => new ChaCha20CryptoProvider(keys),
            _ => throw new ArgumentException("The selected crypto provider is not supported.")
        };
    }

    /// <summary>
    /// Builds the configured envelope codec.
    /// </summary>
    /// <returns>The configured envelope codec.</returns>
    private EnvelopeCodec BuildEnvelopeCodec()
    {
        return m_Configuration.EnvelopeCodec switch
        {
            BINARY_CODEC => EnvelopeCodec.Binary,
            JSON_CODEC => EnvelopeCodec.Json,
            _ => throw new ArgumentException("The selected envelope codec is not supported.")
        };
    }
}
