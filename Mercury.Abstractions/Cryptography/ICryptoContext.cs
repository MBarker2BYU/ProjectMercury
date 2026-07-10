// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="ICryptoContext.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Cryptography;

/// <summary>
/// Interface ICryptoContext
/// </summary>
public interface ICryptoContext
{
    /// <summary>
    /// Gets the encryption.
    /// </summary>
    /// <value>The encryption.</value>
    public AlgorithmId Encryption { get; } 
    /// <summary>
    /// Gets the signature.
    /// </summary>
    /// <value>The signature.</value>
    public AlgorithmId Signature { get; }
    /// <summary>
    /// Gets the sender key identifier.
    /// </summary>
    /// <value>The sender key identifier.</value>
    public KeyId SenderKeyId { get; }
    /// <summary>
    /// Gets the recipient key identifier.
    /// </summary>
    /// <value>The recipient key identifier.</value>
    public KeyId RecipientKeyId { get; }
}