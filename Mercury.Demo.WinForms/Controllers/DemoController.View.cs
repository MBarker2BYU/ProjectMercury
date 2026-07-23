// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.View.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using Mercury.Abstractions;
using Mercury.Abstractions.Primitives;
using Mercury.Demo.WinForms.Demo;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// Chunking the changed.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    public void ChunkingChanged(bool enabled)
    {
        m_View.SetChunkingControlsEnabled(enabled);
    }

    /// <summary>
    /// Builds the envelope display.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns>DemoEnvelopeDisplay.</returns>
    /// <exception cref="InvalidOperationException">Mercury reported success but did not return a validated SecureEnvelope.</exception>
    private static DemoEnvelopeDisplay BuildEnvelopeDisplay(
        IMercuryResult result)
    {
        var secureEnvelope =
            result.ValidatedEnvelope ??
            throw new InvalidOperationException(
                "Mercury reported success but did not return a validated SecureEnvelope.");

        return new DemoEnvelopeDisplay(
            Version:
            secureEnvelope.Version.ToString(),

            EnvelopeId:
            secureEnvelope.Header.EnvelopeId.ToString(),

            Algorithm:
            secureEnvelope.Header.Encryption.ToString(),

            ReplayToken:
            Convert.ToHexString(
                secureEnvelope.Header.ReplayToken.ToArray()),

            ProtectedPayloadSize:
            $"{secureEnvelope.Payload.Length:N0} bytes",

            TotalFrameSize:
            "NOT AVAILABLE",

            RawPayloadVisible:
            "NO",

            HeaderMetadata:
            FormatMetadata(
                secureEnvelope.Header.Meta),

            FooterMetadata:
            FormatMetadata(
                secureEnvelope.Footer.Meta),

            HexPreview:
            FormatHexDump(secureEnvelope.Payload));
    }

    private static string FormatMetadata(Metadata metadata)
    {
        if (metadata.Count == 0)
            return string.Empty;

        return string.Join(Environment.NewLine, metadata.Select(item => $"{item.Key}: {item.Value}"));
    }

    private static string GetFailureMessage(IMercuryResult result)
    {
        return result.Message ??
               result.FailureReason.ToString();
    }

    private static string FormatHexDump(
        ReadOnlyMemory payload)
    {
        if (payload.IsEmpty)
            return string.Empty;

        const int bytesPerLine = 16;

        var bytes =
            payload.ToArray();

        var builder =
            new StringBuilder(
                bytes.Length * 4);

        for (var offset = 0;
             offset < bytes.Length;
             offset += bytesPerLine)
        {
            var count =
                Math.Min(
                    bytesPerLine,
                    bytes.Length - offset);

            builder.Append(
                offset.ToString("X8"));

            builder.Append("  ");

            for (var index = 0;
                 index < bytesPerLine;
                 index++)
            {
                if (index < count)
                {
                    builder.Append(
                        bytes[offset + index]
                            .ToString("X2"));

                    builder.Append(' ');
                }
                else
                {
                    builder.Append("   ");
                }

                if (index == 7)
                    builder.Append(' ');
            }

            builder.Append(" |");

            for (var index = 0;
                 index < count;
                 index++)
            {
                var value =
                    bytes[offset + index];

                builder.Append(
                    value is >= 32 and <= 126
                        ? (char)value
                        : '.');
            }

            builder.AppendLine("|");
        }

        return builder.ToString();
    }
}