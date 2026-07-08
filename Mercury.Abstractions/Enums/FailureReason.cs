// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Abstractions.Enums;

/// <summary>
/// Standardized failure reasons for receive operations.
/// </summary>
public enum FailureReason
{
    /// <summary>
    /// The none
    /// </summary>
    None = 0,
    /// <summary>
    /// The invalid frame
    /// </summary>
    InvalidFrame,
    /// <summary>
    /// The decode failed
    /// </summary>
    DecodeFailed,
    /// <summary>
    /// The replay detected
    /// </summary>
    ReplayDetected,
    /// <summary>
    /// The tamper detected
    /// </summary>
    TamperDetected,
    /// <summary>
    /// The authentication failed
    /// </summary>
    AuthenticationFailed,
    /// <summary>
    /// The wrong key
    /// </summary>
    WrongKey,
    /// <summary>
    /// The payload too large
    /// </summary>
    PayloadTooLarge,
    /// <summary>
    /// The internal error
    /// </summary>
    InternalError,
    /// <summary>
    /// The custom
    /// </summary>
    Custom
}