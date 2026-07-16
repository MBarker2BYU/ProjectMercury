// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.Diagnostics.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Demo.Console.Enums;
using Mercury.Demo.Console.Interface;
using Mercury.Demo.Console.Wrappers;
using System.Text;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Run security diagnostics as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task RunSecurityDiagnosticsAsync()
    {
        var returnRequested = false;

        while (!returnRequested)
        {
            ConsoleScreen.Clear();
            ConsoleScreen.DisplayHeader();

            var menu = new ConsoleMenu<DiagnosticMenuSelection>(
                "Security Diagnostics",
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.ReplayLastFrame, "Replay Last Protected Frame"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.TamperReplayMetadata, "Tamper Replay Metadata"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.TamperAuthenticationTag, "Tamper Authentication Tag"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.TamperProtectedPayload, "Tamper Protected Payload"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.RunAll, "Run All Security Diagnostics"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.Return, "Return to Main Menu", isEscapeSelection: true));

            switch (menu.Display())
            {
                case DiagnosticMenuSelection.ReplayLastFrame:
                    await RunReplayDiagnosticAsync();
                    break;

                case DiagnosticMenuSelection.TamperReplayMetadata:
                    await RunTamperDiagnosticAsync("Replay Metadata Tamper",
                        static transport => transport.SendReplayMetadataTamperedFrameAsync());
                    break;

                case DiagnosticMenuSelection.TamperAuthenticationTag:
                    await RunTamperDiagnosticAsync("Authentication Tag Tamper",
                        static transport => transport.SendAuthenticationTagTamperedFrameAsync());
                    break;

                case DiagnosticMenuSelection.TamperProtectedPayload:
                    await RunTamperDiagnosticAsync("Protected Payload Tamper",
                        static transport => transport.SendProtectedPayloadTamperedFrameAsync());
                    break;

                case DiagnosticMenuSelection.RunAll:
                    await RunAllSecurityDiagnosticsAsync();
                    break;

                case DiagnosticMenuSelection.Return:
                    returnRequested = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Run replay diagnostic as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidOperationException">No protected frame was captured.</exception>
    private async Task RunReplayDiagnosticAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Replay Diagnostic");

        var payload = "Mercury replay protection diagnostic."u8.ToArray();
        var started = DateTimeOffset.Now;
        var originalChunking = m_Configuration.ChunkingEnabled;

        try
        {
            m_Configuration.ChunkingEnabled = false;
            await ApplyConfigurationAsync();

            var baseline = await ExecuteExchangeAsync(payload);

            if (!baseline.Success)
            {
                throw new InvalidOperationException(baseline.Message ?? "Baseline protected exchange failed.");
            }

            if (m_AlphaCaptureTransport is null || !m_AlphaCaptureTransport.HasLastFrame)
                throw new InvalidOperationException("No protected frame was captured.");

            ConsoleScreen.WriteStatus("TEST", "INFO", "Replaying the captured protected frame", ConsoleTheme.WARNING);
            await m_AlphaCaptureTransport.ReplayLastFrameAsync();

            var replayResult = await m_BravoClient!.ReceiveAsync();
            var success = !replayResult.Success;

            ConsoleScreen.WriteStatus("TEST", success ? "PASS" : "FAIL", success
                    ? "Replayed frame was rejected."
                    : "Replayed frame was accepted.",
                success ? ConsoleTheme.SUCCESS : ConsoleTheme.FAILURE);

            RecordTelemetry(started, "Replay Diagnostic",
                payload.Length, success,
                success ? "Replay rejected." : "Replay accepted.");
        }
        catch (Exception exception)
        {
            ConsoleTheme.WriteLine($"  DIAGNOSTIC FAILURE: {exception.Message}", ConsoleTheme.FAILURE);

            RecordTelemetry(started, "Replay Diagnostic",
                payload.Length, false, exception.Message);
        }
        finally
        {
            m_Configuration.ChunkingEnabled = originalChunking;
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Run tamper diagnostic as an asynchronous operation.
    /// </summary>
    /// <param name="diagnosticName">Name of the diagnostic.</param>
    /// <param name="transmitTamperedFrameAsync">The transmit tampered frame asynchronous.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidOperationException">No protected frame was captured.</exception>
    private async Task RunTamperDiagnosticAsync(string diagnosticName,
        Func<CaptureTransport, Task> transmitTamperedFrameAsync)
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection(diagnosticName);

        var payload = Encoding.UTF8.GetBytes(
            $"Mercury {diagnosticName} diagnostic.");

        var started = DateTimeOffset.Now;
        var originalChunking = m_Configuration.ChunkingEnabled;

        try
        {
            m_Configuration.ChunkingEnabled = false;
            await ApplyConfigurationAsync();

            var baseline = await ExecuteExchangeAsync(payload);

            if (!baseline.Success)
            {
                throw new InvalidOperationException(baseline.Message ?? "Baseline protected exchange failed.");
            }

            if (m_AlphaCaptureTransport is null || !m_AlphaCaptureTransport.HasLastFrame)
            {
                throw new InvalidOperationException("No protected frame was captured.");
            }

            ConsoleScreen.WriteStatus("TEST", "INFO", "Transmitting a modified protected frame",
                ConsoleTheme.WARNING);

            await transmitTamperedFrameAsync(m_AlphaCaptureTransport);

            var tamperResult = await m_BravoClient!.ReceiveAsync();
            var passed = !tamperResult.Success;

            ConsoleScreen.WriteStatus("TEST", passed ? "PASS" : "FAIL", passed
                    ? "Modified frame was rejected."
                    : "Modified frame was accepted.",
                passed ? ConsoleTheme.SUCCESS : ConsoleTheme.FAILURE);

            RecordTelemetry(started, diagnosticName, payload.Length,
                passed, passed ? "Tamper rejected." : "Tamper accepted.");
        }
        catch (Exception exception)
        {
            ConsoleTheme.WriteLine(
                $"  DIAGNOSTIC FAILURE: {exception.Message}",
                ConsoleTheme.FAILURE);

            RecordTelemetry(started, diagnosticName, payload.Length,
                false, exception.Message);
        }
        finally
        {
            m_Configuration.ChunkingEnabled = originalChunking;
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Run all security diagnostics as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task RunAllSecurityDiagnosticsAsync()
    {
        await RunReplayDiagnosticAsync();

        await RunTamperDiagnosticAsync("Replay Metadata Tamper",
            static transport => transport.SendReplayMetadataTamperedFrameAsync());

        await RunTamperDiagnosticAsync("Authentication Tag Tamper",
            static transport => transport.SendAuthenticationTagTamperedFrameAsync());

        await RunTamperDiagnosticAsync("Protected Payload Tamper",
            static transport => transport.SendProtectedPayloadTamperedFrameAsync());
    }
}
