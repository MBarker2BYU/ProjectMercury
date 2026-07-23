// ***********************************************************************
// Assembly       : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
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

using MercuryMemory =
    Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Pipeline;

/// <summary>
/// Class MercuryPipelineFailureTests.
/// This class cannot be inherited.
/// </summary>
public sealed class MercuryPipelineFailureTests
{
    /// <summary>
    /// Gets the provider and codec test cases.
    /// </summary>
    /// <returns>The provider and codec test cases.</returns>
    public static IEnumerable<object[]> ProviderCodecCases()
        => MercuryTestFactory.ProviderCodecCases();

    /// <summary>
    /// Verifies that a receiver using the wrong key cannot
    /// authenticate or recover the protected payload.
    /// </summary>
    /// <param name="providerKind">The provider kind.</param>
    /// <param name="codec">The envelope codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_WrongKey_ReturnsAuthenticationFailureWithoutPayload(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var transport = new QueueTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(
                    providerKind, MercuryTestFactory.BuildKeyProvider(MercuryTestFactory.PrimaryKey)), codec,
                transport, new InMemoryReplayProtector());

        var receiver =
            MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind,
                    MercuryTestFactory.BuildKeyProvider(MercuryTestFactory.AlternateKey)), codec,
                transport, new InMemoryReplayProtector());

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.AuthenticationFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Verifies that a receiver using the wrong crypto provider
    /// cannot authenticate the envelope.
    /// </summary>
    /// <param name="senderProviderKind">
    /// The sender provider kind.
    /// </param>
    /// <param name="receiverProviderKind">
    /// The receiver provider kind.
    /// </param>
    [Theory]
    [InlineData(ProviderKind.AesGcm, ProviderKind.ChaCha20)]
    [InlineData(ProviderKind.ChaCha20, ProviderKind.AesGcm)]
    public async Task ReceiveAsync_WrongProvider_ReturnsAuthenticationFailureWithoutPayload(ProviderKind senderProviderKind, ProviderKind receiverProviderKind)
    {
        var transport = new QueueTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(senderProviderKind), 
            EnvelopeCodec.Binary, transport, new InMemoryReplayProtector());

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(receiverProviderKind),
                EnvelopeCodec.Binary, transport, new InMemoryReplayProtector());

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.AuthenticationFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Verifies that modifying the protected payload causes
    /// authentication to fail.
    /// </summary>
    /// <param name="providerKind">The provider kind.</param>
    /// <param name="codecType">The codec type.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedProtectedPayload_ReturnsAuthenticationFailureWithoutPayload(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);

        var recordingTransport = new RecordingTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), provider, codecType,
                recordingTransport, new InMemoryReplayProtector());

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var codec = MercuryTestFactory.BuildCodec(codecType, provider);

        var envelope = codec.Decode(recordingTransport.LastFrame);

        var protectedPayload = envelope.Payload.ToArray();

        protectedPayload[^1] ^= 0x80;

        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, payload:
                    new MercuryMemory(protectedPayload));

        var inbound = new QueueTransport();

        inbound.Inject(codec.Encode(tamperedEnvelope));

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codecType, inbound, new InMemoryReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.AuthenticationFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Verifies that modifying authenticated header information
    /// causes authentication to fail.
    /// </summary>
    /// <param name="providerKind">The provider kind.</param>
    /// <param name="codecType">The codec type.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_TamperedAuthenticatedHeader_ReturnsAuthenticationFailureWithoutPayload(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);

        var recordingTransport = new RecordingTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), provider,
                codecType, recordingTransport, new InMemoryReplayProtector());

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var codec = MercuryTestFactory.BuildCodec(codecType, provider);

        var envelope = codec.Decode(recordingTransport.LastFrame);

        var tamperedHeader = MercuryTestFactory.CloneHeader(envelope.Header, senderKeyId: new KeyId("mallory"));

        var tamperedEnvelope = MercuryTestFactory.CloneEnvelope(envelope, header: tamperedHeader);

        var inbound = new QueueTransport();

        inbound.Inject(codec.Encode(tamperedEnvelope));

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codecType, inbound, new InMemoryReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.AuthenticationFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Verifies that receiving the same protected frame twice
    /// causes the second receive to fail replay validation.
    /// </summary>
    /// <param name="providerKind">The provider kind.</param>
    /// <param name="codecType">The codec type.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task ReceiveAsync_ReplayedFrame_ReturnsReplayDetectedWithoutPayload(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);

        var recordingTransport = new RecordingTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), provider, codecType,
                recordingTransport, new InMemoryReplayProtector());

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var inbound = new QueueTransport();

        inbound.Inject(recordingTransport.LastFrame);

        inbound.Inject(recordingTransport.LastFrame);

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codecType, inbound, new InMemoryReplayProtector());

        var first = await receiver.ReceiveAsync();

        var replay = await receiver.ReceiveAsync();

        Assert.True(first.Success, first.Message);

        Assert.False(replay.Success);

        Assert.Equal(FailureReason.ReplayDetected, replay.FailureReason);

        Assert.True(replay.Payload.IsEmpty);

        Assert.NotNull(replay.ValidatedEnvelope);
    }

    /// <summary>
    /// Verifies that an empty outgoing payload is rejected before
    /// the transport is used.
    /// </summary>
    [Fact]
    public async Task SendAsync_EmptyPayload_ThrowsArgumentExceptionAndDoesNotUseTransport()
    {
        var transport = new RecordingTransport();

        var client =
            MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                EnvelopeCodec.Binary, transport);

        await Assert.ThrowsAsync<ArgumentException>(() => client.SendAsync(MercuryTestFactory.BuildContext(), MercuryMemory.Empty));

        Assert.Equal(0, transport.SendCount);
    }

    /// <summary>
    /// Verifies that a provider failure prevents the transport
    /// from being used.
    /// </summary>
    [Fact]
    public async Task SendAsync_ProviderFailure_ThrowsInvalidOperationExceptionAndDoesNotUseTransport()
    {
        var transport = new RecordingTransport();

        var provider = new DelegatingCryptoProvider(seal: (_, service, _) =>
                    Task.FromResult(service.BuildCryptoProviderResult(false, MercuryMemory.Empty,
                            null, FailureReason.AuthenticationFailed, "Provider refused the payload.")));

        var codec = MercuryTestFactory.BuildCodec(EnvelopeCodec.Binary, MercuryTestFactory.BuildProvider(ProviderKind.AesGcm));

        var client = MercuryFactory.Instance.BuildClient(new TestDependencies(MercuryTestFactory.AlphaClientId(),
                    provider, codec, transport));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendAsync(MercuryTestFactory.BuildContext(),
                    new MercuryMemory([1, 2, 3])));

        Assert.Contains("Provider refused", exception.Message);

        Assert.Equal(0, transport.SendCount);
    }

    /// <summary>
    /// Verifies that an outbound transport exception is propagated.
    /// </summary>
    [Fact]
    public async Task SendAsync_TransportFailure_PropagatesException()
    {
        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                EnvelopeCodec.Binary, new ThrowingTransport(sendException: new IOException("Outbound transport failed.")));

        var exception = await Assert.ThrowsAsync<IOException>(() => client.SendAsync(MercuryTestFactory.BuildContext(), 
            new MercuryMemory([1, 2, 3])));

        Assert.Contains("Outbound transport failed", exception.Message);
    }

    /// <summary>
    /// Verifies that a cancelled send does not use the transport.
    /// </summary>
    [Fact]
    public async Task SendAsync_CanceledToken_ThrowsOperationCanceledExceptionAndDoesNotUseTransport()
    {
        var transport =
            new RecordingTransport();

        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                EnvelopeCodec.Binary, transport);

        using var cancellation = new CancellationTokenSource();

        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => client.SendAsync(MercuryTestFactory.BuildContext(),
                new MercuryMemory([1, 2, 3]), cancellation.Token));

        Assert.Equal(0, transport.SendCount);
    }

    /// <summary>
    /// Verifies that an empty transport frame returns an internal
    /// failure without a payload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_EmptyTransportFrame_ReturnsInternalErrorWithoutPayload()
    {
        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                EnvelopeCodec.Binary, new EmptyFrameTransport());

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.InternalError, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Verifies that an invalid encoded frame returns a decoding
    /// failure without a payload.
    /// </summary>
    /// <param name="codec">The codec.</param>
    [Theory]
    [InlineData(EnvelopeCodec.Binary)]
    [InlineData(EnvelopeCodec.Json)]
    public async Task ReceiveAsync_InvalidEncodedFrame_ReturnsDecodeFailedWithoutPayload(EnvelopeCodec codec)
    {
        var transport = new QueueTransport();

        transport.Inject(new MercuryMemory([0xDE, 0xAD, 0xBE, 0xEF]));

        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                codec, transport);

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.DecodeFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.Null(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Verifies that an inbound transport failure is returned as an
    /// internal failure without a payload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_TransportFailure_ReturnsInternalErrorWithoutPayload()
    {
        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                EnvelopeCodec.Binary, new ThrowingTransport(receiveException: new IOException("Inbound transport failed.")));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.InternalError, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.Contains("Inbound transport failed", result.Message);
    }

    /// <summary>
    /// Verifies that an exception thrown by a provider during open
    /// is returned as an internal failure.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ProviderThrows_ReturnsInternalErrorWithoutPayload()
    {
        var realProvider = MercuryTestFactory.BuildProvider(ProviderKind.AesGcm);

        var envelope = await MercuryTestFactory.SealAsync(realProvider, MercuryTestFactory.CreatePayload());

        var codec = MercuryTestFactory.BuildCodec(EnvelopeCodec.Binary, realProvider);

        var transport = new QueueTransport();

        transport.Inject(codec.Encode(envelope));

        var throwingProvider = new DelegatingCryptoProvider(open: (_, _, _) 
            => Task.FromException<ICryptoProviderResult>(new InvalidOperationException("Provider exploded.")));

        var client = MercuryFactory.Instance.BuildClient(new TestDependencies(MercuryTestFactory.BravoClientId(), throwingProvider,
                    codec, transport, new InMemoryReplayProtector()));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.InternalError, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.Contains("Provider exploded", result.Message);
    }

    /// <summary>
    /// Verifies that a replay protector rejection returns a replay
    /// failure without a payload.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ReplayProtectorRejects_ReturnsReplayDetectedWithoutPayload()
    {
        var transport = new QueueTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(),
                MercuryTestFactory.BuildProvider(ProviderKind.AesGcm), EnvelopeCodec.Binary, transport);

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory([1, 2, 3]));

        var rejector =
            new RejectAllReplayProtector();

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(),
                MercuryTestFactory.BuildProvider(ProviderKind.AesGcm), EnvelopeCodec.Binary, transport, rejector);

        var result =
            await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.ReplayDetected, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.Equal(1, rejector.CallCount);
    }

    /// <summary>
    /// Verifies that a replay protector exception is returned as an
    /// internal failure.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_ReplayProtectorThrows_ReturnsInternalErrorWithoutPayload()
    {
        var transport = new QueueTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), 
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm), EnvelopeCodec.Binary, transport);

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory([1, 2, 3]));

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), 
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm), EnvelopeCodec.Binary, transport,
                new ThrowingReplayProtector());

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.InternalError, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.Contains("Replay check failed", result.Message);
    }

    /// <summary>
    /// Verifies that a cancelled receive propagates cancellation.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        var transport = new QueueTransport();

        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), 
            MercuryTestFactory.BuildProvider(ProviderKind.AesGcm), EnvelopeCodec.Binary, transport);

        using var cancellation = new CancellationTokenSource();

        await cancellation.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => client.ReceiveAsync(cancellation.Token));
    }

    /// <summary>
    /// Verifies that a codec exception returns a decoding failure
    /// without invoking the provider.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_DecodeException_ReturnsDecodeFailedWithoutCallingProvider()
    {
        var providerCalled = false;

        var provider = new DelegatingCryptoProvider(open: (_, service, _) =>
                { providerCalled = true;

                    return Task.FromResult(
                        service.BuildCryptoProviderResult(
                            false,
                            MercuryMemory.Empty,
                            null,
                            FailureReason.InternalError));
                });

        var codec = new DelegatingCodec(decode: _ 
            => throw new FormatException("Codec rejected the frame."));

        var transport = new QueueTransport();

        transport.Inject(new MercuryMemory([1]));

        var client = MercuryFactory.Instance.BuildClient(new TestDependencies(MercuryTestFactory.BravoClientId(),
                    provider, codec, transport));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);

        Assert.Equal(FailureReason.DecodeFailed, result.FailureReason);

        Assert.True(result.Payload.IsEmpty);

        Assert.False(providerCalled);
    }

    /// <summary>
    /// Verifies that replay protection is not called when provider
    /// authentication fails.
    /// </summary>
    /// <param name="providerKind">The provider kind.</param>
    /// <param name="codecType">The codec type.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    [Trait("ArchitectureGuard", "ReplayAfterAuthentication")]
    public async Task ReceiveAsync_AuthenticationFailure_DoesNotCallReplayProtector(ProviderKind providerKind, EnvelopeCodec codecType)
    {
        var provider = MercuryTestFactory.BuildProvider(providerKind);

        var recordingTransport = new RecordingTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), provider, codecType,
                recordingTransport, new InMemoryReplayProtector());

        await sender.SendAsync(MercuryTestFactory.BuildContext(), 
            new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var codec = MercuryTestFactory.BuildCodec(codecType, provider);

        var envelope = codec.Decode(recordingTransport.LastFrame);

        var protectedPayload = envelope.Payload.ToArray();

        protectedPayload[^1] ^= 0x01;

        var inbound = new QueueTransport();

        inbound.Inject(codec.Encode(MercuryTestFactory.CloneEnvelope(envelope, payload:
                        new MercuryMemory(protectedPayload))));

        var replayProtector = new AcceptAllReplayProtector();

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), 
            MercuryTestFactory.BuildProvider(providerKind), codecType, inbound, replayProtector);

        var result = await receiver.ReceiveAsync();

        Assert.False(result.Success);
        
        Assert.Equal(FailureReason.AuthenticationFailed, result.FailureReason);

        Assert.Equal(0, replayProtector.CallCount);
    }

    /// <summary>
    /// Verifies that a provider cannot claim success without
    /// returning a validated secure envelope.
    /// </summary>
    [Fact]
    [Trait("ArchitectureGuard", "ProviderContract")]
    public async Task ReceiveAsync_ProviderClaimsSuccessWithoutValidatedEnvelope_IsRejected()
    {
        var realProvider = MercuryTestFactory.BuildProvider(ProviderKind.AesGcm);

        var envelope = await MercuryTestFactory.SealAsync(realProvider,
                MercuryTestFactory.CreatePayload());

        var codec = MercuryTestFactory.BuildCodec(EnvelopeCodec.Binary, realProvider);

        var transport = new QueueTransport();

        transport.Inject(codec.Encode(envelope));

        var invalidProvider = new DelegatingCryptoProvider(open: (_, service, _)
            => Task.FromResult(service.BuildCryptoProviderResult(true,
                            new MercuryMemory([1, 2, 3]), null, FailureReason.None)));

        var client = MercuryFactory.Instance.BuildClient(new TestDependencies(
                    MercuryTestFactory.BravoClientId(), invalidProvider, codec,
                    transport, new InMemoryReplayProtector()));

        var result = await client.ReceiveAsync();

        Assert.False(result.Success);

        Assert.True(result.Payload.IsEmpty);

        Assert.Null(result.ValidatedEnvelope);

        Assert.NotEqual(FailureReason.None, result.FailureReason);
    }
}