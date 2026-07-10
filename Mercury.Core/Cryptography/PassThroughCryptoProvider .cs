// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="PassThroughCryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Cryptography;

internal sealed class PassThroughCryptoProvider(string name) : ICryptoProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; } = name;

    public Task<ReadOnlyMemory> ProtectAsync(
        ReadOnlyMemory payload,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(payload.Clone());
    }

    public Task<ReadOnlyMemory> UnprotectAsync(
        ReadOnlyMemory payload,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(payload.Clone());
    }
}