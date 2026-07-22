// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.View.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// Clears the event log.
    /// </summary>
    public void ClearEventLog()
    {
        m_View.ClearEventLog();

        Log("INFO", "Event log cleared");
    }

    /// <summary>
    /// Chunking the changed.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    public void ChunkingChanged(bool enabled)
    {
        m_View.SetChunkingControlsEnabled(enabled);
    }
}