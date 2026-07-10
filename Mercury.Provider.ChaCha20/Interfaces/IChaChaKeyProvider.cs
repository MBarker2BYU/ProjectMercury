// ***********************************************************************
// Assembly       : Mercury.Provider.ChaCha20
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="IChaChaKeyProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Provider.ChaCha20.Interfaces;

/// <summary>
/// Interface IChaChaKeyProvider
/// </summary>
public interface IChaChaKeyProvider
{
    /// <summary>
    /// Gets the key asynchronous.
    /// </summary>
    /// <param name="keyId">The key identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    Task<ReadOnlyMemory> GetKeyAsync(KeyId keyId, CancellationToken cancellationToken = default);
}