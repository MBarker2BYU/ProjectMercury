// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.Logging.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Presentation;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// Logs the specified level.
    /// </summary>
    /// <param name="level">The level.</param>
    /// <param name="entry">The entry.</param>
    private void Log(string level, string entry)
        =>AppendLog(new DemoLogEntry(level, entry));

    /// <summary>
    /// Traces the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Trace(string message)
        => Log(ToLevel(LogEntryType.Trace), message);

    /// <summary>
    /// Information the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Info(string message)
        => Log(ToLevel(LogEntryType.Information), message);

    /// <summary>
    /// Warns the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Warn(string message)
        => Log(ToLevel(LogEntryType.Warning), message);

    /// <summary>
    /// Errors the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    public void Error(string message, Exception? exception = null)
        => Log(ToLevel(LogEntryType.Error), message);

    /// <summary>
    /// Converts to level.
    /// </summary>
    /// <param name="logEntryType">Type of the log entry.</param>
    /// <returns>System.String.</returns>
    private static string ToLevel(LogEntryType logEntryType)
    {
        switch (logEntryType)
        {
            case LogEntryType.Error:
                return @"ERROR";
            case LogEntryType.Information:
                return @"INFO";
            case LogEntryType.Trace:
                return @"TRACE";
            case LogEntryType.Warning:
                return @"WARN";
                default:
                    return @"Custom";
        }
    }

    /// <summary>
    /// Appends the log.
    /// </summary>
    /// <param name="logEntry">The log entry.</param>
    public void AppendLog(DemoLogEntry logEntry)
    {
        if (m_View.InvokeRequired)
        {
            m_View.BeginInvoke(() => AppendLog(logEntry));

            return;
        }

        var eventLog = m_View.EventLog;

        var levelColor = logEntry.Level switch
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

        eventLog.SelectionStart = eventLog.TextLength;

        eventLog.SelectionLength = 0;

        eventLog.SelectionColor = MercuryTheme.MutedColor;

        eventLog.AppendText($"{logEntry.Timestamp:HH:mm:ss.fff}  ");

        eventLog.SelectionColor = levelColor;

        eventLog.AppendText($"[{logEntry.Level}] ");

        eventLog.SelectionColor = MercuryTheme.ForeColor;

        eventLog.AppendText(logEntry.Entry + Environment.NewLine);

        eventLog.SelectionStart = eventLog.TextLength;

        eventLog.ScrollToCaret();
    }

    /// <summary>
    /// Clears the event log.
    /// </summary>
    internal void ClearEventLog()
    {
        m_View.EventLog.Clear();

        Info("Event log cleared");
    }


}