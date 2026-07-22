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
    {
        m_View.AppendLog(new DemoLogEntry(level, entry));
    }

    
    public void Trace(string message)
        => Log(ToLevel(LogEntryType.Trace), message);


    public void Info(string message)
        => Log(ToLevel(LogEntryType.Information), message);

   
    public void Warn(string message)
        => Log(ToLevel(LogEntryType.Warning), message);

    
    public void Error(string message, Exception? exception = null)
        => Log(ToLevel(LogEntryType.Error), message);

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

}