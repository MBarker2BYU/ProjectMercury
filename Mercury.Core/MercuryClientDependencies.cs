// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="MercuryClientDependencies.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Transport;
using Mercury.Core.Codecs;
using Mercury.Core.Services;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClientDependencies. This class cannot be inherited.
/// Implements the <see cref="IMercuryClientDependencies" />
/// </summary>
/// <param name="cryptoProvider">The crypto provider.</param>
/// <param name="envelopeCodec">The envelope codec.</param>
/// <param name="transport">The transport.</param>
/// <seealso cref="IMercuryClientDependencies" />
public sealed class MercuryClientDependencies(ICryptoProvider cryptoProvider, EnvelopeCodec envelopeCodec, ITransport transport) : IMercuryClientDependencies
{
    /// <summary>
    /// Gets the crypto provider.
    /// </summary>
    /// <value>The crypto.</value>
    public ICryptoProvider CryptoProvider { get; } = cryptoProvider 
                                                     ?? throw new ArgumentNullException(nameof(cryptoProvider));

    /// <summary>
    /// Gets the envelope codec.
    /// </summary>
    /// <value>The envelope codec.</value>
    public IEnvelopeCodec EnvelopeCodec { get; } = ResolveCodec(envelopeCodec);


    /// <summary>
    /// Gets the transport.
    /// </summary>
    /// <value>The transport.</value>
    public ITransport Transport { get; } = transport
                                           ?? throw new ArgumentNullException(nameof(transport));

    private static IEnvelopeCodec ResolveCodec(
        EnvelopeCodec codecType)
    {
        switch (codecType)
        {
            case Abstractions.Enums.EnvelopeCodec.Binary:
                return new BinaryEnvelopeCodec(
                    EnvelopeService.Instance);

            case Abstractions.Enums.EnvelopeCodec.Json:
                return new JsonEnvelopeCodec(
                    EnvelopeService.Instance);

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(codecType),
                    codecType,
                    "Unsupported envelope codec.");
        }
    }
}