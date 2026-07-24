// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="ConsoleScreen.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using Terminal = System.Console;

namespace Mercury.Demo.Console.Interface;

/// <summary>
/// Class ConsoleScreen.
/// </summary>
internal static class ConsoleScreen
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    /// <value>The width.</value>
    public static int Width
    {
        get
        {
            try
            {
                return Math.Max(64, Math.Min(Terminal.WindowWidth - 1, 104));
            }
            catch
            {
                return 80;
            }
        }
    }

    /// <summary>
    /// Clears this instance.
    /// </summary>
    public static void Clear()
    {
        ConsoleTheme.Apply();
        Terminal.CursorVisible = false;
    }

    /// <summary>
    /// Displays the header.
    /// </summary>
    public static void DisplayHeader()
    {
        var width = Width;

        ConsoleTheme.WriteLine(new string('═', width), ConsoleTheme.PRIMARY);
        WriteCentered("MERCURY SECURE COMMUNICATIONS FRAMEWORK", ConsoleTheme.PRIMARY);
        WriteCentered("CROSS-PLATFORM CONSOLE DEMONSTRATION | RC 1.2", ConsoleTheme.SECONDARY);
        ConsoleTheme.WriteLine(new string('═', width), ConsoleTheme.PRIMARY);
        Terminal.WriteLine();
    }

    /// <summary>
    /// Displays the footer.
    /// </summary>
    /// <param name="instructions">The instructions.</param>
    public static void DisplayFooter(string instructions = "↑ ↓ SELECT     ENTER EXECUTE     ESC BACK")
    {
        Terminal.WriteLine();
        ConsoleTheme.WriteLine(new string('─', Width), ConsoleTheme.SECONDARY);
        WriteCentered(instructions, ConsoleTheme.MUTED);
    }

    /// <summary>
    /// Writes the section.
    /// </summary>
    /// <param name="title">The title.</param>
    public static void WriteSection(string title)
    {
        var normalizedTitle = title.ToUpperInvariant();
        var remaining = Math.Max(0, Width - normalizedTitle.Length - 4);

        ConsoleTheme.WriteLine($"┌─ {normalizedTitle} {new string('─', remaining)}", ConsoleTheme.SECONDARY);
    }

    /// <summary>
    /// Writes the section end.
    /// </summary>
    public static void WriteSectionEnd()
        => ConsoleTheme.WriteLine($"└{new string('─', Width - 1)}", ConsoleTheme.SECONDARY);

    /// <summary>
    /// Writes the label.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="value">The value.</param>
    /// <param name="valueColor">Color of the value.</param>
    public static void WriteLabel(string label, string value, ConsoleColor valueColor = ConsoleColor.Gray)
    {
        ConsoleTheme.WriteSecondary($"  {label.ToUpperInvariant(),-20}");
        ConsoleTheme.WriteLine(value, valueColor);
    }

    /// <summary>
    /// Writes the status.
    /// </summary>
    /// <param name="endpoint">The endpoint.</param>
    /// <param name="marker">The marker.</param>
    /// <param name="message">The message.</param>
    /// <param name="color">The color.</param>
    public static void WriteStatus(string endpoint, string marker, string message, ConsoleColor color)
    {
        ConsoleTheme.WriteSecondary($"  [{endpoint,-6}] ");
        ConsoleTheme.WriteLine($"[{marker,-4}] {message}", color);
    }

    /// <summary>
    /// Writes the centered.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="color">The color.</param>
    public static void WriteCentered(string value, ConsoleColor color)
    {
        if (value.Length >= Width)
        {
            ConsoleTheme.WriteLine(value[..Width], color);
            return;
        }

        var leftPadding = (Width - value.Length) / 2;
        ConsoleTheme.WriteLine($"{new string(' ', leftPadding)}{value}", color);
    }

    /// <summary>
    /// Reads the required line.
    /// </summary>
    /// <param name="prompt">The prompt.</param>
    /// <returns>System.String.</returns>
    public static string ReadRequiredLine(string prompt)
    {
        while (true)
        {
            Terminal.CursorVisible = true;
            ConsoleTheme.WritePrimary(prompt);

            var value = ReadInputLine();

            Terminal.CursorVisible = false;

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            ConsoleTheme.WriteLine("  A value is required.", ConsoleTheme.WARNING);
            Terminal.WriteLine();
        }
    }

    private static string ReadInputLine()
    {
        var input = new StringBuilder();

        while (true)
        {
            var keyInfo = Terminal.ReadKey(intercept: true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    Terminal.WriteLine();
                    return input.ToString();

                case ConsoleKey.Backspace:
                    if (input.Length == 0)
                        continue;

                    input.Length--;
                    Terminal.Write("\b \b");
                    break;

                case ConsoleKey.Escape:
                    while (input.Length > 0)
                    {
                        input.Length--;
                        Terminal.Write("\b \b");
                    }
                    break;

                default:
                    if (char.IsControl(keyInfo.KeyChar))
                        continue;

                    input.Append(keyInfo.KeyChar);
                    Terminal.Write(keyInfo.KeyChar);
                    break;
            }
        }
    }

    /// <summary>
    /// Pauses the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public static void Pause(string message = "Press any key to continue...")
    {
        Terminal.CursorVisible = false;
        Terminal.WriteLine();
        ConsoleTheme.WriteMuted(message);
        Terminal.ReadKey(true);
    }
}
