// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="MercuryPipelineFailureTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using Mercury.Core.Replay;
using Mercury.Tests.Support;
using System.Security.Cryptography;
using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Pipeline;

/// <summary>
/// Class MercuryPipelineFailureTests. This class cannot be inherited.
/// </summary>
public sealed class MercuryPipelineFailureTests
{
    /// <summary>
    /// Providers the codec cases.
    /// </summary>
    /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
    public static IEnumerable<object[]> ProviderCodecCases()
        => MercuryTestFactory.ProviderCodecCases();

    /// <summary>
    /// Gets the alpha client identifier.
    /// </summary>
    /// <value>The alpha client identifier.</value>
    public static MercuryMemory AlphaClientId()
        => RandomNumberGenerator.GetBytes(32);

    /// <summary>
    /// Defines the test method ReceiveAsync_WrongKey_ReturnsAuthenticationFailureWithoutPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_WrongKey_ReturnsAuthenticationFailureWithoutPayload(
        ProviderKind providerKind,
        EnvelopeCodec codec)
    {
        var transport = new QueueTransport();
        var sender = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(
                providerKind,
                MercuryTestFactory.BuildKeyProvider(
                    MercuryTestFactory.PrimaryKey)),
            codec,
            transport,
            new InMemoryReplayProtector());
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(
                providerKind,
                MercuryTestFactory.BuildKeyProvider(
                    MercuryTestFactory.AlternateKey)),
            codec,
            transport,
            new InMemoryReplayProtector());

        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));
        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_WrongProvider_ReturnsAuthenticationFailureWithoutPayload.
    /// </summary>
    /// <param name="senderProviderKind">Kind of the sender provider.</param>
    /// <param name="receiverProviderKind">Kind of the receiver provider.</param>
    [Theory]
    [InlineData(ProviderKind.AesGcm, ProviderKind.ChaCha20)]
    [InlineData(ProviderKind.ChaCha20, ProviderKind.AesGcm)]
    public async Task ReceiveAsync_WrongProvider_ReturnsAuthenticationFailureWithoutPayload(
        ProviderKind senderProviderKind,
        ProviderKind receiverProviderKind)
    {
        var transport = new QueueTransport();
        var sender = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(senderProviderKind),
            EnvelopeCodec.Binary,
            transport,
            new InMemoryReplayProtector());
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(receiverProviderKind),
            EnvelopeCodec.Binary,
            transport,
            new InMemoryReplayProtector());

        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));
        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_TamperedProtectedPayload_ReturnsAuthenticationFailureWithoutPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedProtectedPayload_ReturnsAuthenticationFailureWithoutPayload(
        ProviderKind providerKind,
        EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);
        var recordingTransport = new RecordingTransport();
        var sender = MercuryTestFactory.BuildClient(
            provider,
            codecType,
            recordingTransport,
            new InMemoryReplayProtector());
        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));
        var codec = MercuryTestFactory.BuildCodec(codecType, provider);
        var envelope = codec.Decode(recordingTransport.LastFrame);
        var protectedPayload = envelope.Payload.ToArray();
        protectedPayload[^1] ^= 0x80;
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            payload: new MercuryMemory(protectedPayload));
        var inbound = new QueueTransport();
        inbound.Inject(codec.Encode(tamperedEnvelope));
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(providerKind),
            codecType,
            inbound,
            new InMemoryReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_TamperedAuthenticatedHeader_ReturnsAuthenticationFailureWithoutPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedAuthenticatedHeader_ReturnsAuthenticationFailureWithoutPayload(
        ProviderKind providerKind,
        EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);
        var recordingTransport = new RecordingTransport();
        var sender = MercuryTestFactory.BuildClient(
            provider,
            codecType,
            recordingTransport,
            new InMemoryReplayProtector());
        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));
        var codec = MercuryTestFactory.BuildCodec(codecType, provider);
        var envelope = codec.Decode(recordingTransport.LastFrame);
        var tamperedHeader = MercuryTestFactory.CloneHeader(
            envelope.Header,
            senderKeyId: new KeyId("mallory"));
        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(
            envelope,
            header: tamperedHeader);
        var inbound = new QueueTransport();
        inbound.Inject(codec.Encode(tamperedEnvelope));
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(providerKind),
            codecType,
            inbound,
            new InMemoryReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_ReplayedFrame_ReturnsReplayDetectedWithoutPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_ReplayedFrame_ReturnsReplayDetectedWithoutPayload(
        ProviderKind providerKind,
        EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);
        var recordingTransport = new RecordingTransport();
        var sender = MercuryTestFactory.BuildClient(
            provider,
            codecType,
            recordingTransport,
            new InMemoryReplayProtector());
        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));
        var inbound = new QueueTransport();
        inbound.Inject(recordingTransport.LastFrame);
        inbound.Inject(recordingTransport.LastFrame);
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(providerKind),
            codecType,
            inbound,
            new InMemoryReplayProtector());

        var first = await receiver.ReceiveAsync();
        var replay = await receiver.ReceiveAsync();

        Assert.True(first.Success, first.Message);
        Assert.False(replay.Success);
        Assert.Equal(FailureReason.ReplayDetected,
            replay.FailureReason);
        Assert.True(replay.Payload.IsEmpty);
        Assert.NotNull(replay.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method SendAsync_EmptyPayload_ThrowsArgumentExceptionAndDoesNotUseTransport.
    /// </summary>
    [Fact]
    public async Task SendAsync_EmptyPayload_ThrowsArgumentExceptionAndDoesNotUseTransport()
    {
        var transport = new RecordingTransport();
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport);

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.SendAsync(
                MercuryTestFactory.BuildContext(),
                MercuryMemory.Empty));

        Assert.Equal(0, transport.SendCount);
    }

    /// <summary>
    /// Defines the test method SendAsync_ProviderFailure_ThrowsInvalidOperationExceptionAndDoesNotUseTransport.
    /// </summary>
    [Fact]
    public async Task SendAsync_ProviderFailure_ThrowsInvalidOperationExceptionAndDoesNotUseTransport()
    {
        var transport = new RecordingTransport();
        var provider = new DelegatingCryptoProvider(
            seal: (_, service, _) => Task.FromResult(
                service.BuildCryptoProviderResult(
                    false,
                    MercuryMemory.Empty,
                    null,
                    FailureReason.AuthenticationFailed,
                    "Provider refused the payload.")));

        var codec = MercuryTestFactory.BuildCodec(
            EnvelopeCodec.Binary, MercuryTestFactory.BuildProvider(ProviderKind.AesGcm));

        var client = MercuryFactory.Instance.BuildClient(new TestDependencies(AlphaClientId(), provider, codec, transport));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.SendAsync(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory([1, 2, 3])));

        Assert.Contains("Provider refused", exception.Message);
        Assert.Equal(0, transport.SendCount);
    }

    /// <summary>
    /// Defines the test method SendAsync_TransportFailure_PropagatesException.
    /// </summary>
    [Fact]
    public async Task SendAsync_TransportFailure_PropagatesException()
    {
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            new ThrowingTransport(
                sendException: new IOException("Outbound transport failed.")));

        var exception = await Assert.ThrowsAsync<IOException>(
            () => client.SendAsync(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory([1, 2, 3])));

        Assert.Contains("Outbound transport failed", exception.Message);
    }

    /// <summary>
    /// Defines the test method SendAsync_CanceledToken_ThrowsOperationCanceledExceptionAndDoesNotUseTransport.
    /// </summary>
    [Fact]
    public async Task SendAsync_CanceledToken_ThrowsOperationCanceledExceptionAndDoesNotUseTransport()
    {
        var transport = new RecordingTransport();
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => client.SendAsync(
                MercuryTestFactory.BuildContext(),
                new MercuryMemory([1, 2, 3]),
                cancellation.Token));

        Assert.Equal(0, transport.SendCount);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_EmptyTransportFrame_ReturnsInternalErrorWithoutPayload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_EmptyTransportFrame_ReturnsInternalErrorWithoutPayload()
    {
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            new EmptyFrameTransport());

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_InvalidEncodedFrame_ReturnsDecodeFailedWithoutPayload.
    /// </summary>
    /// <param name="codec">The codec.</param>
    [Theory]
    [InlineData(EnvelopeCodec.Binary)]
    [InlineData(EnvelopeCodec.Json)]
    public async Task ReceiveAsync_InvalidEncodedFrame_ReturnsDecodeFailedWithoutPayload(
        EnvelopeCodec codec)
    {
        var transport = new QueueTransport();
        transport.Inject(new MercuryMemory([0xDE, 0xAD, 0xBE, 0xEF]));
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            codec,
            transport);

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.DecodeFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_TransportFailure_ReturnsInternalErrorWithoutPayload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_TransportFailure_ReturnsInternalErrorWithoutPayload()
    {
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            new ThrowingTransport(
                receiveException: new IOException("Inbound transport failed.")));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Contains("Inbound transport failed", result.Message);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_ProviderThrows_ReturnsInternalErrorWithoutPayload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ProviderThrows_ReturnsInternalErrorWithoutPayload()
    {
        var realProvider = MercuryTestFactory.BuildProvider(
            ProviderKind.AesGcm);
        var envelope = await MercuryTestFactory.SealAsync(
            realProvider,
            MercuryTestFactory.CreatePayload());
        var codec = MercuryTestFactory.BuildCodec(
            EnvelopeCodec.Binary,
            realProvider);
        var transport = new QueueTransport();
        transport.Inject(codec.Encode(envelope));
        var throwingProvider = new DelegatingCryptoProvider(
            open: (_, _, _) => Task.FromException<ICryptoProviderResult>(
                new InvalidOperationException("Provider exploded.")));
        var client = MercuryFactory.Instance.BuildClient(
            new TestDependencies(AlphaClientId(),
                throwingProvider,
                codec,
                transport,
                new InMemoryReplayProtector()));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Contains("Provider exploded", result.Message);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_ReplayProtectorRejects_ReturnsReplayDetectedWithoutPayload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ReplayProtectorRejects_ReturnsReplayDetectedWithoutPayload()
    {
        var transport = new QueueTransport();
        var sender = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport);
        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory([1, 2, 3]));
        var rejector = new RejectAllReplayProtector();
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport,
            rejector);

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.ReplayDetected,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Equal(1, rejector.CallCount);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_ReplayProtectorThrows_ReturnsInternalErrorWithoutPayload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ReplayProtectorThrows_ReturnsInternalErrorWithoutPayload()
    {
        var transport = new QueueTransport();
        var sender = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport);
        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory([1, 2, 3]));
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport,
            new ThrowingReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.InternalError,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.Contains("Replay check failed", result.Message);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var transport = new QueueTransport();
        var client = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
            EnvelopeCodec.Binary,
            transport);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => client.ReceiveAsync(cancellation.Token));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_DecodeException_ReturnsDecodeFailedWithoutCallingProvider.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_DecodeException_ReturnsDecodeFailedWithoutCallingProvider()
    {
        var providerCalled = false;
        var provider = new DelegatingCryptoProvider(
            open: (_, service, _) =>
            {
                providerCalled = true;
                return Task.FromResult(
                    service.BuildCryptoProviderResult(
                        false,
                        MercuryMemory.Empty,
                        null,
                        FailureReason.InternalError));
            });
        var codec = new DelegatingCodec(
            decode: _ => throw new FormatException("Codec rejected the frame."));
        var transport = new QueueTransport();
        transport.Inject(new MercuryMemory([1]));
        var client = MercuryFactory.Instance.BuildClient(
            new TestDependencies(AlphaClientId(), provider, codec, transport));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.DecodeFailed,
            result.FailureReason);
        Assert.True(result.Payload.IsEmpty);
        Assert.False(providerCalled);
    }
    /// <summary>
    /// Defines the test method ReceiveAsync_AuthenticationFailure_DoesNotCallReplayProtector.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codecType">Type of the codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    [Trait("ArchitectureGuard", "ReplayAfterAuthentication")]
    public async Task ReceiveAsync_AuthenticationFailure_DoesNotCallReplayProtector(
        ProviderKind providerKind,
        EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);
        var recordingTransport = new RecordingTransport();
        var sender = MercuryTestFactory.BuildClient(
            provider,
            codecType,
            recordingTransport,
            new InMemoryReplayProtector());
        await sender.SendAsync(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));
        var codec = MercuryTestFactory.BuildCodec(codecType, provider);
        var envelope = codec.Decode(recordingTransport.LastFrame);
        var protectedPayload = envelope.Payload.ToArray();
        protectedPayload[^1] ^= 0x01;
        var inbound = new QueueTransport();
        inbound.Inject(codec.Encode(
            MercuryTestFactory.CloneEnvelope(
                envelope,
                payload: new MercuryMemory(protectedPayload))));
        var replayProtector = new AcceptAllReplayProtector();
        var receiver = MercuryTestFactory.BuildClient(
            MercuryTestFactory.BuildProvider(providerKind),
            codecType,
            inbound,
            replayProtector);

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        Assert.Equal(FailureReason.AuthenticationFailed,
            result.FailureReason);
        Assert.Equal(0, replayProtector.CallCount);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_ProviderClaimsSuccessWithoutValidatedEnvelope_IsRejected.
    /// </summary>
    [Fact]
    [Trait("ArchitectureGuard", "ProviderContract")]
    public async Task ReceiveAsync_ProviderClaimsSuccessWithoutValidatedEnvelope_IsRejected()
    {
        var realProvider = MercuryTestFactory.BuildProvider(
            ProviderKind.AesGcm);
        var envelope = await MercuryTestFactory.SealAsync(
            realProvider,
            MercuryTestFactory.CreatePayload());
        var codec = MercuryTestFactory.BuildCodec(
            EnvelopeCodec.Binary,
            realProvider);
        var transport = new QueueTransport();
        transport.Inject(codec.Encode(envelope));
        var invalidProvider = new DelegatingCryptoProvider(
            open: (_, service, _) => Task.FromResult(
                service.BuildCryptoProviderResult(
                    true,
                    new MercuryMemory([1, 2, 3]),
                    null,
                    FailureReason.None)));
        var client = MercuryFactory.Instance.BuildClient(
            new TestDependencies(AlphaClientId(),
                invalidProvider,
                codec,
                transport,
                new InMemoryReplayProtector()));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);
        Assert.True(result.Payload.IsEmpty);
        Assert.Null(result.ValidatedEnvelope);
        Assert.NotEqual(FailureReason.None, result.FailureReason);
    }

}
