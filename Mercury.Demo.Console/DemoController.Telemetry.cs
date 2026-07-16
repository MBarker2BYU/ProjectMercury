// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.Telemetry">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Demo.Console.Interface;
using Mercury.Demo.Console.Models;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Displays the telemetry.
    /// </summary>
    private void DisplayTelemetry()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Telemetry");

        if (m_TelemetryStore.Entries.Count == 0)
        {
            ConsoleTheme.WriteLine("  No telemetry has been recorded.", ConsoleTheme.WARNING);
        }
        else
        {
            foreach (var entry in m_TelemetryStore.Entries.TakeLast(25))
            {
                var color = entry.Success ? ConsoleTheme.SUCCESS : ConsoleTheme.FAILURE;
                var marker = entry.Success ? "PASS" : "FAIL";

                ConsoleTheme.WriteLine(
                    $"  [{entry.Timestamp:HH:mm:ss}] [{marker}] {entry.Operation}",
                    color);

                ConsoleTheme.WriteLine(
                    $"           {entry.Provider} / {entry.Transport} / {entry.Codec} / " +
                    $"{(entry.ChunkingEnabled ? "Chunked" : "Single")} / {entry.PayloadSize:N0} bytes",
                    ConsoleTheme.MUTED);

                ConsoleTheme.WriteLine($"           {entry.Details}", ConsoleTheme.NORMAL);
            }
        }

        ConsoleScreen.WriteSectionEnd();
        ConsoleScreen.Pause();
    }

    /// <summary>
    /// Records the telemetry.
    /// </summary>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="operation">The operation.</param>
    /// <param name="payloadSize">Size of the payload.</param>
    /// <param name="success">if set to <c>true</c> [success].</param>
    /// <param name="details">The details.</param>
    private void RecordTelemetry(DateTimeOffset timestamp, string operation, int payloadSize, bool success, string details)
    {
        m_TelemetryStore.Add(new TelemetryEntry(timestamp, operation, m_Configuration.CryptoProvider, m_Configuration.Transport,
                m_Configuration.EnvelopeCodec, m_Configuration.ChunkingEnabled, payloadSize, success, details));
    }
}
