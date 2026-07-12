// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="IMercuryClientDependencies.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
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
    /// Gets the crypto provider.
    /// </summary>
    /// <value>The crypto.</value>
    ICryptoProvider CryptoProvider { get; }

    /// <summary>
    /// Gets the envelope codec.
    /// </summary>
    /// <value>The envelope codec.</value>
    IEnvelopeCodec EnvelopeCodec { get; }
    /// <summary>
    /// Gets the transport.
    /// </summary>
    /// <value>The transport.</value>
    ITransport Transport { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    IMercuryLogger Logger { get; }
}