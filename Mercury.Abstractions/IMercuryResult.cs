// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="IMercuryResult.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions;

/// <summary>
/// Interface IMercuryResult
/// </summary>
public interface IMercuryResult
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IMercuryResult"/> is success.
    /// </summary>
    /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
    bool Success { get; }
    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    ReadOnlyMemory Payload { get; }
    /// <summary>
    /// Gets the validated envelope.
    /// </summary>
    /// <value>The validated envelope.</value>
    ISecureEnvelope? ValidatedEnvelope { get; }
    /// <summary>
    /// Gets the failure reason.
    /// </summary>
    /// <value>The failure reason.</value>
    FailureReason FailureReason { get; }
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    string? ResultMessage { get; }
}