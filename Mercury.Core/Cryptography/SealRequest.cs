// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="SealRequest.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class SealRequest. This class cannot be inherited.
/// Implements the <see cref="ISealRequest" />
/// </summary>
/// <param name="cryptoContext">The crypto context.</param>
/// <param name="payload">The payload.</param>
/// <param name="headerMeta">The header meta.</param>
/// <param name="footerMeta">The footer meta.</param>
/// <seealso cref="ISealRequest" />
public sealed class SealRequest(ICryptoContext cryptoContext, ReadOnlyMemory payload, Metadata? headerMeta = null, Metadata? footerMeta = null) : ISealRequest
{
    /// <summary>
    /// Gets the crypto context.
    /// </summary>
    /// <value>The crypto context.</value>
    public ICryptoContext CryptoContext { get; } = cryptoContext;
    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    public ReadOnlyMemory Payload { get; } = payload;
    /// <summary>
    /// Gets the header meta.
    /// </summary>
    /// <value>The header meta.</value>
    public Metadata? HeaderMeta { get; } = headerMeta;
    /// <summary>
    /// Gets the footer meta.
    /// </summary>
    /// <value>The footer meta.</value>
    public Metadata? FooterMeta { get; } = footerMeta;
}