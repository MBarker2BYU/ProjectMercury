// ***********************************************************************
// Assembly       : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="MercuryFactoryExtensionsTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Security.Cryptography;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Core.Factories;
using Mercury.Extensions.EasySetup;
using Mercury.Provider.AesGcm;
using Mercury.Transport.InMemory;

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Extensions.EasySetup;

/// <summary>
/// Tests the <see cref="MercuryFactoryExtensions"/> class.
/// </summary>
public sealed class MercuryFactoryExtensionsTests
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
    /// Gets the alpha client identifier.
    /// </summary>
    /// <value>The alpha client identifier.</value>
    public static KeyId AlphaClientId()
        => ALPHA_NODE;

    /// <summary>
    /// Verifies that a valid easy client can be built.
    /// </summary>
    [Fact]
    public async void BuildEasyClient_WithValidComponents_ReturnsClient()
    {
        var keyProvider = EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var cryptoProvider = new AesGcmCryptoProvider(keyProvider);

        var (alphaTransport, _) = InMemoryDuplexTransport.CreateConnectedPair();
        
        var client = MercuryFactory.Instance.BuildEasyClient(ALPHA_NODE, cryptoProvider, EnvelopeCodec.Binary, alphaTransport);

        Assert.NotNull(client);
    }

    /// <summary>
    /// Verifies that a null factory is rejected.
    /// </summary>
    [Fact]
    public void BuildEasyClient_WithNullFactory_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => MercuryFactoryExtensions.BuildEasyClient(null!,null!, null!,
                        EnvelopeCodec.Binary, null!));

        Assert.Equal("factory", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a null crypto provider is rejected.
    /// </summary>
    [Fact]
    public void BuildEasyClient_WithNullCryptoProvider_ThrowsArgumentNullException()
    {
        var (alphaTransport, _) = InMemoryDuplexTransport.CreateConnectedPair();

        var exception = Assert.Throws<ArgumentNullException>(() =>
                    MercuryFactory.Instance.BuildEasyClient(AlphaClientId(),null!, EnvelopeCodec.Binary, alphaTransport));

        Assert.Equal("cryptoProvider", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a null transport is rejected.
    /// </summary>
    [Fact]
    public void BuildEasyClient_WithNullTransport_ThrowsArgumentNullException()
    {
        var keyProvider = EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var cryptoProvider = new AesGcmCryptoProvider(keyProvider);

        var exception = Assert.Throws<ArgumentNullException>(() =>
                    MercuryFactory.Instance.BuildEasyClient(AlphaClientId(), cryptoProvider, EnvelopeCodec.Binary, null!));

        Assert.Equal("transport", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a valid ephemeral client pair is returned.
    /// </summary>
    [Fact]
    public void BuildEphemeralClientPair_WithValidConfiguration_ReturnsPair()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var pair = MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE,
                BuildCryptoProvider, EnvelopeCodec.Binary, alphaTransport, bravoTransport).Result;

        Assert.NotNull(pair);
        Assert.NotNull(pair.AlphaClient);
        Assert.NotNull(pair.BravoClient);
        Assert.NotNull(pair.AlphaToBravoContext);
        Assert.NotNull(pair.BravoToAlphaContext);
    }

    /// <summary>
    /// Verifies that separate Mercury clients are created.
    /// </summary>
    [Fact]
    public void BuildEphemeralClientPair_CreatesSeparateClients()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var pair =
            MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE,
                BuildCryptoProvider, EnvelopeCodec.Binary, alphaTransport, bravoTransport).Result;

        Assert.NotSame(pair.AlphaClient, pair.BravoClient);
    }

    /// <summary>
    /// Verifies that both providers receive the same temporary key provider.
    /// </summary>
    [Fact]
    public void BuildEphemeralClientPair_UsesSharedKeyProvider()
    {
        var receivedKeyProviders = new List<SymmetricKeyProviderDictionary>();

        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var pair = MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE, keyProvider 
                => { receivedKeyProviders.Add(keyProvider); return new AesGcmCryptoProvider(keyProvider); }, EnvelopeCodec.Binary,
                alphaTransport, bravoTransport);

        Assert.NotNull(pair);

        Assert.Equal(2, receivedKeyProviders.Count);

        Assert.Same(receivedKeyProviders[0], receivedKeyProviders[1]);
    }

    /// <summary>
    /// Verifies that invalid Alpha key names are rejected.
    /// </summary>
    /// <param name="invalidKeyName">The invalid key name.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task BuildEphemeralClientPair_WithInvalidAlphaKeyName_ThrowsArgumentException(
        string? invalidKeyName)
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => MercuryFactory.Instance.BuildEphemeralClientPair(
                        invalidKeyName!, BRAVO_NODE, BuildCryptoProvider, EnvelopeCodec.Binary,
                        alphaTransport, bravoTransport));

        Assert.Equal("alphaKeyName", exception.ParamName);
    }

    /// <summary>
    /// Verifies that invalid Bravo key names are rejected.
    /// </summary>
    /// <param name="invalidKeyName">The invalid key name.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task BuildEphemeralClientPair_WithInvalidBravoKeyName_ThrowsArgumentException(string? invalidKeyName)
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                    MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, invalidKeyName!,
                        BuildCryptoProvider, EnvelopeCodec.Binary, alphaTransport, bravoTransport));

        Assert.Equal("bravoKeyName", exception.ParamName);
    }

    /// <summary>
    /// Verifies that the two key names must be different.
    /// </summary>
    [Fact]
    public async Task BuildEphemeralClientPair_WithMatchingKeyNames_ThrowsArgumentException()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        await Assert.ThrowsAsync<ArgumentException>(() => MercuryFactory.Instance.BuildEphemeralClientPair(
                    ALPHA_NODE, ALPHA_NODE, BuildCryptoProvider,
                    EnvelopeCodec.Binary, alphaTransport, bravoTransport));
    }

    /// <summary>
    /// Verifies that a null factory is rejected.
    /// </summary>
    [Fact]
    public async Task BuildEphemeralClientPair_WithNullFactory_ThrowsArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => MercuryFactoryExtensions.BuildEphemeralClientPair(null!, ALPHA_NODE,
                        BRAVO_NODE, null!, EnvelopeCodec.Binary, null!, null!));

        Assert.Equal("factory", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a null provider factory is rejected.
    /// </summary>
    [Fact]
    public async Task BuildEphemeralClientPair_WithNullProviderFactory_ThrowsArgumentNullException()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE,
                        null!, EnvelopeCodec.Binary, alphaTransport, bravoTransport));

        Assert.Equal("cryptoProviderFactory", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a null Alpha transport is rejected.
    /// </summary>
    [Fact]
    public async Task BuildEphemeralClientPair_WithNullAlphaTransport_ThrowsArgumentNullException()
    {
        var (_, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE,
                        BuildCryptoProvider, EnvelopeCodec.Binary, null!, bravoTransport));

        Assert.Equal("alphaTransport", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a null Bravo transport is rejected.
    /// </summary>
    [Fact]
    public async Task BuildEphemeralClientPair_WithNullBravoTransport_ThrowsArgumentNullException()
    {
        var (alphaTransport, _) = InMemoryDuplexTransport.CreateConnectedPair();

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE,
                        BuildCryptoProvider, EnvelopeCodec.Binary, alphaTransport, null!));

        Assert.Equal("bravoTransport", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a provider factory returning null fails safely.
    /// </summary>
    [Fact]
    public async Task BuildEphemeralClientPair_WhenProviderFactoryReturnsNull_ThrowsInvalidOperationException()
    {
        var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
                MercuryFactory.Instance.BuildEphemeralClientPair(ALPHA_NODE, BRAVO_NODE,
                    _ => null!, EnvelopeCodec.Binary, alphaTransport, bravoTransport));
    }

    /// <summary>
    /// Builds an AES-GCM provider.
    /// </summary>
    /// <param name="keyProvider">
    /// The symmetric key provider.
    /// </param>
    /// <returns>The crypto provider.</returns>
    private static ICryptoProvider BuildCryptoProvider(SymmetricKeyProviderDictionary keyProvider)
    {
        return new AesGcmCryptoProvider(keyProvider);
    }
}