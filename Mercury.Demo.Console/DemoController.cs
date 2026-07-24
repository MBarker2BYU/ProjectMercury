// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DemoController.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Transport;
using Mercury.Demo.Console.Enums;
using Mercury.Demo.Console.Interface;
using Mercury.Demo.Console.Models;
using Mercury.Demo.Console.Proxy;
using Mercury.Demo.Console.Wrappers;

using Terminal = System.Console;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury RC 1.2 console demonstration application.
/// </summary>
internal sealed partial class DemoController : IAsyncDisposable
{
    /// <summary>
    /// The Mercury release label.
    /// </summary>
    internal const string RELEASE_LABEL = "RC 1.2";
    /// <summary>
    /// The Mercury release version.
    /// </summary>
    internal const string RELEASE_VERSION = "1.2.0-rc.1";

    /// <summary>
    /// The alpha node.
    /// </summary>
    internal const string ALPHA_NODE = "Alpha Node";
    /// <summary>
    /// The bravo node.
    /// </summary>
    internal const string BRAVO_NODE = "Bravo Node";
    /// <summary>
    /// The Charlie node used for wrong-key demonstrations.
    /// </summary>
    internal const string CHARLIE_NODE = "Charlie Node";

    /// <summary>
    /// The AES-GCM provider.
    /// </summary>
    internal const string AES_GCM = "AES-GCM";
    /// <summary>
    /// The ChaCha20-Poly1305 provider.
    /// </summary>
    internal const string CHA_CHA_20 = "ChaCha20-Poly1305";

    /// <summary>
    /// The in-memory transport.
    /// </summary>
    internal const string IN_MEMORY_TRANSPORT = "In-Memory";
    /// <summary>
    /// The TCP transport.
    /// </summary>
    internal const string TCP_TRANSPORT = "TCP";

    /// <summary>
    /// The binary codec.
    /// </summary>
    internal const string BINARY_CODEC = "Binary";
    /// <summary>
    /// The JSON codec.
    /// </summary>
    internal const string JSON_CODEC = "Json";

    /// <summary>
    /// The return menu selection.
    /// </summary>
    private const string RETURN = "Return";

    /// <summary>
    /// The active demonstration configuration.
    /// </summary>
    private readonly DemoConfiguration m_Configuration = new();
    /// <summary>
    /// The telemetry store.
    /// </summary>
    private readonly TelemetryStore m_TelemetryStore = new();

    /// <summary>
    /// The Alpha client.
    /// </summary>
    private IMercuryClient? m_AlphaClient;
    /// <summary>
    /// The Bravo client.
    /// </summary>
    private IMercuryClient? m_BravoClient;
    /// <summary>
    /// The Alpha capture transport.
    /// </summary>
    private CaptureTransport? m_AlphaCaptureTransport;
    /// <summary>
    /// The Alpha transport.
    /// </summary>
    private ITransport? m_AlphaTransport;
    /// <summary>
    /// The Bravo transport.
    /// </summary>
    private ITransport? m_BravoTransport;
    /// <summary>
    /// The TCP replay and tamper simulator.
    /// </summary>
    private TcpAttackSimulator? m_AttackSimulator;

    /// <summary>
    /// Runs the demonstration application.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Unsupported main-menu selection.</exception>
    public async Task RunAsync()
    {
        Terminal.Title = $"Mercury Console Demonstration - {RELEASE_LABEL}";
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
            await DisposeTransportsAsync();
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
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.SecurityDiagnostics, "Run Security Demonstration"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.RunTestMatrix, "Run Provider / Transport / Codec Matrix"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.ViewTelemetry, "View Telemetry"),
            new ConsoleMenuItem<MainMenuSelection>(MainMenuSelection.Exit, "Exit Mercury", isEscapeSelection: true));

        return menu.Display();
    }

    /// <summary>
    /// Displays the active configuration.
    /// </summary>
    private void DisplayConfiguration()
    {
        ConsoleScreen.WriteSection("Active Configuration");
        ConsoleScreen.WriteLabel("Release", $"Mercury {RELEASE_LABEL} ({RELEASE_VERSION})", ConsoleTheme.SUCCESS);
        ConsoleScreen.WriteLabel("Platform", GetPlatformName(), ConsoleTheme.SUCCESS);
        ConsoleScreen.WriteLabel("Provider", m_Configuration.CryptoProvider, ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Transport", m_Configuration.Transport, ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Codec", m_Configuration.EnvelopeCodec);
        ConsoleScreen.WriteLabel("Chunking", m_Configuration.ChunkingEnabled
                ? $"Enabled / {FormatByteSize(m_Configuration.ChunkSize)}"
                : "Disabled",
            m_Configuration.ChunkingEnabled ? ConsoleTheme.SUCCESS : ConsoleTheme.WARNING);
        ConsoleScreen.WriteLabel("TCP Attacks", m_Configuration.Transport == TCP_TRANSPORT
                ? "Replay and protected-payload tamper available"
                : "Select TCP to enable replay or tamper",
            m_Configuration.Transport == TCP_TRANSPORT ? ConsoleTheme.SUCCESS : ConsoleTheme.MUTED);

        ConsoleScreen.WriteSectionEnd();
        Terminal.WriteLine();
    }

    /// <summary>
    /// Releases the controller and active transports.
    /// </summary>
    /// <returns>A value task representing the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await DisposeTransportsAsync();
    }
}
