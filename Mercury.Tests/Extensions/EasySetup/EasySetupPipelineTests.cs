// ***********************************************************************
// Assembly       : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="EasySetupPipelineTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Transport;
using Mercury.Core.Factories;
using Mercury.Extensions.EasySetup;
using Mercury.Provider.AesGcm;
using Mercury.Transport.InMemory;

namespace Mercury.Tests.Extensions.EasySetup;

/// <summary>
/// Tests the EasySetup layer through the complete Mercury pipeline.
/// </summary>
public sealed class EasySetupPipelineTests
{
    /// <summary>
    /// The Alpha key name.
    /// </summary>
    private const string ALPHA_NODE = "Alpha Node";

    /// <summary>
    /// The Bravo key name.
    /// </summary>
    private const string BRAVO_NODE = "Bravo Node";

    /// <summary>
    /// Verifies an Alpha-to-Bravo round trip.
    /// </summary>
    /// <param name="envelopeCodec">The envelope codec.</param>
    [Theory]
    [InlineData(EnvelopeCodec.Binary)]
    [InlineData(EnvelopeCodec.Json)]
    public async Task AlphaToBravo_RoundTrip_ReturnsOriginalPayload(EnvelopeCodec envelopeCodec)
    {
        var pair = BuildPair(envelopeCodec);

        var payload = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };

        await pair.AlphaClient.SendAsync(pair.AlphaToBravoContext, payload);

        var result = await pair.BravoClient.ReceiveAsync();

        Assert.True(result.Success);

        Assert.Equal(payload, result.Payload.ToArray());

        Assert.NotNull(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Verifies a Bravo-to-Alpha round trip.
    /// </summary>
    /// <param name="envelopeCodec">The envelope codec.</param>
    [Theory]
    [InlineData(EnvelopeCodec.Binary)]
    [InlineData(EnvelopeCodec.Json)]
    public async Task BravoToAlpha_RoundTrip_ReturnsOriginalPayload(EnvelopeCodec envelopeCodec)
    {
        var pair = BuildPair(envelopeCodec);

        var payload = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };

        await pair.BravoClient.SendAsync(pair.BravoToAlphaContext, payload);

        var result = await pair.AlphaClient.ReceiveAsync();

        Assert.True(result.Success);

        Assert.Equal(payload, result.Payload.ToArray());

        Assert.NotNull(result.ValidatedEnvelope);
    }

    /// <summary>
    /// Verifies that separately created temporary sessions cannot
    /// unprotect each other's frames.
    /// </summary>
    [Fact]
    public async Task SeparateEphemeralSessions_CannotUnprotectEachOthersFrames()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var firstPair = BuildPair(EnvelopeCodec.Binary, alphaTransport, bravoTransport);

        var secondPair = BuildPair(EnvelopeCodec.Binary, alphaTransport, bravoTransport);

        var payload = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

        await firstPair.AlphaClient.SendAsync(firstPair.AlphaToBravoContext, payload);

        var result = await secondPair.BravoClient.ReceiveAsync();

        Assert.False(result.Success);

        Assert.True(result.Payload.IsEmpty);
    }

    /// <summary>
    /// Builds a client pair with newly created connected transports.
    /// </summary>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <returns>The client pair.</returns>
    private static MercuryClientPair BuildPair(EnvelopeCodec envelopeCodec)
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        return BuildPair(envelopeCodec, alphaTransport, bravoTransport);
    }

    /// <summary>
    /// Builds a client pair with supplied transports.
    /// </summary>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="alphaTransport">The Alpha transport.</param>
    /// <param name="bravoTransport">The Bravo transport.</param>
    /// <returns>The client pair.</returns>
    private static MercuryClientPair BuildPair(EnvelopeCodec envelopeCodec, ITransport alphaTransport, ITransport bravoTransport)
    {
        return MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE, keyProvider 
                => new AesGcmCryptoProvider(keyProvider), envelopeCodec, alphaTransport, bravoTransport);
    }
}