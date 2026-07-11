// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="IEnvelopeService.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Services;

/// <summary>
/// Interface IEnvelopeService
/// </summary>
public interface IEnvelopeService
{
    /// <summary>
    /// Builds the envelope header.
    /// </summary>
    /// <returns>IEnvelopeHeader.</returns>
    IEnvelopeHeader BuildEnvelopeHeader();

    /// <summary>
    /// Builds the envelope header.
    /// </summary>
    /// <param name="envelopeId">The envelope identifier.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <param name="encryption">The encryption.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="replayToken">The replay token.</param>
    /// <param name="meta">The meta.</param>
    /// <returns>IEnvelopeHeader.</returns>
    IEnvelopeHeader BuildEnvelopeHeader(KeyId envelopeId, DateTimeOffset timestamp, KeyId senderKeyId,
        KeyId recipientKeyId, AlgorithmId encryption, AlgorithmId signature, ReadOnlyMemory replayToken,
        Metadata? meta = null);

    /// <summary>
    /// Builds the envelope footer.
    /// </summary>
    /// <returns>IEnvelopeFooter.</returns>
    IEnvelopeFooter BuildEnvelopeFooter();

    /// <summary>
    /// Builds the envelope footer.
    /// </summary>
    /// <param name="meta">The meta.</param>
    /// <returns>IEnvelopeFooter.</returns>
    IEnvelopeFooter BuildEnvelopeFooter(Metadata? meta);

    /// <summary>
    /// Packs the envelope.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="footer">The footer.</param>
    /// <returns>IMercuryResult.</returns>
    IMercuryResult PackEnvelope(IEnvelopeHeader? header, ReadOnlyMemory payload, IEnvelopeFooter? footer);

    /// <summary>
    /// Unpacks the envelope.
    /// </summary>
    /// <param name="secureEnvelope">The secure envelope.</param>
    /// <returns>IMercuryResult.</returns>
    IMercuryResult UnpackEnvelope(ISecureEnvelope? secureEnvelope);
}