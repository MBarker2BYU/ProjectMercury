// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DiagnosticMenuSelection.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.Console.Enums;

/// <summary>
/// Security demonstration menu selections.
/// </summary>
internal enum DiagnosticMenuSelection
{
    /// <summary>
    /// Replay a protected TCP frame.
    /// </summary>
    Replay,
    /// <summary>
    /// Tamper with the protected payload of a TCP frame.
    /// </summary>
    Tamper,
    /// <summary>
    /// Send using the Charlie recipient key while Bravo receives.
    /// </summary>
    WrongKey,
    /// <summary>
    /// Return to the main menu.
    /// </summary>
    Return
}
