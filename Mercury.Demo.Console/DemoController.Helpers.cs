// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.Helpers">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Determines whether the specified source contains sequence.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="sequence">The sequence.</param>
    /// <returns><c>true</c> if the specified source contains sequence; otherwise, <c>false</c>.</returns>
    private static bool ContainsSequence(byte[] source, byte[] sequence)
    {
        if (sequence.Length == 0 || sequence.Length > source.Length)
            return false;

        for (var sourceIndex = 0; sourceIndex <= source.Length - sequence.Length; sourceIndex++)
        {
            var match = true;

            for (var sequenceIndex = 0; sequenceIndex < sequence.Length; sequenceIndex++)
            {
                if (source[sourceIndex + sequenceIndex] == sequence[sequenceIndex])
                    continue;

                match = false;
                break;
            }

            if (match)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the name of the platform.
    /// </summary>
    /// <returns>System.String.</returns>
    private static string GetPlatformName()
    {
        if (OperatingSystem.IsWindows())
            return "Windows";

        if (OperatingSystem.IsMacOS())
            return "macOS";

        if (OperatingSystem.IsLinux())
            return "Linux";

        return "Unknown";
    }

    /// <summary>
    /// Formats the size of the byte.
    /// </summary>
    /// <param name="sizeBytes">The size bytes.</param>
    /// <returns>System.String.</returns>
    private static string FormatByteSize(int sizeBytes)
    {
        if (sizeBytes >= 1024 * 1024)
            return $"{sizeBytes / (1024 * 1024):N0} MB";

        return $"{sizeBytes / 1024:N0} KB";
    }
}
