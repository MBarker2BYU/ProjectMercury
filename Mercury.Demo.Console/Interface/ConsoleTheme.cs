// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="ConsoleTheme.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Terminal = System.Console;

namespace Mercury.Demo.Console.Interface;

/// <summary>
/// Class ConsoleTheme.
/// </summary>
internal static class ConsoleTheme
{
    /// <summary>
    /// The background
    /// </summary>
    public const ConsoleColor BACKGROUND = ConsoleColor.Black;
    /// <summary>
    /// The primary
    /// </summary>
    public const ConsoleColor PRIMARY = ConsoleColor.Cyan;
    /// <summary>
    /// The secondary
    /// </summary>
    public const ConsoleColor SECONDARY = ConsoleColor.DarkCyan;
    /// <summary>
    /// The normal
    /// </summary>
    public const ConsoleColor NORMAL = ConsoleColor.Gray;
    /// <summary>
    /// The muted
    /// </summary>
    public const ConsoleColor MUTED = ConsoleColor.DarkGray;
    /// <summary>
    /// The highlight background
    /// </summary>
    public const ConsoleColor HIGHLIGHT_BACKGROUND = ConsoleColor.DarkCyan;
    /// <summary>
    /// The highlight foreground
    /// </summary>
    public const ConsoleColor HIGHLIGHT_FOREGROUND = ConsoleColor.White;
    /// <summary>
    /// The success
    /// </summary>
    public const ConsoleColor SUCCESS = ConsoleColor.Green;
    /// <summary>
    /// The warning
    /// </summary>
    public const ConsoleColor WARNING = ConsoleColor.Yellow;
    /// <summary>
    /// The failure
    /// </summary>
    public const ConsoleColor FAILURE = ConsoleColor.Red;

    /// <summary>
    /// Applies this instance.
    /// </summary>
    public static void Apply()
    {
        Terminal.BackgroundColor = BACKGROUND;
        Terminal.ForegroundColor = NORMAL;
        Terminal.Clear();
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public static void Reset()
    {
        Terminal.ResetColor();
        Terminal.CursorVisible = true;
    }

    /// <summary>
    /// Writes the primary.
    /// </summary>
    /// <param name="value">The value.</param>
    public static void WritePrimary(string value)
        => Write(value, PRIMARY);

    /// <summary>
    /// Writes the secondary.
    /// </summary>
    /// <param name="value">The value.</param>
    public static void WriteSecondary(string value)
        => Write(value, SECONDARY);

    /// <summary>
    /// Writes the muted.
    /// </summary>
    /// <param name="value">The value.</param>
    public static void WriteMuted(string value)
        => Write(value, MUTED);

    /// <summary>
    /// Writes the success.
    /// </summary>
    /// <param name="value">The value.</param>
    public static void WriteSuccess(string value)
        => Write(value, SUCCESS);

    /// <summary>
    /// Writes the warning.
    /// </summary>
    /// <param name="value">The value.</param>
    public static void WriteWarning(string value)
        => Write(value, WARNING);

    /// <summary>
    /// Writes the failure.
    /// </summary>
    /// <param name="value">The value.</param>
    public static void WriteFailure(string value)
        => Write(value, FAILURE);

    /// <summary>
    /// Writes the line.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="foregroundColor">Color of the foreground.</param>
    public static void WriteLine(
        string value = "",
        ConsoleColor? foregroundColor = null)
    {
        if (foregroundColor.HasValue)
            Terminal.ForegroundColor = foregroundColor.Value;

        Terminal.WriteLine(value);

        Terminal.ForegroundColor = NORMAL;
        Terminal.BackgroundColor = BACKGROUND;
    }

    /// <summary>
    /// Writes the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="foregroundColor">Color of the foreground.</param>
    private static void Write(
        string value,
        ConsoleColor foregroundColor)
    {
        Terminal.ForegroundColor = foregroundColor;
        Terminal.Write(value);
        Terminal.ForegroundColor = NORMAL;
    }
}
