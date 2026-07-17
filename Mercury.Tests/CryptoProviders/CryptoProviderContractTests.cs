// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="CryptoProviderContractTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Services;
using Mercury.Tests.Support;

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.CryptoProviders;

/// <summary>
/// Class CryptoProviderContractTests.
/// </summary>
public abstract class CryptoProviderContractTests
{
    /// <summary>
    /// Gets the expected name of the provider.
    /// </summary>
    /// <value>The expected name of the provider.</value>
    protected abstract string ExpectedProviderName { get; }

    /// <summary>
    /// Creates the provider.
    /// </summary>
    /// <param name="keyProvider">The key provider.</param>
    /// <returns>ICryptoProvider.</returns>
    protected abstract ICryptoProvider CreateProvider(ISymmetricKeyProvider keyProvider);

    /// <summary>
    /// Defines the test method Constructor_NullKeyProvider_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public void Constructor_NullKeyProvider_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CreateProvider(null!));
    }

    /// <summary>
    /// Defines the test method Name_ReturnsExpectedAlgorithmIdentifier.
    /// </summary>
    [Fact]
    public void Name_ReturnsExpectedAlgorithmIdentifier()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        Assert.Equal(ExpectedProviderName, provider.Name);
    }

    /// <summary>
    /// Defines the test method SealAsync_ValidRequest_ReturnsProtectedEnvelope.
    /// </summary>
    [Fact]
    public async Task SealAsync_ValidRequest_ReturnsProtectedEnvelope()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var payload = MercuryTestFactory.CreatePayload();

        var result = await provider.SealAsync(new TestSealRequest(
                MercuryTestFactory.BuildContext(), new MercuryMemory(payload)),
            EnvelopeService.Instance);

        Assert.True(result.Success, result.Message);
        Assert.Equal(FailureReason.None, result.FailureReason);
        Assert.NotNull(result.ValidatedEnvelope);
        Assert.False(result.Payload.IsEmpty);
        Assert.Equal(payload.Length + 29, result.Payload.Length);
        Assert.Equal(ExpectedProviderName,
            result.ValidatedEnvelope.Header.Encryption.Value);
        Assert.Equal(MercuryTestFactory.SenderKeyId,
            result.ValidatedEnvelope.Header.SenderKeyId.Value);
        Assert.Equal(MercuryTestFactory.RecipientKeyId,
            result.ValidatedEnvelope.Header.RecipientKeyId.Value);
        Assert.Equal(16,
            result.ValidatedEnvelope.Header.ReplayToken.Length);
        Assert.False(result.ValidatedEnvelope.Header.EnvelopeId.IsEmpty);
        Assert.False(payload.SequenceEqual(result.Payload.ToArray()));
    }

    /// <summary>
    /// Defines the test method SealAsync_SamePayloadTwice_ProducesDifferentProtectedEnvelopes.
    /// </summary>
    [Fact]
    public async Task SealAsync_SamePayloadTwice_ProducesDifferentProtectedEnvelopes()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var payload = MercuryTestFactory.CreatePayload();
        var request = new TestSealRequest(MercuryTestFactory.BuildContext(),
            new MercuryMemory(payload));

        var first = await provider.SealAsync(request, EnvelopeService.Instance);
        var second = await provider.SealAsync(request,
            EnvelopeService.Instance);

        Assert.True(first.Success, first.Message);
        Assert.True(second.Success, second.Message);
        Assert.NotEqual(first.Payload.ToArray(), second.Payload.ToArray());
        Assert.NotEqual(
            first.ValidatedEnvelope!.Header.ReplayToken.ToArray(),
            second.ValidatedEnvelope!.Header.ReplayToken.ToArray());
        Assert.NotEqual(
            first.ValidatedEnvelope.Header.EnvelopeId,
            second.ValidatedEnvelope.Header.EnvelopeId);
    }

    /// <summary>
    /// Defines the test method SealAsync_Metadata_IsCopiedIntoEnvelope.
    /// </summary>
    [Fact]
    public async Task SealAsync_Metadata_IsCopiedIntoEnvelope()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var headerMeta = new Metadata
        {
            { "route", "alpha" }
        };
        var footerMeta = new Metadata
        {
            { "purpose", "provider-test" }
        };

        var result = await provider.SealAsync(new TestSealRequest(MercuryTestFactory.BuildContext(),
                new MercuryMemory(MercuryTestFactory.CreatePayload()), headerMeta, footerMeta),
            EnvelopeService.Instance);

        headerMeta.Add("route", "changed");
        footerMeta.Add("purpose", "changed");

        Assert.True(result.Success, result.Message);
        Assert.Equal("alpha",
            result.ValidatedEnvelope!.Header.Meta["route"]);
        Assert.Equal("provider-test",
            result.ValidatedEnvelope.Footer.Meta["purpose"]);
    }

    /// <summary>
    /// Defines the test method OpenAsync_SealedEnvelope_ReturnsOriginalPayload.
    /// </summary>
    [Fact]
    public async Task OpenAsync_SealedEnvelope_ReturnsOriginalPayload()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var payload = MercuryTestFactory.CreatePayload(1024);
        var envelope = await MercuryTestFactory.SealAsync(provider, payload);

        var result = await provider.OpenAsync(new TestOpenRequest(envelope), EnvelopeService.Instance);

        Assert.True(result.Success, result.Message);
        Assert.Equal(FailureReason.None, result.FailureReason);
        Assert.Equal(payload, result.Payload.ToArray());
        Assert.Same(envelope, result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method SealAsync_EmptyPayload_ReturnsFailureWithoutEnvelope.
    /// </summary>
    [Fact]
    public async Task SealAsync_EmptyPayload_ReturnsFailureWithoutEnvelope()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(new TestSealRequest(
                MercuryTestFactory.BuildContext(), MercuryMemory.Empty),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method SealAsync_EmptyContext_ReturnsFailureWithoutEnvelope.
    /// </summary>
    [Fact]
    public async Task SealAsync_EmptyContext_ReturnsFailureWithoutEnvelope()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(new TestSealRequest(new TestCryptoContext(KeyId.Empty, KeyId.Empty), 
                new MercuryMemory(MercuryTestFactory.CreatePayload())), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method SealAsync_MissingSenderKeyId_ReturnsFailure.
    /// </summary>
    [Fact]
    public async Task SealAsync_MissingSenderKeyId_ReturnsFailure()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(new TestSealRequest(new TestCryptoContext(KeyId.Empty,
                    new KeyId(MercuryTestFactory.RecipientKeyId)), new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method SealAsync_MissingRecipientKeyId_ReturnsFailure.
    /// </summary>
    [Fact]
    public async Task SealAsync_MissingRecipientKeyId_ReturnsFailure()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(new TestSealRequest(new TestCryptoContext(
                    new KeyId(MercuryTestFactory.SenderKeyId), KeyId.Empty),
                new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method SealAsync_UnknownRecipientKey_ReturnsInternalError.
    /// </summary>
    [Fact]
    public async Task SealAsync_UnknownRecipientKey_ReturnsInternalError()
    {
        var provider = CreateProvider(new TestSymmetricKeyProvider());

        var result = await provider.SealAsync(new TestSealRequest(MercuryTestFactory.BuildContext(),
                new MercuryMemory(MercuryTestFactory.CreatePayload())), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method SealAsync_InvalidKeyLength_ReturnsInternalError.
    /// </summary>
    /// <param name="keyLength">Length of the key.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(31)]
    [InlineData(33)]
    public async Task SealAsync_InvalidKeyLength_ReturnsInternalError(
        int keyLength)
    {
        var invalidKey = new byte[keyLength];
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider(invalidKey));

        var result = await provider.SealAsync(new TestSealRequest(
                MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_WrongKey_ReturnsAuthenticationFailure.
    /// </summary>
    [Fact]
    public async Task OpenAsync_WrongKey_ReturnsAuthenticationFailure()
    {
        var sealingProvider = CreateProvider(MercuryTestFactory.BuildKeyProvider(
                MercuryTestFactory.PrimaryKey)); var openingProvider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider(MercuryTestFactory.AlternateKey));

        var envelope = await MercuryTestFactory.SealAsync(sealingProvider, MercuryTestFactory.CreatePayload());

        var result = await openingProvider.OpenAsync(new TestOpenRequest(envelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.NotNull(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method OpenAsync_TamperedProtectedPayload_ReturnsFailure.
    /// </summary>
    /// <param name="byteIndex">Index of the byte.</param>
    [Theory]
    [InlineData(1)]
    [InlineData(13)]
    [InlineData(-1)]
    public async Task OpenAsync_TamperedProtectedPayload_ReturnsFailure(
        int byteIndex)
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var protectedPayload = envelope.Payload.ToArray();

        var index = byteIndex < 0
            ? protectedPayload.Length - 1
            : byteIndex;
        protectedPayload[index] ^= 0x80;
        
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, payload: new MercuryMemory(protectedPayload));

        var result = await provider.OpenAsync(new TestOpenRequest(tamperedEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_InvalidNonceLength_ReturnsInternalError.
    /// </summary>
    [Fact]
    public async Task OpenAsync_InvalidNonceLength_ReturnsInternalError()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var protectedPayload = envelope.Payload.ToArray();

        protectedPayload[0] = 11;
        
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope,
            payload: new MercuryMemory(protectedPayload));

        var result = await provider.OpenAsync(new TestOpenRequest(tamperedEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_TruncatedProtectedPayload_ReturnsInternalError.
    /// </summary>
    [Fact]
    public async Task OpenAsync_TruncatedProtectedPayload_ReturnsInternalError()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());

        var truncatedEnvelope = MercuryTestFactory.CloneEnvelope(envelope,
            payload: envelope.Payload.Slice(0, 12));

        var result = await provider.OpenAsync(new TestOpenRequest(truncatedEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_WrongAlgorithmIdentifier_ReturnsAuthenticationFailure.
    /// </summary>
    [Fact]
    public async Task OpenAsync_WrongAlgorithmIdentifier_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(envelope.Header, encryption: new AlgorithmId("wrong-provider"));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, header: header);

        var result = await provider.OpenAsync(new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_TamperedSenderKeyId_ReturnsAuthenticationFailure.
    /// </summary>
    [Fact]
    public async Task OpenAsync_TamperedSenderKeyId_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(envelope.Header, senderKeyId: new KeyId("mallory"));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, header: header);

        var result = await provider.OpenAsync(new TestOpenRequest(tamperedEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_TamperedRecipientKeyId_ReturnsAuthenticationFailure.
    /// </summary>
    [Fact]
    public async Task OpenAsync_TamperedRecipientKeyId_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(envelope.Header, recipientKeyId: new KeyId(
                MercuryTestFactory.AlternateRecipientKeyId));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, header: header);

        var result = await provider.OpenAsync(new TestOpenRequest(tamperedEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_TamperedReplayToken_ReturnsAuthenticationFailure.
    /// </summary>
    [Fact]
    public async Task OpenAsync_TamperedReplayToken_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var replayToken = envelope.Header.ReplayToken.ToArray();

        replayToken[0] ^= 0x40;

        var header = MercuryTestFactory.CloneHeader(envelope.Header, replayToken: new MercuryMemory(replayToken));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, header: header);

        var result = await provider.OpenAsync(new TestOpenRequest(tamperedEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method OpenAsync_MissingReplayToken_ReturnsInternalError.
    /// </summary>
    [Fact]
    public async Task OpenAsync_MissingReplayToken_ReturnsInternalError()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(envelope.Header, replayToken: MercuryMemory.Empty);
        var invalidEnvelope = MercuryTestFactory.CloneEnvelope(envelope, header: header);

        var result = await provider.OpenAsync(new TestOpenRequest(invalidEnvelope), EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method SealAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task SealAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        using var cancellation = new CancellationTokenSource();

        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => provider.SealAsync(
                new TestSealRequest(MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload())),
                EnvelopeService.Instance, cancellation.Token));
    }

    /// <summary>
    /// Defines the test method OpenAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task OpenAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider,
            MercuryTestFactory.CreatePayload());
        using var cancellation = new CancellationTokenSource();
        
        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => provider.OpenAsync(new TestOpenRequest(envelope),
                EnvelopeService.Instance, cancellation.Token));
    }

    /// <summary>
    /// Defines the test method SealAsync_NullRequest_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public async Task SealAsync_NullRequest_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(() => provider.SealAsync(
                null!, EnvelopeService.Instance));
    }

    /// <summary>
    /// Defines the test method SealAsync_NullEnvelopeService_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public async Task SealAsync_NullEnvelopeService_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(() => provider.SealAsync(new TestSealRequest(
                    MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload())),
                null!));
    }

    /// <summary>
    /// Defines the test method OpenAsync_NullRequest_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public async Task OpenAsync_NullRequest_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => provider.OpenAsync(null!, EnvelopeService.Instance));
    }

    /// <summary>
    /// Defines the test method OpenAsync_NullEnvelopeService_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public async Task OpenAsync_NullEnvelopeService_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(provider, MercuryTestFactory.CreatePayload());

        await Assert.ThrowsAsync<ArgumentNullException>(() => provider.OpenAsync(
                new TestOpenRequest(envelope), null!));
    }
}
