// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
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
    /// Encodes the specified secure envelope.
    /// </summary>
    /// <param name="secureEnvelope">The secure envelope.</param>
    /// <returns>System.Byte[].</returns>
    byte[] Encode(ISecureEnvelope secureEnvelope);

    /// <summary>
    /// Decodes the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>ISecureEnvelope.</returns>
    ISecureEnvelope Decode(ReadOnlyMemory data);
}