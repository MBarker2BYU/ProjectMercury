// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-03-2026
// ***********************************************************************
// <copyright file="ReadOnlyMemory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Primitives;

/// <summary>
/// Immutable wrapper around byte[] for payloads.
/// </summary>
public readonly struct ReadOnlyMemory(byte[]? data)
{
    /// <summary>
    /// Gets the empty.
    /// </summary>
    /// <value>The empty.</value>
    public static ReadOnlyMemory Empty => new(null);

    /// <summary>
    /// The data
    /// </summary>
    private readonly byte[]? m_Data = data != null ? (byte[])data.Clone() : [];
    
    /// <summary>
    /// Converts to array.
    /// </summary>
    /// <returns>System.Byte[].</returns>
    public byte[] ToArray()
    {
        var data = m_Data;

        return data is { Length: > 0 }
            ? (byte[])data.Clone()
            : [];
    }

    /// <summary>
    /// Clones this instance.
    /// </summary>
    /// <returns>System.Byte[].</returns>
    public ReadOnlyMemory Clone()
            => new ReadOnlyMemory(ToArray());

    /// <summary>
    /// Slices the specified start.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="length">The length.</param>
    /// <returns>Mercury.Abstractions.Primitives.ReadOnlyMemory.</returns>
    public ReadOnlyMemory Slice(int start, int length)
    {
        if (start < 0 || length < 0 || start + length > Length)
            throw new ArgumentOutOfRangeException();

        var slice = new byte[length];
        if (m_Data != null) 
            Array.Copy(m_Data, start, slice, 0, length);

        return new ReadOnlyMemory(slice);
    }

    /// <summary>
    /// Gets the length.
    /// </summary>
    /// <value>The length.</value>
    public int Length => m_Data?.Length ?? 0;

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
    public bool IsEmpty => Length == 0;

    /// <summary>
    /// Performs an implicit conversion from <see>
    ///     <cref>System.Byte[]</cref>
    /// </see>
    /// to <see cref="Mercury.Abstractions.Primitives.ReadOnlyMemory" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator ReadOnlyMemory(byte[] value) => new(value);
}