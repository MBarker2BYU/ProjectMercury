// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="ConfigurationMenuSelection.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.Console.Enums;

/// <summary>
/// Enum ConfigurationMenuSelection
/// </summary>
internal enum ConfigurationMenuSelection
{
    /// <summary>
    /// The crypto provider
    /// </summary>
    CryptoProvider,
    /// <summary>
    /// The transport
    /// </summary>
    Transport,
    /// <summary>
    /// The envelope codec
    /// </summary>
    EnvelopeCodec,
    /// <summary>
    /// The chunking
    /// </summary>
    Chunking,
    /// <summary>
    /// The chunk size
    /// </summary>
    ChunkSize,
    /// <summary>
    /// The return
    /// </summary>
    Return
}
