// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Kim K. Brown
// Created          : 07-18-2026
//
// Last Modified By : Kim K. Brown
// Last Modified On : 07-18-2026
// ***********************************************************************
// <copyright file="AesCcmCryptoProviderTests.cs">
//     Copyright (c) Kim K. Brown. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Core.Services;
using Mercury.Provider.AesCcm;
using Mercury.Tests.Support;

using MercuryMemory =
    Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.CryptoProviders;

/// <summary>
/// Tests the AES-CCM crypto provider using the shared provider contract
/// and AES-CCM-specific payload-format tests.
/// </summary>
public sealed class AesCcmCryptoProviderTests
    : CryptoProviderContractTests
{
    /// <summary>
    /// Gets the expected AES-CCM algorithm identifier.
    /// </summary>
    protected override string ExpectedProviderName => "aes-ccm-256";

    /// <summary>
    /// Creates an AES-CCM provider for each contract test.
    /// </summary>
    /// <param name="keyProvider">
    /// The symmetric-key provider used by AES-CCM.
    /// </param>
    /// <returns>The configured AES-CCM provider.</returns>
    protected override ICryptoProvider CreateProvider(
        ISymmetricKeyProvider keyProvider)
        => new AesCcmCryptoProvider(keyProvider);

    /// <summary>
    /// Verifies round trips around common block boundaries and confirms
    /// the expected protected-payload layout.
    /// </summary>
    /// <param name="payloadLength">The plaintext payload length.</param>
    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(1024)]
    [InlineData(4096)]
    public async Task
        SealAndOpenAsync_VariousPayloadLengths_RoundTripsWithExpectedLayout(
            int payloadLength)
    {
        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        var payload =
            MercuryTestFactory.CreatePayload(payloadLength);

        var sealRequest = new TestSealRequest(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(payload));

        var sealedResult = await provider.SealAsync(
            sealRequest,
            EnvelopeService.Instance);

        Assert.True(sealedResult.Success, sealedResult.Message);
        Assert.NotNull(sealedResult.ValidatedEnvelope);

        var protectedPayload = sealedResult.Payload.ToArray();

        // Payload format:
        // 1 byte nonce-length prefix
        // 12 byte nonce
        // 16 byte authentication tag
        // N byte ciphertext
        Assert.Equal(12, protectedPayload[0]);
        Assert.Equal(
            payloadLength + 1 + 12 + 16,
            protectedPayload.Length);

        var openRequest =
            new TestOpenRequest(sealedResult.ValidatedEnvelope);

        var openedResult = await provider.OpenAsync(
            openRequest,
            EnvelopeService.Instance);

        Assert.True(openedResult.Success, openedResult.Message);
        Assert.Equal(payload, openedResult.Payload.ToArray());
    }

    /// <summary>
    /// Verifies that sealing identical plaintext twice generates distinct
    /// AES-CCM nonces.
    /// </summary>
    [Fact]
    public async Task
        SealAsync_SamePayloadTwice_GeneratesDistinctNonces()
    {
        const int nonceLength = 12;

        var provider = CreateProvider(
            MercuryTestFactory.BuildKeyProvider());

        var request = new TestSealRequest(
            MercuryTestFactory.BuildContext(),
            new MercuryMemory(MercuryTestFactory.CreatePayload()));

        var first = await provider.SealAsync(
            request,
            EnvelopeService.Instance);

        var second = await provider.SealAsync(
            request,
            EnvelopeService.Instance);

        Assert.True(first.Success, first.Message);
        Assert.True(second.Success, second.Message);

        var firstProtectedPayload = first.Payload.ToArray();
        var secondProtectedPayload = second.Payload.ToArray();

        Assert.Equal(nonceLength, firstProtectedPayload[0]);
        Assert.Equal(nonceLength, secondProtectedPayload[0]);

        var firstNonce = firstProtectedPayload
            .Skip(1)
            .Take(nonceLength)
            .ToArray();

        var secondNonce = secondProtectedPayload
            .Skip(1)
            .Take(nonceLength)
            .ToArray();

        Assert.False(firstNonce.SequenceEqual(secondNonce));
    }
}