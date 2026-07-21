// ***********************************************************************
// Assembly     : Mercury.Extensions.EasySetup
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="EphemeralKeyProviderFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using System.Security.Cryptography;

namespace Mercury.Extensions.EasySetup;

/// <summary>
/// Builds temporary in-memory symmetric key providers for simple Mercury
/// demonstrations, tests, and temporary communication sessions.
/// </summary>
/// <remarks>
/// Keys created by this factory are not persisted. Applications requiring
/// external provisioning, rotation, vault integration, or long-term key
/// storage should use the standard Mercury key-provider configuration path.
/// </remarks>
public static class EphemeralKeyProviderFactory
{
    /// <summary>
    /// The default symmetric key size in bytes.
    /// </summary>
    private const int SYMMETRIC_KEY_SIZE = 32;

    /// <summary>
    /// Creates a symmetric key provider containing one cryptographically
    /// random 256-bit key for each supplied key name.
    /// </summary>
    /// <param name="keyNames">
    /// The logical names used to identify the generated keys.
    /// </param>
    /// <returns>
    /// A symmetric key provider containing the generated keys.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="keyNames"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when no key names are supplied, a key name is empty,
    /// or duplicate key names are supplied.
    /// </exception>
    public static SymmetricKeyProviderDictionary Create(params string[] keyNames)
    {
        if (keyNames == null)
            throw new ArgumentNullException(nameof(keyNames));

        if (keyNames.Length == 0)
        {
            throw new ArgumentException("At least one key name is required.", nameof(keyNames));
        }

        ValidateKeyNames(keyNames);

        var keys = new Dictionary<KeyId, byte[]>(keyNames.Length);

        try
        {
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                foreach (var keyName in keyNames)
                {
                    var key = new byte[SYMMETRIC_KEY_SIZE];

                    randomNumberGenerator.GetBytes(key);

                    keys.Add(new KeyId(keyName), key);
                }
            }

            return new SymmetricKeyProviderDictionary(keys);
        }
        catch
        {
            ClearGeneratedKeys(keys);
            throw;
        }
    }

    /// <summary>
    /// Validates the supplied key names.
    /// </summary>
    /// <param name="keyNames">The key names.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when a key name is invalid or duplicated.
    /// </exception>
    private static void ValidateKeyNames(IEnumerable<string> keyNames)
    {
        var uniqueKeyNames =
            new HashSet<string>(StringComparer.Ordinal);

        foreach (var keyName in keyNames)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException("Key names cannot be null, empty, or whitespace.", nameof(keyNames));
            }

            if (!uniqueKeyNames.Add(keyName))
            {
                throw new ArgumentException($"The key name '{keyName}' was supplied more than once.", nameof(keyNames));
            }
        }
    }

    /// <summary>
    /// Clears generated key arrays when provider construction fails.
    /// </summary>
    /// <param name="keys">The generated keys.</param>
    private static void ClearGeneratedKeys(IDictionary<KeyId, byte[]> keys)
    {
        foreach (var key in keys.Values)
        {
            if (key != null)
            {
                Array.Clear(key, 0, key.Length);
            }
        }
    }
}