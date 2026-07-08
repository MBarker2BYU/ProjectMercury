// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
// ***********************************************************************
// <copyright file="CryptoContext.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class CryptoContext. This class cannot be inherited.
/// Implements the <see cref="ICryptoContext" />
/// </summary>
/// <param name="encryption">The encryption.</param>
/// <param name="signature">The signature.</param>
/// <param name="senderKeyId">The sender key identifier.</param>
/// <param name="recipientKeyId">The recipient key identifier.</param>
/// <seealso cref="ICryptoContext" />
public sealed class CryptoContext(AlgorithmId encryption, AlgorithmId signature, KeyId senderKeyId, KeyId recipientKeyId) : ICryptoContext
{
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
    /// Gets the sender key identifier.
    /// </summary>
    /// <value>The sender key identifier.</value>
    public KeyId SenderKeyId { get; } = senderKeyId;
    /// <summary>
    /// Gets the recipient key identifier.
    /// </summary>
    /// <value>The recipient key identifier.</value>
    public KeyId RecipientKeyId { get; } = recipientKeyId;
}