// ***********************************************************************
// Assembly       : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="FileDropPipelineTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Core.Replay;
using Mercury.Tests.Support;
using Mercury.Transport.FileDrop;

using MercuryMemory =
    Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Transports;

/// <summary>
/// Class FileDropPipelineTests. This class cannot be inherited.
/// </summary>
public sealed class FileDropPipelineTests
{
    /// <summary>
    /// Gets the provider and codec test cases.
    /// </summary>
    /// <returns>The provider and codec test cases.</returns>
    public static IEnumerable<object[]> ProviderCodecCases()
        => MercuryTestFactory.ProviderCodecCases();

    /// <summary>
    /// Defines the test method
    /// FileDropPipeline_SendThenReceive_ReturnsOriginalPayload.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task FileDropPipeline_SendThenReceive_ReturnsOriginalPayload(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var rootPath = Path.Combine(Path.GetTempPath(), "Mercury.Tests", Guid.NewGuid().ToString("N"));

        try
        {
            var alphaInbox = Path.Combine(rootPath, "alpha-inbox");

            var bravoInbox = Path.Combine(rootPath, "bravo-inbox");

            var alphaTransport = new FileDropTransport(alphaInbox, bravoInbox);

            var bravoTransport = new FileDropTransport(bravoInbox, alphaInbox);

            var alphaClient = MercuryTestFactory.BuildClient(MercuryTestFactory.AlphaClientId(), MercuryTestFactory.BuildProvider(providerKind),
                    codec, alphaTransport, new InMemoryReplayProtector());

            var bravoClient = MercuryTestFactory.BuildClient(MercuryTestFactory.BravoClientId(), MercuryTestFactory.BuildProvider(providerKind),
                    codec, bravoTransport, new InMemoryReplayProtector());

            var expected = MercuryTestFactory.CreatePayload(8192);

            await alphaClient.SendAsync(MercuryTestFactory.BuildContext(), new MercuryMemory(expected));

            var result = await bravoClient.ReceiveAsync();

            Assert.True(result.Success, result.Message);

            Assert.Equal(FailureReason.None, result.FailureReason);

            Assert.Equal(expected, result.Payload.ToArray());

            Assert.NotNull(result.ValidatedEnvelope);

            Assert.Empty(Directory.EnumerateFiles(bravoInbox, "*.mercury"));
        }
        finally
        {
            if (Directory.Exists(rootPath))
            {
                Directory.Delete(rootPath, recursive: true);
            }
        }
    }

    /// <summary>
    /// Defines the test method
    /// FileDropPipeline_BidirectionalExchange_ReturnsBothPayloads.
    /// </summary>
    /// <param name="providerKind">Kind of the provider.</param>
    /// <param name="codec">The codec.</param>
    [Theory]
    [MemberData(nameof(ProviderCodecCases))]
    public async Task FileDropPipeline_BidirectionalExchange_ReturnsBothPayloads(ProviderKind providerKind, EnvelopeCodec codec)
    {
        var rootPath = Path.Combine(Path.GetTempPath(), "Mercury.Tests", Guid.NewGuid().ToString("N"));

        try
        {
            var alphaInbox = Path.Combine(rootPath, "alpha-inbox");

            var bravoInbox = Path.Combine(rootPath, "bravo-inbox");

            var alphaTransport = new FileDropTransport(alphaInbox, bravoInbox);

            var bravoTransport = new FileDropTransport(bravoInbox, alphaInbox);

            var alphaClientId = MercuryTestFactory.AlphaClientId();

            var bravoClientId = MercuryTestFactory.BravoClientId();

            /*
             * The normal test key provider only needs the Bravo
             * recipient key for Alpha-to-Bravo tests.
             *
             * A bidirectional exchange also requires an Alpha
             * recipient key because Bravo sends back to Alpha.
             */
            var bidirectionalKeys = new TestSymmetricKeyProvider((alphaClientId.Value, MercuryTestFactory.PrimaryKey),
                    (bravoClientId.Value, MercuryTestFactory.PrimaryKey));

            var alphaProvider = MercuryTestFactory.BuildProvider(providerKind, bidirectionalKeys);

            var bravoProvider = MercuryTestFactory.BuildProvider(providerKind, bidirectionalKeys);

            var alphaClient = MercuryTestFactory.BuildClient(alphaClientId, alphaProvider, codec, alphaTransport, 
                new InMemoryReplayProtector());

            var bravoClient = MercuryTestFactory.BuildClient(bravoClientId, bravoProvider, codec, bravoTransport,
                    new InMemoryReplayProtector());

            var alphaPayload = MercuryTestFactory.CreatePayload(4096);

            var bravoPayload = MercuryTestFactory.CreatePayload(6144);

            var alphaToBravoContext = MercuryTestFactory.BuildContext(alphaClientId.Value, bravoClientId.Value);

            var bravoToAlphaContext = MercuryTestFactory.BuildContext(bravoClientId.Value, alphaClientId.Value);

            await Task.WhenAll(alphaClient.SendAsync(alphaToBravoContext, new MercuryMemory(alphaPayload)),
                bravoClient.SendAsync(bravoToAlphaContext, new MercuryMemory(bravoPayload)));

            var alphaResult = await alphaClient.ReceiveAsync();

            var bravoResult = await bravoClient.ReceiveAsync();

            Assert.True(alphaResult.Success, alphaResult.Message);

            Assert.True(bravoResult.Success, bravoResult.Message);

            Assert.Equal(bravoPayload, alphaResult.Payload.ToArray());

            Assert.Equal(alphaPayload, bravoResult.Payload.ToArray());
        }
        finally
        {
            if (Directory.Exists(rootPath))
            {
                Directory.Delete(
                    rootPath,
                    recursive: true);
            }
        }
    }
}