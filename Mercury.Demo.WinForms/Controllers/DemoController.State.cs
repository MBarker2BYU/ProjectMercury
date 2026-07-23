// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.State.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.Enums;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// Gets the current attack mode.
    /// </summary>
    /// <value>The current attack mode.</value>
    internal DemoAttackMode CurrentAttackMode
    {
        get
        {
            if (m_WrongKeyEnabled)
                return DemoAttackMode.WrongKey;

            if (m_AttackSimulator?.TamperEnabled == true)
                return DemoAttackMode.Tamper;

            if (m_AttackSimulator?.ReplayEnabled == true)
                return DemoAttackMode.Replay;

            return DemoAttackMode.None;
        }
    }
}