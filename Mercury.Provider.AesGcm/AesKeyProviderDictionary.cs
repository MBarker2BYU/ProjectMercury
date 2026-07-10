// ***********************************************************************
// Assembly       : Mercury.Provider.AesGcm
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="AesKeyProviderDictionary.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Provider.AesGcm.Interfaces;
using System.Collections.Concurrent;

namespace Mercury.Provider.AesGcm;

/// <summary>
/// Simple in-memory key provider for demos/tests.
/// </summary>
public sealed class AesKeyProviderDictionary : IAesKeyProvider
{
    /// <summary>
    /// The keys
    /// </summary>
    private readonly ConcurrentDictionary<string, byte[]> m_Keys = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="AesKeyProviderDictionary"/> class.
    /// </summary>
    /// <param name="keys">The keys.</param>
    public AesKeyProviderDictionary(IDictionary<string, byte[]> keys)
    {
        foreach (var kvp in keys)
            m_Keys[kvp.Key] = kvp.Value;
    }

    /// <summary>
    /// Gets the key asynchronous.
    /// </summary>
    /// <param name="keyId">The key identifier.</param>
    /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    /// <exception cref="System.ArgumentException">KeyId is required. - keyId</exception>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">No AES key found for KeyId '{keyId}'.</exception>
    public Task<ReadOnlyMemory> GetKeyAsync(KeyId keyId, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(keyId.Value))
            throw new ArgumentException("KeyId is required.", nameof(keyId));

        if (!m_Keys.TryGetValue(keyId.Value, out var key))
            throw new KeyNotFoundException($"No AES key found for KeyId '{keyId}'.");

        return Task.FromResult(new ReadOnlyMemory(key));
    }
}