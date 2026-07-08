// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="MercuryFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Detection;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Factories;
using Mercury.Abstractions.Logging;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;
using Mercury.Core.Cryptography;

namespace Mercury.Core.Factories;

/// <summary>
/// Class MercuryFactory.
/// Implements the <see cref="IMercuryFactory" />
/// </summary>
/// <seealso cref="IMercuryFactory" />
public sealed class MercuryFactory : IMercuryFactory
{

    /// <summary>
    /// Gets the sm mercury factory.
    /// </summary>
    /// <value>The sm mercury factory.</value>
    private static readonly Lazy<MercuryFactory> sm_MercuryFactory
        = new(() => new MercuryFactory());

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static MercuryFactory Instance => sm_MercuryFactory.Value;


    #region MercuryClient

    /// <summary>
    /// Builds the client dependencies.
    /// </summary>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <param name="replayProtector">The replay protector.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    public IMercuryClientDependencies BuildClientDependencies(ICryptoProvider cryptoProvider,
        IEnvelopeCodec envelopeCodec, ITransport transport, IReplayProtector replayProtector, ILogger? logger = null)
        => new MercuryClientDependencies(cryptoProvider, envelopeCodec, transport, replayProtector, logger);


    /// <summary>
    /// Builds the mercury client.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <returns>IMercuryClient.</returns>
    public IMercuryClient BuildClient(IMercuryClientDependencies dependencies)
        => new MercuryClient(dependencies);

    #endregion

    #region Cryptography

    /// <summary>
    /// Builds the crypto context.
    /// </summary>
    /// <param name="encryption">The encryption.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <returns>ICryptoContext.</returns>
    public ICryptoContext BuildCryptoContext(AlgorithmId encryption, AlgorithmId signature, KeyId senderKeyId,
        KeyId recipientKeyId)
        => new CryptoContext(encryption, signature, senderKeyId, recipientKeyId);

    #endregion

}