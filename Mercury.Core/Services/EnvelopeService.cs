// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="EnvelopeService.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using Mercury.Core.Envelope;

namespace Mercury.Core.Services;

/// <summary>
/// Class EnvelopeService. This class cannot be inherited.
/// Implements the <see cref="IEnvelopeService" />
/// </summary>
/// <seealso cref="IEnvelopeService" />
public sealed class EnvelopeService : IEnvelopeService
{
    /// <summary>
    /// The static Envelope Service
    /// </summary>
    private static readonly Lazy<EnvelopeService> sm_EnvelopeService = new(() => new EnvelopeService());

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static EnvelopeService Instance => sm_EnvelopeService.Value;

    

    /// <summary>
    /// Builds the envelope header.
    /// </summary>
    /// <returns>IEnvelopeHeader.</returns>
    public IEnvelopeHeader BuildEnvelopeHeader()
        => EnvelopeHeader.Empty;

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
    public IEnvelopeHeader BuildEnvelopeHeader(KeyId envelopeId, DateTimeOffset timestamp, KeyId senderKeyId,
        KeyId recipientKeyId,
        AlgorithmId encryption, AlgorithmId signature, ReadOnlyMemory replayToken, Metadata? meta = null)
        => new EnvelopeHeader(envelopeId, timestamp, senderKeyId, recipientKeyId, encryption, signature, replayToken, meta);

    /// <summary>
    /// Builds the envelope footer.
    /// </summary>
    /// <returns>IEnvelopeFooter.</returns>
    public IEnvelopeFooter BuildEnvelopeFooter()
        => EnvelopeFooter.Empty;

    /// <summary>
    /// Builds the envelope footer.
    /// </summary>
    /// <param name="meta">The meta.</param>
    /// <returns>IEnvelopeFooter.</returns>
    public IEnvelopeFooter BuildEnvelopeFooter(Metadata? meta)
        => new EnvelopeFooter(meta);

    /// <summary>
    /// Packs the envelope.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="footer">The footer.</param>
    /// <returns>IMercuryResult.</returns>
    public IMercuryResult PackEnvelope(IEnvelopeHeader? header, ReadOnlyMemory payload, IEnvelopeFooter? footer)
    {
        if (header == null)
        {
            return new MercuryResult(false, ReadOnlyMemory.Empty,
                null, FailureReason.Custom,
                "The envelope header cannot be null.");
        }

        if (payload.IsEmpty)
        {
            return new MercuryResult(false, ReadOnlyMemory.Empty, 
                null, FailureReason.Custom,
                "The envelope payload cannot be empty.");
        }

        if (footer == null)
        {
            return new MercuryResult(false, ReadOnlyMemory.Empty,
                null, FailureReason.Custom,
                "The envelope footer cannot be null.");
        }

        ISecureEnvelope secureEnvelope =
            new SecureEnvelope(FrameworkVersion.V1, header, payload, footer);

        return new MercuryResult(true,
            payload, secureEnvelope, FailureReason.None);
    }

    /// <summary>
    /// Unpacks the envelope.
    /// </summary>
    /// <param name="secureEnvelope">The secure envelope.</param>
    /// <returns>IMercuryResult.</returns>
    public IMercuryResult UnpackEnvelope(ISecureEnvelope? secureEnvelope)
    {
        if (secureEnvelope == null)
        {
            return new MercuryResult(false, ReadOnlyMemory.Empty,
                null, FailureReason.Custom,
                "The secure envelope cannot be null.");
        }
        
        if (secureEnvelope.Payload.IsEmpty)
        {
            return new MercuryResult(false, ReadOnlyMemory.Empty,
                secureEnvelope, FailureReason.Custom,
                "The secure envelope payload cannot be empty.");
        }
        
        return new MercuryResult(true, secureEnvelope.Payload,
            secureEnvelope, FailureReason.None);
    }
}