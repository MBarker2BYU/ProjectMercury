// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DemoController.Exchange.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Core.Chunking;
using Mercury.Core.Factories;
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
    /// Runs a protected payload exchange.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RunProtectedExchangeAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Protected Exchange");

        var payloadText = ConsoleScreen.ReadRequiredLine("  Enter payload: ");
        var payload = Encoding.UTF8.GetBytes(payloadText);
        var started = DateTimeOffset.Now;

        try
        {
            Terminal.WriteLine();
            await ApplyConfigurationAsync();

            ConsoleScreen.WriteStatus("ALPHA", "OK", "Payload accepted", ConsoleTheme.SUCCESS);
            ConsoleScreen.WriteStatus("ALPHA", "INFO", $"Payload size: {payload.Length:N0} bytes", ConsoleTheme.NORMAL);

            var result = await ExecuteExchangeAsync(payload);

            DisplayTransportInspection(payload);

            if (!result.Success)
                throw new InvalidOperationException(FormatFailure(result));

            var restoredPayload = Encoding.UTF8.GetString(result.Payload.ToArray());

            ConsoleScreen.WriteStatus("BRAVO", "OK", "SecureEnvelope received and validated", ConsoleTheme.SUCCESS);
            ConsoleScreen.WriteStatus("BRAVO", "OK", $"Payload restored: {result.Payload.Length:N0} bytes", ConsoleTheme.SUCCESS);

            DisplayValidatedEnvelope(result);

            Terminal.WriteLine();
            ConsoleTheme.WriteLine("  RESULT: SUCCESS", ConsoleTheme.SUCCESS);
            ConsoleTheme.WriteSecondary("  Restored payload: ");
            ConsoleTheme.WriteLine(restoredPayload);

            RecordTelemetry(started, "Protected Exchange", payload.Length,
                true, $"Restored {result.Payload.Length:N0} bytes.");
        }
        catch (Exception exception)
        {
            Terminal.WriteLine();
            ConsoleTheme.WriteLine("  RESULT: FAILURE", ConsoleTheme.FAILURE);
            ConsoleTheme.WriteLine($"  {exception.Message}", ConsoleTheme.FAILURE);

            RecordTelemetry(started, "Protected Exchange", payload.Length, false, exception.Message);
        }
        finally
        {
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Executes one Mercury exchange.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <returns>The Mercury receive result.</returns>
    private async Task<IMercuryResult> ExecuteExchangeAsync(byte[] payload, string recipientKeyId = BRAVO_NODE)
    {
        if (m_AlphaClient is null || m_BravoClient is null)
            throw new InvalidOperationException("Mercury clients have not been configured.");

        var cryptoContext = MercuryFactory.Instance.BuildCryptoContext(ALPHA_NODE, recipientKeyId);

        if (m_Configuration.ChunkingEnabled)
        {
            ConsoleScreen.WriteStatus("ALPHA", "TX", $"Sending protected chunks at {FormatByteSize(m_Configuration.ChunkSize)}",
                ConsoleTheme.WARNING);
            ConsoleScreen.WriteStatus("BRAVO", "RX", "Receiving protected chunk sequence", ConsoleTheme.WARNING);

            var receiveTask = m_BravoClient.ReceiveChunkedAsync();
            var sendTask = m_AlphaClient.SendChunkedAsync(cryptoContext, payload, m_Configuration.ChunkSize);

            await Task.WhenAll(sendTask, receiveTask);

            return await receiveTask;
        }

        ConsoleScreen.WriteStatus("ALPHA", "TX", "Sending protected SecureEnvelope", ConsoleTheme.WARNING);
        ConsoleScreen.WriteStatus("BRAVO", "RX", "Receiving protected SecureEnvelope", ConsoleTheme.WARNING);

        var singleReceiveTask = m_BravoClient.ReceiveAsync();
        var singleSendTask = m_AlphaClient.SendAsync(cryptoContext, payload);

        await Task.WhenAll(singleSendTask, singleReceiveTask);

        return await singleReceiveTask;
    }

    /// <summary>
    /// Displays the encoded frame observed by the configured transport.
    /// </summary>
    /// <param name="originalPayload">The original payload.</param>
    private void DisplayTransportInspection(byte[] originalPayload)
    {
        if (m_AlphaCaptureTransport is null || !m_AlphaCaptureTransport.HasLastFrame)
            return;

        var frame = m_AlphaCaptureTransport.LastFrame.ToArray();
        var rawPayloadVisible = ContainsSequence(frame, originalPayload);

        Terminal.WriteLine();
        ConsoleScreen.WriteSection("Transport Inspection");
        Terminal.WriteLine();

        ConsoleScreen.WriteLabel("Source Payload", $"{originalPayload.Length:N0} bytes");
        ConsoleScreen.WriteLabel("Protected Frame", $"{frame.Length:N0} bytes", ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Raw Data Visible", rawPayloadVisible ? "YES" : "NO",
            rawPayloadVisible ? ConsoleTheme.FAILURE : ConsoleTheme.SUCCESS);

        var previewLength = Math.Min(frame.Length, 96);
        var preview = Convert.ToHexString(frame.AsSpan(0, previewLength));

        Terminal.WriteLine();
        ConsoleTheme.WriteLine("  PROTECTED FRAME PREVIEW", ConsoleTheme.SECONDARY);
        Terminal.WriteLine();

        WriteHexPreview($"{preview}{(frame.Length > previewLength ? "..." : string.Empty)}");

        Terminal.WriteLine();
        ConsoleScreen.WriteSectionEnd();
    }

    /// <summary>
    /// Writes a wrapped hexadecimal preview.
    /// </summary>
    /// <param name="preview">The hexadecimal preview.</param>
    private static void WriteHexPreview(string preview)
    {
        const int segmentSize = 64;

        while (preview.Length > 0)
        {
            var segmentLength = Math.Min(segmentSize, preview.Length);
            ConsoleTheme.WriteLine($"    {preview[..segmentLength]}", ConsoleTheme.MUTED);
            preview = preview[segmentLength..];
        }
    }
}
