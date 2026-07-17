// ***********************************************************************
// Assembly       : Mercury.Tests
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="EphemeralKeyProviderFactoryTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Extensions.EasySetup;

namespace Mercury.Tests.Extensions.EasySetup;

/// <summary>
/// Tests the <see cref="EphemeralKeyProviderFactory"/> class.
/// </summary>
public sealed class EphemeralKeyProviderFactoryTests
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
    /// Verifies that the factory creates both requested keys.
    /// </summary>
    [Fact]
    public async Task Create_WithTwoKeyNames_CreatesBothKeys()
    {
        var provider =
            EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var alphaKey = await provider.GetKeyAsync(new KeyId(ALPHA_NODE));

        var bravoKey = await provider.GetKeyAsync(new KeyId(BRAVO_NODE));

        Assert.False(alphaKey.IsEmpty);
        Assert.False(bravoKey.IsEmpty);
    }

    /// <summary>
    /// Verifies that generated keys are 256 bits.
    /// </summary>
    [Fact]
    public async Task Create_GeneratesThirtyTwoByteKeys()
    {
        var provider =
            EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var alphaKey = await provider.GetKeyAsync(new KeyId(ALPHA_NODE));

        var bravoKey = await provider.GetKeyAsync(new KeyId(BRAVO_NODE));

        Assert.Equal(32, alphaKey.Length);

        Assert.Equal(32, bravoKey.Length);
    }

    /// <summary>
    /// Verifies that different key names receive different key material.
    /// </summary>
    [Fact]
    public async Task Create_GeneratesDifferentKeysForEachName()
    {
        var provider = EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var alphaKey = await provider.GetKeyAsync(new KeyId(ALPHA_NODE));

        var bravoKey = await provider.GetKeyAsync(new KeyId(BRAVO_NODE));

        Assert.False(alphaKey.ToArray().SequenceEqual(bravoKey.ToArray()));
    }

    /// <summary>
    /// Verifies that separate factory calls generate fresh key material.
    /// </summary>
    [Fact]
    public async Task Create_CalledTwice_GeneratesFreshKeyMaterial()
    {
        var firstProvider =
            EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var secondProvider = EphemeralKeyProviderFactory.Create(ALPHA_NODE, BRAVO_NODE);

        var firstAlphaKey = await firstProvider.GetKeyAsync(new KeyId(ALPHA_NODE));

        var secondAlphaKey = await secondProvider.GetKeyAsync(new KeyId(ALPHA_NODE));

        var firstBravoKey = await firstProvider.GetKeyAsync(new KeyId(BRAVO_NODE));

        var secondBravoKey = await secondProvider.GetKeyAsync(new KeyId(BRAVO_NODE));

        Assert.False(firstAlphaKey.ToArray().SequenceEqual(secondAlphaKey.ToArray()));

        Assert.False(firstBravoKey.ToArray().SequenceEqual(secondBravoKey.ToArray()));
    }

    /// <summary>
    /// Verifies that a null key-name collection is rejected.
    /// </summary>
    [Fact]
    public void Create_WithNullKeyNames_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
                    EphemeralKeyProviderFactory.Create((string[])null!));

        Assert.Equal("keyNames", exception.ParamName);
    }

    /// <summary>
    /// Verifies that at least one key name is required.
    /// </summary>
    [Fact]
    public void Create_WithNoKeyNames_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
                    EphemeralKeyProviderFactory.Create());

        Assert.Equal("keyNames", exception.ParamName);
    }

    /// <summary>
    /// Verifies that invalid individual key names are rejected.
    /// </summary>
    /// <param name="invalidKeyName">The invalid key name.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidKeyName_ThrowsArgumentException(
        string? invalidKeyName)
    {
        var exception =
            Assert.Throws<ArgumentException>(() =>
                    EphemeralKeyProviderFactory.Create(ALPHA_NODE, invalidKeyName!));

        Assert.Equal("keyNames", exception.ParamName);
    }

    /// <summary>
    /// Verifies that duplicate key names are rejected.
    /// </summary>
    [Fact]
    public void Create_WithDuplicateKeyNames_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
                    EphemeralKeyProviderFactory.Create(ALPHA_NODE, ALPHA_NODE));

        Assert.Equal("keyNames", exception.ParamName);
    }
}