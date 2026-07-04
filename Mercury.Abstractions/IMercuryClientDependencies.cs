// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-02-2026
// ***********************************************************************
// <copyright file="IMercuryClientDependencies.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Detection;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Logging;
using Mercury.Abstractions.Transport;

namespace Mercury.Abstractions;

/// <summary>
/// Interface IMercuryClientDependencies
/// </summary>
public interface IMercuryClientDependencies
{
    /// <summary>
    /// Gets the crypto.
    /// </summary>
    /// <value>The crypto.</value>
    ICryptoProvider Crypto { get; }
    /// <summary>
    /// Gets the codec.
    /// </summary>
    /// <value>The codec.</value>
    IEnvelopeCodec Codec { get; }
    /// <summary>
    /// Gets the transport.
    /// </summary>
    /// <value>The transport.</value>
    ITransport Transport { get; }
    /// <summary>
    /// Gets the replay.
    /// </summary>
    /// <value>The replay.</value>
    IReplayProtector Replay { get; }
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    ILogger? EffectiveLogger { get; }
}