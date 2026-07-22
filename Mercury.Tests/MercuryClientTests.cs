// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="MercuryClientTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using System.Security.Cryptography;

namespace Mercury.Tests;
/// <summary>
/// Class MercuryClientTests.
/// </summary>
public class MercuryClientTests
{

    internal const string ALPHA = @"Alpha";
    internal const string BRAVO = @"Bravo";

    /// <summary>
    /// Defines the test method BuildClient_ReturnsClient.
    /// </summary>
    [Fact]
    public void BuildClient_ReturnsClient()
    {
        var client =
            MercuryFactory.Instance.BuildClient(ALPHA);

        Assert.NotNull(client);
    }

    /// <summary>
    /// Defines the test method SendAsync_ThenReceiveAsync_ReturnsPayload.
    /// </summary>
    [Fact]
    public async Task SendAsync_ThenReceiveAsync_ReturnsPayload()
    {
        
        var client =
            MercuryFactory.Instance.BuildClient(ALPHA);

        var cryptoContext = MercuryFactory.Instance.BuildCryptoContext(ALPHA, BRAVO);

        var expected = new byte[] { 1, 2, 3, 4 };

        await client.SendAsync(cryptoContext, new ReadOnlyMemory(expected));

        var result =
            await client.ReceiveAsync();

        Assert.True(result.Success);
        Assert.Equal(expected, result.Payload.ToArray());
    }
}