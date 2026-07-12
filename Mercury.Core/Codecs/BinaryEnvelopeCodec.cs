// ***********************************************************************
// Assembly       : Mercury.Core
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="BinaryEnvelopeCodec.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;

namespace Mercury.Core.Codecs;

/// <summary>
/// Encodes and decodes Mercury secure envelopes using a binary format.
/// This class cannot be inherited.
/// </summary>
internal sealed class BinaryEnvelopeCodec : IEnvelopeCodec
{
    /// <summary>
    /// Prototype-compatible envelope magic.
    /// </summary>
    private static readonly byte[] sm_Magic =
    [
        (byte)'M',
        (byte)'E',
        (byte)'R',
        (byte)'1'
    ];

    /// <summary>
    /// Maximum encoded string length.
    /// </summary>
    private const uint MAX_STRING_BYTES = 1024 * 1024;

    /// <summary>
    /// Maximum encoded binary value length.
    /// </summary>
    private const uint MAX_BLOB_BYTES = 8 * 1024 * 1024;

    /// <summary>
    /// Maximum number of metadata entries.
    /// </summary>
    private const uint MAX_METADATA_ENTRIES = 100_000;

    /// <summary>
    /// The envelope service.
    /// </summary>
    private readonly IEnvelopeService m_EnvelopeService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="BinaryEnvelopeCodec"/> class.
    /// </summary>
    /// <param name="envelopeService">The envelope service.</param>
    /// <exception cref="ArgumentNullException">
    /// envelopeService
    /// </exception>
    internal BinaryEnvelopeCodec(
        IEnvelopeService envelopeService)
    {
        m_EnvelopeService = envelopeService
            ?? throw new ArgumentNullException(
                nameof(envelopeService));
    }

    /// <summary>
    /// Encodes the specified envelope.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>The encoded envelope frame.</returns>
    /// <exception cref="ArgumentNullException">
    /// envelope
    /// </exception>
    public ReadOnlyMemory Encode(
        ISecureEnvelope envelope)
    {
        if (envelope == null)
        {
            throw new ArgumentNullException(
                nameof(envelope));
        }

        using (var memoryStream =
               new MemoryStream(
                   256 + envelope.Payload.Length))
        using (var writer =
               new BinaryWriter(
                   memoryStream,
                   Encoding.UTF8,
                   true))
        {
            writer.Write(sm_Magic);

            writer.Write(envelope.Version.Major);
            writer.Write(envelope.Version.Minor);

            WriteString(
                writer,
                envelope.Header.EnvelopeId.Value);

            writer.Write(
                envelope.Header.Timestamp
                    .ToUnixTimeMilliseconds());

            WriteString(
                writer,
                envelope.Header.SenderKeyId.Value);

            WriteString(
                writer,
                envelope.Header.RecipientKeyId.Value);

            WriteString(
                writer,
                envelope.Header.Encryption.Value);

            WriteString(
                writer,
                envelope.Header.Signature.Value);

            WriteBytes(
                writer,
                envelope.Header.ReplayToken);

            WriteMetadata(
                writer,
                envelope.Header.Meta);

            WriteBytes(
                writer,
                envelope.Payload);

            WriteMetadata(
                writer,
                envelope.Footer.Meta);

            writer.Flush();

            return new ReadOnlyMemory(
                memoryStream.ToArray());
        }
    }

    /// <summary>
    /// Decodes the specified frame.
    /// </summary>
    /// <param name="data">The encoded envelope frame.</param>
    /// <returns>The decoded secure envelope.</returns>
    /// <exception cref="ArgumentException">
    /// Data cannot be empty.
    /// </exception>
    /// <exception cref="FormatException">
    /// The encoded envelope is invalid.
    /// </exception>
    public ISecureEnvelope Decode(
        ReadOnlyMemory data)
    {
        if (data.IsEmpty)
        {
            throw new ArgumentException(
                "Data cannot be empty.",
                nameof(data));
        }

        var encodedData = data.ToArray();

        if (encodedData.Length < 6)
        {
            throw new FormatException(
                "Binary envelope is too small.");
        }

        using (var memoryStream =
               new MemoryStream(
                   encodedData,
                   false))
        using (var reader =
               new BinaryReader(
                   memoryStream,
                   Encoding.UTF8,
                   true))
        {
            ValidateMagic(reader);

            var major = reader.ReadByte();
            var minor = reader.ReadByte();

            ValidateVersion(
                major,
                minor);

            var envelopeId =
                ReadString(reader);

            var timestampMilliseconds =
                reader.ReadInt64();

            var senderKeyId =
                ReadString(reader);

            var recipientKeyId =
                ReadString(reader);

            var encryption =
                ReadString(reader);

            var signature =
                ReadString(reader);

            var replayToken =
                ReadBytes(reader);

            var headerMetadata =
                ReadMetadata(reader);

            var protectedPayload =
                ReadBytes(reader);

            var footerMetadata =
                ReadMetadata(reader);

            if (memoryStream.Position !=
                memoryStream.Length)
            {
                throw new FormatException(
                    "Binary envelope contains unexpected trailing data.");
            }

            var header =
                m_EnvelopeService
                    .BuildEnvelopeHeader(
                        new KeyId(envelopeId),
                        DateTimeOffset
                            .FromUnixTimeMilliseconds(
                                timestampMilliseconds),
                        new KeyId(senderKeyId),
                        new KeyId(recipientKeyId),
                        new AlgorithmId(encryption),
                        new AlgorithmId(signature),
                        replayToken,
                        headerMetadata);

            var footer =
                m_EnvelopeService
                    .BuildEnvelopeFooter(
                        footerMetadata);

            var result =
                m_EnvelopeService
                    .PackEnvelope(
                        header,
                        protectedPayload,
                        footer);

            if (!result.Success ||
                result.ValidatedEnvelope == null)
            {
                throw new FormatException(
                    result.Message ??
                    "The decoded envelope could not be constructed.");
            }

            return result.ValidatedEnvelope;
        }
    }

    /// <summary>
    /// Validates the envelope magic.
    /// </summary>
    /// <param name="reader">The binary reader.</param>
    private static void ValidateMagic(
        BinaryReader reader)
    {
        var magic = reader.ReadBytes(4);

        if (magic.Length != 4 ||
            magic[0] != sm_Magic[0] ||
            magic[1] != sm_Magic[1] ||
            magic[2] != sm_Magic[2] ||
            magic[3] != sm_Magic[3])
        {
            throw new FormatException(
                "Invalid binary envelope magic.");
        }
    }

    /// <summary>
    /// Validates the envelope version.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    private static void ValidateVersion(
        byte major,
        byte minor)
    {
        if (major != FrameworkVersion.V1.Major ||
            minor != FrameworkVersion.V1.Minor)
        {
            throw new FormatException(
                string.Format(
                    "Unsupported envelope version: {0}.{1}.",
                    major,
                    minor));
        }
    }

    /// <summary>
    /// Writes a UTF-8 string.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    private static void WriteString(
        BinaryWriter writer,
        string value)
    {
        var safeValue =
            value ?? string.Empty;

        var bytes =
            Encoding.UTF8.GetBytes(
                safeValue);

        if ((uint)bytes.Length >
            MAX_STRING_BYTES)
        {
            throw new InvalidOperationException(
                "String is too large to encode.");
        }

        WriteUInt32(
            writer,
            (uint)bytes.Length);

        writer.Write(bytes);
    }

    /// <summary>
    /// Reads a UTF-8 string.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>The decoded string.</returns>
    private static string ReadString(
        BinaryReader reader)
    {
        var length =
            ReadUInt32(reader);

        if (length > MAX_STRING_BYTES ||
            length > int.MaxValue)
        {
            throw new FormatException(
                "String is too large.");
        }

        var bytes =
            reader.ReadBytes(
                (int)length);

        if (bytes.Length !=
            (int)length)
        {
            throw new EndOfStreamException(
                "Unexpected end of envelope while reading a string.");
        }

        return Encoding.UTF8.GetString(
            bytes);
    }

    /// <summary>
    /// Writes a binary value.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    private static void WriteBytes(
        BinaryWriter writer,
        ReadOnlyMemory value)
    {
        var bytes =
            value.ToArray();

        if ((uint)bytes.Length >
            MAX_BLOB_BYTES)
        {
            throw new InvalidOperationException(
                "Binary value is too large to encode.");
        }

        WriteUInt32(
            writer,
            (uint)bytes.Length);

        if (bytes.Length > 0)
        {
            writer.Write(bytes);
        }
    }

    /// <summary>
    /// Reads a binary value.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>The decoded value.</returns>
    private static ReadOnlyMemory ReadBytes(
        BinaryReader reader)
    {
        var length =
            ReadUInt32(reader);

        if (length > MAX_BLOB_BYTES ||
            length > int.MaxValue)
        {
            throw new FormatException(
                "Binary value is too large.");
        }

        var bytes =
            reader.ReadBytes(
                (int)length);

        if (bytes.Length !=
            (int)length)
        {
            throw new EndOfStreamException(
                "Unexpected end of envelope while reading binary data.");
        }

        return new ReadOnlyMemory(
            bytes);
    }

    /// <summary>
    /// Writes metadata.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="metadata">The metadata.</param>
    private static void WriteMetadata(
        BinaryWriter writer,
        Metadata metadata)
    {
        if (metadata == null ||
            metadata.Count == 0)
        {
            WriteUInt32(
                writer,
                0);

            return;
        }

        if ((uint)metadata.Count >
            MAX_METADATA_ENTRIES)
        {
            throw new InvalidOperationException(
                "Metadata contains too many entries.");
        }

        WriteUInt32(
            writer,
            (uint)metadata.Count);

        foreach (var item
                 in metadata)
        {
            WriteString(
                writer,
                item.Key);

            WriteString(
                writer,
                item.Value);
        }
    }

    /// <summary>
    /// Reads metadata.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>The decoded metadata.</returns>
    private static Metadata ReadMetadata(
        BinaryReader reader)
    {
        var count =
            ReadUInt32(reader);

        if (count >
            MAX_METADATA_ENTRIES)
        {
            throw new FormatException(
                "Metadata contains too many entries.");
        }

        var metadata =
            new Metadata();

        for (uint index = 0;
             index < count;
             index++)
        {
            var key =
                ReadString(reader);

            var value =
                ReadString(reader);

            metadata.Add(
                key,
                value);
        }

        return metadata;
    }

    /// <summary>
    /// Writes a big-endian unsigned 32-bit value.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    private static void WriteUInt32(
        BinaryWriter writer,
        uint value)
    {
        var bytes = new byte[4];

        bytes[0] =
            (byte)(value >> 24);

        bytes[1] =
            (byte)(value >> 16);

        bytes[2] =
            (byte)(value >> 8);

        bytes[3] =
            (byte)value;

        writer.Write(bytes);
    }

    /// <summary>
    /// Reads a big-endian unsigned 32-bit value.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>The value.</returns>
    private static uint ReadUInt32(
        BinaryReader reader)
    {
        var bytes =
            reader.ReadBytes(4);

        if (bytes.Length != 4)
        {
            throw new EndOfStreamException(
                "Unexpected end of envelope while reading a length value.");
        }

        return
            ((uint)bytes[0] << 24) |
            ((uint)bytes[1] << 16) |
            ((uint)bytes[2] << 8) |
            bytes[3];
    }
}