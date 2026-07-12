// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class SealRequest. This class cannot be inherited.
/// Implements the <see cref="ISealRequest" />
/// </summary>
/// <param name="cryptoContext"></param>
/// <param name="payload">The payload.</param>
/// <param name="headerMeta">The header meta.</param>
/// <param name="footerMeta">The footer meta.</param>
/// <seealso cref="ISealRequest" />
internal sealed class SealRequest(ICryptoContext cryptoContext, ReadOnlyMemory payload, Metadata headerMeta, Metadata footerMeta) : ISealRequest
{
    /// <summary>
    /// Gets the crypto context.
    /// </summary>
    /// <value>The crypto context.</value>
    public ICryptoContext CryptoContext { get; } = cryptoContext 
                                                   ?? throw new ArgumentNullException(nameof(cryptoContext));

    /// <summary>
    /// Gets the payload to protect.
    /// </summary>
    /// <value>The payload.</value>
    public ReadOnlyMemory Payload { get; } = payload;
    /// <summary>
    /// Gets the envelope header metadata.
    /// </summary>
    /// <value>The header meta.</value>
    public Metadata HeaderMeta { get; } = headerMeta 
                                          ?? new Metadata();
    /// <summary>
    /// Gets the envelope footer metadata.
    /// </summary>
    /// <value>The footer meta.</value>
    public Metadata FooterMeta { get; } = footerMeta ?? new Metadata();
}