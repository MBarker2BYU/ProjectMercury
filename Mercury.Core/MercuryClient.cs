// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
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
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClient.
/// Implements the <see cref="IMercuryClient" />
/// </summary>
/// <param name="cryptoProvider">The crypto provider.</param>
/// <param name="envelopeCodec">The envelope codec.</param>
/// <param name="transport">The transport.</param>
/// <param name="replayProtector">The replay protector.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="IMercuryClient" />
internal class MercuryClient(ICryptoProvider cryptoProvider, IEnvelopeCodec envelopeCodec, 
    ITransport transport, IReplayProtector replayProtector, ILogger? logger = null) : IMercuryClient
{

    /// <summary>
    /// Initializes a new instance of the <see cref="MercuryClient"/> class.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    internal MercuryClient(IMercuryClientDependencies dependencies)
        :this(dependencies.CryptoProvider, dependencies.EnvelopeCodec, dependencies.Transport, 
            dependencies.ReplayProtector, dependencies.EffectiveLogger)
    {}

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="cryptoContext">The crypto context.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SendAsync(ICryptoContext cryptoContext, ReadOnlyMemory payload, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IMercuryResult&gt;.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IMercuryResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}