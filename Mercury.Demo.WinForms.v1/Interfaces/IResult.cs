// ***********************************************************************
// Assembly       : Mercury.Core.WinForms
// Author           : Matthew D. Barker
// Created          : 07-13-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-13-2026
// ***********************************************************************
// <copyright file="IResult.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.WinForms.v1.Interfaces;

/// <summary>
/// Interface IResult
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IResult"/> is success.
    /// </summary>
    /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
    bool Success { get; }

    /// <summary>
    /// Gets the exception.
    /// </summary>
    /// <value>The exception.</value>
    Exception? Exception { get; }
}