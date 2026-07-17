// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="KeyId.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Primitives;

/// <summary>
/// Struct KeyId
/// </summary>
/// <param name="value">The value.</param>
public readonly struct KeyId(string value) : IEquatable<KeyId>
{
    /// <summary>
    /// Gets an empty copy.
    /// </summary>
    /// <value>The empty.</value>
    public static KeyId Empty => new(string.Empty);

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; } = !HasPipeInValue(value) ? 
        value : throw new ArgumentException("The pipe '|' is an invalid character.");

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
        => Value ?? string.Empty;

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="KeyId"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator KeyId(string value) 
        => new(value);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(KeyId other)
    {
        return Value == other.Value;
    }
    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is KeyId other && Equals(other);
    }
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
        => (Value ?? string.Empty).GetHashCode();


    /// <summary>
    /// Determines whether [has pipe in value] [the specified value].
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if [has pipe in value] [the specified value]; otherwise, <c>false</c>.</returns>
    public static bool HasPipeInValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return value != null && value.Contains("|");
    }
}

