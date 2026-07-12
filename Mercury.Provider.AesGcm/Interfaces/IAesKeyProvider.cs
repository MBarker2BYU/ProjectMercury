// ***********************************************************************
// Assembly     : Mercury.Providers.AesGcm
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="IAesKeyProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Provider.AesGcm.Interfaces;

/// <summary>
/// Resolves symmetric AES keys by <see cref="KeyId"/>.
/// Keys should be 32 bytes for AES-GCM-256.
/// </summary>
public interface IAesKeyProvider
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
    /// A task that returns the symmetric key.
    /// </returns>
    ValueTask<ReadOnlyMemory> GetKeyAsync(KeyId keyId, CancellationToken cancellationToken = default);
}