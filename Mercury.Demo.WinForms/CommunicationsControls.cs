// ***********************************************************************
// Assembly       : Mercury.Core.WinForms
// Author         : Matthew D. Barker
// Created        : 07-13-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-13-2026
// ***********************************************************************
// <copyright file="CommunicationsControls.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using Mercury.Demo.WinForms.Controls;
using Mercury.Demo.WinForms.Interfaces;

namespace Mercury.Demo.WinForms;

/// <summary>
/// Class CommunicationsControls. This class cannot be inherited.
/// Implements the <see cref="System.IDisposable" />
/// </summary>
/// <param name="senderTextBox">The sender text box.</param>
/// <param name="senderClearButton">The sender clear button.</param>
/// <param name="senderMarquee">The sender marquee.</param>
/// <param name="recipientTextBox">The recipient text box.</param>
/// <param name="recipientClearButton">The recipient clear button.</param>
/// <param name="recipientMarquee">The recipient marquee.</param>
/// <param name="sendButton">The send button.</param>
/// <seealso cref="System.IDisposable" />
internal sealed class CommunicationsControls(TextBox senderTextBox, MercuryGlassButton senderClearButton, Label senderPayloadSize, MercuryMarqueeLabel senderMarquee,
    TextBox recipientTextBox, MercuryGlassButton recipientClearButton, Label recipientPayloadSize, MercuryMarqueeLabel recipientMarquee, MercuryGlassButton sendButton) : IDisposable
{
    /// <summary>
    /// Occurs when [send requested].
    /// </summary>
    public event EventHandler? SendRequested;

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <returns>IResult.</returns>
    public IResult Initialize()
        => Reset();

    /// <summary>
    /// Resets this instance.
    /// </summary>
    /// <returns>IResult.</returns>
    public IResult Reset()
    {
        try
        {
            DetachEvents();

            SenderTextBox.Clear();
            RecipientTextBox.Clear();

            SenderMarqueeLabel.Visible = false;
            SenderMarqueeLabel.Enabled = false;

            RecipientMarqueeLabel.Visible = false;
            RecipientMarqueeLabel.Enabled = false;

            RecipientTextBox.ReadOnly = true;

            AttachEvents();

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    /// <summary>
    /// Handles the <see cref="E:SendRequested" /> event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void OnSendRequested(object? sender, EventArgs eventArgs)
    {
        SendRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles the <see>
    ///     <cref>E:SenderClear</cref>
    /// </see>
    /// event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void OnSenderClear(object? sender, EventArgs eventArgs)
    {
        SenderPayloadSize.Text =
            $"Payload Size: 0 bytes";

        SenderTextBox.Clear();
    }

    /// <summary>
    /// Handles the <see>
    ///     <cref>E:RecipientClear</cref>
    /// </see>
    /// event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void OnRecipientClear(object? sender, EventArgs eventArgs)
    {
        RecipientPayloadSize.Text =
            $"Payload Size: 0 bytes";

        RecipientTextBox.Clear();
    }

    /// <summary>
    /// Attaches the events.
    /// </summary>
    private void AttachEvents()
    {
        DetachEvents();

        SendButton.Click += OnSendRequested;
        SenderClearButton.Click += OnSenderClear;
        RecipientClearButton.Click += OnRecipientClear;
    }
    
    /// <summary>
    /// Detaches the events.
    /// </summary>
    private void DetachEvents()
    {
        SendButton.Click -= OnSendRequested;
        SenderClearButton.Click -= OnSenderClear;
        RecipientClearButton.Click -= OnRecipientClear;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        DetachEvents();
        SendRequested = null;
    }

    /// <summary>
    /// Gets the sender text box.
    /// </summary>
    /// <value>The sender text box.</value>
    public TextBox SenderTextBox { get; } = senderTextBox;

    /// <summary>
    /// Gets the sender clear button.
    /// </summary>
    /// <value>The sender clear button.</value>
    public MercuryGlassButton SenderClearButton { get; } =
        senderClearButton;

    /// <summary>
    /// Gets the size of the sender payload.
    /// </summary>
    /// <value>The size of the sender payload.</value>
    public Label SenderPayloadSize { get; } = senderPayloadSize;

    /// <summary>
    /// Gets the sender marquee label.
    /// </summary>
    /// <value>The sender marquee label.</value>
    public MercuryMarqueeLabel SenderMarqueeLabel { get; } =
        senderMarquee;

    /// <summary>
    /// Gets the recipient text box.
    /// </summary>
    /// <value>The recipient text box.</value>
    public TextBox RecipientTextBox { get; } =
        recipientTextBox;

    /// <summary>
    /// Gets the recipient clear button.
    /// </summary>
    /// <value>The recipient clear button.</value>
    public MercuryGlassButton RecipientClearButton { get; } =
        recipientClearButton;

    /// <summary>
    /// Gets the size of the recipient payload.
    /// </summary>
    /// <value>The size of the recipient payload.</value>
    public Label RecipientPayloadSize { get; } = recipientPayloadSize;

    /// <summary>
    /// Gets the recipient marquee label.
    /// </summary>
    /// <value>The recipient marquee label.</value>
    public MercuryMarqueeLabel RecipientMarqueeLabel { get; } =
        recipientMarquee;

    /// <summary>
    /// Gets the send button.
    /// </summary>
    /// <value>The send button.</value>
    public MercuryGlassButton SendButton { get; } =
        sendButton;
}