// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="TelemetryEntry.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.Console.Models;

/// <summary>
/// Class TelemetryEntry. This class cannot be inherited.
/// </summary>
/// <param name="Timestamp">The timestamp.</param>
/// <param name="Operation">The operation.</param>
/// <param name="Provider">The provider.</param>
/// <param name="Transport">The transport.</param>
/// <param name="Codec">The codec.</param>
/// <param name="ChunkingEnabled">if set to <c>true</c> [chunking enabled].</param>
/// <param name="PayloadSize">Size of the payload.</param>
/// <param name="Success">if set to <c>true</c> [success].</param>
/// <param name="Details">The details.</param>
internal sealed record TelemetryEntry(DateTimeOffset Timestamp, string Operation, string Provider,
    string Transport, string Codec, bool ChunkingEnabled, int PayloadSize, bool Success, string Details);
