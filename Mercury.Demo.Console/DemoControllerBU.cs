// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="DemoController.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Abstractions.Transport;
using Mercury.Core.Chunking;
using Mercury.Core.Factories;
using Mercury.Demo.Console.Enums;
using Mercury.Demo.Console.Interface;
using Mercury.Demo.Console.Models;
using Mercury.Demo.Console.Wrappers;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

using Terminal = System.Console;

namespace Mercury.Demo.Console;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// </summary>
internal sealed class DemoControllerBU
{
    /// <summary>
    /// The alpha node
    /// </summary>
    internal const string ALPHA_NODE = "Alpha Node";
    /// <summary>
    /// The bravo node
    /// </summary>
    internal const string BRAVO_NODE = "Bravo Node";

    /// <summary>
    /// The aes GCM
    /// </summary>
    internal const string AES_GCM = "AES-GCM";
    /// <summary>
    /// The cha cha 20
    /// </summary>
    internal const string CHA_CHA_20 = "ChaCha20-Poly1305";

    /// <summary>
    /// The in memory transport
    /// </summary>
    internal const string IN_MEMORY_TRANSPORT = "In-Memory";
    /// <summary>
    /// The TCP transport
    /// </summary>
    internal const string TCP_TRANSPORT = "TCP";

    /// <summary>
    /// The binary codec
    /// </summary>
    internal const string BINARY_CODEC = "Binary";
    /// <summary>
    /// The json codec
    /// </summary>
    internal const string JSON_CODEC = "Json";

    /// <summary>
    /// The return
    /// </summary>
    private const string RETURN = "Return";

    /// <summary>
    /// The m configuration
    /// </summary>
    private readonly DemoConfiguration m_Configuration = new();
    /// <summary>
    /// The m telemetry store
    /// </summary>
    private readonly TelemetryStore m_TelemetryStore = new();

    /// <summary>
    /// The m alpha client
    /// </summary>
    private IMercuryClient? m_AlphaClient;
    /// <summary>
    /// The m bravo client
    /// </summary>
    private IMercuryClient? m_BravoClient;
    /// <summary>
    /// The m alpha capture transport
    /// </summary>
    private CaptureTransport? m_AlphaCaptureTransport;

    /// <summary>
    /// Run as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Unsupported main-menu selection.</exception>
    public async Task RunAsync()
    {
        Terminal.Title = "Mercury Secure Communications Framework";
        ConsoleTheme.Apply();

        try
        {
            var exitRequested = false;

            while (!exitRequested)
            {
                ConsoleScreen.Clear();
                ConsoleScreen.DisplayHeader();
                DisplayConfiguration();

                switch (DisplayMainMenu())
                {
                    case MainMenuSelection.RunProtectedExchange:
                        await RunProtectedExchangeAsync();
                        break;

                    case MainMenuSelection.SendProtectedFile:
                        await RunProtectedFileTransferAsync();
                        break;

                    case MainMenuSelection.ConfigureSession:
                        ConfigureSession();
                        break;

                    case MainMenuSelection.SecurityDiagnostics:
                        await RunSecurityDiagnosticsAsync();
                        break;

                    case MainMenuSelection.RunTestMatrix:
                        await RunTestMatrixAsync();
                        break;

                    case MainMenuSelection.ViewTelemetry:
                        DisplayTelemetry();
                        break;

                    case MainMenuSelection.Exit:
                        exitRequested = true;
                        break;

                    default:
                        throw new InvalidOperationException("Unsupported main-menu selection.");
                }
            }
        }
        finally
        {
            ConsoleTheme.Reset();
            Terminal.Clear();
        }
    }

    /// <summary>
    /// Displays the main menu.
    /// </summary>
    /// <returns>MainMenuSelection.</returns>
    private static MainMenuSelection DisplayMainMenu()
    {
        var menu = new ConsoleMenu<MainMenuSelection>(
            "Command Menu",
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.RunProtectedExchange, "Send Protected Payload"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.SendProtectedFile, "Send Protected File"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.ConfigureSession, "Configure Session"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.SecurityDiagnostics, "Run Security Diagnostics"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.RunTestMatrix, "Run Provider / Transport Test Matrix"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.ViewTelemetry, "View Telemetry"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.Exit, "Exit Mercury", isEscapeSelection: true));

        return menu.Display();
    }

    /// <summary>
    /// Displays the configuration.
    /// </summary>
    private void DisplayConfiguration()
    {
        ConsoleScreen.WriteSection("Active Configuration");
        ConsoleScreen.WriteLabel("Platform", GetPlatformName(), ConsoleTheme.SUCCESS);
        ConsoleScreen.WriteLabel("Provider", m_Configuration.CryptoProvider, ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Transport", m_Configuration.Transport, ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Codec", m_Configuration.EnvelopeCodec);
        ConsoleScreen.WriteLabel("Chunking", m_Configuration.ChunkingEnabled
                ? $"Enabled / {FormatByteSize(m_Configuration.ChunkSize)}"
                : "Disabled",
            m_Configuration.ChunkingEnabled ? ConsoleTheme.SUCCESS : ConsoleTheme.WARNING);

        ConsoleScreen.WriteSectionEnd();
        Terminal.WriteLine();
    }

    /// <summary>
    /// Run protected exchange as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
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
            {
                throw new InvalidOperationException(
                    result.Message ?? "Mercury failed to receive the payload.");
            }

            var restoredPayload = Encoding.UTF8.GetString(result.Payload.ToArray());

            ConsoleScreen.WriteStatus("BRAVO", "OK", "SecureEnvelope received and validated", ConsoleTheme.SUCCESS);
            ConsoleScreen.WriteStatus("BRAVO", "OK", $"Payload restored: {result.Payload.Length:N0} bytes", ConsoleTheme.SUCCESS);

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

            RecordTelemetry(started, "Protected Exchange",
                payload.Length, false, exception.Message);
        }
        finally
        {
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Run protected file transfer as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidDataException">The received file hash does not match the source file hash.</exception>
    private async Task RunProtectedFileTransferAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Protected File Transfer");

        var sourcePath = ReadExistingFilePath();
        var started = DateTimeOffset.Now;
        var sourceFileName = Path.GetFileName(sourcePath);
        var sourceFileBytes = await File.ReadAllBytesAsync(sourcePath);
        var sourceHash = SHA256.HashData(sourceFileBytes);
        var transferPayload = new FileTransferPayload(
            sourceFileName,
            sourceFileBytes).Encode();

        try
        {
            Terminal.WriteLine();
            await ApplyConfigurationAsync();

            ConsoleScreen.WriteStatus("ALPHA", "FILE", $"Loaded {sourceFileName} ({sourceFileBytes.LongLength:N0} bytes)",
                ConsoleTheme.SUCCESS);

            ConsoleScreen.WriteStatus("ALPHA", "HASH", Convert.ToHexString(sourceHash),
                ConsoleTheme.MUTED);

            var result = await ExecuteExchangeAsync(transferPayload);

            DisplayTransportInspection(sourceFileBytes);

            if (!result.Success)
            {
                throw new InvalidOperationException(
                    result.Message ?? "Mercury failed to receive the protected file.");
            }

            var receivedFile = FileTransferPayload.Decode(result.Payload.ToArray());
            var receivedHash = SHA256.HashData(receivedFile.FileBytes);
            var hashesMatch = CryptographicOperations.FixedTimeEquals(sourceHash, receivedHash);

            if (!hashesMatch)
                throw new InvalidDataException("The received file hash does not match the source file hash.");

            var destinationPath = BuildReceivedFilePath(sourcePath, receivedFile.FileName);

            await File.WriteAllBytesAsync(destinationPath, receivedFile.FileBytes);

            ConsoleScreen.WriteStatus("BRAVO", "OK", "SecureEnvelope received and validated",
                ConsoleTheme.SUCCESS);

            ConsoleScreen.WriteStatus("BRAVO", "FILE",
                $"Restored {receivedFile.FileName} ({receivedFile.FileBytes.LongLength:N0} bytes)",
                ConsoleTheme.SUCCESS);

            ConsoleScreen.WriteStatus("BRAVO", "HASH",
                Convert.ToHexString(receivedHash),
                ConsoleTheme.MUTED);

            Terminal.WriteLine();
            
            ConsoleTheme.WriteLine("  RESULT: FILE VERIFIED", ConsoleTheme.SUCCESS);
            ConsoleScreen.WriteLabel("Saved To", destinationPath, ConsoleTheme.PRIMARY);
            ConsoleScreen.WriteLabel("Hash Match", "YES", ConsoleTheme.SUCCESS);

            RecordTelemetry(started, "Protected File Transfer",
                transferPayload.Length, true,
                $"{sourceFileName} verified and saved to {destinationPath}");
        }
        catch (Exception exception)
        {
            Terminal.WriteLine();
            ConsoleTheme.WriteLine("  RESULT: FILE TRANSFER FAILURE", ConsoleTheme.FAILURE);
            ConsoleTheme.WriteLine($"  {exception.Message}", ConsoleTheme.FAILURE);

            RecordTelemetry(started, "Protected File Transfer", transferPayload.Length,
                false, exception.Message);
        }
        finally
        {
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Reads the existing file path.
    /// </summary>
    /// <returns>System.String.</returns>
    private static string ReadExistingFilePath()
    {
        while (true)
        {
            var enteredPath = ConsoleScreen.ReadRequiredLine(
                "  Enter file path (drag and drop is supported): ");

            var normalizedPath = NormalizeEnteredPath(enteredPath);

            if (File.Exists(normalizedPath))
                return Path.GetFullPath(normalizedPath);

            ConsoleTheme.WriteLine(
                "  The selected file does not exist.",
                ConsoleTheme.WARNING);

            Terminal.WriteLine();
        }
    }

    /// <summary>
    /// Normalizes the entered path.
    /// </summary>
    /// <param name="enteredPath">The entered path.</param>
    /// <returns>System.String.</returns>
    private static string NormalizeEnteredPath(string enteredPath)
    {
        var normalizedPath = enteredPath.Trim();

        if (normalizedPath.Length >= 2 &&
            ((normalizedPath[0] == '\"' && normalizedPath[^1] == '\"') ||
             (normalizedPath[0] == '\'' && normalizedPath[^1] == '\'')))
        {
            normalizedPath = normalizedPath[1..^1];
        }

        if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            normalizedPath = normalizedPath
                .Replace("\\ ", " ")
                .Replace("\\(", "(")
                .Replace("\\)", ")");
        }

        return normalizedPath;
    }

    /// <summary>
    /// Builds the received file path.
    /// </summary>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="receivedFileName">Name of the received file.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="IOException">A unique destination file name could not be created.</exception>
    private static string BuildReceivedFilePath(string sourcePath, string receivedFileName)
    {
        var sourceDirectory = Path.GetDirectoryName(sourcePath)
            ?? Environment.CurrentDirectory;

        var outputDirectory = Path.Combine(sourceDirectory, "Mercury-Received");

        Directory.CreateDirectory(outputDirectory);

        var safeFileName = Path.GetFileName(receivedFileName);
        var baseName = Path.GetFileNameWithoutExtension(safeFileName);
        var extension = Path.GetExtension(safeFileName);
        var candidate = Path.Combine(outputDirectory,
            $"{baseName}.received{extension}");

        if (!File.Exists(candidate))
            return candidate;

        for (var sequence = 2; sequence < int.MaxValue; sequence++)
        {
            candidate = Path.Combine(
                outputDirectory,
                $"{baseName}.received-{sequence}{extension}");

            if (!File.Exists(candidate))
                return candidate;
        }

        throw new IOException("A unique destination file name could not be created.");
    }

    /// <summary>
    /// Execute exchange as an asynchronous operation.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>A Task&lt;IMercuryResult&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Mercury clients have not been configured.</exception>
    private async Task<IMercuryResult> ExecuteExchangeAsync(byte[] payload)
    {
        if (m_AlphaClient is null || m_BravoClient is null)
            throw new InvalidOperationException("Mercury clients have not been configured.");

        var cryptoContext = MercuryFactory.Instance.BuildCryptoContext(ALPHA_NODE, BRAVO_NODE);

        if (m_Configuration.ChunkingEnabled)
        {
            ConsoleScreen.WriteStatus("ALPHA", "TX",
                $"Sending protected chunks at {FormatByteSize(m_Configuration.ChunkSize)}",
                ConsoleTheme.WARNING);

            await m_AlphaClient.SendChunkedAsync(cryptoContext, payload, m_Configuration.ChunkSize);

            ConsoleScreen.WriteStatus("BRAVO", "RX", "Receiving protected chunk sequence", ConsoleTheme.WARNING);

            return await m_BravoClient.ReceiveChunkedAsync();
        }

        ConsoleScreen.WriteStatus("ALPHA", "TX", "Sending protected SecureEnvelope", ConsoleTheme.WARNING);
        await m_AlphaClient.SendAsync(cryptoContext, payload);
        ConsoleScreen.WriteStatus("BRAVO", "RX", "Receiving protected SecureEnvelope", ConsoleTheme.WARNING);

        return await m_BravoClient.ReceiveAsync();
    }

    /// <summary>
    /// Displays the transport inspection.
    /// </summary>
    /// <param name="originalPayload">The original payload.</param>
    private void DisplayTransportInspection(byte[] originalPayload)
    {
        if (m_AlphaCaptureTransport is null ||
            !m_AlphaCaptureTransport.HasLastFrame)
        {
            return;
        }

        var frame = m_AlphaCaptureTransport.LastFrame.ToArray();
        var rawPayloadVisible = ContainsSequence(frame, originalPayload);

        Terminal.WriteLine();
        ConsoleScreen.WriteSection("Transport Inspection");

        Terminal.WriteLine();

        ConsoleScreen.WriteLabel("Source Payload ", $"{originalPayload.Length:N0} bytes");
        ConsoleScreen.WriteLabel("Protected Frame ", $"{frame.Length:N0} bytes", ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Raw Data Visible ", rawPayloadVisible ? "YES" : "NO",
            rawPayloadVisible ? ConsoleTheme.FAILURE : ConsoleTheme.SUCCESS);
        
        var previewLength = Math.Min(frame.Length, 96);
        var preview = Convert.ToHexString(frame.AsSpan(0, previewLength));

        Terminal.WriteLine();

        ConsoleTheme.WriteLine("  PROTECTED FRAME PREVIEW", ConsoleTheme.SECONDARY);

        const int size = 64;
        
        preview = $"{preview}{(frame.Length > previewLength ? "..." : string.Empty)}";

        var previewSegment = string.Empty;

        while (true)
        {
            if (preview.Length > size)
            {
                previewSegment = preview[..size];
                preview = preview[16..];
            }
            else
            {
                previewSegment = preview;
                preview = string.Empty;
            }

            ConsoleTheme.WriteLine($"    {previewSegment}", ConsoleTheme.MUTED);
            
            if(preview.Length == 0)
                break;

        }

        Terminal.WriteLine();

        ConsoleScreen.WriteSectionEnd();
    }

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

        for (var sourceIndex = 0;
             sourceIndex <= source.Length - sequence.Length;
             sourceIndex++)
        {
            var match = true;

            for (var sequenceIndex = 0;
                 sequenceIndex < sequence.Length;
                 sequenceIndex++)
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
    /// Configures the session.
    /// </summary>
    private void ConfigureSession()
    {
        var returnRequested = false;

        while (!returnRequested)
        {
            ConsoleScreen.Clear();
            ConsoleScreen.DisplayHeader();
            DisplayConfiguration();

            var menu = new ConsoleMenu<ConfigurationMenuSelection>(
                "Session Configuration",
                new ConsoleMenuItem<ConfigurationMenuSelection>(ConfigurationMenuSelection.CryptoProvider, "Select Crypto Provider"),
                new ConsoleMenuItem<ConfigurationMenuSelection>(ConfigurationMenuSelection.Transport, "Select Transport"),
                new ConsoleMenuItem<ConfigurationMenuSelection>(ConfigurationMenuSelection.EnvelopeCodec, "Select Envelope Codec"),
                new ConsoleMenuItem<ConfigurationMenuSelection>(ConfigurationMenuSelection.Chunking, "Enable or Disable Chunking"),
                new ConsoleMenuItem<ConfigurationMenuSelection>(ConfigurationMenuSelection.ChunkSize, "Select Chunk Size"),
                new ConsoleMenuItem<ConfigurationMenuSelection>(ConfigurationMenuSelection.Return, "Return to Main Menu", isEscapeSelection: true));

            switch (menu.Display())
            {
                case ConfigurationMenuSelection.CryptoProvider:
                    ConfigureCryptoProvider();
                    break;

                case ConfigurationMenuSelection.Transport:
                    ConfigureTransport();
                    break;

                case ConfigurationMenuSelection.EnvelopeCodec:
                    ConfigureEnvelopeCodec();
                    break;

                case ConfigurationMenuSelection.Chunking:
                    ConfigureChunking();
                    break;

                case ConfigurationMenuSelection.ChunkSize:
                    ConfigureChunkSize();
                    break;

                case ConfigurationMenuSelection.Return:
                    returnRequested = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Configures the crypto provider.
    /// </summary>
    private void ConfigureCryptoProvider()
    {
        var menu = new ConsoleMenu<string>(
            "Select Crypto Provider",
            new ConsoleMenuItem<string>(AES_GCM, AES_GCM),
            new ConsoleMenuItem<string>(CHA_CHA_20, CHA_CHA_20),
            new ConsoleMenuItem<string>(RETURN, RETURN, isEscapeSelection: true));

        var selection = DisplayConfigurationMenu(menu);

        if (selection != RETURN)
            m_Configuration.CryptoProvider = selection;
    }

    /// <summary>
    /// Configures the transport.
    /// </summary>
    private void ConfigureTransport()
    {
        var menu = new ConsoleMenu<string>(
            "Select Transport",
            new ConsoleMenuItem<string>(IN_MEMORY_TRANSPORT, IN_MEMORY_TRANSPORT),
            new ConsoleMenuItem<string>(TCP_TRANSPORT, TCP_TRANSPORT),
            new ConsoleMenuItem<string>(RETURN, RETURN, isEscapeSelection: true));

        var selection = DisplayConfigurationMenu(menu);

        if (selection != RETURN)
            m_Configuration.Transport = selection;
    }

    /// <summary>
    /// Configures the envelope codec.
    /// </summary>
    private void ConfigureEnvelopeCodec()
    {
        var menu = new ConsoleMenu<string>(
            "Select Envelope Codec",
            new ConsoleMenuItem<string>(BINARY_CODEC, BINARY_CODEC),
            new ConsoleMenuItem<string>(JSON_CODEC, JSON_CODEC),
            new ConsoleMenuItem<string>(RETURN, RETURN, isEscapeSelection: true));

        var selection = DisplayConfigurationMenu(menu);

        if (selection != RETURN)
            m_Configuration.EnvelopeCodec = selection;
    }

    /// <summary>
    /// Configures the chunking.
    /// </summary>
    private void ConfigureChunking()
    {
        const string ENABLED = "Enabled";
        const string DISABLED = "Disabled";

        var menu = new ConsoleMenu<string>(
            "Configure Chunking",
            new ConsoleMenuItem<string>(ENABLED, ENABLED),
            new ConsoleMenuItem<string>(DISABLED, DISABLED),
            new ConsoleMenuItem<string>(RETURN, RETURN, isEscapeSelection: true));

        var selection = DisplayConfigurationMenu(menu);

        if (selection == ENABLED)
            m_Configuration.ChunkingEnabled = true;
        else if (selection == DISABLED)
            m_Configuration.ChunkingEnabled = false;
    }

    /// <summary>
    /// Configures the size of the chunk.
    /// </summary>
    private void ConfigureChunkSize()
    {
        var menuItems = new List<ConsoleMenuItem<int>>();

        for (var sizeKilobytes = 1; sizeKilobytes <= 1024; sizeKilobytes *= 2)
        {
            var sizeBytes = checked(sizeKilobytes * 1024);
            menuItems.Add(new ConsoleMenuItem<int>(sizeBytes, FormatByteSize(sizeBytes)));
        }

        menuItems.Add(new ConsoleMenuItem<int>(0, RETURN, isEscapeSelection: true));

        var menu = new ConsoleMenu<int>("Select Chunk Size", [.. menuItems]);
        var selection = DisplayConfigurationMenu(menu);

        if (selection == 0)
            return;

        m_Configuration.ChunkSize = selection;
        m_Configuration.ChunkingEnabled = true;
    }

    /// <summary>
    /// Displays the configuration menu.
    /// </summary>
    /// <typeparam name="TSelection">The type of the t selection.</typeparam>
    /// <param name="menu">The menu.</param>
    /// <returns>TSelection.</returns>
    private static TSelection DisplayConfigurationMenu<TSelection>(ConsoleMenu<TSelection> menu)
        where TSelection : notnull
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();

        return menu.Display();
    }

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

        var payload = Encoding.UTF8.GetBytes("Mercury replay protection diagnostic.");
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

            if (m_AlphaCaptureTransport is null ||
                !m_AlphaCaptureTransport.HasLastFrame)
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
    /// Apply configuration as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task ApplyConfigurationAsync()
    {
        var keys = new Dictionary<KeyId, byte[]>
        {
            [ALPHA_NODE] = RandomNumberGenerator.GetBytes(32),
            [BRAVO_NODE] = RandomNumberGenerator.GetBytes(32)
        };

        var keyProvider = new SymmetricKeyProviderDictionary(keys);
        var alphaCryptoProvider = BuildCryptoProvider(keyProvider);
        var bravoCryptoProvider = BuildCryptoProvider(keyProvider);
        var (alphaTransport, bravoTransport) = await BuildTransportAsync();
        var envelopeCodec = BuildEnvelopeCodec();

        m_AlphaCaptureTransport = new CaptureTransport(alphaTransport);

        var alphaDependencies = MercuryFactory.Instance.BuildDependencies(
            alphaCryptoProvider,
            envelopeCodec,
            m_AlphaCaptureTransport);

        var bravoDependencies = MercuryFactory.Instance.BuildDependencies(
            bravoCryptoProvider,
            envelopeCodec,
            bravoTransport);

        m_AlphaClient = MercuryFactory.Instance.BuildClient(alphaDependencies);
        m_BravoClient = MercuryFactory.Instance.BuildClient(bravoDependencies);
    }

    /// <summary>
    /// Builds the crypto provider.
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <returns>ICryptoProvider.</returns>
    private ICryptoProvider BuildCryptoProvider(SymmetricKeyProviderDictionary keys)
    {
        return m_Configuration.CryptoProvider switch
        {
            AES_GCM => new AesGcmCryptoProvider(keys),
            CHA_CHA_20 => new ChaCha20CryptoProvider(keys),
            _ => throw new ArgumentException("The selected crypto provider is not supported.")
        };
    }

    /// <summary>
    /// Build transport as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">The selected transport is not supported.</exception>
    private async Task<(ITransport alphaTransport, ITransport bravoTransport)> BuildTransportAsync(
        CancellationToken cancellationToken = default)
    {
        switch (m_Configuration.Transport)
        {
            case IN_MEMORY_TRANSPORT:
                return InMemoryDuplexTransport.CreateConnectedPair();

            case TCP_TRANSPORT:
            {
                var listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();

                try
                {
                    var localEndpoint = (IPEndPoint)listener.LocalEndpoint;

                    var bravoTransportTask = TcpTransport.AcceptAsync(listener, cancellationToken);
                    var alphaTransportTask = TcpTransport.ConnectAsync(
                        IPAddress.Loopback.ToString(),
                        localEndpoint.Port,
                        cancellationToken);

                    await Task.WhenAll(alphaTransportTask, bravoTransportTask).ConfigureAwait(false);

                    return (
                        await alphaTransportTask.ConfigureAwait(false),
                        await bravoTransportTask.ConfigureAwait(false));
                }
                finally
                {
                    listener.Stop();
                }
            }

            default:
                throw new ArgumentException("The selected transport is not supported.");
        }
    }

    /// <summary>
    /// Builds the envelope codec.
    /// </summary>
    /// <returns>EnvelopeCodec.</returns>
    private EnvelopeCodec BuildEnvelopeCodec()
    {
        return m_Configuration.EnvelopeCodec switch
        {
            BINARY_CODEC => EnvelopeCodec.Binary,
            JSON_CODEC => EnvelopeCodec.Json,
            _ => throw new ArgumentException("The selected envelope codec is not supported.")
        };
    }

    /// <summary>
    /// Records the telemetry.
    /// </summary>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="operation">The operation.</param>
    /// <param name="payloadSize">Size of the payload.</param>
    /// <param name="success">if set to <c>true</c> [success].</param>
    /// <param name="details">The details.</param>
    private void RecordTelemetry(
        DateTimeOffset timestamp,
        string operation,
        int payloadSize,
        bool success,
        string details)
    {
        m_TelemetryStore.Add(
            new TelemetryEntry(
                timestamp,
                operation,
                m_Configuration.CryptoProvider,
                m_Configuration.Transport,
                m_Configuration.EnvelopeCodec,
                m_Configuration.ChunkingEnabled,
                payloadSize,
                success,
                details));
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
