// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Kim K. Brown
// Last Modified On : 07-18-2026
// ***********************************************************************
// <copyright file="MercuryTestFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Security.Cryptography;
using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Replay;
using Mercury.Abstractions.Transport;
using Mercury.Core.Factories;
using Mercury.Core.Services;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using Xunit;

using MercuryMemory =
    Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Support;

/// <summary>
/// Identifies the crypto providers used by the test suite.
/// </summary>
public enum ProviderKind
{
    /// <summary>
    /// AES-GCM with a 256-bit key.
    /// </summary>
    AesGcm,

    /// <summary>
    /// ChaCha20-Poly1305.
    /// </summary>
    ChaCha20
}

/// <summary>
/// Provides shared objects and data used by Mercury tests.
/// </summary>
internal static class MercuryTestFactory
{
    /// <summary>
    /// The sender key identifier.
    /// </summary>
    public const string SenderKeyId = "alpha";

    /// <summary>
    /// The recipient key identifier.
    /// </summary>
    public const string RecipientKeyId = "bravo";

    /// <summary>
    /// The alternate recipient key identifier.
    /// </summary>
    public const string AlternateRecipientKeyId = "charlie";

    /// <summary>
    /// The primary 256-bit test key.
    /// </summary>
    public static readonly byte[] PrimaryKey =
    [
        .. Enumerable.Range(1, 32)
            .Select(value => (byte)value)
    ];

    /// <summary>
    /// The alternate 256-bit test key.
    /// </summary>
    public static readonly byte[] AlternateKey =
    [
        .. Enumerable.Range(101, 32)
            .Select(value => (byte)value)
    ];

    /// <summary>
    /// Provides the crypto-provider test cases.
    /// </summary>
    /// <returns>The provider test cases.</returns>
    public static IEnumerable<object[]> ProviderCases()
    {
        yield return [ProviderKind.AesGcm];
        yield return [ProviderKind.ChaCha20];
    }

    /// <summary>
    /// Provides strongly typed provider and codec combinations.
    /// </summary>
    /// <returns>The provider and codec test cases.</returns>
    public static TheoryData<ProviderKind, EnvelopeCodec>
        ProviderCodecCases()
    {
        var cases =
            new TheoryData<ProviderKind, EnvelopeCodec>();

        foreach (var provider in Enum.GetValues<ProviderKind>())
        {
            cases.Add(provider, EnvelopeCodec.Binary);
            cases.Add(provider, EnvelopeCodec.Json);
        }

        return cases;
    }

    /// <summary>
    /// Builds a symmetric-key provider.
    /// </summary>
    /// <param name="recipientKey">
    /// The recipient key, or <see langword="null"/> to use the primary key.
    /// </param>
    /// <returns>The symmetric-key provider.</returns>
    public static ISymmetricKeyProvider BuildKeyProvider(
        byte[]? recipientKey = null)
    {
        var selectedKey =
            recipientKey ?? PrimaryKey;

        return new TestSymmetricKeyProvider(
            (RecipientKeyId, selectedKey),
            (AlternateRecipientKeyId, selectedKey));
    }

    /// <summary>
    /// Builds the requested crypto provider.
    /// </summary>
    /// <param name="providerKind">The provider kind.</param>
    /// <param name="keyProvider">
    /// The key provider, or <see langword="null"/> to create one.
    /// </param>
    /// <returns>The configured crypto provider.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The provider kind is unsupported.
    /// </exception>
    public static ICryptoProvider BuildProvider(
        ProviderKind providerKind,
        ISymmetricKeyProvider? keyProvider = null)
    {
        var keys =
            keyProvider ?? BuildKeyProvider();

        return providerKind switch
        {
            ProviderKind.AesGcm =>
                new AesGcmCryptoProvider(keys),

            ProviderKind.ChaCha20 =>
                new ChaCha20CryptoProvider(keys),

            _ => throw new ArgumentOutOfRangeException(
                nameof(providerKind),
                providerKind,
                "Unsupported provider kind.")
        };
    }

    /// <summary>
    /// Builds a crypto context.
    /// </summary>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">
    /// The recipient key identifier.
    /// </param>
    /// <returns>The crypto context.</returns>
    public static ICryptoContext BuildContext(
        string senderKeyId = SenderKeyId,
        string recipientKeyId = RecipientKeyId)
    {
        return new TestCryptoContext(
            new KeyId(senderKeyId),
            new KeyId(recipientKeyId));
    }

    /// <summary>
    /// Builds an envelope codec.
    /// </summary>
    /// <param name="codec">The codec kind.</param>
    /// <param name="provider">
    /// The crypto provider, or <see langword="null"/> to create one.
    /// </param>
    /// <param name="transport">
    /// The transport, or <see langword="null"/> to create one.
    /// </param>
    /// <returns>The configured envelope codec.</returns>
    public static IEnvelopeCodec BuildCodec(
        EnvelopeCodec codec,
        ICryptoProvider? provider = null,
        ITransport? transport = null)
    {
        var selectedProvider =
            provider ?? BuildProvider(ProviderKind.AesGcm);

        var selectedTransport =
            transport ?? new QueueTransport();

        return MercuryFactory.Instance
            .BuildDependencies(
                selectedProvider,
                codec,
                selectedTransport)
            .EnvelopeCodec;
    }

    /// <summary>
    /// Builds a Mercury client.
    /// </summary>
    /// <param name="provider">The crypto provider.</param>
    /// <param name="codec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <param name="replayProtector">
    /// The optional replay protector.
    /// </param>
    /// <returns>The configured Mercury client.</returns>
    public static IMercuryClient BuildClient(
        ICryptoProvider provider,
        EnvelopeCodec codec,
        ITransport transport,
        IReplayProtector? replayProtector = null)
    {
        var envelopeCodec =
            BuildCodec(codec, provider, transport);

        var dependencies = new TestDependencies(
            provider,
            envelopeCodec,
            transport,
            replayProtector);

        return MercuryFactory.Instance.BuildClient(dependencies);
    }

    /// <summary>
    /// Seals a payload and returns the validated secure envelope.
    /// </summary>
    /// <param name="provider">The crypto provider.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="context">The optional crypto context.</param>
    /// <param name="headerMeta">The optional header metadata.</param>
    /// <param name="footerMeta">The optional footer metadata.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The validated secure envelope.</returns>
    public static async Task<ISecureEnvelope> SealAsync(
        ICryptoProvider provider,
        byte[] payload,
        ICryptoContext? context = null,
        Metadata? headerMeta = null,
        Metadata? footerMeta = null,
        CancellationToken cancellationToken = default)
    {
        var request = new TestSealRequest(
            context ?? BuildContext(),
            new MercuryMemory(payload),
            headerMeta,
            footerMeta);

        var result = await provider.SealAsync(
                request,
                EnvelopeService.Instance,
                cancellationToken)
            .ConfigureAwait(false);

        Assert.True(result.Success, result.Message);
        Assert.NotNull(result.ValidatedEnvelope);

        return result.ValidatedEnvelope;
    }

    /// <summary>
    /// Clones an envelope header.
    /// </summary>
    /// <param name="source">The source header.</param>
    /// <param name="envelopeId">The optional envelope identifier.</param>
    /// <param name="timestamp">The optional timestamp.</param>
    /// <param name="senderKeyId">The optional sender key identifier.</param>
    /// <param name="recipientKeyId">
    /// The optional recipient key identifier.
    /// </param>
    /// <param name="encryption">
    /// The optional encryption algorithm.
    /// </param>
    /// <param name="signature">The optional signature algorithm.</param>
    /// <param name="replayToken">The optional replay token.</param>
    /// <param name="meta">The optional metadata.</param>
    /// <returns>The cloned header.</returns>
    public static IEnvelopeHeader CloneHeader(
        IEnvelopeHeader source,
        KeyId? envelopeId = null,
        DateTimeOffset? timestamp = null,
        KeyId? senderKeyId = null,
        KeyId? recipientKeyId = null,
        AlgorithmId? encryption = null,
        AlgorithmId? signature = null,
        MercuryMemory? replayToken = null,
        Metadata? meta = null)
    {
        return EnvelopeService.Instance.BuildEnvelopeHeader(
            envelopeId ?? source.EnvelopeId,
            timestamp ?? source.Timestamp,
            senderKeyId ?? source.SenderKeyId,
            recipientKeyId ?? source.RecipientKeyId,
            encryption ?? source.Encryption,
            signature ?? source.Signature,
            replayToken ?? source.ReplayToken,
            meta ?? source.Meta.Clone());
    }

    /// <summary>
    /// Clones a secure envelope.
    /// </summary>
    /// <param name="source">The source envelope.</param>
    /// <param name="header">The optional replacement header.</param>
    /// <param name="payload">The optional replacement payload.</param>
    /// <param name="footer">The optional replacement footer.</param>
    /// <param name="version">The optional replacement version.</param>
    /// <returns>The cloned secure envelope.</returns>
    public static ISecureEnvelope CloneEnvelope(
        ISecureEnvelope source,
        IEnvelopeHeader? header = null,
        MercuryMemory? payload = null,
        IEnvelopeFooter? footer = null,
        FrameworkVersion? version = null)
    {
        return new TestSecureEnvelope(
            version ?? source.Version,
            header ?? source.Header,
            payload ?? source.Payload,
            footer ?? source.Footer);
    }

    /// <summary>
    /// Creates a cryptographically random test payload.
    /// </summary>
    /// <param name="length">The payload length.</param>
    /// <returns>The generated payload.</returns>
    public static byte[] CreatePayload(int length = 256)
    {
        var payload =
            new byte[length];

        RandomNumberGenerator.Fill(payload);

        return payload;
    }
}
