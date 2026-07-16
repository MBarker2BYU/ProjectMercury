// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Demo.Console.Enums;
using Mercury.Demo.Console.Interface;
using Mercury.Demo.Console.Models;
using Mercury.Demo.Console.Wrappers;

using Terminal = System.Console;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
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
    /// The Configuration
    /// </summary>
    private readonly DemoConfiguration m_Configuration = new();
    /// <summary>
    /// The Telemetry store
    /// </summary>
    private readonly TelemetryStore m_TelemetryStore = new();

    /// <summary>
    /// The Alpha client
    /// </summary>
    private IMercuryClient? m_AlphaClient;
    /// <summary>
    /// The Bravo client
    /// </summary>
    private IMercuryClient? m_BravoClient;

    /// <summary>
    /// The Alpha capture transport
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
}
