// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="MercuryClientDependencies.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Detection;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Logging;
using Mercury.Abstractions.Transport;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClientDependencies.
/// Implements the <see cref="IMercuryClientDependencies" />
/// </summary>
/// <param name="cryptoProvider">The crypto provider.</param>
/// <param name="envelopeCodec">The envelope codec.</param>
/// <param name="transport">The transport.</param>
/// <param name="replayProtector">The replay protector.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="IMercuryClientDependencies" />
internal class MercuryClientDependencies(ICryptoProvider cryptoProvider, IEnvelopeCodec envelopeCodec, ITransport transport, IReplayProtector replayProtector, ILogger? logger) : IMercuryClientDependencies
{
    /// <summary>
    /// Gets the crypto.
    /// </summary>
    /// <value>The crypto.</value>
    public ICryptoProvider CryptoProvider { get; } = cryptoProvider;
    /// <summary>
    /// Gets the envelope codec.
    /// </summary>
    /// <value>The codec.</value>
    public IEnvelopeCodec EnvelopeCodec { get; } = envelopeCodec;
    /// <summary>
    /// Gets the transport.
    /// </summary>
    /// <value>The transport.</value>
    public ITransport Transport { get; } = transport;
    /// <summary>
    /// Gets the replay.
    /// </summary>
    /// <value>The replay.</value>
    public IReplayProtector ReplayProtector { get; } = replayProtector;
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    public ILogger? EffectiveLogger { get; } = logger;    
}