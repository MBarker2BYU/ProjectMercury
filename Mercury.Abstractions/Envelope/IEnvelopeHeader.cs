// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="IEnvelopeHeader.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Envelope;

/// <summary>
/// Interface IEnvelopeHeader
/// </summary>
public interface IEnvelopeHeader
{
    /// <summary>
    /// Gets the envelope identifier.
    /// </summary>
    /// <value>The envelope identifier.</value>
    KeyId EnvelopeId { get; }
    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    /// <value>The timestamp.</value>
    DateTimeOffset Timestamp { get; }

    /// <summary>
    /// Gets the sender key identifier.
    /// </summary>
    /// <value>The sender key identifier.</value>
    KeyId SenderKeyId { get; }
    /// <summary>
    /// Gets the recipient key identifier.
    /// </summary>
    /// <value>The recipient key identifier.</value>
    KeyId RecipientKeyId { get; }

    /// <summary>
    /// Gets the encryption.
    /// </summary>
    /// <value>The encryption.</value>
    AlgorithmId Encryption { get; }
    /// <summary>
    /// Gets the signature.
    /// </summary>
    /// <value>The signature.</value>
    AlgorithmId Signature { get; }

    /// <summary>
    /// Gets the replay token.
    /// </summary>
    /// <value>The replay token.</value>
    ReadOnlyMemory ReplayToken { get; }

    /// <summary>
    /// Gets the meta.
    /// </summary>
    /// <value>The meta.</value>
    Metadata Meta { get; }
}