// ***********************************************************************
// Assembly       : Mercury.Core.WinForms
// Author           : Matthew D. Barker
// Created          : 07-13-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-13-2026
// ***********************************************************************
// <copyright file="ConfigurationPanelControls.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.v1.Controls;
using Mercury.Demo.WinForms.v1.Interfaces;

namespace Mercury.Demo.WinForms.v1;

/// <summary>
/// Class ConfigurationPanelControls. This class cannot be inherited.
/// Implements the <see cref="System.IDisposable" />
/// </summary>
/// <param name="cryptoProviderCombo">The crypto provider combo.</param>
/// <param name="transportCombo">The transport combo.</param>
/// <param name="envelopeCodecCombo">The envelope codec combo.</param>
/// <param name="chunkingEnabled">The chunking enabled.</param>
/// <param name="chunkSizeCombo">The chunk size combo.</param>
/// <param name="loggerCombo">The logger combo.</param>
/// <param name="applyButton">The apply button.</param>
/// <seealso cref="System.IDisposable" />
public sealed class ConfigurationPanelControls(MercuryGlassComboBox cryptoProviderCombo, MercuryGlassComboBox transportCombo, 
    MercuryGlassComboBox envelopeCodecCombo, CheckBox chunkingEnabled, MercuryGlassComboBox chunkSizeCombo, 
    MercuryGlassComboBox loggerCombo, MercuryGlassButton applyButton) : IDisposable
{

    internal const int DEFAULT_CHUNK_SIZE = 64;
    internal const bool DEFAULT_CHUNKING_ENABLED = true;
    internal const bool DEFAULT_LOGGING_ENABLED = false;
    
    /// <summary>
    /// Occurs when [apply configuration] is pressed.
    /// </summary>
    public event EventHandler? ApplyConfiguration;

    /// <summary>
    /// Handles the <see cref="E:ApplyConfiguration" /> event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void OnApplyConfiguration(object? sender, EventArgs args)
        => ApplyConfiguration?.Invoke(sender, args);

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <returns>IResult.</returns>
    public IResult Initialize()
        => Reset();

    /// <summary>
    /// Loads the crypto provider combo.
    /// </summary>
    /// <returns>Result.</returns>
    private Result LoadCryptoProviderCombo()
    {
        try
        {
            CryptoProviderCombo.Items.Clear();

            CryptoProviderCombo.Items.Add(DemoController.AES_GCM);
            CryptoProviderCombo.Items.Add(DemoController.CHA_CHA_20);

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    /// <summary>
    /// Loads the transport combo.
    /// </summary>
    /// <returns>Result.</returns>
    private Result LoadTransportCombo()
    {
        try
        {
            TransportCombo.Items.Clear();

            TransportCombo.Items.Add(DemoController.IN_MEMORY_TRANSPORT);
            TransportCombo.Items.Add(DemoController.TCP_TRANSPORT);

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    /// <summary>
    /// Loads the envelope codec combo.
    /// </summary>
    /// <returns>Result.</returns>
    private Result LoadEnvelopeCodecCombo()
    {
        try
        {
            EnvelopeCodecCombo.Items.Clear();

            EnvelopeCodecCombo.Items.Add("Binary");
            EnvelopeCodecCombo.Items.Add("Json");

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    /// <summary>
    /// Loads the chunk size combo.
    /// </summary>
    /// <param name="chunkingEnabled">if set to <c>true</c> [chunking enabled].</param>
    /// <returns>Result.</returns>
    private Result LoadChunkSizeCombo(bool chunkingEnabled = DEFAULT_CHUNKING_ENABLED)
    {
        try
        {
            ChunkSizeCombo.Items.Clear();

            var chunkSize = 1;
            var defaultIndex = 0;

            while (true)
            {
                ChunkSizeCombo.Items.Add($"{chunkSize} KB");

                if (chunkSize < DEFAULT_CHUNK_SIZE)
                    defaultIndex += 1;

                if (chunkSize >= 1024)
                    break;

                chunkSize *= 2;
            }

            ChunkSizeCombo.SelectedIndex = defaultIndex;
            ChunkSizeCombo.Enabled = chunkingEnabled;

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    /// <summary>
    /// Handles the <see>
    ///     <cref>E:ChunkingEnabledChanged</cref>
    /// </see>
    /// event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void OnChunkingEnabledChanged(object? sender, EventArgs eventArgs)
    {
        if(sender is CheckBox checkBox)
            ChunkSizeCombo.Enabled = checkBox.Checked;
    }

    private Result LoadLoggerCombo(bool enabled = DEFAULT_LOGGING_ENABLED)
    {
        try
        {
            LoggerCombo.Items.Clear();
            
            LoggerCombo.Items.Add("None");
            LoggerCombo.Items.Add("Simple");
            LoggerCombo.Items.Add("Verbose");

            LoggerCombo.Enabled = DEFAULT_LOGGING_ENABLED;

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    /// <summary>
    /// Detaches the events.
    /// </summary>
    private void DetachEvents()
    {
        ChunkingEnabledCheckBox.CheckedChanged -= OnChunkingEnabledChanged;
        ApplyButton.Click -= OnApplyConfiguration;
    }

    /// <summary>
    /// Attaches the events.
    /// </summary>
    private void AttachEvents()
    {
        DetachEvents();

        ChunkingEnabledCheckBox.CheckedChanged += OnChunkingEnabledChanged;
        ApplyButton.Click += OnApplyConfiguration;
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    /// <returns>Mercury.Demo.WinForms.Interfaces.IResult.</returns>
    internal IResult Reset()
    {
        try
        {
            var result = LoadCryptoProviderCombo();

            if (result is { Success: false, Exception: not null })
                throw result.Exception;

            result = LoadTransportCombo();

            if (result is { Success: false, Exception: not null })
                throw result.Exception;

            result = LoadEnvelopeCodecCombo();

            if (result is { Success: false, Exception: not null })
                throw result.Exception;

            ChunkingEnabledCheckBox.Checked = DEFAULT_CHUNKING_ENABLED;

            result = LoadChunkSizeCombo();

            if (result is { Success: false, Exception: not null })
                throw result.Exception;

            result = LoadLoggerCombo();

            if (result is { Success: false, Exception: not null })
                throw result.Exception;

            AttachEvents();

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }



    /// <summary>
    /// Gets or sets a value indicating whether [logging enabled].
    /// </summary>
    /// <value><c>true</c> if [logging enabled]; otherwise, <c>false</c>.</value>
    public bool LoggingEnabled
    {
        get => LoggerCombo.Enabled;
        set => LoggerCombo.Enabled = value;
    }

    /// <summary>
    /// Gets the crypto provider combo.
    /// </summary>
    /// <value>The crypto provider combo.</value>
    public MercuryGlassComboBox CryptoProviderCombo { get; } = cryptoProviderCombo;
    /// <summary>
    /// Gets the transport combo.
    /// </summary>
    /// <value>The transport combo.</value>
    public MercuryGlassComboBox TransportCombo { get; } = transportCombo;
    /// <summary>
    /// Gets the envelope codec combo.
    /// </summary>
    /// <value>The envelope codec combo.</value>
    public MercuryGlassComboBox EnvelopeCodecCombo { get; } = envelopeCodecCombo;
    /// <summary>
    /// Gets the chunking enabled CheckBox.
    /// </summary>
    /// <value>The chunking enabled CheckBox.</value>
    public CheckBox ChunkingEnabledCheckBox { get; } = chunkingEnabled;

    /// <summary>
    /// Gets the chunk size combo.
    /// </summary>
    /// <value>The chunk size combo.</value>
    public MercuryGlassComboBox ChunkSizeCombo { get; } = chunkSizeCombo;
    /// <summary>
    /// Gets the logger combo.
    /// </summary>
    /// <value>The logger combo.</value>
    public MercuryGlassComboBox LoggerCombo { get; } = loggerCombo;
    /// <summary>
    /// Gets the apply button.
    /// </summary>
    /// <value>The apply button.</value>
    public MercuryGlassButton ApplyButton { get; } = applyButton;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        DetachEvents();

        ApplyConfiguration = null;
    }
}