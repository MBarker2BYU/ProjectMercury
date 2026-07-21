// ***********************************************************************
// Assembly       : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="MercuryClientPairTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using Mercury.Extensions.EasySetup;
using Mercury.Provider.AesGcm;
using Mercury.Transport.InMemory;

namespace Mercury.Tests.Extensions.EasySetup;

/// <summary>
/// Tests the <see cref="MercuryClientPair"/> class.
/// </summary>
public sealed class MercuryClientPairTests
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
    /// Verifies that the Alpha client is assigned.
    /// </summary>
    [Fact]
    public void BuildEphemeralClientPair_AssignsAlphaClient()
    {
        var pair = BuildPair();

        Assert.NotNull(pair.AlphaClient);
    }

    /// <summary>
    /// Verifies that the Bravo client is assigned.
    /// </summary>
    [Fact]
    public void BuildEphemeralClientPair_AssignsBravoClient()
    {
        var pair = BuildPair();
        
        Assert.NotNull(pair.BravoClient);
    }

    /// <summary>
    /// Verifies the Alpha-to-Bravo context.
    /// </summary>
    [Fact]
    public void AlphaToBravoContext_UsesExpectedKeyIdentifiers()
    {
        var pair = BuildPair();

        Assert.Equal(new KeyId(ALPHA_NODE), pair.AlphaToBravoContext.SenderKeyId);

        Assert.Equal(new KeyId(BRAVO_NODE), pair.AlphaToBravoContext.RecipientKeyId);
    }

    /// <summary>
    /// Verifies the Bravo-to-Alpha context.
    /// </summary>
    [Fact]
    public void BravoToAlphaContext_UsesExpectedKeyIdentifiers()
    {
        var pair = BuildPair();

        Assert.Equal(new KeyId(BRAVO_NODE), pair.BravoToAlphaContext.SenderKeyId);

        Assert.Equal(new KeyId(ALPHA_NODE), pair.BravoToAlphaContext.RecipientKeyId);
    }

    /// <summary>
    /// Builds a valid temporary client pair.
    /// </summary>
    /// <returns>The client pair.</returns>
    private static MercuryClientPair BuildPair()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        return MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE, keyProvider 
                => new AesGcmCryptoProvider(keyProvider), EnvelopeCodec.Binary, alphaTransport, bravoTransport).Result;
    }
}