// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
// ***********************************************************************
// <copyright file="IUnsealRequest.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Envelope;

namespace Mercury.Abstractions.Cryptography;

/// <summary>
/// Interface IUnsealRequest
/// </summary>
public interface IUnsealRequest
{
    /// <summary>
    /// Gets the secure envelope.
    /// </summary>
    /// <value>The secure envelope.</value>
    ISecureEnvelope SecureEnvelope { get; }
}