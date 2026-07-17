using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Services;
using Mercury.Tests.Support;
using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.CryptoProviders;

public abstract class CryptoProviderContractTests
{
    protected abstract string ExpectedProviderName { get; }

    protected abstract ICryptoProvider CreateProvider(
        ISymmetricKeyProvider keyProvider);

    [Fact]
    public void Constructor_NullKeyProvider_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateProvider(null!));
    }

    [Fact]
    public void Name_ReturnsExpectedAlgorithmIdentifier()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        Assert.Equal(ExpectedProviderName, provider.Name);
    }

    [Fact]
    public async Task SealAsync_ValidRequest_ReturnsProtectedEnvelope()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var payload = MercuryTestFactory.CreatePayload();

        var result = await provider.SealAsync(
            new TestSealRequest(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory(payload)),
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

    [Fact]
    public async Task SealAsync_SamePayloadTwice_ProducesDifferentProtectedEnvelopes()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var payload = MercuryTestFactory.CreatePayload();
        var request = new TestSealRequest(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(payload));

        var first = await provider.SealAsync(
            request,
            EnvelopeService.Instance);
        var second = await provider.SealAsync(
            request,
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

    [Fact]
    public async Task SealAsync_Metadata_IsCopiedIntoEnvelope()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var headerMeta = new Metadata
        {
            { "route", "alpha" }
        };
        var footerMeta = new Metadata
        {
            { "purpose", "provider-test" }
        };

        var result = await provider.SealAsync(
            new TestSealRequest(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory(MercuryTestFactory.CreatePayload()),
                headerMeta,
                footerMeta),
            EnvelopeService.Instance);

        headerMeta.Add("route", "changed");
        footerMeta.Add("purpose", "changed");

        Assert.True(result.Success, result.Message);
        Assert.Equal("alpha",
            result.ValidatedEnvelope!.Header.Meta["route"]);
        Assert.Equal("provider-test",
            result.ValidatedEnvelope.Footer.Meta["purpose"]);
    }

    [Fact]
    public async Task OpenAsync_SealedEnvelope_ReturnsOriginalPayload()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var payload = MercuryTestFactory.CreatePayload(1024);
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            payload);

        var result = await provider.OpenAsync(
            new TestOpenRequest(envelope),
            EnvelopeService.Instance);

        Assert.True(result.Success, result.Message);
        Assert.Equal(FailureReason.None, result.FailureReason);
        Assert.Equal(payload, result.Payload.ToArray());
        Assert.Same(envelope, result.ValidatedEnvelope);
    }

    [Fact]
    public async Task SealAsync_EmptyPayload_ReturnsFailureWithoutEnvelope()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(
            new TestSealRequest(
                MercuryTestFactory.BuildContext(),
                MercuryMemory.Empty),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    [Fact]
    public async Task SealAsync_EmptyContext_ReturnsFailureWithoutEnvelope()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(
            new TestSealRequest(
                new TestCryptoContext(KeyId.Empty, KeyId.Empty),
                new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    [Fact]
    public async Task SealAsync_MissingSenderKeyId_ReturnsFailure()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(
            new TestSealRequest(
                new TestCryptoContext(
                    KeyId.Empty,
                    new KeyId(MercuryTestFactory.RecipientKeyId)),
                new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task SealAsync_MissingRecipientKeyId_ReturnsFailure()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        var result = await provider.SealAsync(
            new TestSealRequest(
                new TestCryptoContext(
                    new KeyId(MercuryTestFactory.SenderKeyId),
                    KeyId.Empty),
                new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.Custom, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task SealAsync_UnknownRecipientKey_ReturnsInternalError()
    {
        var provider = CreateProvider(
            new TestSymmetricKeyProvider());

        var result = await provider.SealAsync(
            new TestSealRequest(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

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
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider(invalidKey));

        var result = await provider.SealAsync(
            new TestSealRequest(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory(MercuryTestFactory.CreatePayload())),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError, result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_WrongKey_ReturnsAuthenticationFailure()
    {
        var sealingProvider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider(
                MercuryTestFactory.PrimaryKey));
        var openingProvider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider(
                MercuryTestFactory.AlternateKey));
        var envelope = await MercuryTestFactory.SealAsync(
            sealingProvider,
            MercuryTestFactory.CreatePayload());

        var result = await openingProvider.OpenAsync(
            new TestOpenRequest(envelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.NotNull(result.ValidatedEnvelope);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(13)]
    [InlineData(-1)]
    public async Task OpenAsync_TamperedProtectedPayload_ReturnsFailure(
        int byteIndex)
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var protectedPayload = envelope.Payload.ToArray();
        var index = byteIndex < 0
            ? protectedPayload.Length - 1
            : byteIndex;
        protectedPayload[index] ^= 0x80;
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            payload: new MercuryMemory(protectedPayload));

        var result = await provider.OpenAsync(
            new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_InvalidNonceLength_ReturnsInternalError()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var protectedPayload = envelope.Payload.ToArray();
        protectedPayload[0] = 11;
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            payload: new MercuryMemory(protectedPayload));

        var result = await provider.OpenAsync(
            new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_TruncatedProtectedPayload_ReturnsInternalError()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var truncatedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            payload: envelope.Payload.Slice(0, 12));

        var result = await provider.OpenAsync(
            new TestOpenRequest(truncatedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_WrongAlgorithmIdentifier_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(
            envelope.Header,
            encryption: new AlgorithmId("wrong-provider"));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            header: header);

        var result = await provider.OpenAsync(
            new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_TamperedSenderKeyId_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(
            envelope.Header,
            senderKeyId: new KeyId("mallory"));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            header: header);

        var result = await provider.OpenAsync(
            new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_TamperedRecipientKeyId_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(
            envelope.Header,
            recipientKeyId: new KeyId(
                MercuryTestFactory.AlternateRecipientKeyId));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            header: header);

        var result = await provider.OpenAsync(
            new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_TamperedReplayToken_ReturnsAuthenticationFailure()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var replayToken = envelope.Header.ReplayToken.ToArray();
        replayToken[0] ^= 0x40;
        var header = MercuryTestFactory.CloneHeader(
            envelope.Header,
            replayToken: new MercuryMemory(replayToken));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            header: header);

        var result = await provider.OpenAsync(
            new TestOpenRequest(tamperedEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task OpenAsync_MissingReplayToken_ReturnsInternalError()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        var header = MercuryTestFactory.CloneHeader(
            envelope.Header,
            replayToken: MercuryMemory.Empty);
        var invalidEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            header: header);

        var result = await provider.OpenAsync(
            new TestOpenRequest(invalidEnvelope),
            EnvelopeService.Instance);

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    [Fact]
    public async Task SealAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => provider.SealAsync(
                new TestSealRequest(
                    MercuryTestFactory.BuildContext(),
                    new MercuryMemory(MercuryTestFactory.CreatePayload())),
                EnvelopeService.Instance,
                cancellation.Token));
    }

    [Fact]
    public async Task OpenAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => provider.OpenAsync(
                new TestOpenRequest(envelope),
                EnvelopeService.Instance,
                cancellation.Token));
    }

    [Fact]
    public async Task SealAsync_NullRequest_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => provider.SealAsync(
                null!,
                EnvelopeService.Instance));
    }

    [Fact]
    public async Task SealAsync_NullEnvelopeService_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => provider.SealAsync(
                new TestSealRequest(
                    MercuryTestFactory.BuildContext(),
                    new MercuryMemory(MercuryTestFactory.CreatePayload())),
                null!));
    }

    [Fact]
    public async Task OpenAsync_NullRequest_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => provider.OpenAsync(
                null!,
                EnvelopeService.Instance));
    }

    [Fact]
    public async Task OpenAsync_NullEnvelopeService_ThrowsArgumentNullException()
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());
        var envelope = await MercuryTestFactory.SealAsync(
            provider,
            MercuryTestFactory.CreatePayload());

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => provider.OpenAsync(
                new TestOpenRequest(envelope),
                null!));
    }
}
