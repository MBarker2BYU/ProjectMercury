// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="ISealRequest.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Cryptography;

/// <summary>
/// Interface ISealRequest
/// </summary>
public interface ISealRequest
{
    /// <summary>
    /// Gets the crypto context.
    /// </summary>
    /// <value>The crypto context.</value>
    ICryptoContext CryptoContext { get; }

    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    ReadOnlyMemory Payload { get; }

    /// <summary>
    /// Gets the header meta.
    /// </summary>
    /// <value>The header meta.</value>
    Metadata? HeaderMeta { get; }

    /// <summary>
    /// Gets the footer meta.
    /// </summary>
    /// <value>The footer meta.</value>
    Metadata? FooterMeta { get; }
}