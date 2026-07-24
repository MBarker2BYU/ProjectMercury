// ***********************************************************************
// Assembly     : Mercury.Abstractions
// Author         : Matthew D. Barker
// Created        : 07-23-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="AuthenticatedEnvelopeData.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Shared;

/// <summary>
/// Builds the canonical additional authenticated data used by
/// Mercury authenticated-encryption providers.
/// </summary>
/// <remarks>
/// Values are encoded in a fixed order using length-prefixed binary
/// fields. Metadata entries are sorted using ordinal key comparison.
/// </remarks>
public static class AuthenticatedEnvelopeData
{
    /// <summary>
    /// Identifies the Mercury authenticated-envelope data format.
    /// </summary>
    private static readonly byte[] sm_Magic =
    {
        (byte)'M',
        (byte)'A',
        (byte)'A',
        (byte)'D'
    };

    /// <summary>
    /// The authenticated-envelope data format version.
    /// </summary>
    private const byte FORMAT_VERSION = 1;

    /// <summary>
    /// Builds the canonical additional authenticated data.
    /// </summary>
    /// <param name="versionMajor">
    /// The major Mercury framework version.
    /// </param>
    /// <param name="versionMinor">
    /// The minor Mercury framework version.
    /// </param>
    /// <param name="header">
    /// The envelope header.
    /// </param>
    /// <param name="footer">
    /// The envelope footer.
    /// </param>
    /// <returns>
    /// The canonical additional authenticated data.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The header or footer is null.
    /// </exception>
    public static byte[] Build(byte versionMajor, byte versionMinor, IEnvelopeHeader header, IEnvelopeFooter footer)
    {
        if (header == null)
            throw new ArgumentNullException(nameof(header));
        

        if (footer == null)
            throw new ArgumentNullException(nameof(footer));


        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
        {
            writer.Write(sm_Magic);

            writer.Write(FORMAT_VERSION);

            writer.Write(versionMajor);

            writer.Write(versionMinor);

            WriteString(writer, header.EnvelopeId.Value);

            WriteInt64BigEndian(writer, header.Timestamp.ToUnixTimeMilliseconds());

            WriteString(writer, header.SenderKeyId.Value);

            WriteString(writer, header.RecipientKeyId.Value);

            WriteString(writer, header.Encryption.Value);

            WriteString(writer, header.Signature.Value);

            WriteBytes(writer, header.ReplayToken);

            WriteMetadata(writer, header.Meta);

            WriteMetadata(writer, footer.Meta);

            writer.Flush();
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Writes a UTF-8 string using a big-endian length prefix.
    /// </summary>
    /// <param name="writer">
    /// The binary writer.
    /// </param>
    /// <param name="value">
    /// The string value.
    /// </param>
    private static void WriteString(BinaryWriter writer, string value)
    {
        var safeValue = value ?? string.Empty;

        var bytes = Encoding.UTF8.GetBytes(safeValue);

        WriteUInt32BigEndian(writer, checked((uint)bytes.Length));

        if (bytes.Length > 0)
            writer.Write(bytes);
        
    }

    /// <summary>
    /// Writes a binary value using a big-endian length prefix.
    /// </summary>
    /// <param name="writer">
    /// The binary writer.
    /// </param>
    /// <param name="value">
    /// The binary value.
    /// </param>
    private static void WriteBytes(BinaryWriter writer, ReadOnlyMemory value)
    {
        var bytes = value.ToArray();

        WriteUInt32BigEndian(writer, checked((uint)bytes.Length));

        if (bytes.Length > 0)
            writer.Write(bytes);
        
    }

    /// <summary>
    /// Writes metadata in deterministic ordinal key order.
    /// </summary>
    /// <param name="writer">
    /// The binary writer.
    /// </param>
    /// <param name="metadata">
    /// The metadata collection.
    /// </param>
    private static void WriteMetadata(BinaryWriter writer, Metadata metadata)
    {
        if (metadata == null || metadata.Count == 0)
        {
            WriteUInt32BigEndian(writer, 0);

            return;
        }

        var orderedEntries = metadata
                .OrderBy(item => item.Key, StringComparer.Ordinal)
                .ToArray();

        WriteUInt32BigEndian(writer, checked((uint)orderedEntries.Length));

        foreach (var entry in orderedEntries)
        {
            WriteString(writer, entry.Key);

            WriteString(writer, entry.Value);
        }
    }

    /// <summary>
    /// Writes an unsigned 32-bit value in big-endian order.
    /// </summary>
    /// <param name="writer">
    /// The binary writer.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    private static void WriteUInt32BigEndian(BinaryWriter writer, uint value)
    {
        writer.Write((byte)(value >> 24));

        writer.Write((byte)(value >> 16));

        writer.Write((byte)(value >> 8));

        writer.Write((byte)value);
    }

    /// <summary>
    /// Writes a signed 64-bit value in big-endian order.
    /// </summary>
    /// <param name="writer">
    /// The binary writer.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    private static void WriteInt64BigEndian(BinaryWriter writer, long value)
    {
        var unsignedValue = unchecked((ulong)value);

        writer.Write((byte)(unsignedValue >> 56));

        writer.Write((byte)(unsignedValue >> 48));

        writer.Write((byte)(unsignedValue >> 40));

        writer.Write((byte)(unsignedValue >> 32));

        writer.Write((byte)(unsignedValue >> 24));

        writer.Write((byte)(unsignedValue >> 16));

        writer.Write((byte)(unsignedValue >> 8));

        writer.Write((byte)unsignedValue);
    }
}