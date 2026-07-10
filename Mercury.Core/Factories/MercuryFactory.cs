// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="MercuryFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Factories;

namespace Mercury.Core.Factories;

/// <summary>
/// Class MercuryFactory. This class cannot be inherited.
/// Implements the <see cref="IMercuryFactory" />
/// </summary>
/// <seealso cref="IMercuryFactory" />
public sealed class MercuryFactory : IMercuryFactory
{

    /// <summary>
    /// The static mercury factory
    /// </summary>
    private static readonly Lazy<MercuryFactory> sm_MercuryFactory = new(() => new MercuryFactory());

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static MercuryFactory Instance => sm_MercuryFactory.Value;

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <returns>IMercuryClient.</returns>
    public IMercuryClient BuildClient()
        => new MercuryClient();
}