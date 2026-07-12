// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="IOpenRequest.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;

namespace Mercury.Abstractions.Cryptograph;

/// <summary>
/// Interface IOpenRequest
/// </summary>
public interface IOpenRequest
{
    /// <summary>
    /// Gets the secure envelope.
    /// </summary>
    /// <value>The secure envelope.</value>
    ISecureEnvelope SecureEnvelope { get; }
}