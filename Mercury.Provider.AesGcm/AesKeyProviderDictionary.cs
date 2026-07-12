// ***********************************************************************
// Assembly       : Mercury.Providers.AesGcm
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="AesKeyProviderDictionary.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions.Primitives;
using Mercury.Provider.AesGcm.Interfaces;

namespace Mercury.Provider.AesGcm;

/// <summary>
/// Provides AES keys from an in-memory dictionary for demonstrations and tests.
/// </summary>
public sealed class AesKeyProviderDictionary : IAesKeyProvider
{
    /// <summary>
    /// The stored AES keys.
    /// </summary>
    private readonly ConcurrentDictionary<KeyId, byte[]> m_Keys;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AesKeyProviderDictionary"/> class.
    /// </summary>
    /// <param name="keys">The AES keys indexed by key identifier.</param>
    /// <exception cref="ArgumentNullException">
    /// The key dictionary is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A supplied key is null or empty.
    /// </exception>
    public AesKeyProviderDictionary(IDictionary<KeyId, byte[]> keys)
    {
        ArgumentNullException.ThrowIfNull(keys);

        m_Keys =
            new ConcurrentDictionary<KeyId, byte[]>();

        foreach (var keyPair in keys)
        {
            if (keyPair.Value == null ||
                keyPair.Value.Length == 0)
            {
                throw new ArgumentException(
                    $"The AES key for KeyId '{keyPair.Key}' cannot be null or empty.",
                    nameof(keys));
            }

            m_Keys[keyPair.Key] =
                (byte[])keyPair.Value.Clone();
        }
    }

    /// <summary>
    /// Gets the AES key associated with the specified key identifier.
    /// </summary>
    /// <param name="keyId">The key identifier.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A value task containing the AES key.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The key identifier is empty.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// No AES key exists for the specified key identifier.
    /// </exception>
    public ValueTask<ReadOnlyMemory> GetKeyAsync(KeyId keyId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (keyId.Value == KeyId.Empty.Value)
        {
            throw new ArgumentException(
                "KeyId is required.",
                nameof(keyId));
        }

        if (!m_Keys.TryGetValue(
                keyId,
                out byte[]? key))
        {
            throw new KeyNotFoundException(
                $"No AES key was found for KeyId '{keyId}'.");
        }

        var result =
            new ReadOnlyMemory((byte[])key.Clone());

        return ValueTask.FromResult(result);
    }
}