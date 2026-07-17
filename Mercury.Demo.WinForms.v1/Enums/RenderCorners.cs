// ***********************************************************************
// Assembly       : Mercury.Demo
// Author           : Matthew D. Barker
// Created          : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
//< copyright file = "RenderedCorners.cs" >
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.WinForms.v1.Enums;

/// <summary>
/// Enum RenderedCorners
/// </summary>
[Flags]
public enum RenderedCorners
{
    /// <summary>
    /// The none
    /// </summary>
    None = 0,
    /// <summary>
    /// The top left
    /// </summary>
    TopLeft = 1,
    /// <summary>
    /// The top right
    /// </summary>
    TopRight = 2,
    /// <summary>
    /// The bottom left
    /// </summary>
    BottomLeft = 4,
    /// <summary>
    /// The bottom right
    /// </summary>
    BottomRight = 8,
    /// <summary>
    /// All
    /// </summary>
    All = TopLeft | TopRight | BottomLeft | BottomRight
}