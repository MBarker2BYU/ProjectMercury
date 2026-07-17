// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
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

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Support;

public enum ProviderKind
{
    AesGcm,
    ChaCha20
}

internal static class MercuryTestFactory
{
    public const string SenderKeyId = "alpha";
    public const string RecipientKeyId = "bravo";
    public const string AlternateRecipientKeyId = "charlie";

    public static readonly byte[] PrimaryKey =
        Enumerable.Range(1, 32).Select(value => (byte)value).ToArray();

    public static readonly byte[] AlternateKey =
        Enumerable.Range(101, 32).Select(value => (byte)value).ToArray();

    public static IEnumerable<object[]> ProviderCases()
    {
        yield return [ProviderKind.AesGcm];
        yield return [ProviderKind.ChaCha20];
    }

    public static IEnumerable<object[]> ProviderCodecCases()
    {
        foreach (var provider in Enum.GetValues<ProviderKind>())
        {
            yield return [provider, EnvelopeCodec.Binary];
            yield return [provider, EnvelopeCodec.Json];
        }
    }

    public static ISymmetricKeyProvider BuildKeyProvider(byte[]? recipientKey = null)
        => new TestSymmetricKeyProvider(
            (RecipientKeyId, recipientKey ?? PrimaryKey),
            (AlternateRecipientKeyId, recipientKey ?? PrimaryKey));

    public static ICryptoProvider BuildProvider(
        ProviderKind providerKind,
        ISymmetricKeyProvider? keyProvider = null)
    {
        var keys = keyProvider ?? BuildKeyProvider();

        return providerKind switch
        {
            ProviderKind.AesGcm => new AesGcmCryptoProvider(keys),
            ProviderKind.ChaCha20 => new ChaCha20CryptoProvider(keys),
            _ => throw new ArgumentOutOfRangeException(
                nameof(providerKind),
                providerKind,
                "Unsupported provider kind.")
        };
    }

    public static ICryptoContext BuildContext(
        string senderKeyId = SenderKeyId,
        string recipientKeyId = RecipientKeyId)
        => new TestCryptoContext(
            new KeyId(senderKeyId),
            new KeyId(recipientKeyId));

    public static IEnvelopeCodec BuildCodec(
        EnvelopeCodec codec,
        ICryptoProvider? provider = null,
        ITransport? transport = null)
    {
        var selectedProvider = provider ?? BuildProvider(ProviderKind.AesGcm);
        var selectedTransport = transport ?? new QueueTransport();

        return MercuryFactory.Instance
            .BuildDependencies(selectedProvider, codec, selectedTransport)
            .EnvelopeCodec;
    }

    public static IMercuryClient BuildClient(
        ICryptoProvider provider,
        EnvelopeCodec codec,
        ITransport transport,
        IReplayProtector? replayProtector = null)
    {
        var envelopeCodec = BuildCodec(codec, provider, transport);

        var dependencies = new TestDependencies(
            provider,
            envelopeCodec,
            transport,
            replayProtector);

        return MercuryFactory.Instance.BuildClient(dependencies);
    }

    public static async Task<ISecureEnvelope> SealAsync(
        ICryptoProvider provider,
        byte[] payload,
        ICryptoContext? context = null,
        Metadata? headerMeta = null,
        Metadata? footerMeta = null,
        CancellationToken cancellationToken = default)
    {
        var result = await provider
            .SealAsync(
                new TestSealRequest(
                    context ?? BuildContext(),
                    new MercuryMemory(payload),
                    headerMeta,
                    footerMeta),
                EnvelopeService.Instance,
                cancellationToken)
            .ConfigureAwait(false);

        Assert.True(result.Success, result.Message);
        Assert.NotNull(result.ValidatedEnvelope);

        return result.ValidatedEnvelope;
    }

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
        => EnvelopeService.Instance.BuildEnvelopeHeader(
            envelopeId ?? source.EnvelopeId,
            timestamp ?? source.Timestamp,
            senderKeyId ?? source.SenderKeyId,
            recipientKeyId ?? source.RecipientKeyId,
            encryption ?? source.Encryption,
            signature ?? source.Signature,
            replayToken ?? source.ReplayToken,
            meta ?? source.Meta.Clone());

    public static ISecureEnvelope CloneEnvelope(
        ISecureEnvelope source,
        IEnvelopeHeader? header = null,
        MercuryMemory? payload = null,
        IEnvelopeFooter? footer = null,
        FrameworkVersion? version = null)
        => new TestSecureEnvelope(
            version ?? source.Version,
            header ?? source.Header,
            payload ?? source.Payload,
            footer ?? source.Footer);

    public static byte[] CreatePayload(int length = 256)
    {
        var payload = new byte[length];
        RandomNumberGenerator.Fill(payload);
        return payload;
    }
}
