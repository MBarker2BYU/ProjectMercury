// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="IKeyMaterial.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Cryptography;

/// <summary>
/// Interface IKeyMaterial
/// </summary>
public interface IKeyMaterial
{
    /// <summary>
    /// Gets the key identifier.
    /// </summary>
    /// <value>The key identifier.</value>
    KeyId KeyId { get; }
    /// <summary>
    /// Gets the public key.
    /// </summary>
    /// <value>The public key.</value>
    ReadOnlyMemory PublicKey { get; }
    /// <summary>
    /// Gets the private key.
    /// </summary>
    /// <value>The private key.</value>
    ReadOnlyMemory PrivateKey { get; }
}