// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="IEnvelopeCodec.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Envelope;

/// <summary>
/// Interface IEnvelopeCodec
/// </summary>
public interface IEnvelopeCodec
{

    /// <summary>
    /// Encodes the specified envelope.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>ReadOnlyMemory.</returns>
    ReadOnlyMemory Encode(ISecureEnvelope envelope);

    /// <summary>
    /// Decodes the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>ISecureEnvelope.</returns>
    ISecureEnvelope Decode(ReadOnlyMemory data);
}