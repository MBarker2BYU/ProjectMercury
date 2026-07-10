// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="ISecureEnvelopeFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Factories;

/// <summary>
/// Interface ISecureEnvelopeFactory
/// </summary>
public interface ISecureEnvelopeFactory
{
    /// <summary>
    /// Builds the specified payload.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>ISecureEnvelope.</returns>
    ISecureEnvelope Build(ReadOnlyMemory payload);
}