// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
// ***********************************************************************
// <copyright file="IMercuryFactory.cs">
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

namespace Mercury.Abstractions.Factories;

/// <summary>
/// Interface IMercuryFactory
/// </summary>
public interface IMercuryFactory
{
    /// <summary>
    /// Builds the client dependencies.
    /// </summary>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <param name="replayProtector">The replay protector.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    IMercuryClientDependencies BuildClientDependencies(ICryptoProvider cryptoProvider, IEnvelopeCodec envelopeCodec, ITransport transport, IReplayProtector replayProtector, ILogger? logger = null);

    /// <summary>
    /// Builds the mercury client.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <returns>IMercuryClient.</returns>
    IMercuryClient BuildClient(IMercuryClientDependencies dependencies);
}