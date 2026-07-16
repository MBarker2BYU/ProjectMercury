// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="DemoConfiguration.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

namespace Mercury.Demo.Console.Models;

/// <summary>
/// Class DemoConfiguration. This class cannot be inherited.
/// </summary>
internal sealed class DemoConfiguration
{
    /// <summary>
    /// The default chunk size
    /// </summary>
    public const int DEFAULT_CHUNK_SIZE = 64 * 1024;

    /// <summary>
    /// Gets or sets the crypto provider.
    /// </summary>
    /// <value>The crypto provider.</value>
    public string CryptoProvider { get; set; } = DemoController.AES_GCM;

    /// <summary>
    /// Gets or sets the transport.
    /// </summary>
    /// <value>The transport.</value>
    public string Transport { get; set; } = DemoController.IN_MEMORY_TRANSPORT;

    /// <summary>
    /// Gets or sets the envelope codec.
    /// </summary>
    /// <value>The envelope codec.</value>
    public string EnvelopeCodec { get; set; } = DemoController.BINARY_CODEC;

    /// <summary>
    /// Gets or sets a value indicating whether [chunking enabled].
    /// </summary>
    /// <value><c>true</c> if [chunking enabled]; otherwise, <c>false</c>.</value>
    public bool ChunkingEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the chunk.
    /// </summary>
    /// <value>The size of the chunk.</value>
    public int ChunkSize { get; set; } = DEFAULT_CHUNK_SIZE;
}
