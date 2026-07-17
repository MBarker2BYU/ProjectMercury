// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="ChaCha20CryptoProviderTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Provider.ChaCha20;

namespace Mercury.Tests.CryptoProviders;

/// <summary>
/// Class ChaCha20CryptoProviderTests. This class cannot be inherited.
/// Implements the <see cref="Mercury.Tests.CryptoProviders.CryptoProviderContractTests" />
/// </summary>
/// <seealso cref="Mercury.Tests.CryptoProviders.CryptoProviderContractTests" />
public sealed class ChaCha20CryptoProviderTests : CryptoProviderContractTests
{
    /// <summary>
    /// Gets the expected name of the provider.
    /// </summary>
    /// <value>The expected name of the provider.</value>
    protected override string ExpectedProviderName => "chacha20-poly1305";

    /// <summary>
    /// Creates the provider.
    /// </summary>
    /// <param name="keyProvider">The key provider.</param>
    /// <returns>ICryptoProvider.</returns>
    protected override ICryptoProvider CreateProvider(ISymmetricKeyProvider keyProvider)
        => new ChaCha20CryptoProvider(keyProvider);
}
