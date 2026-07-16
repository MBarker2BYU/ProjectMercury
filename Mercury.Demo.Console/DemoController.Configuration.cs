// ***********************************************************************
// Assembly      : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.Configuration.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Demo.Console.Enums;
using Mercury.Demo.Console.Interface;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
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
        const string enabled = "Enabled";
        const string disabled = "Disabled";

        var menu = new ConsoleMenu<string>(
            "Configure Chunking",
            new ConsoleMenuItem<string>(enabled, enabled),
            new ConsoleMenuItem<string>(disabled, disabled),
            new ConsoleMenuItem<string>(RETURN, RETURN, isEscapeSelection: true));

        var selection = DisplayConfigurationMenu(menu);

        if (selection == enabled)
            m_Configuration.ChunkingEnabled = true;
        else if (selection == disabled)
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
}
