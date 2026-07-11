// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
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
/// Class EnvelopeFooter. This class cannot be inherited.
/// Implements the <see cref="IEnvelopeFooter" />
/// </summary>
/// <param name="meta">The meta.</param>
/// <seealso cref="IEnvelopeFooter" />
internal sealed class EnvelopeFooter(Metadata? meta = null) : IEnvelopeFooter
{
    /// <summary>
    /// Gets the empty.
    /// </summary>
    /// <value>The empty.</value>
    public static IEnvelopeFooter Empty => new EnvelopeFooter();

    /// <summary>
    /// Gets the meta.
    /// </summary>
    /// <value>The meta.</value>
    public Metadata Meta { get; } = 
        meta ?? new Metadata();
}