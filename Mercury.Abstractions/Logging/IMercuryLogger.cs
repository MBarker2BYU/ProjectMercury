// ***********************************************************************
// Assembly     : Mercury.Abstractions
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="IMercuryLogger.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;

namespace Mercury.Abstractions.Logging;

/// <summary>
/// Provides logging for Mercury operations.
/// </summary>
public interface IMercuryLogger
{
    /// <summary>
    /// Writes a trace message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Trace(string message);

    /// <summary>
    /// Writes an informational message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Info(string message);

    /// <summary>
    /// Writes a warning message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Warn(string message);

    /// <summary>
    /// Writes an error message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void Error(string message, Exception? exception = null);
    
    /// <summary>
    /// Logs the entry.
    /// </summary>
    /// <param name="logEntryType">Type of the log entry.</param>
    /// <param name="entry">The entry.</param>
    /// <param name="exception">The exception.</param>
    void LogEntry(LogEntryType logEntryType, string entry, Exception? exception = null);
}