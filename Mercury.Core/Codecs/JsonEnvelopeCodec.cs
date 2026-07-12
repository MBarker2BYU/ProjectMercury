// ***********************************************************************
// Assembly       : Mercury.Core
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="JsonEnvelopeCodec.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;

namespace Mercury.Core.Codecs;

/// <summary>
/// JSON envelope codec intended for demonstrations and diagnostics.
/// Binary values are encoded using Base64.
/// This class cannot be inherited.
/// </summary>
internal sealed class JsonEnvelopeCodec : IEnvelopeCodec
{
    /// <summary>
    /// The serializer options.
    /// </summary>
    private static readonly JsonSerializerOptions sm_Options =
        new JsonSerializerOptions
        {
            PropertyNamingPolicy =
                JsonNamingPolicy.CamelCase,

            WriteIndented = false,

            DefaultIgnoreCondition =
                JsonIgnoreCondition.WhenWritingNull
        };

    /// <summary>
    /// The envelope service.
    /// </summary>
    private readonly IEnvelopeService m_EnvelopeService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="JsonEnvelopeCodec"/> class.
    /// </summary>
    /// <param name="envelopeService">
    /// The envelope service.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// envelopeService
    /// </exception>
    internal JsonEnvelopeCodec(
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
    /// <returns>The encoded JSON frame.</returns>
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

        var envelopeDto =
            EnvelopeDataTransferObject.FromEnvelope(
                envelope);

        var json =
            JsonSerializer.Serialize(
                envelopeDto,
                sm_Options);

        var encodedData =
            Encoding.UTF8.GetBytes(
                json);

        return new ReadOnlyMemory(
            encodedData);
    }

    /// <summary>
    /// Decodes the specified JSON frame.
    /// </summary>
    /// <param name="data">The encoded JSON frame.</param>
    /// <returns>The decoded secure envelope.</returns>
    /// <exception cref="ArgumentException">
    /// Data cannot be empty.
    /// </exception>
    /// <exception cref="FormatException">
    /// The JSON envelope is invalid.
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

        var json =
            Encoding.UTF8.GetString(
                data.ToArray());

        EnvelopeDataTransferObject? envelopeDataTransferObject;

        try
        {
            envelopeDataTransferObject =
                JsonSerializer.Deserialize<EnvelopeDataTransferObject>(
                    json,
                    sm_Options);
        }
        catch (JsonException exception)
        {
            throw new FormatException(
                "The JSON envelope could not be deserialized.",
                exception);
        }

        if (envelopeDataTransferObject == null)
        {
            throw new FormatException(
                "The JSON envelope could not be deserialized.");
        }

        ValidateVersion(
            envelopeDataTransferObject.VersionMajor,
            envelopeDataTransferObject.VersionMinor);

        if (envelopeDataTransferObject.Header == null)
        {
            throw new FormatException(
                "The JSON envelope does not contain a header.");
        }

        if (envelopeDataTransferObject.Footer == null)
        {
            throw new FormatException(
                "The JSON envelope does not contain a footer.");
        }

        var header =
            envelopeDataTransferObject.Header.ToHeader(
                m_EnvelopeService);

        var footer =
            envelopeDataTransferObject.Footer.ToFooter(
                m_EnvelopeService);

        var protectedPayload =
            DecodeBase64(
                envelopeDataTransferObject.ProtectedPayloadBase64,
                "protected payload");

        var result =
            m_EnvelopeService.PackEnvelope(
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

    /// <summary>
    /// Validates the decoded framework version.
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
    /// Decodes a Base64 value.
    /// </summary>
    /// <param name="value">The Base64 value.</param>
    /// <param name="fieldName">The field name.</param>
    /// <returns>The decoded bytes.</returns>
    private static ReadOnlyMemory DecodeBase64(
        string value,
        string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ReadOnlyMemory.Empty;
        }

        try
        {
            return new ReadOnlyMemory(
                Convert.FromBase64String(
                    value));
        }
        catch (FormatException exception)
        {
            throw new FormatException(
                string.Format(
                    "The JSON envelope contains invalid Base64 data for {0}.",
                    fieldName),
                exception);
        }
    }

    /// <summary>
    /// Converts metadata into a serializable dictionary.
    /// </summary>
    /// <param name="metadata">The metadata.</param>
    /// <returns>The dictionary.</returns>
    private static Dictionary<string, string>
        ToDictionary(
            Metadata metadata)
    {
        var values =
            new Dictionary<string, string>(
                StringComparer.Ordinal);

        if (metadata == null)
        {
            return values;
        }

        foreach (var item
                 in metadata)
        {
            values[item.Key] =
                item.Value;
        }

        return values;
    }

    /// <summary>
    /// Converts a dictionary into Mercury metadata.
    /// </summary>
    /// <param name="values">The dictionary.</param>
    /// <returns>The metadata.</returns>
    private static Metadata ToMetadata(
        Dictionary<string, string> values)
    {
        var metadata =
            new Metadata();

        if (values == null)
        {
            return metadata;
        }

        foreach (var item
                 in values)
        {
            metadata.Add(
                item.Key,
                item.Value);
        }

        return metadata;
    }

    /// <summary>
    /// JSON representation of a secure envelope.
    /// </summary>
    private sealed class EnvelopeDataTransferObject
    {
        /// <summary>
        /// Gets or sets the major framework version.
        /// </summary>
        public byte VersionMajor { get; set; }

        /// <summary>
        /// Gets or sets the minor framework version.
        /// </summary>
        public byte VersionMinor { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public HeaderDataTransferObject Header { get; set; }
            = new();

        /// <summary>
        /// Gets or sets the protected payload.
        /// </summary>
        public string ProtectedPayloadBase64 { get; set; }
            = string.Empty;

        /// <summary>
        /// Gets or sets the footer.
        /// </summary>
        public FooterDataTransferObject Footer { get; set; }
            = new();

        /// <summary>
        /// Creates a DTO from the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The envelope DTO.</returns>
        public static EnvelopeDataTransferObject FromEnvelope(
            ISecureEnvelope envelope)
        {
            return new EnvelopeDataTransferObject
            {
                VersionMajor =
                    envelope.Version.Major,

                VersionMinor =
                    envelope.Version.Minor,

                Header =
                    HeaderDataTransferObject.FromHeader(
                        envelope.Header),

                ProtectedPayloadBase64 =
                    Convert.ToBase64String(
                        envelope.Payload.ToArray()),

                Footer =
                    FooterDataTransferObject.FromFooter(
                        envelope.Footer)
            };
        }
    }

    /// <summary>
    /// JSON representation of an envelope header.
    /// </summary>
    private sealed class HeaderDataTransferObject
    {
        /// <summary>
        /// Gets or sets the envelope identifier.
        /// </summary>
        public string EnvelopeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public long TimestampUnixMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the sender key identifier.
        /// </summary>
        public string SenderKeyId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient key identifier.
        /// </summary>
        public string RecipientKeyId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the encryption algorithm.
        /// </summary>
        public string Encryption { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signature algorithm.
        /// </summary>
        public string Signature { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the replay token.
        /// </summary>
        public string ReplayTokenBase64 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the header metadata.
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
            = new(StringComparer.Ordinal);

        /// <summary>
        /// Creates a DTO from the specified header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>The header DTO.</returns>
        public static HeaderDataTransferObject FromHeader(
            IEnvelopeHeader header)
        {
            return new HeaderDataTransferObject
            {
                EnvelopeId =
                    header.EnvelopeId.Value,

                TimestampUnixMilliseconds =
                    header.Timestamp
                        .ToUnixTimeMilliseconds(),

                SenderKeyId =
                    header.SenderKeyId.Value,

                RecipientKeyId =
                    header.RecipientKeyId.Value,

                Encryption =
                    header.Encryption.Value,

                Signature =
                    header.Signature.Value,

                ReplayTokenBase64 =
                    Convert.ToBase64String(
                        header.ReplayToken.ToArray()),

                Meta =
                    ToDictionary(
                        header.Meta)
            };
        }

        /// <summary>
        /// Converts the DTO into an envelope header.
        /// </summary>
        /// <param name="envelopeService">
        /// The envelope service.
        /// </param>
        /// <returns>The envelope header.</returns>
        public IEnvelopeHeader ToHeader(
            IEnvelopeService envelopeService)
        {
            return envelopeService
                .BuildEnvelopeHeader(
                    new KeyId(
                        EnvelopeId),

                    DateTimeOffset
                        .FromUnixTimeMilliseconds(
                            TimestampUnixMilliseconds),

                    new KeyId(
                        SenderKeyId),

                    new KeyId(
                        RecipientKeyId),

                    new AlgorithmId(
                        Encryption),

                    new AlgorithmId(
                        Signature),

                    DecodeBase64(
                        ReplayTokenBase64,
                        "replay token"),

                    ToMetadata(
                        Meta));
        }
    }

    /// <summary>
    /// JSON representation of an envelope footer.
    /// </summary>
    private sealed class FooterDataTransferObject
    {
        /// <summary>
        /// Gets or sets the footer metadata.
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
            = new(StringComparer.Ordinal);

        /// <summary>
        /// Creates a DTO from the specified footer.
        /// </summary>
        /// <param name="footer">The footer.</param>
        /// <returns>The footer DTO.</returns>
        public static FooterDataTransferObject FromFooter(
            IEnvelopeFooter footer)
        {
            return new FooterDataTransferObject
            {
                Meta =
                    ToDictionary(
                        footer.Meta)
            };
        }

        /// <summary>
        /// Converts the DTO into an envelope footer.
        /// </summary>
        /// <param name="envelopeService">
        /// The envelope service.
        /// </param>
        /// <returns>The envelope footer.</returns>
        public IEnvelopeFooter ToFooter(
            IEnvelopeService envelopeService)
        {
            return envelopeService
                .BuildEnvelopeFooter(
                    ToMetadata(
                        Meta));
        }
    }
}