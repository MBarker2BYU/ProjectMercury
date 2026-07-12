// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class CryptoContext. This class cannot be inherited.
/// Implements the <see cref="ICryptoContext" />
/// </summary>
/// <param name="senderKeyId">The sender key identifier.</param>
/// <param name="recipientKeyId">The recipient key identifier.</param>
/// <seealso cref="ICryptoContext" />
internal sealed class CryptoContext(KeyId senderKeyId, KeyId recipientKeyId) : ICryptoContext
{
    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
    public bool IsEmpty => SenderKeyId.IsEmpty && RecipientKeyId.IsEmpty;

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