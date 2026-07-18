// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Kim K. Brown
// Last Modified On : 07-18-2026
// ***********************************************************************
// <copyright file="MercuryClientTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using Mercury.Tests.Support;

namespace Mercury.Tests;

/// <summary>
/// Tests the Mercury client.
/// </summary>
public sealed class MercuryClientTests
{
    /// <summary>
    /// Verifies that the factory builds a client from explicit dependencies.
    /// </summary>
    [Fact]
    public void BuildClient_ReturnsClient()
    {
        var client = BuildClient();

        Assert.NotNull(client);
    }

    /// <summary>
    /// Verifies that a sent payload can be received and recovered.
    /// </summary>
    [Fact]
    public async Task SendAsync_ThenReceiveAsync_ReturnsPayload()
    {
        var client = BuildClient();

        var cryptoContext =
            MercuryFactory.Instance.BuildCryptoContext(
                new KeyId(MercuryTestFactory.SenderKeyId),
                new KeyId(MercuryTestFactory.RecipientKeyId));

        byte[] expected = [1, 2, 3, 4];

        await client.SendAsync(
            cryptoContext,
            new ReadOnlyMemory(expected));

        var result = await client.ReceiveAsync();

        Assert.True(result.Success, result.Message);
        Assert.Equal(expected, result.Payload.ToArray());
    }

    /// <summary>
    /// Builds a client using explicit, non-obsolete dependencies.
    /// </summary>
    /// <returns>The configured Mercury client.</returns>
    private static IMercuryClient BuildClient()
    {
        var provider = MercuryTestFactory.BuildProvider(
            ProviderKind.AesGcm);

        var transport = new QueueTransport();

        var dependencies =
            MercuryFactory.Instance.BuildDependencies(
                provider,
                EnvelopeCodec.Json,
                transport);

        return MercuryFactory.Instance.BuildClient(dependencies);
    }
}