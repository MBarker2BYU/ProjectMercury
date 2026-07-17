// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="AesGcmCryptoProviderTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Provider.AesGcm;

namespace Mercury.Tests.CryptoProviders;

/// <summary>
/// Class AesGcmCryptoProviderTests. This class cannot be inherited.
/// Implements the <see cref="Mercury.Tests.CryptoProviders.CryptoProviderContractTests" />
/// </summary>
/// <seealso cref="Mercury.Tests.CryptoProviders.CryptoProviderContractTests" />
public sealed class AesGcmCryptoProviderTests : CryptoProviderContractTests
{
    /// <summary>
    /// Gets the expected name of the provider.
    /// </summary>
    /// <value>The expected name of the provider.</value>
    protected override string ExpectedProviderName => "aes-gcm-256";

    /// <summary>
    /// Creates the provider.
    /// </summary>
    /// <param name="keyProvider">The key provider.</param>
    /// <returns>ICryptoProvider.</returns>
    protected override ICryptoProvider CreateProvider(ISymmetricKeyProvider keyProvider)
        => new AesGcmCryptoProvider(keyProvider);
}
