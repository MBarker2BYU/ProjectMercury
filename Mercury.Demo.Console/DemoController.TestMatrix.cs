// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.TestMatrix">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Demo.Console.Interface;
using System.Text;

using Terminal = System.Console;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Run test matrix as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task RunTestMatrixAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Provider / Transport Test Matrix");

        var originalProvider = m_Configuration.CryptoProvider;
        var originalTransport = m_Configuration.Transport;
        var originalCodec = m_Configuration.EnvelopeCodec;
        var originalChunking = m_Configuration.ChunkingEnabled;

        var providers = new[] { AES_GCM, CHA_CHA_20 };
        var transports = new[] { IN_MEMORY_TRANSPORT, TCP_TRANSPORT };
        var codecs = new[] { BINARY_CODEC, JSON_CODEC };
        var chunkingModes = new[] { false, true };
        var payload = Encoding.UTF8.GetBytes("Mercury cross-platform matrix exchange.");

        var passed = 0;
        var failed = 0;

        try
        {
            foreach (var provider in providers)
            foreach (var transport in transports)
            foreach (var codec in codecs)
            foreach (var chunking in chunkingModes)
            {
                m_Configuration.CryptoProvider = provider;
                m_Configuration.Transport = transport;
                m_Configuration.EnvelopeCodec = codec;
                m_Configuration.ChunkingEnabled = chunking;

                var started = DateTimeOffset.Now;
                var testName = $"{provider} / {transport} / {codec} / {(chunking ? "Chunked" : "Single")}";

                try
                {
                    await ApplyConfigurationAsync();
                    var result = await ExecuteExchangeAsync(payload);

                    if (!result.Success || !result.Payload.ToArray().SequenceEqual(payload))
                    {
                        throw new InvalidOperationException(
                            result.Message ?? "Restored payload did not match the source payload.");
                    }

                    passed += 1;
                    ConsoleTheme.WriteLine($"  [PASS] {testName}", ConsoleTheme.SUCCESS);
                    RecordTelemetry(started, "Test Matrix", payload.Length, true, testName);
                }
                catch (Exception exception)
                {
                    failed += 1;
                    ConsoleTheme.WriteLine($"  [FAIL] {testName} — {exception.Message}", ConsoleTheme.FAILURE);
                    RecordTelemetry(started, "Test Matrix", payload.Length, false, $"{testName}: {exception.Message}");
                }
            }

            Terminal.WriteLine();
            ConsoleTheme.WriteLine($"  PASSED: {passed}", ConsoleTheme.SUCCESS);
            ConsoleTheme.WriteLine($"  FAILED: {failed}", failed == 0 ? ConsoleTheme.SUCCESS : ConsoleTheme.FAILURE);
            ConsoleTheme.WriteLine($"  TOTAL:  {passed + failed}", ConsoleTheme.PRIMARY);
        }
        finally
        {
            m_Configuration.CryptoProvider = originalProvider;
            m_Configuration.Transport = originalTransport;
            m_Configuration.EnvelopeCodec = originalCodec;
            m_Configuration.ChunkingEnabled = originalChunking;

            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }
}
