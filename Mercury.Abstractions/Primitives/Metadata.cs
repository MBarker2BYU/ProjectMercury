// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
// ***********************************************************************
// <copyright file="Metadata.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections;

namespace Mercury.Abstractions.Primitives;
/// <summary>
/// Class Metadata. This class cannot be inherited.
/// Implements the <see cref="string" />
/// </summary>
/// <seealso cref="string" />
public sealed class Metadata : IReadOnlyDictionary<string, string>
{
    /// <summary>
    /// Gets the empty.
    /// </summary>
    /// <value>The empty.</value>
    public static Metadata Empty => new Metadata();

    /// <summary>
    /// The m metadata
    /// </summary>
    private readonly Dictionary<string, string> m_Metadata;

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    public Metadata()
    {
        m_Metadata = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    public Metadata(IReadOnlyDictionary<string, string>? source)
    {
        m_Metadata = new Dictionary<string, string>(StringComparer.Ordinal);

        if (source == null) return;

        foreach (var kvp in source)
        {
            if (string.IsNullOrWhiteSpace(kvp.Key))
                continue;

            m_Metadata[kvp.Key] = kvp.Value ?? string.Empty;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => m_Metadata.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Supports collection initializer syntax: new Metadata { ["key"] = "value" }
    /// </summary>
    public void Add(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        m_Metadata[key] = value ?? string.Empty;
    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    /// <value>The count.</value>
    public int Count => m_Metadata.Count;

    /// <summary>
    /// Determines whether the read-only dictionary contains an element that has the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <returns>true if the read-only dictionary contains an element that has the specified key; otherwise, false.</returns>
    public bool ContainsKey(string key) => m_Metadata.ContainsKey(key);

    /// <summary>
    /// Gets the value that is associated with the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
    /// <returns>true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"></see> interface contains an element that has the specified key; otherwise, false.</returns>
    public bool TryGetValue(string key, out string value)
    {
        if (m_Metadata.TryGetValue(key, out value))
        {
            return true;
        }

        value = string.Empty;
        return false;
    }

    /// <summary>
    /// Gets the <see cref="System.String"/> with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>System.String.</returns>
    public string this[string key]
        => m_Metadata.TryGetValue(key, out var value) ? value : string.Empty;

    /// <summary>
    /// Gets an enumerable collection that contains the keys in the read-only dictionary.
    /// </summary>
    /// <value>The keys.</value>
    public IEnumerable<string> Keys => m_Metadata.Keys;
    /// <summary>
    /// Gets an enumerable collection that contains the values in the read-only dictionary.
    /// </summary>
    /// <value>The values.</value>
    public IEnumerable<string> Values => m_Metadata.Values;
}