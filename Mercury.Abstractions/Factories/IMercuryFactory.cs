// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="IMercuryFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Transport;

namespace Mercury.Abstractions.Factories;
/// <summary>
/// Interface IMercuryFactory
/// </summary>
public interface IMercuryFactory
{
    /// <summary>
    /// Builds the dependencies.
    /// </summary>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec"></param>
    /// <param name="transport">The transport.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    IMercuryClientDependencies BuildDependencies(ICryptoProvider cryptoProvider, EnvelopeCodec envelopeCodec, ITransport transport);

    /// <summary>
    /// Builds the dependencies with a passthrough crypto provider
    /// </summary>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    IMercuryClientDependencies BuildDependencies(EnvelopeCodec envelopeCodec, ITransport transport);

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <returns>IMercuryClient.</returns>
    IMercuryClient BuildClient();

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <param name="mercuryClientDependencies"></param>
    /// <returns>IMercuryClient.</returns>
    IMercuryClient BuildClient(IMercuryClientDependencies mercuryClientDependencies);
}