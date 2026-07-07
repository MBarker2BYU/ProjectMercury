// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
// ***********************************************************************
// <copyright file="FrameworkVersion.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Primitives;
/// <summary>
/// Struct FrameworkVersion
/// </summary>
/// <param name="major">The major.</param>
/// <param name="minor">The minor.</param>
public readonly struct FrameworkVersion(byte major, byte minor)
{
    /// <summary>
    /// The v1
    /// </summary>
    public static FrameworkVersion V1 = new(1, 0);

    /// <summary>
    /// Gets the major.
    /// </summary>
    /// <value>The major.</value>
    public byte Major { get; } = major;

    /// <summary>
    /// Gets the minor.
    /// </summary>
    /// <value>The minor.</value>
    public byte Minor { get; } = minor;

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
        => $@"{Major}.{Minor}";
}