// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="EnvelopeCodecTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Tests.Support;
using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Codecs;

/// <summary>
/// Class EnvelopeCodecTests. This class cannot be inherited.
/// </summary>
public sealed class EnvelopeCodecTests
{
    /// <summary>
    /// Codecs the cases.
    /// </summary>
    /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
    public static IEnumerable<object[]> CodecCases()
    {
        yield return [EnvelopeCodec.Binary];
        yield return [EnvelopeCodec.Json];
    }

    /// <summary>
    /// Defines the test method EncodeThenDecode_PreservesEntireSecureEnvelope.
    /// </summary>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(CodecCases))]
    public async Task EncodeThenDecode_PreservesEntireSecureEnvelope(EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(ProviderKind.AesGcm);
        
        var headerMeta = new Metadata
        {
            { "route", "alpha-bravo" },
            { "correlation", "12345" }
        };
        
        var footerMeta = new Metadata
        {
            { "purpose", "codec-roundtrip" }
        };

        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload(2048),
            headerMeta: headerMeta, footerMeta: footerMeta);

        var codec = MercuryTestFactory.BuildCodec(codecType, provider);

        var frame = codec.Encode(envelope);
        var decoded = codec.Decode(frame);

        Assert.False(frame.IsEmpty);
        Assert.Equal(envelope.Version.Major, decoded.Version.Major);
        Assert.Equal(envelope.Version.Minor, decoded.Version.Minor);
        Assert.Equal(envelope.Header.EnvelopeId,
            decoded.Header.EnvelopeId);
        Assert.Equal(envelope.Header.Timestamp,
            decoded.Header.Timestamp);
        Assert.Equal(envelope.Header.SenderKeyId,
            decoded.Header.SenderKeyId);
        Assert.Equal(envelope.Header.RecipientKeyId,
            decoded.Header.RecipientKeyId);
        Assert.Equal(envelope.Header.Encryption,
            decoded.Header.Encryption);
        Assert.Equal(envelope.Header.Signature,
            decoded.Header.Signature);
        Assert.Equal(envelope.Header.ReplayToken.ToArray(),
            decoded.Header.ReplayToken.ToArray());
        Assert.Equal(envelope.Payload.ToArray(),
            decoded.Payload.ToArray());
        Assert.Equal("alpha-bravo",
            decoded.Header.Meta["route"]);
        Assert.Equal("12345",
            decoded.Header.Meta["correlation"]);
        Assert.Equal("codec-roundtrip",
            decoded.Footer.Meta["purpose"]);
    }

    /// <summary>
    /// Defines the test method Encode_NullEnvelope_ThrowsArgumentNullException.
    /// </summary>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(CodecCases))]
    public void Encode_NullEnvelope_ThrowsArgumentNullException(EnvelopeCodec codecType)
    {
        var codec = MercuryTestFactory.BuildCodec(codecType);

        Assert.Throws<ArgumentNullException>(() => codec.Encode(null!));
    }

    /// <summary>
    /// Defines the test method Decode_EmptyFrame_ThrowsArgumentException.
    /// </summary>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(CodecCases))]
    public void Decode_EmptyFrame_ThrowsArgumentException(EnvelopeCodec codecType)
    {
        var codec = MercuryTestFactory.BuildCodec(codecType);

        Assert.Throws<ArgumentException>(() => codec.Decode(MercuryMemory.Empty));
    }

    /// <summary>
    /// Defines the test method BinaryDecode_TruncatedFrame_Throws.
    /// </summary>
    [Fact]
    public void BinaryDecode_TruncatedFrame_Throws()
    {
        var codec = MercuryTestFactory.BuildCodec(EnvelopeCodec.Binary);

        Assert.ThrowsAny<Exception>(() => codec.Decode(new MercuryMemory([1, 0, 0])));
    }

    /// <summary>
    /// Defines the test method JsonDecode_MalformedJson_ThrowsFormatException.
    /// </summary>
    [Fact]
    public void JsonDecode_MalformedJson_ThrowsFormatException()
    {
        var codec = MercuryTestFactory.BuildCodec(EnvelopeCodec.Json);
        var malformed = Encoding.UTF8.GetBytes("{not-valid-json}");

        Assert.Throws<FormatException>(() => codec.Decode(new MercuryMemory(malformed)));
    }

    /// <summary>
    /// Defines the test method BinaryFrame_DecodedAsJson_IsRejected.
    /// </summary>
    [Fact]
    public async Task BinaryFrame_DecodedAsJson_IsRejected()
    {
        var provider = MercuryTestFactory.BuildProvider(ProviderKind.AesGcm);

        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());

        var binary = MercuryTestFactory.BuildCodec(EnvelopeCodec.Binary, provider);

        var json = MercuryTestFactory.BuildCodec(EnvelopeCodec.Json, provider);

        var frame = binary.Encode(envelope);

        Assert.ThrowsAny<Exception>(
            () => json.Decode(frame));
    }

    /// <summary>
    /// Defines the test method JsonFrame_DecodedAsBinary_IsRejected.
    /// </summary>
    [Fact]
    public async Task JsonFrame_DecodedAsBinary_IsRejected()
    {
        var provider = MercuryTestFactory.BuildProvider(ProviderKind.AesGcm);

        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());

        var binary = MercuryTestFactory.BuildCodec(EnvelopeCodec.Binary, provider);

        var json = MercuryTestFactory.BuildCodec(EnvelopeCodec.Json, provider);

        var frame = json.Encode(envelope);

        Assert.ThrowsAny<Exception>(() => binary.Decode(frame));
    }

    /// <summary>
    /// Defines the test method Encode_DoesNotExposeOrAlterOriginalEnvelopePayload.
    /// </summary>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(CodecCases))]
    public async Task Encode_DoesNotExposeOrAlterOriginalEnvelopePayload(
        EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(ProviderKind.AesGcm);

        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());

        var original = envelope.Payload.ToArray();

        var codec = MercuryTestFactory.BuildCodec(codecType, provider);

        var frame = codec.Encode(envelope);

        var frameBytes = frame.ToArray();

        frameBytes[0] ^= 0xFF;

        Assert.Equal(original, envelope.Payload.ToArray());
    }
}
