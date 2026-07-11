// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="SecureEnvelope.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Envelope;

/// <summary>
/// Class SecureEnvelope. This class cannot be inherited.
/// Implements the <see cref="ISecureEnvelope" />
/// </summary>
/// <param name="payload">The payload.</param>
/// <seealso cref="ISecureEnvelope" />
internal sealed class SecureEnvelope(IEnvelopeHeader header, ReadOnlyMemory payload, IEnvelopeFooter footer) : ISecureEnvelope
{
    public IEnvelopeHeader Header { get; } = header;

    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    public ReadOnlyMemory Payload { get; } = !payload.IsEmpty 
        ? payload 
        : throw new ArgumentException("The secure envelope cannot be created with an empty payload.", nameof(payload));

    public IEnvelopeFooter Footer { get; } = footer;
}