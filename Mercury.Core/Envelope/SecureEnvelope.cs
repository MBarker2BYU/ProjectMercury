// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
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
/// <seealso cref="ISecureEnvelope" />
public sealed  class SecureEnvelope(FrameworkVersion frameworkVersion, IEnvelopeHeader header, ReadOnlyMemory payload, IEnvelopeFooter footer) : ISecureEnvelope
{
    /// <summary>
    /// Gets the framework version.
    /// </summary>
    /// <value>The framework version.</value>
    public FrameworkVersion FrameworkVersion { get; } = frameworkVersion;
    /// <summary>
    /// Gets the header.
    /// </summary>
    /// <value>The header.</value>
    public IEnvelopeHeader Header { get; } = header;

    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    public ReadOnlyMemory Payload { get; } = payload;

    /// <summary>
    /// Gets the footer.
    /// </summary>
    /// <value>The footer.</value>
    public IEnvelopeFooter Footer { get; } = footer;
}