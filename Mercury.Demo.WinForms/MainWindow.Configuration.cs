
// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="MainWindow.Configuration.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.Controllers;
using Mercury.Demo.WinForms.Controls;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Presentation;

namespace Mercury.Demo.WinForms;

/// <summary>
/// Class MainWindow.
/// Implements the <see cref="System.Windows.Forms.Form" />
/// </summary>
/// <seealso cref="System.Windows.Forms.Form" />
public partial class MainWindow
{
    /// <summary>
    /// Applies the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="connected">if set to <c>true</c> [connected].</param>
    internal void ApplyConfiguration(DemoConfiguration configuration, bool connected)
    {
        lblProviderValue.Text = configuration.CryptoProvider;

        lblTransportValue.Text = configuration.Transport;

        lblTransportState.Text = connected
                ? "CONNECTED"
                : "OFFLINE";

        lblTransportState.ForeColor =
            connected
                ? MercuryTheme.SuccessColor
                : MercuryTheme.FailureColor;

        lblSystemState.Text =
            connected
                ? "OPERATIONAL"
                : "OFFLINE";

        lblSystemState.ForeColor =
            connected
                ? MercuryTheme.SuccessColor
                : MercuryTheme.FailureColor;

        lblFlowTransport.Text = configuration.Transport
                .ToUpperInvariant();

        lblFlowProvider.Text = configuration.CryptoProvider
                .ToUpperInvariant();

        lblIntegrityState.Text = @"READY";
        lblIntegrityState.ForeColor = MercuryTheme.MutedColor;

        lblReplayState.Text = @"ACTIVE";
        lblReplayState.ForeColor = MercuryTheme.SuccessColor;

        lblTamperState.Text = @"CLEAN";
        lblTamperState.ForeColor = MercuryTheme.SuccessColor;

        SetStatusIndicator(picProviderCheck, connected);

        SetStatusIndicator(picTransportCheck, connected);

        SetStatusIndicator(picIntegrityCheck, connected);

        SetStatusIndicator(picReplayCheck, connected);

        SetStatusIndicator(picTamperCheck, connected);
    }

    /// <summary>
    /// Binds the configuration options.
    /// </summary>
    internal void BindConfigurationOptions()
    {
        BindItems(cboCryptoProvider, DemoController.CryptoProviders);

        BindItems(cboTransport, DemoController.Transports);

        BindItems(cboEnvelopeCodec, DemoController.EnvelopeCodecs);

        BindItems(cboLogging, DemoController.LoggingLevels);

        var chunkSizes = DemoController.ChunkSizes
                .Select(bytes => new ChunkSizeOption(bytes));

        BindItems(cboChunkSize, chunkSizes);

        cboCryptoProvider.Text =
            DemoConstants.AES_GCM;

        cboTransport.Text =
            DemoConstants.IN_MEMORY_TRANSPORT;

        cboEnvelopeCodec.Text =
            DemoConstants.BINARY_CODEC;

        cboLogging.Text =
            DemoConstants.VERBOSE_LOGGING;

        cboChunkSize.Text = "64 KB";

        tglChunking.Checked = true;

        SetChunkingControlsEnabled(true);
    }

    /// <summary>
    /// Builds the configuration.
    /// </summary>
    /// <returns>DemoConfiguration.</returns>
    internal DemoConfiguration BuildConfiguration()
    {
        var chunkSize = cboChunkSize.SelectedItem is ChunkSizeOption option
                    ? option.Bytes
                    : DemoConstants.DEFAULT_CHUNK_SIZE;

        return new DemoConfiguration(cboCryptoProvider.Text, cboTransport.Text, cboEnvelopeCodec.Text,
            tglChunking.Checked, chunkSize, cboLogging.Text);
    }

    /// <summary>
    /// Sets the TCP attack controls enabled.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    internal void SetTcpAttackControlsEnabled(
        bool enabled)
    {
        if (!enabled)
        {
            tglTamperAttack.Checked = false;
            tglReplayAttack.Checked = false;
        }

        tglTamperAttack.Enabled = enabled;
        tglReplayAttack.Enabled = enabled;
    }

    /// <summary>
    /// Sets the chunking controls enabled.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    internal void SetChunkingControlsEnabled(bool enabled)
    {
        cboChunkSize.Enabled = enabled;
        lblChunkSize.Enabled = enabled;
    }

    /// <summary>
    /// Binds the items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comboBox">The combo box.</param>
    /// <param name="items">The items.</param>
    /// <param name="defaultIndex">The default index.</param>
    private static void BindItems<T>(MercuryGlassComboBox comboBox, IEnumerable<T> items, int defaultIndex = 0)
    {
        comboBox.Items.Clear();

        foreach (var item in items)
        {
            if(item == null) continue;
            comboBox.Items.Add(item);
        }

        if (comboBox.Items.Count > 0)
        {
            comboBox.SelectedIndex = defaultIndex;
        }
    }
}