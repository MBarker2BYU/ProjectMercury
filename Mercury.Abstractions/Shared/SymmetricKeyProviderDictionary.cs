// ***********************************************************************
// Assembly     : Mercury.Abstractions
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="SymmetricKeyProviderDictionary.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Shared;

/// <summary>
/// Provides symmetric cryptographic keys from an in-memory dictionary.
/// Intended for demonstrations, testing, and development.
/// </summary>
public sealed class SymmetricKeyProviderDictionary : ISymmetricKeyProvider
{
    /// <summary>
    /// The symmetric keys.
    /// </summary>
    private readonly ConcurrentDictionary<KeyId, byte[]> m_Keys;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SymmetricKeyProviderDictionary"/> class.
    /// </summary>
    /// <param name="keys">
    /// The symmetric keys indexed by <see cref="KeyId"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The key collection is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A supplied key is null or empty.
    /// </exception>
    public SymmetricKeyProviderDictionary(IDictionary<KeyId, byte[]> keys)
    {
        if (keys == null)
        {
            throw new ArgumentNullException(nameof(keys));
        }

        m_Keys =
            new ConcurrentDictionary<KeyId, byte[]>();

        foreach (var keyPair in keys)
        {
            if (keyPair.Value == null ||
                keyPair.Value.Length == 0)
            {
                throw new ArgumentException($"The symmetric key for KeyId '{keyPair.Key}' cannot be null or empty.", nameof(keys));
            }

            m_Keys[keyPair.Key] =
                (byte[])keyPair.Value.Clone();
        }
    }

    /// <summary>
    /// Gets the symmetric key associated with the specified key identifier.
    /// </summary>
    /// <param name="keyId">
    /// The key identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the symmetric key.
    /// </returns>
    public Task<ReadOnlyMemory> GetKeyAsync(KeyId keyId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (keyId.IsEmpty)
        {
            throw new ArgumentException(
                "KeyId is required.",
                nameof(keyId));
        }

        if (!m_Keys.TryGetValue(keyId, out var key))
        {
            throw new KeyNotFoundException($"No symmetric key was found for KeyId '{keyId}'.");
        }

        var result =
            new ReadOnlyMemory(
                (byte[])key.Clone());

        return Task.FromResult(result);
    }
}