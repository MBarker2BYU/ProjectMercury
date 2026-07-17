// ***********************************************************************
// Assembly     : Mercury.Extensions.EasySetup
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="MercuryFactoryExtensions.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Shared;
using Mercury.Abstractions.Transport;
using Mercury.Core.Factories;

namespace Mercury.Extensions.EasySetup;

/// <summary>
/// Provides simplified construction methods over the standard Mercury
/// factory configuration path.
/// </summary>
public static class MercuryFactoryExtensions
{
    /// <summary>
    /// Builds a Mercury client without requiring the caller to construct
    /// an explicit dependency object.
    /// </summary>
    /// <param name="factory">The Mercury factory.</param>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <returns>A configured Mercury client.</returns>
    public static IMercuryClient BuildEasyClient(
        this MercuryFactory factory,
        ICryptoProvider cryptoProvider,
        EnvelopeCodec envelopeCodec,
        ITransport transport)
    {
        if (factory == null)
            throw new ArgumentNullException(nameof(factory));

        if (cryptoProvider == null)
        {
            throw new ArgumentNullException(nameof(cryptoProvider));
        }

        if (transport == null)
            throw new ArgumentNullException(nameof(transport));

        var dependencies =
            factory.BuildDependencies(cryptoProvider, envelopeCodec, transport);

        return factory.BuildClient(dependencies);
    }

    /// <summary>
    /// Creates two Mercury clients with shared temporary symmetric keys and
    /// communication contexts for exchanges in either direction.
    /// </summary>
    /// <param name="factory">The Mercury factory.</param>
    /// <param name="alphaKeyName">
    /// The logical key name assigned to the Alpha client.
    /// </param>
    /// <param name="bravoKeyName">
    /// The logical key name assigned to the Bravo client.
    /// </param>
    /// <param name="cryptoProviderFactory">
    /// A function that builds a crypto provider using the generated
    /// symmetric key provider.
    /// </param>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="alphaTransport">The Alpha transport.</param>
    /// <param name="bravoTransport">The Bravo transport.</param>
    /// <returns>
    /// A configured Mercury client pair.
    /// </returns>
    public static MercuryClientPair BuildEphemeralClientPair(this MercuryFactory factory, string alphaKeyName,
        string bravoKeyName, Func<SymmetricKeyProviderDictionary, ICryptoProvider> cryptoProviderFactory,
        EnvelopeCodec envelopeCodec, ITransport alphaTransport, ITransport bravoTransport)
    {
        if (factory == null)
            throw new ArgumentNullException(nameof(factory));

        if (string.IsNullOrWhiteSpace(alphaKeyName))
        {
            throw new ArgumentException("The Alpha key name cannot be null, empty, or whitespace.", nameof(alphaKeyName));
        }

        if (string.IsNullOrWhiteSpace(bravoKeyName))
        {
            throw new ArgumentException("The Bravo key name cannot be null, empty, or whitespace.", nameof(bravoKeyName));
        }

        if (string.Equals(alphaKeyName, bravoKeyName, StringComparison.Ordinal))
        {
            throw new ArgumentException("The Alpha and Bravo key names must be different.");
        }

        if (cryptoProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(cryptoProviderFactory));
        }

        if (alphaTransport == null)
        {
            throw new ArgumentNullException(nameof(alphaTransport));
        }

        if (bravoTransport == null)
        {
            throw new ArgumentNullException(nameof(bravoTransport));
        }

        var keyProvider =
            EphemeralKeyProviderFactory.Create(alphaKeyName, bravoKeyName);

        var alphaCryptoProvider = cryptoProviderFactory(keyProvider);

        var bravoCryptoProvider = cryptoProviderFactory(keyProvider);

        if (alphaCryptoProvider == null)
        {
            throw new InvalidOperationException("The crypto provider factory returned a null Alpha provider.");
        }

        if (bravoCryptoProvider == null)
        {
            throw new InvalidOperationException("The crypto provider factory returned a null Bravo provider.");
        }

        var alphaClient = factory.BuildEasyClient(alphaCryptoProvider, envelopeCodec, alphaTransport);

        var bravoClient = factory.BuildEasyClient(bravoCryptoProvider, envelopeCodec, bravoTransport);

        var alphaToBravoContext = factory.BuildCryptoContext(alphaKeyName, bravoKeyName);

        var bravoToAlphaContext = factory.BuildCryptoContext(bravoKeyName, alphaKeyName);

        return new MercuryClientPair(alphaClient, bravoClient, alphaToBravoContext, bravoToAlphaContext);
    }
}