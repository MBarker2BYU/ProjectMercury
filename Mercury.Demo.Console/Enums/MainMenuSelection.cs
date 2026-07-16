// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="MainMenuSelection.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.Console.Enums;

/// <summary>
/// Enum MainMenuSelection
/// </summary>
internal enum MainMenuSelection
{
    /// <summary>
    /// The run protected exchange
    /// </summary>
    RunProtectedExchange,
    /// <summary>
    /// The send protected file
    /// </summary>
    SendProtectedFile,
    /// <summary>
    /// The configure session
    /// </summary>
    ConfigureSession,
    /// <summary>
    /// The security diagnostics
    /// </summary>
    SecurityDiagnostics,
    /// <summary>
    /// The run test matrix
    /// </summary>
    RunTestMatrix,
    /// <summary>
    /// The view telemetry
    /// </summary>
    ViewTelemetry,
    /// <summary>
    /// The exit
    /// </summary>
    Exit
}
