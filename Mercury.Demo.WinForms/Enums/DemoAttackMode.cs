// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoAttackMode.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.WinForms.Enums;

/// <summary>
/// Enum DemoAttackMode
/// </summary>
internal enum DemoAttackMode
{
    /// <summary>
    /// The none
    /// </summary>
    None,
    /// <summary>
    /// The replay
    /// </summary>
    Replay,
    /// <summary>
    /// The tamper
    /// </summary>
    Tamper,
    /// <summary>
    /// The wrong key
    /// </summary>
    WrongKey
}