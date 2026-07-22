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

/// <summary>
/// Enum ProviderKind
/// </summary>
public enum ProviderKind
{
    /// <summary>
    /// The aes GCM
    /// </summary>
    AesGcm,
    /// <summary>
    /// The cha cha20
    /// </summary>
    ChaCha20
}

/// <summary>
/// Class MercuryTestFactory.
/// </summary>
internal static class MercuryTestFactory
{
    internal const string ALPHA = @"Alpha";
    internal const string BRAVO = @"Bravo";

    /// <summary>
    /// Gets the alpha client identifier.
    /// </summary>
    /// <value>The alpha client identifier.</value>
    public static KeyId AlphaClientId()
        => ALPHA;

    /// <summary>
    /// The sender key identifier
    /// </summary>
    public const string SenderKeyId = "alpha";
    /// <summary>
    /// The recipient key identifier
    /// </summary>
    public const string RecipientKeyId = "bravo";
    /// <summary>
    /// The alternate recipient key identifier
    /// </summary>
    public const string AlternateRecipientKeyId = "charlie";

    /// <summary>
    /// The primary key
    /// </summary>
    public static readonly byte[] PrimaryKey =
        Enumerable.Range(1, 32).Select(value => (byte)value).ToArray();

    /// <summary>
    /// The alternate key
    /// </summary>
    public static readonly byte[] AlternateKey =
        Enumerable.Range(101, 32).Select(value => (byte)value).ToArray();

    /// <summary>
    /// Providers the cases.
    /// </summary>
    /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
    public static IEnumerable<object[]> ProviderCases()
    {
        yield return [ProviderKind.AesGcm];
        yield return [ProviderKind.ChaCha20];
    }

    /// <summary>
    /// Providers the codec cases.
    /// </summary>
    /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
    public static IEnumerable<object[]> ProviderCodecCases()
    {
        foreach (var provider in Enum.GetValues<ProviderKind>())
        {
            yield return [provider, EnvelopeCodec.Binary];
            yield return [provider, EnvelopeCodec.Json];
        }
    }

    /// <summary>
    /// Builds the key provider.
    /// </summary>
    /// <param name="recipientKey">The recipient key.</param>
    /// <returns>ISymmetricKeyProvider.</returns>
    public static ISymmetricKeyProvider BuildKeyProvider(byte[]? recipientKey = null)
        => new TestSymmetricKeyProvider((RecipientKeyId, recipientKey ?? PrimaryKey),
            (AlternateRecipientKeyId, recipientKey ?? PrimaryKey));

    /// <summary>
    /// Builds the provider.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="keyProvider">The key provider.</param>
    /// <returns>ICryptoProvider.</returns>
    public static ICryptoProvider BuildProvider(ProviderKind providerKind, ISymmetricKeyProvider? keyProvider = null)
    {
        var keys = keyProvider ?? BuildKeyProvider();

        return providerKind switch
        {
            ProviderKind.AesGcm => new AesGcmCryptoProvider(keys),
            ProviderKind.ChaCha20 => new ChaCha20CryptoProvider(keys),
            _ => throw new ArgumentOutOfRangeException(nameof(providerKind),
                providerKind, "Unsupported provider kind.")
        };
    }

    /// <summary>
    /// Builds the context.
    /// </summary>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <returns>ICryptoContext.</returns>
    public static ICryptoContext BuildContext(string senderKeyId = SenderKeyId, string recipientKeyId = RecipientKeyId)
        => new TestCryptoContext(new KeyId(senderKeyId), new KeyId(recipientKeyId));

    /// <summary>
    /// Builds the codec.
    /// </summary>
    /// <param name="codec">The codec.</param>
    /// <param name="provider">The provider.</param>
    /// <param name="transport">The transport.</param>
    /// <returns>IEnvelopeCodec.</returns>
    public static IEnvelopeCodec BuildCodec(EnvelopeCodec codec, ICryptoProvider? provider = null, ITransport? transport = null)
    {
        var selectedProvider = provider ?? BuildProvider(ProviderKind.AesGcm);
        var selectedTransport = transport ?? new QueueTransport();

        return MercuryFactory.Instance
            .BuildDependencies(AlphaClientId(), selectedProvider, codec, selectedTransport)
            .EnvelopeCodec;
    }

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="codec">The codec.</param>
    /// <param name="transport">The transport.</param>
    /// <param name="replayProtector">The replay protector.</param>
    /// <returns>IMercuryClient.</returns>
    public static IMercuryClient BuildClient(ICryptoProvider provider, EnvelopeCodec codec,
        ITransport transport, IReplayProtector? replayProtector = null)
    {
        var envelopeCodec = BuildCodec(codec, provider, transport);

        var dependencies = new TestDependencies(AlphaClientId(), provider, envelopeCodec, transport, replayProtector);

        return MercuryFactory.Instance.BuildClient(dependencies);
    }

    /// <summary>
    /// Seal as an asynchronous operation.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="context">The context.</param>
    /// <param name="headerMeta">The header meta.</param>
    /// <param name="footerMeta">The footer meta.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ISecureEnvelope&gt; representing the asynchronous operation.</returns>
    public static async Task<ISecureEnvelope> SealAsync(ICryptoProvider provider, byte[] payload, ICryptoContext? context = null,
        Metadata? headerMeta = null, Metadata? footerMeta = null, CancellationToken cancellationToken = default)
    {
        var result = await provider
            .SealAsync(new TestSealRequest(context ?? BuildContext(), new MercuryMemory(payload),
                    headerMeta, footerMeta), EnvelopeService.Instance, cancellationToken)
            .ConfigureAwait(false);

        Assert.True(result.Success, result.Message);
        Assert.NotNull(result.ValidatedEnvelope);

        return result.ValidatedEnvelope;
    }

    /// <summary>
    /// Clones the header.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="envelopeId">The envelope identifier.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <param name="encryption">The encryption.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="replayToken">The replay token.</param>
    /// <param name="meta">The meta.</param>
    /// <returns>IEnvelopeHeader.</returns>
    public static IEnvelopeHeader CloneHeader(IEnvelopeHeader source, KeyId? envelopeId = null, DateTimeOffset? timestamp = null,
        KeyId? senderKeyId = null, KeyId? recipientKeyId = null, AlgorithmId? encryption = null, AlgorithmId? signature = null,
        MercuryMemory? replayToken = null, Metadata? meta = null)
        => EnvelopeService.Instance.BuildEnvelopeHeader(envelopeId ?? source.EnvelopeId, timestamp ?? source.Timestamp,
            senderKeyId ?? source.SenderKeyId, recipientKeyId ?? source.RecipientKeyId, encryption ?? source.Encryption,
            signature ?? source.Signature, replayToken ?? source.ReplayToken, meta ?? source.Meta.Clone());

    /// <summary>
    /// Clones the envelope.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="header">The header.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="footer">The footer.</param>
    /// <param name="version">The version.</param>
    /// <returns>ISecureEnvelope.</returns>
    public static ISecureEnvelope CloneEnvelope(ISecureEnvelope source, IEnvelopeHeader? header = null, MercuryMemory? payload = null,
        IEnvelopeFooter? footer = null, FrameworkVersion? version = null)
        => new TestSecureEnvelope(version ?? source.Version, header ?? source.Header, payload ?? source.Payload, footer ?? source.Footer);

    /// <summary>
    /// Creates the payload.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] CreatePayload(int length = 256)
    {
        var payload = new byte[length];
        RandomNumberGenerator.Fill(payload);
        return payload;
    }
}
