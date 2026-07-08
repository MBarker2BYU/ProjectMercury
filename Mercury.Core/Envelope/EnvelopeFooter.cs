// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-04-2026
// ***********************************************************************
// <copyright file="EnvelopeFooter.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Envelope;

/// <summary>
/// Class EnvelopeFooter.
/// Implements the <see cref="IEnvelopeFooter" />
/// </summary>
/// <param name="Meta">The meta.</param>
/// <seealso cref="IEnvelopeFooter" />
public class EnvelopeFooter(Metadata? Meta) : IEnvelopeFooter
{
    /// <summary>
    /// Gets the meta.
    /// </summary>
    /// <value>The meta.</value>
    public Metadata? Meta { get; } = Meta ?? new Metadata();
}