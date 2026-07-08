// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="MercuryResult.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core;

/// <summary>
/// Class MercuryResult.
/// Implements the <see cref="IMercuryResult" />
/// </summary>
/// <param name="success">if set to <c>true</c> [success].</param>
/// <param name="payload">The payload.</param>
/// <param name="validatedEnvelope">The validated envelope.</param>
/// <param name="failureReason">The failure reason.</param>
/// <seealso cref="IMercuryResult" />
public class MercuryResult(bool success, ReadOnlyMemory payload, ISecureEnvelope? validatedEnvelope, FailureReason failureReason) : IMercuryResult
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="T:Mercury.Abstractions.IMercuryResult" /> is success.
    /// </summary>
    /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
    public bool Success { get; } = success;
    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    public ReadOnlyMemory Payload { get; } = payload;
    /// <summary>
    /// Gets the validated envelope.
    /// </summary>
    /// <value>The validated envelope.</value>
    public ISecureEnvelope? ValidatedEnvelope { get; } = validatedEnvelope;
    /// <summary>
    /// Gets the failure reason.
    /// </summary>
    /// <value>The failure reason.</value>
    public FailureReason FailureReason { get; } = failureReason;
}