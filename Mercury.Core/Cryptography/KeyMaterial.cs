// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="KeyMaterial.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class KeyMaterial.
/// Implements the <see cref="IKeyMaterial" />
/// </summary>
/// <param name="keyId">The key identifier.</param>
/// <param name="publicKey">The public key.</param>
/// <param name="privateKey">The private key.</param>
/// <seealso cref="IKeyMaterial" />
public class KeyMaterial(KeyId keyId, ReadOnlyMemory publicKey, ReadOnlyMemory privateKey) : IKeyMaterial
{
    /// <summary>
    /// Gets the key identifier.
    /// </summary>
    /// <value>The key identifier.</value>
    public KeyId KeyId { get; } = keyId;
    /// <summary>
    /// Gets the public key.
    /// </summary>
    /// <value>The public key.</value>
    public ReadOnlyMemory PublicKey { get; } = publicKey;
    /// <summary>
    /// Gets the private key.
    /// </summary>
    /// <value>The private key.</value>
    public ReadOnlyMemory PrivateKey { get; } = privateKey;    
}