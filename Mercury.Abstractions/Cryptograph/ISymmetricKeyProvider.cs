// ***********************************************************************
// Assembly     : Mercury.Abstractions
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="ISymmetricKeyProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Cryptograph;

/// <summary>
/// Resolves symmetric cryptographic keys by <see cref="KeyId"/>.
/// </summary>
public interface ISymmetricKeyProvider
{
    /// <summary>
    /// Gets the symmetric key associated with the specified key identifier.
    /// </summary>
    /// <param name="keyId">The key identifier.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A task containing the symmetric key.
    /// </returns>
    Task<ReadOnlyMemory> GetKeyAsync(KeyId keyId, CancellationToken cancellationToken = default);
}