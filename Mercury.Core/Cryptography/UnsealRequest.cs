// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-08-2026
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
using Mercury.Abstractions.Envelope;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class UnsealRequest.
/// Implements the <see cref="IUnsealRequest" />
/// </summary>
/// <seealso cref="IUnsealRequest" />
public class UnsealRequest(ISecureEnvelope secureEnvelope) : IUnsealRequest
{
    /// <summary>
    /// Gets the secure envelope.
    /// </summary>
    /// <value>The secure envelope.</value>
    public ISecureEnvelope SecureEnvelope { get; } = secureEnvelope;
}