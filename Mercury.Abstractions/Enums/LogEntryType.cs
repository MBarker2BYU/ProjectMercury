// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="LogEntryType.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Enums;

/// <summary>
/// Enum LogEntryType
/// </summary>
public enum LogEntryType
{
    /// <summary>
    /// The error
    /// </summary>
    Error,
    /// <summary>
    /// The information
    /// </summary>
    Information,
    /// <summary>
    /// The trace
    /// </summary>
    Trace,
    /// <summary>
    /// The warning
    /// </summary>
    Warning
}