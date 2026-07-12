// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="MercuryFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Factories;
using Mercury.Abstractions.Services;
using Mercury.Abstractions.Transport;
using Mercury.Core.Cryptography;
using Mercury.Core.Services;
using Mercury.Core.Transport;

namespace Mercury.Core.Factories;

/// <summary>
/// Class MercuryFactory. This class cannot be inherited.
/// Implements the <see cref="IMercuryFactory" />
/// </summary>
/// <seealso cref="IMercuryFactory" />
public sealed class MercuryFactory : IMercuryFactory
{

    /// <summary>
    /// The static mercury factory
    /// </summary>
    private static readonly Lazy<MercuryFactory> sm_MercuryFactory = new(() => new MercuryFactory());

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static MercuryFactory Instance => sm_MercuryFactory.Value;

    /// <summary>
    /// The secure envelope factory
    /// </summary>
    private static readonly IEnvelopeService sm_EnvelopeService =
        EnvelopeService.Instance;

    private static readonly EnvelopeCodec sm_EnvelopeCodec = EnvelopeCodec.Json;
        
    /// <summary>
    /// Builds the dependencies.
    /// </summary>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec"></param>
    /// <param name="transport">The transport.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    public IMercuryClientDependencies BuildDependencies(ICryptoProvider cryptoProvider, EnvelopeCodec envelopeCodec, ITransport transport)
        => new MercuryClientDependencies(cryptoProvider, envelopeCodec, transport);

    /// <summary>
    /// Builds the dependencies with a passthrough crypto provider
    /// </summary>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    [Obsolete("Remove prior to release", false)]
    public IMercuryClientDependencies BuildDependencies(EnvelopeCodec envelopeCodec, ITransport transport)
        => new MercuryClientDependencies(new PassThroughCryptoProvider(nameof(PassThroughCryptoProvider)), envelopeCodec, transport);

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <returns>IMercuryClient.</returns>
    [Obsolete("Remove prior to release", false)]
    public IMercuryClient BuildClient()
    {
        var dependencies = BuildDependencies(new PassThroughCryptoProvider(nameof(PassThroughCryptoProvider)), sm_EnvelopeCodec, new LoopbackTransport());

        return BuildClient(dependencies);
    }

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <param name="mercuryClientDependencies"></param>
    /// <returns>IMercuryClient.</returns>
    public IMercuryClient BuildClient(IMercuryClientDependencies mercuryClientDependencies)
        => new MercuryClient(mercuryClientDependencies, sm_EnvelopeService);
}