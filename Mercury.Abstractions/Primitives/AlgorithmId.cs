// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
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
/// <param name="value">The value.</param>
public readonly struct AlgorithmId(string value)
{
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
    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; } = value;

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
        => Value ?? string.Empty;

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="AlgorithmId"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator AlgorithmId(string value) 
        => new(value);
}