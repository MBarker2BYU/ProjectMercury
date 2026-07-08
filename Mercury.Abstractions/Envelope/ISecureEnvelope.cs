// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="ISecureEnvelope.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Envelope;

/// <summary>
/// Interface ISecureEnvelope
/// </summary>
public interface ISecureEnvelope
{
    /// <summary>
    /// Gets the framework version.
    /// </summary>
    /// <value>The framework version.</value>
    FrameworkVersion FrameworkVersion { get; }
    /// <summary>
    /// Gets the header.
    /// </summary>
    /// <value>The header.</value>
    IEnvelopeHeader Header { get; }
    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    ReadOnlyMemory Payload { get; }
    /// <summary>
    /// Gets the footer.
    /// </summary>
    /// <value>The footer.</value>
    IEnvelopeFooter Footer { get; }
}