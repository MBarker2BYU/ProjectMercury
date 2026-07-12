// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="OpenRequest.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Envelope;

namespace Mercury.Core.Cryptography;

/// <summary>
/// Class OpenRequest. This class cannot be inherited.
/// Implements the <see cref="IOpenRequest" />
/// </summary>
/// <param name="secureEnvelope">The secure envelope.</param>
/// <seealso cref="IOpenRequest" />
internal sealed class OpenRequest(ISecureEnvelope secureEnvelope) : IOpenRequest
{
    /// <summary>
    /// Gets the secure envelope.
    /// </summary>
    /// <value>The secure envelope.</value>
    public ISecureEnvelope SecureEnvelope { get; } = secureEnvelope;
}