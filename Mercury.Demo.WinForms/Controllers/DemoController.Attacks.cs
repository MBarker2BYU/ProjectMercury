
// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.Attacks.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.Proxy;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// The m attack simulator
    /// </summary>
    private TcpAttackSimulator? m_AttackSimulator;
    /// <summary>
    /// The m wrong key enabled
    /// </summary>
    private bool m_WrongKeyEnabled;

    /// <summary>
    /// Gets a value indicating whether [replay attack enabled].
    /// </summary>
    /// <value><c>true</c> if [replay attack enabled]; otherwise, <c>false</c>.</value>
    private bool ReplayAttackEnabled =>
        m_AttackSimulator?.ReplayEnabled == true;

    /// <summary>
    /// Replays the attack changed.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    public void ReplayAttackChanged(bool enabled)
    {
        try
        {
            SetReplayAttack(enabled);

            if (enabled)
            {
                m_View.SetAttackSelection(tamper: false, replay: true, wrongKey: false);
            }
        }
        catch (Exception exception)
        {
            m_View.SetAttackSelection(tamper: false, replay: false, wrongKey: false);

            m_View.DisplayError(exception.Message);
        }
    }

    /// <summary>
    /// Tampers the attack changed.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    public void TamperAttackChanged(bool enabled)
    {
        try
        {
            SetTamperAttack(enabled);

            if (enabled)
            {
                m_View.SetAttackSelection(tamper: true, replay: false, wrongKey: false);
            }
        }
        catch (Exception exception)
        {
            m_View.SetAttackSelection(tamper: false, replay: false, wrongKey: false);

            m_View.DisplayError(exception.Message);
        }
    }

    /// <summary>
    /// Wrongs the key attack changed.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    public void WrongKeyAttackChanged(bool enabled)
    {
        try
        {
            SetWrongKeyAttack(enabled);

            if (enabled)
            {
                m_View.SetAttackSelection(tamper: false, replay: false, wrongKey: true);
            }
        }
        catch (Exception exception)
        {
            m_View.SetAttackSelection(tamper: false, replay: false, wrongKey: false);

            m_View.DisplayError(exception.Message);
        }
    }

    /// <summary>
    /// Sets the tamper attack.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <exception cref="InvalidOperationException">Tamper simulation requires the TCP transport.</exception>
    private void SetTamperAttack(bool enabled)
    {
        if (m_AttackSimulator is null)
        {
            if (enabled)
            {
                throw new InvalidOperationException(@"Tamper simulation requires the TCP transport.");
            }

            return;
        }

        m_AttackSimulator.TamperEnabled = enabled;

        if (!enabled)
            return;

        m_AttackSimulator.ReplayEnabled = false;
        m_WrongKeyEnabled = false;
    }

    /// <summary>
    /// Sets the replay attack.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <exception cref="InvalidOperationException">Replay simulation requires the TCP transport.</exception>
    private void SetReplayAttack(bool enabled)
    {
        if (m_AttackSimulator is null)
        {
            if (enabled)
            {
                throw new InvalidOperationException(@"Replay simulation requires the TCP transport.");
            }

            return;
        }

        m_AttackSimulator.ReplayEnabled = enabled;

        if (!enabled)
            return;

        m_AttackSimulator.ClearLastFrame();
        m_AttackSimulator.TamperEnabled = false;
        m_WrongKeyEnabled = false;
    }

    /// <summary>
    /// Sets the wrong key attack.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    private void SetWrongKeyAttack(bool enabled)
    {
        m_WrongKeyEnabled = enabled;

        if (!enabled || m_AttackSimulator is null)
            return;
        

        m_AttackSimulator.TamperEnabled = false;
        m_AttackSimulator.ReplayEnabled = false;
    }

    /// <summary>
    /// Clears the replay frame.
    /// </summary>
    private void ClearReplayFrame()
    {
        m_AttackSimulator?.ClearLastFrame();
    }
}