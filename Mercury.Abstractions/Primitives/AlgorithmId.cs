// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="AlgorithmId.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Primitives;

/// <summary>
/// Struct AlgorithmId
/// </summary>
public readonly struct AlgorithmId : IEquatable<AlgorithmId>
{
    /// <summary>
    /// The underlying value.
    /// </summary>
    private readonly string? m_Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AlgorithmId"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public AlgorithmId(string? value)
    {
        m_Value = value;
    }

    /// <summary>
    /// Gets the none.
    /// </summary>
    /// <value>The none.</value>
    public static AlgorithmId None => new("none");

    /// <summary>
    /// Gets an empty copy.
    /// </summary>
    /// <value>The empty.</value>
    public static AlgorithmId Empty => new(string.Empty);

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
    public bool IsEmpty => string.IsNullOrWhiteSpace(m_Value);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value => m_Value ?? string.Empty;

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
        => Value;

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="AlgorithmId"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator AlgorithmId(string value)
        => new(value);

    /// <summary>
    /// Determines whether two algorithm identifiers are equal.
    /// </summary>
    /// <param name="left">The left identifier.</param>
    /// <param name="right">The right identifier.</param>
    /// <returns><c>true</c> if the identifiers are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(AlgorithmId left, AlgorithmId right)
        => left.Equals(right);

    /// <summary>
    /// Determines whether two algorithm identifiers are not equal.
    /// </summary>
    /// <param name="left">The left identifier.</param>
    /// <param name="right">The right identifier.</param>
    /// <returns><c>true</c> if the identifiers are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(AlgorithmId left, AlgorithmId right)
        => !left.Equals(right);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(AlgorithmId other)
    {
        return StringComparer.Ordinal.Equals(Value, other.Value);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is AlgorithmId other && Equals(other);
    

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
        => StringComparer.Ordinal.GetHashCode(Value);
}