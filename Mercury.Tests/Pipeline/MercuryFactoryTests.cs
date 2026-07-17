// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="MercuryFactoryTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Core.Factories;
using Mercury.Tests.Support;

namespace Mercury.Tests.Pipeline;

/// <summary>
/// Class MercuryFactoryTests. This class cannot be inherited.
/// </summary>
public sealed class MercuryFactoryTests
{
    /// <summary>
    /// Defines the test method BuildDependencies_NullProvider_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public void BuildDependencies_NullProvider_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => MercuryFactory.Instance.BuildDependencies(
                null!,
                EnvelopeCodec.Binary,
                new QueueTransport()));
    }

    /// <summary>
    /// Defines the test method BuildDependencies_NullTransport_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public void BuildDependencies_NullTransport_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => MercuryFactory.Instance.BuildDependencies(
                MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                EnvelopeCodec.Binary,
                null!));
    }

    /// <summary>
    /// Defines the test method BuildDependencies_UnsupportedCodec_ThrowsArgumentOutOfRangeException.
    /// </summary>
    [Fact]
    public void BuildDependencies_UnsupportedCodec_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => MercuryFactory.Instance.BuildDependencies(
                MercuryTestFactory.BuildProvider(ProviderKind.AesGcm),
                (EnvelopeCodec)999,
                new QueueTransport()));
    }

    /// <summary>
    /// Defines the test method BuildDependencies_ValidConfiguration_ReturnsAllDependencies.
    /// </summary>
    /// <param name="codec">The codec.</param>
    [Theory]
    [InlineData(EnvelopeCodec.Binary)]
    [InlineData(EnvelopeCodec.Json)]
    public void BuildDependencies_ValidConfiguration_ReturnsAllDependencies(
        EnvelopeCodec codec)
    {
        var provider = MercuryTestFactory.BuildProvider(
            ProviderKind.AesGcm);
        var transport = new QueueTransport();

        var dependencies = MercuryFactory.Instance.BuildDependencies(
            provider,
            codec,
            transport);

        Assert.Same(provider, dependencies.CryptoProvider);
        Assert.Same(transport, dependencies.Transport);
        Assert.NotNull(dependencies.EnvelopeCodec);
        Assert.NotNull(dependencies.ReplayProtector);
        Assert.NotNull(dependencies.Logger);
    }
}
