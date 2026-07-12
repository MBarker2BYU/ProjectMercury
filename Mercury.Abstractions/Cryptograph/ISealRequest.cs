// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="ISealRequest.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Cryptograph;

/// <summary>
/// Defines the data supplied to a crypto provider for one protection operation.
/// </summary>
public interface ISealRequest
{
    /// <summary>
    /// Gets the payload to protect.
    /// </summary>
    ReadOnlyMemory Payload { get; }

    /// <summary>
    /// Gets the envelope header metadata.
    /// </summary>
    Metadata HeaderMeta { get; }

    /// <summary>
    /// Gets the envelope footer metadata.
    /// </summary>
    Metadata FooterMeta { get; }
}