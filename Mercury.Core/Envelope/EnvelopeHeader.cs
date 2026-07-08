// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="EnvelopeHeader.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Envelope;

/// <summary>
/// Class EnvelopeHeader.
/// Implements the <see cref="IEnvelopeHeader" />
/// </summary>
/// <param name="envelopeId">The envelope identifier.</param>
/// <param name="timestamp">The timestamp.</param>
/// <param name="senderKeyId">The sender key identifier.</param>
/// <param name="recipientKeyId">The recipient key identifier.</param>
/// <param name="encryption">The encryption.</param>
/// <param name="signature">The signature.</param>
/// <param name="replayToken">The replay token.</param>
/// <param name="meta">The meta.</param>
/// <seealso cref="IEnvelopeHeader" />
public class EnvelopeHeader(KeyId envelopeId, DateTimeOffset timestamp, KeyId senderKeyId, KeyId recipientKeyId, AlgorithmId encryption, AlgorithmId signature, ReadOnlyMemory replayToken, Metadata? meta = null) : IEnvelopeHeader
{
    /// <summary>
    /// Gets the envelope identifier.
    /// </summary>
    /// <value>The envelope identifier.</value>
    public KeyId EnvelopeId { get; } = envelopeId;
    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    /// <value>The timestamp.</value>
    public DateTimeOffset Timestamp { get; } = timestamp;
    /// <summary>
    /// Gets the sender key identifier.
    /// </summary>
    /// <value>The sender key identifier.</value>
    public KeyId SenderKeyId { get; } = senderKeyId;
    /// <summary>
    /// Gets the recipient key identifier.
    /// </summary>
    /// <value>The recipient key identifier.</value>
    public KeyId RecipientKeyId { get; } = recipientKeyId;
    /// <summary>
    /// Gets the encryption.
    /// </summary>
    /// <value>The encryption.</value>
    public AlgorithmId Encryption { get; } = encryption;
    /// <summary>
    /// Gets the signature.
    /// </summary>
    /// <value>The signature.</value>
    public AlgorithmId Signature { get; } = signature;
    /// <summary>
    /// Gets the replay token.
    /// </summary>
    /// <value>The replay token.</value>
    public ReadOnlyMemory ReplayToken { get; } = replayToken;
    /// <summary>
    /// Gets the meta.
    /// </summary>
    /// <value>The meta.</value>
    public Metadata Meta { get; } = meta ?? new Metadata();
}