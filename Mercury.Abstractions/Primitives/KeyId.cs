// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author         : Matthew D. Barker
// Created        : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="KeyId.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Primitives;

/// <summary>
/// Represents a cryptographic key identifier.
/// </summary>
public readonly struct KeyId : IEquatable<KeyId>
{
    /// <summary>
    /// The underlying identifier value.
    /// </summary>
    private readonly string? m_Value;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="KeyId"/> structure.
    /// </summary>
    /// <param name="value">
    /// The identifier value.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identifier contains an invalid pipe character.
    /// </exception>
    public KeyId(string? value)
    {
        if (HasPipeInValue(value))
            throw new ArgumentException("The pipe '|' is an invalid character.", nameof(value));
        
        m_Value = value;
    }

    /// <summary>
    /// Gets an empty key identifier.
    /// </summary>
    /// <value>The empty identifier.</value>
    public static KeyId Empty =>
        new(string.Empty);

    /// <summary>
    /// Gets a value indicating whether this identifier is empty.
    /// </summary>
    /// <value>
    /// <c>true</c> when the identifier is null, empty, or whitespace;
    /// otherwise, <c>false</c>.
    /// </value>
    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(m_Value);

    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    /// <value>
    /// The identifier value, or an empty string for a default instance.
    /// </value>
    public string Value =>
        m_Value ?? string.Empty;

    /// <summary>
    /// Converts a string into a key identifier.
    /// </summary>
    /// <param name="value">
    /// The identifier value.
    /// </param>
    /// <returns>The key identifier.</returns>
    public static implicit operator KeyId(string value)
    {
        return new KeyId(value);
    }

    /// <summary>
    /// Determines whether two key identifiers are equal.
    /// </summary>
    /// <param name="left">
    /// The left identifier.
    /// </param>
    /// <param name="right">
    /// The right identifier.
    /// </param>
    /// <returns>
    /// <c>true</c> when the identifiers are equal.
    /// </returns>
    public static bool operator ==(KeyId left, KeyId right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two key identifiers are different.
    /// </summary>
    /// <param name="left">
    /// The left identifier.
    /// </param>
    /// <param name="right">
    /// The right identifier.
    /// </param>
    /// <returns>
    /// <c>true</c> when the identifiers are different.
    /// </returns>
    public static bool operator !=(KeyId left, KeyId right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Determines whether this identifier equals another identifier.
    /// </summary>
    /// <param name="other">
    /// The other identifier.
    /// </param>
    /// <returns>
    /// <c>true</c> when the identifier values are equal.
    /// </returns>
    public bool Equals(KeyId other)
    {
        return StringComparer.Ordinal.Equals(Value, other.Value);
    }

    /// <summary>
    /// Determines whether this identifier equals another object.
    /// </summary>
    /// <param name="obj">
    /// The other object.
    /// </param>
    /// <returns>
    /// <c>true</c> when the object is an equal key identifier.
    /// </returns>
    public override bool Equals(object? obj)
        => obj is KeyId other && Equals(other);
    

    /// <summary>
    /// Returns the hash code for this identifier.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
        => StringComparer.Ordinal.GetHashCode(Value);
    

    /// <summary>
    /// Returns the identifier value.
    /// </summary>
    /// <returns>The identifier value.</returns>
    public override string ToString()
        => Value;
    

    /// <summary>
    /// Determines whether the value contains a pipe character.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// <c>true</c> when the value contains a pipe character;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool HasPipeInValue(string? value)
        =>!string.IsNullOrWhiteSpace(value) && value != null && value.Contains("|");
    
}