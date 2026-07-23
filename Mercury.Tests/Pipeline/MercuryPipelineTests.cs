// ***********************************************************************
// Assembly     : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="MercuryPipelineTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Net.Sockets;
using System.Text;
using Mercury.Abstractions.Enums;
using Mercury.Core.Replay;
using Mercury.Tests.Support;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Pipeline;

/// <summary>
/// Class MercuryPipelineTests. This class cannot be inherited.
/// </summary>
public sealed class MercuryPipelineTests
{
    /// <summary>
    /// Gets the provider and codec test cases.
    /// </summary>
    /// <returns>The provider and codec test cases.</returns>
    public static IEnumerable<object[]> ProviderCodecCases()
        => MercuryTestFactory.ProviderCodecCases();

    /// <summary>
    /// Defines the test method
    /// InMemoryPipeline_SendThenReceive_ReturnsOriginalPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task InMemoryPipeline_SendThenReceive_ReturnsOriginalPayload(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var alphaClient = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(),
                MercuryTestFactory.BuildProvider(providerKind), codec, alphaTransport,
                new InMemoryReplayProtector());

        var bravoClient = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(),
                MercuryTestFactory.BuildProvider(providerKind), codec, bravoTransport, 
                new InMemoryReplayProtector());

        var payload = MercuryTestFactory.CreatePayload(4096);

        await alphaClient.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(payload));

        var result = await bravoClient.ReceiveAsync();

        Assert.True(result.Success, result.Message);

        Assert.Equal(FailureReason.None, result.FailureReason);

        Assert.Equal(payload, result.Payload.ToArray());

        Assert.NotNull(result.ValidatedEnvelope);

        Assert.Equal(MercuryTestFactory.BuildProvider(providerKind).Name,
            result.ValidatedEnvelope.Header.Encryption.Value);
    }

    /// <summary>
    /// Defines the test method
    /// TcpPipeline_SendThenReceive_ReturnsOriginalPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task TcpPipeline_SendThenReceive_ReturnsOriginalPayload(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var (alphaTransport, bravoTransport) = await CreateTcpPairAsync();

        await using (alphaTransport)
        await using (bravoTransport)
        {
            var alphaClient =
                MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(providerKind),
                    codec, alphaTransport, new InMemoryReplayProtector());

            var bravoClient = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                    codec, bravoTransport, new InMemoryReplayProtector());

            var payload = MercuryTestFactory.CreatePayload(8192);

            await alphaClient.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(payload));

            var result = await bravoClient.ReceiveAsync();

            Assert.True(result.Success, result.Message);

            Assert.Equal(payload, result.Payload.ToArray());
        }
    }

    /// <summary>
    /// Defines the test method
    /// InMemoryPipeline_MultipleExchanges_PreservesOrderAndPayloads.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task InMemoryPipeline_MultipleExchanges_PreservesOrderAndPayloads(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair(capacity: 64);

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, alphaTransport, new InMemoryReplayProtector());

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, bravoTransport, new InMemoryReplayProtector());

        var payloads = Enumerable.Range(1, 25)
                .Select(index =>
                        Enumerable.Range(0, index * 17)
                            .Select(value => (byte)(value + index))
                            .ToArray())
                .ToArray();

        foreach (var payload in payloads)
        {
            await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(payload));
        }

        foreach (var expected in payloads)
        {
            var result = await receiver.ReceiveAsync();

            Assert.True(result.Success, result.Message);

            Assert.Equal(expected, result.Payload.ToArray());
        }
    }

    /// <summary>
    /// Defines the test method
    /// Pipeline_BinaryPayloadWithNullsAndAllByteValues_RoundTripsExactly.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task Pipeline_BinaryPayloadWithNullsAndAllByteValues_RoundTripsExactly(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var transport = new QueueTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, transport, new InMemoryReplayProtector());

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, transport, new InMemoryReplayProtector());

        var payload = Enumerable.Range(0, 2048)
                .Select(value => (byte)(value % 256))
                .ToArray();

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(payload));

        var result = await receiver.ReceiveAsync();

        Assert.True(result.Success, result.Message);

        Assert.Equal(payload, result.Payload.ToArray());
    }

    /// <summary>
    /// Defines the test method
    /// SendAsync_TransportBoundaryDoesNotContainOriginalPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task SendAsync_TransportBoundaryDoesNotContainOriginalPayload(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var transport = new RecordingTransport();

        var client = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, transport, new InMemoryReplayProtector());

        var payload = Encoding.UTF8.GetBytes("MERCURY-RAW-PAYLOAD-BOUNDARY-CHECK-" +
                                             string.Concat(Enumerable.Repeat("0123456789ABCDEF", 16)));

        await client.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(payload));

        var frame = transport.LastFrame.ToArray();

        Assert.Equal(1, transport.SendCount);

        Assert.NotEmpty(frame);

        Assert.False(ByteSearch.ContainsSubsequence(frame, payload), "The encoded transport frame contains the complete original payload.");
    }

    /// <summary>
    /// Defines the test method
    /// Pipeline_OneMegabytePayload_RoundTripsExactly.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task Pipeline_OneMegabytePayload_RoundTripsExactly(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var transport = new QueueTransport();

        var sender = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, transport, new InMemoryReplayProtector());

        var receiver = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                codec, transport, new InMemoryReplayProtector());

        var payload = MercuryTestFactory.CreatePayload(1024 * 1024);

        await sender.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(payload));

        var result = await receiver.ReceiveAsync();

        Assert.True(result.Success, result.Message);

        Assert.Equal(payload, result.Payload.ToArray());
    }

    /// <summary>
    /// Creates a connected TCP transport pair.
    /// </summary>
    /// <returns>
    /// The connected Alpha and Bravo TCP transports.
    /// </returns>
    private static async Task<(TcpTransport Alpha, TcpTransport Bravo)> CreateTcpPairAsync()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();

        try
        {
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            
            var acceptTask = TcpTransport.AcceptAsync(listener);

            var connectTask = TcpTransport.ConnectAsync("127.0.0.1", port);

            await Task.WhenAll(acceptTask, connectTask);

            return (await connectTask, await acceptTask);
        }
        finally
        {
            listener.Stop();
        }
    }
}