
// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="MainWindow.Logging.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Presentation;

namespace Mercury.Demo.WinForms;

/// <summary>
/// Class MainWindow.
/// Implements the <see cref="System.Windows.Forms.Form" />
/// </summary>
/// <seealso cref="System.Windows.Forms.Form" />
public partial class MainWindow
{
    /// <summary>
    /// Appends the log.
    /// </summary>
    /// <param name="logEntry">The log entry.</param>
    internal void AppendLog(DemoLogEntry logEntry)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendLog(logEntry));

            return;
        }

        var levelColor =
            logEntry.Level switch
            {
                "ERROR" =>
                    MercuryTheme.FailureColor,

                "WARN" =>
                    MercuryTheme.WarningColor,

                "TRACE" =>
                    MercuryTheme.MutedColor,

                _ =>
                    MercuryTheme.SuccessColor
            };

        rtbEventLog.SelectionStart = rtbEventLog.TextLength;

        rtbEventLog.SelectionLength = 0;

        rtbEventLog.SelectionColor = MercuryTheme.MutedColor;

        rtbEventLog.AppendText($"{logEntry.Timestamp:HH:mm:ss.fff}  ");

        rtbEventLog.SelectionColor = levelColor;

        rtbEventLog.AppendText($"[{logEntry.Level}] ");

        rtbEventLog.SelectionColor = MercuryTheme.ForeColor;

        rtbEventLog.AppendText(logEntry.Entry + Environment.NewLine);

        rtbEventLog.SelectionStart = rtbEventLog.TextLength;

        rtbEventLog.ScrollToCaret();
    }

    /// <summary>
    /// Clears the event log.
    /// </summary>
    internal void ClearEventLog()
    {
        rtbEventLog.Clear();
    }
}