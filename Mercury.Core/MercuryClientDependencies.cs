// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="MercuryClientDependencies.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Transport;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClientDependencies. This class cannot be inherited.
/// Implements the <see cref="IMercuryClientDependencies" />
/// </summary>
/// <param name="cryptoProvider">The crypto provider.</param>
/// <param name="transport">The transport.</param>
/// <seealso cref="IMercuryClientDependencies" />
public sealed class MercuryClientDependencies(ICryptoProvider cryptoProvider, ITransport transport) : IMercuryClientDependencies
{
    /// <summary>
    /// Gets the crypto provider.
    /// </summary>
    /// <value>The crypto.</value>
    public ICryptoProvider CryptoProvider { get; } = cryptoProvider
                                             ?? throw new ArgumentNullException(nameof(cryptoProvider));

    /// <summary>
    /// Gets the transport.
    /// </summary>
    /// <value>The transport.</value>
    public ITransport Transport { get; } = transport
                                           ?? throw new ArgumentNullException(nameof(transport));
}