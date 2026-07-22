// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="NoOpMercuryLogger.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;

namespace Mercury.Abstractions.Logging;

/// <summary>
/// Class NoOpMercuryLogger. This class cannot be inherited.
/// Implements the <see cref="Mercury.Abstractions.Logging.IMercuryLogger" />
/// </summary>
/// <seealso cref="Mercury.Abstractions.Logging.IMercuryLogger" />
public sealed class NoOpMercuryLogger : IMercuryLogger
{
    /// <summary>
    /// The sm no op mercury logger
    /// </summary>
    private static readonly Lazy<NoOpMercuryLogger> sm_NoOpMercuryLogger = new(() => new NoOpMercuryLogger());

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static IMercuryLogger Instance => sm_NoOpMercuryLogger.Value;

    /// <inheritdoc />
    public void Trace(string message) 
        => LogEntry(LogEntryType.Trace, message);
    

    /// <inheritdoc />
    public void Info(string message)
        => LogEntry(LogEntryType.Information, message);

    /// <inheritdoc />
    public void Warn(string message)
        => LogEntry(LogEntryType.Warning, message);

    /// <inheritdoc />
    public void Error(string message, Exception? exception = null)
        => LogEntry(LogEntryType.Error, message);

    /// <inheritdoc />
    public void LogEntry(LogEntryType logEntryType, string entry, Exception? exception = null)
    { }
}