// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="SecureEnvelopeFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Factories;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Envelope;

namespace Mercury.Core.Factories;

/// <summary>
/// Class SecureEnvelopeFactory. This class cannot be inherited.
/// Implements the <see cref="ISecureEnvelopeFactory" />
/// </summary>
/// <seealso cref="ISecureEnvelopeFactory" />
internal sealed class SecureEnvelopeFactory : ISecureEnvelopeFactory
{
    /// <summary>
    /// Creates the specified payload.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>ISecureEnvelope.</returns>
    public ISecureEnvelope Build(ReadOnlyMemory payload)
    {
        return new SecureEnvelope(payload);
    }
}