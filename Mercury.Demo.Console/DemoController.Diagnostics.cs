// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DemoController.Diagnostics.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Demo.Console.Enums;
using Mercury.Demo.Console.Interface;
using System.Text;

using Terminal = System.Console;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury RC 1.2 console demonstration application.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Displays and executes the RC 1.2 security demonstrations.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RunSecurityDiagnosticsAsync()
    {
        var returnRequested = false;

        while (!returnRequested)
        {
            ConsoleScreen.Clear();
            ConsoleScreen.DisplayHeader();
            DisplayConfiguration();

            var menu = new ConsoleMenu<DiagnosticMenuSelection>(
                "Security Demonstration",
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.Replay, "Replay Protected TCP Frame"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.Tamper, "Tamper Protected TCP Payload"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.WrongKey, "Wrong-Key Rejection"),
                new ConsoleMenuItem<DiagnosticMenuSelection>(DiagnosticMenuSelection.Return, "Return to Main Menu", isEscapeSelection: true));

            switch (menu.Display())
            {
                case DiagnosticMenuSelection.Replay:
                    await RunReplayDiagnosticAsync();
                    break;

                case DiagnosticMenuSelection.Tamper:
                    await RunTamperDiagnosticAsync();
                    break;

                case DiagnosticMenuSelection.WrongKey:
                    await RunWrongKeyDiagnosticAsync();
                    break;

                case DiagnosticMenuSelection.Return:
                    returnRequested = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Demonstrates replay rejection through the TCP attack simulator.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RunReplayDiagnosticAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("TCP Replay Attack");

        if (m_Configuration.Transport != TCP_TRANSPORT)
        {
            ConsoleTheme.WriteLine("  TEST NOT AVAILABLE IN THIS TRANSPORT", ConsoleTheme.WARNING);
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
            return;
        }

        var payload = Encoding.UTF8.GetBytes("Mercury RC 1.2 replay protection demonstration.");
        var started = DateTimeOffset.Now;
        var originalChunking = m_Configuration.ChunkingEnabled;

        try
        {
            m_Configuration.ChunkingEnabled = false;
            await ApplyConfigurationAsync();

            var attackSimulator = m_AttackSimulator ??
                throw new InvalidOperationException("The TCP attack simulator is not available.");

            attackSimulator.ClearLastFrame();
            attackSimulator.TamperEnabled = false;
            attackSimulator.ReplayEnabled = true;

            ConsoleScreen.WriteStatus("MITM", "ARM", "Replay attack armed on the TCP proxy", ConsoleTheme.WARNING);

            var baselineResult = await ExecuteExchangeAsync(payload);

            if (!baselineResult.Success)
                throw new InvalidOperationException(baselineResult.Message ?? "The baseline protected exchange failed.");

            ConsoleScreen.WriteStatus("BRAVO", "PASS", "Original protected frame accepted", ConsoleTheme.SUCCESS);
            ConsoleScreen.WriteStatus("MITM", "SEND", "Submitting the captured frame again", ConsoleTheme.WARNING);

            var replayResult = await ExecuteExchangeAsync(payload);
            var passed = !replayResult.Success && replayResult.FailureReason == FailureReason.ReplayDetected;

            DisplaySecurityResult(passed,
                passed ? "Replayed frame rejected as ReplayDetected." :
                $"Expected ReplayDetected; received {FormatFailure(replayResult)}.");

            RecordTelemetry(started, "TCP Replay Attack", payload.Length, passed,
                passed ? "Replay rejected." : FormatFailure(replayResult));
        }
        catch (Exception exception)
        {
            DisplaySecurityResult(false, exception.Message);
            RecordTelemetry(started, "TCP Replay Attack", payload.Length, false, exception.Message);
        }
        finally
        {
            if (m_AttackSimulator is not null)
                m_AttackSimulator.ReplayEnabled = false;

            m_Configuration.ChunkingEnabled = originalChunking;
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Demonstrates protected-payload tamper rejection through the TCP attack simulator.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RunTamperDiagnosticAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("TCP Tamper Attack");

        if (m_Configuration.Transport != TCP_TRANSPORT)
        {
            ConsoleTheme.WriteLine("  TEST NOT AVAILABLE IN THIS TRANSPORT", ConsoleTheme.WARNING);
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
            return;
        }

        var payload = Encoding.UTF8.GetBytes("Mercury RC 1.2 protected-payload tamper demonstration.");
        var started = DateTimeOffset.Now;
        var originalChunking = m_Configuration.ChunkingEnabled;

        try
        {
            m_Configuration.ChunkingEnabled = false;
            await ApplyConfigurationAsync();

            var attackSimulator = m_AttackSimulator ??
                throw new InvalidOperationException("The TCP attack simulator is not available.");

            attackSimulator.ClearLastFrame();
            attackSimulator.ReplayEnabled = false;
            attackSimulator.TamperEnabled = true;

            ConsoleScreen.WriteStatus("MITM", "ARM", "Protected-payload tamper attack armed", ConsoleTheme.WARNING);
            ConsoleScreen.WriteStatus("MITM", "EDIT", $"Preserving the {m_Configuration.EnvelopeCodec} envelope structure", ConsoleTheme.WARNING);

            var tamperResult = await ExecuteExchangeAsync(payload);
            var passed = !tamperResult.Success &&
                         tamperResult.FailureReason is FailureReason.AuthenticationFailed or FailureReason.TamperDetected;

            DisplaySecurityResult(passed,
                passed ? $"Tampered protected payload rejected as {tamperResult.FailureReason}." :
                $"Expected authentication or tamper failure; received {FormatFailure(tamperResult)}.");

            RecordTelemetry(started, "TCP Tamper Attack", payload.Length, passed,
                passed ? "Tamper rejected." : FormatFailure(tamperResult));
        }
        catch (Exception exception)
        {
            DisplaySecurityResult(false, exception.Message);
            RecordTelemetry(started, "TCP Tamper Attack", payload.Length, false, exception.Message);
        }
        finally
        {
            if (m_AttackSimulator is not null)
                m_AttackSimulator.TamperEnabled = false;

            m_Configuration.ChunkingEnabled = originalChunking;
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Demonstrates that Bravo rejects a message protected for Charlie.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RunWrongKeyDiagnosticAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Wrong-Key Attack");

        var payload = Encoding.UTF8.GetBytes("Mercury RC 1.2 wrong-key rejection demonstration.");
        var started = DateTimeOffset.Now;
        var originalChunking = m_Configuration.ChunkingEnabled;

        try
        {
            m_Configuration.ChunkingEnabled = false;
            await ApplyConfigurationAsync();

            ConsoleScreen.WriteStatus("ALPHA", "KEY", $"Protecting the payload for {CHARLIE_NODE}", ConsoleTheme.WARNING);
            ConsoleScreen.WriteStatus("BRAVO", "RX", "Attempting receive as Bravo Node", ConsoleTheme.WARNING);

            var wrongKeyResult = await ExecuteExchangeAsync(payload, CHARLIE_NODE);
            var passed = !wrongKeyResult.Success &&
                         wrongKeyResult.FailureReason is FailureReason.WrongKey or FailureReason.AuthenticationFailed;

            DisplaySecurityResult(passed,
                passed ? $"Wrong-key message rejected as {wrongKeyResult.FailureReason}." :
                $"Expected wrong-key or authentication failure; received {FormatFailure(wrongKeyResult)}.");

            RecordTelemetry(started, "Wrong-Key Attack", payload.Length, passed,
                passed ? "Wrong key rejected." : FormatFailure(wrongKeyResult));
        }
        catch (Exception exception)
        {
            DisplaySecurityResult(false, exception.Message);
            RecordTelemetry(started, "Wrong-Key Attack", payload.Length, false, exception.Message);
        }
        finally
        {
            m_Configuration.ChunkingEnabled = originalChunking;
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Displays a security demonstration result.
    /// </summary>
    /// <param name="passed">Whether the security control passed.</param>
    /// <param name="message">The result message.</param>
    private static void DisplaySecurityResult(bool passed, string message)
    {
        Terminal.WriteLine();
        ConsoleTheme.WriteLine(passed ? "  RESULT: SECURITY CONTROL PASSED" : "  RESULT: SECURITY CONTROL FAILED",
            passed ? ConsoleTheme.SUCCESS : ConsoleTheme.FAILURE);
        ConsoleTheme.WriteLine($"  {message}", passed ? ConsoleTheme.SUCCESS : ConsoleTheme.FAILURE);
    }
}
