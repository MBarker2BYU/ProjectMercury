// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="DiagnosticMenuSelection.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.Console.Enums;

/// <summary>
/// Enum DiagnosticMenuSelection
/// </summary>
internal enum DiagnosticMenuSelection
{
    /// <summary>
    /// The replay last frame
    /// </summary>
    ReplayLastFrame,
    /// <summary>
    /// The tamper replay metadata
    /// </summary>
    TamperReplayMetadata,
    /// <summary>
    /// The tamper authentication tag
    /// </summary>
    TamperAuthenticationTag,
    /// <summary>
    /// The tamper protected payload
    /// </summary>
    TamperProtectedPayload,
    /// <summary>
    /// The run all
    /// </summary>
    RunAll,
    /// <summary>
    /// The return
    /// </summary>
    Return
}
