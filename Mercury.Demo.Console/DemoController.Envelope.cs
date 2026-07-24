// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-24-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DemoController.Envelope.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Primitives;
using Mercury.Demo.Console.Interface;

using Terminal = System.Console;

namespace Mercury.Demo.Console;

/// <summary>
/// Coordinates the Mercury RC 1.2 console demonstration application.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Displays the validated SecureEnvelope returned by Mercury.
    /// </summary>
    /// <param name="result">The successful Mercury result.</param>
    private static void DisplayValidatedEnvelope(IMercuryResult result)
    {
        var secureEnvelope = result.ValidatedEnvelope ??
            throw new InvalidOperationException("Mercury reported success but did not return a validated SecureEnvelope.");

        Terminal.WriteLine();
        ConsoleScreen.WriteSection("Validated SecureEnvelope");
        Terminal.WriteLine();

        ConsoleScreen.WriteLabel("Version", secureEnvelope.Version.ToString(), ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Envelope ID", secureEnvelope.Header.EnvelopeId.ToString());
        ConsoleScreen.WriteLabel("Timestamp", secureEnvelope.Header.Timestamp.ToString());
        ConsoleScreen.WriteLabel("Sender", secureEnvelope.Header.SenderKeyId.ToString());
        ConsoleScreen.WriteLabel("Recipient", secureEnvelope.Header.RecipientKeyId.ToString());
        ConsoleScreen.WriteLabel("Encryption", secureEnvelope.Header.Encryption.ToString(), ConsoleTheme.SUCCESS);
        ConsoleScreen.WriteLabel("Signature", secureEnvelope.Header.Signature.ToString());
        ConsoleScreen.WriteLabel("Replay Token", Convert.ToHexString(secureEnvelope.Header.ReplayToken.ToArray()));
        ConsoleScreen.WriteLabel("Protected Data", $"{secureEnvelope.Payload.Length:N0} bytes", ConsoleTheme.PRIMARY);
        ConsoleScreen.WriteLabel("Header Meta", FormatMetadata(secureEnvelope.Header.Meta));
        ConsoleScreen.WriteLabel("Footer Meta", FormatMetadata(secureEnvelope.Footer.Meta));

        Terminal.WriteLine();
        ConsoleTheme.WriteLine("  PROTECTED PAYLOAD PREVIEW", ConsoleTheme.SECONDARY);
        Terminal.WriteLine();

        var protectedBytes = secureEnvelope.Payload.ToArray();
        var previewLength = Math.Min(protectedBytes.Length, 96);
        var preview = Convert.ToHexString(protectedBytes.AsSpan(0, previewLength));

        WriteHexPreview($"{preview}{(protectedBytes.Length > previewLength ? "..." : string.Empty)}");

        Terminal.WriteLine();
        ConsoleScreen.WriteSectionEnd();
    }

    /// <summary>
    /// Formats Mercury metadata for console display.
    /// </summary>
    /// <param name="metadata">The metadata.</param>
    /// <returns>The formatted metadata.</returns>
    private static string FormatMetadata(Metadata metadata)
    {
        if (metadata.Count == 0)
            return "None";

        return string.Join("; ", metadata.Select(item => $"{item.Key}: {item.Value}"));
    }

    /// <summary>
    /// Formats a failed Mercury result.
    /// </summary>
    /// <param name="result">The Mercury result.</param>
    /// <returns>The failure reason and message.</returns>
    private static string FormatFailure(IMercuryResult result)
    {
        var message = string.IsNullOrWhiteSpace(result.Message) ? string.Empty : $" - {result.Message}";
        return $"{result.FailureReason}{message}";
    }
}
