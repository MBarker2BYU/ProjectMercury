// ***********************************************************************
// Assembly     : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-23-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="AuthenticatedEnvelopeTamperTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Replay;
using Mercury.Core.Services;
using Mercury.Tests.Support;

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Pipeline;

/// <summary>
/// Verifies that authenticated envelope fields cannot be changed
/// without causing provider authentication to fail.
/// </summary>
public sealed class AuthenticatedEnvelopeTamperTests
{
    /// <summary>
    /// Gets the provider and codec test cases.
    /// </summary>
    /// <returns>
    /// The provider and codec test cases.
    /// </returns>
    public static IEnumerable<object[]> ProviderCodecCases()
        => MercuryTestFactory.ProviderCodecCases();

    /// <summary>
    /// Verifies that changing the envelope identifier causes
    /// authentication to fail.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedEnvelopeId_ReturnsAuthenticationFailure(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var context = await CreateProtectedEnvelopeAsync(providerKind, codecType);

        var tamperedHeader = EnvelopeService.Instance.BuildEnvelopeHeader(new KeyId(Guid.NewGuid().ToString("N")),
                context.Envelope.Header.Timestamp, context.Envelope.Header.SenderKeyId, context.Envelope.Header.RecipientKeyId,
                context.Envelope.Header.Encryption, context.Envelope.Header.Signature, context.Envelope.Header.ReplayToken, context.Envelope.Header.Meta.Clone());

        var tamperedEnvelope = PackEnvelope(tamperedHeader, context.Envelope.Payload, context.Envelope.Footer);

        await AssertAuthenticationFailureAsync(providerKind, codecType, context.Codec, tamperedEnvelope);
    }

    /// <summary>
    /// Verifies that changing the timestamp causes
    /// authentication to fail.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedTimestamp_ReturnsAuthenticationFailure(
        ProviderKind providerKind,
        EnvelopeCodec codecType)
    {
        var context =
            await CreateProtectedEnvelopeAsync(providerKind, codecType);

        var tamperedHeader = EnvelopeService.Instance.BuildEnvelopeHeader(context.Envelope.Header.EnvelopeId, context.Envelope.Header.Timestamp
                    .AddSeconds(1), context.Envelope.Header.SenderKeyId, context.Envelope.Header.RecipientKeyId, context.Envelope.Header.Encryption, context.Envelope.Header.Signature,
                context.Envelope.Header.ReplayToken, context.Envelope.Header.Meta.Clone());

        var tamperedEnvelope = PackEnvelope(tamperedHeader, context.Envelope.Payload, context.Envelope.Footer);

        await AssertAuthenticationFailureAsync(providerKind, codecType, context.Codec, tamperedEnvelope);
    }

    /// <summary>
    /// Verifies that changing the signature algorithm identifier
    /// causes authentication to fail.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedSignatureAlgorithm_ReturnsAuthenticationFailure(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var context = await CreateProtectedEnvelopeAsync(providerKind, codecType);

        var tamperedHeader = EnvelopeService.Instance.BuildEnvelopeHeader(context.Envelope.Header.EnvelopeId, context.Envelope.Header.Timestamp,
                context.Envelope.Header.SenderKeyId, context.Envelope.Header.RecipientKeyId, context.Envelope.Header.Encryption, 
                new AlgorithmId("tampered-signature"),
                context.Envelope.Header.ReplayToken,
                context.Envelope.Header.Meta.Clone());

        var tamperedEnvelope = PackEnvelope(tamperedHeader, context.Envelope.Payload, context.Envelope.Footer);

        await AssertAuthenticationFailureAsync(providerKind, codecType, context.Codec, tamperedEnvelope);
    }

    /// <summary>
    /// Verifies that changing header metadata causes
    /// authentication to fail.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedHeaderMetadata_ReturnsAuthenticationFailure(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var context = await CreateProtectedEnvelopeAsync(providerKind, codecType);

        var tamperedMetadata = context.Envelope.Header.Meta.Clone();

        tamperedMetadata.Add("tampered-header", "true");

        var tamperedHeader = EnvelopeService.Instance.BuildEnvelopeHeader(context.Envelope.Header.EnvelopeId, context.Envelope.Header.Timestamp,
                context.Envelope.Header.SenderKeyId, context.Envelope.Header.RecipientKeyId, context.Envelope.Header.Encryption,
                context.Envelope.Header.Signature, context.Envelope.Header.ReplayToken, tamperedMetadata);

        var tamperedEnvelope = PackEnvelope(tamperedHeader, context.Envelope.Payload, context.Envelope.Footer);

        await AssertAuthenticationFailureAsync(providerKind, codecType, context.Codec, tamperedEnvelope);
    }

    /// <summary>
    /// Verifies that changing footer metadata causes
    /// authentication to fail.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedFooterMetadata_ReturnsAuthenticationFailure(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var context = await CreateProtectedEnvelopeAsync(providerKind, codecType);

        var tamperedMetadata = context.Envelope.Footer.Meta.Clone();

        tamperedMetadata.Add("tampered-footer", "true");

        var tamperedFooter = EnvelopeService.Instance.BuildEnvelopeFooter(tamperedMetadata);

        var tamperedEnvelope = PackEnvelope(context.Envelope.Header, context.Envelope.Payload, tamperedFooter);

        await AssertAuthenticationFailureAsync(providerKind, codecType, context.Codec, tamperedEnvelope);
    }

    /// <summary>
    /// Creates one valid protected envelope for alteration.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    /// <returns>
    /// The protected envelope and its codec.
    /// </returns>
    private static async Task<TamperTestContext>
        CreateProtectedEnvelopeAsync(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);

        var transport = new RecordingTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), provider,
                codecType, transport, new InMemoryReplayProtector());

        var headerMetadata = new Metadata { { "header-value", "original" } };

        var footerMetadata = new Metadata { { "footer-value", "original" } };

        var cryptoContext = MercuryTestFactory.BuildContext();

        await sender.SendAsync(cryptoContext, new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var codec = MercuryTestFactory.BuildCodec(codecType, provider);

        var envelope = codec.Decode(transport.LastFrame);

        return new TamperTestContext(envelope, codec);
    }

    /// <summary>
    /// Creates a structurally valid envelope from altered fields.
    /// </summary>
    /// <param name="header">
    /// The envelope header.
    /// </param>
    /// <param name="payload">
    /// The original protected payload.
    /// </param>
    /// <param name="footer">
    /// The envelope footer.
    /// </param>
    /// <returns>
    /// The altered secure envelope.
    /// </returns>
    private static ISecureEnvelope PackEnvelope(IEnvelopeHeader header, MercuryMemory payload, IEnvelopeFooter footer)
    {
        var result = EnvelopeService.Instance.PackEnvelope(header, payload, footer);

        Assert.True(result.Success, result.Message);

        Assert.NotNull(result.ValidatedEnvelope);

        return result.ValidatedEnvelope!;
    }

    /// <summary>
    /// Verifies that the altered envelope is rejected without
    /// releasing plaintext.
    /// </summary>
    /// <param name="providerKind">
    /// The provider kind.
    /// </param>
    /// <param name="codecType">
    /// The envelope codec.
    /// </param>
    /// <param name="codec">
    /// The codec used to encode the altered envelope.
    /// </param>
    /// <param name="tamperedEnvelope">
    /// The altered envelope.
    /// </param>
    private static async Task AssertAuthenticationFailureAsync(ProviderKind providerKind, EnvelopeCodec codecType,
        IEnvelopeCodec codec, ISecureEnvelope tamperedEnvelope)
    {
        var inbound = new QueueTransport();

        inbound.Inject(codec.Encode(tamperedEnvelope));

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(),
                MercuryTestFactory.BuildProvider(providerKind), codecType, inbound,
                new InMemoryReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.AuthenticationFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.NotNull(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Stores the envelope and codec used by one tamper test.
    /// </summary>
    private sealed class TamperTestContext
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TamperTestContext"/> class.
        /// </summary>
        /// <param name="envelope">
        /// The protected envelope.
        /// </param>
        /// <param name="codec">
        /// The envelope codec.
        /// </param>
        public TamperTestContext(ISecureEnvelope envelope, IEnvelopeCodec codec)
        {
            Envelope = envelope;

            Codec = codec;
        }

        /// <summary>
        /// Gets the protected envelope.
        /// </summary>
        public ISecureEnvelope Envelope { get; }

        /// <summary>
        /// Gets the envelope codec.
        /// </summary>
        public IEnvelopeCodec Codec { get; }
    }
}