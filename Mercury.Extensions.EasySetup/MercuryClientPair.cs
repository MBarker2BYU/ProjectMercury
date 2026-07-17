// ***********************************************************************
// Assembly       : Mercury.Extensions.EasySetup
// Author         : Matthew D. Barker
// Created        : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="MercuryClientPair.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;

namespace Mercury.Extensions.EasySetup;

/// <summary>
/// Contains two configured Mercury clients and the communication contexts
/// required for exchanges in either direction.
/// </summary>
public sealed class MercuryClientPair
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MercuryClientPair"/> class.
    /// </summary>
    /// <param name="alphaClient">The Alpha Mercury client.</param>
    /// <param name="bravoClient">The Bravo Mercury client.</param>
    /// <param name="alphaToBravoContext">
    /// The Alpha-to-Bravo crypto context.
    /// </param>
    /// <param name="bravoToAlphaContext">
    /// The Bravo-to-Alpha crypto context.
    /// </param>
    internal MercuryClientPair(IMercuryClient alphaClient, IMercuryClient bravoClient, 
        ICryptoContext alphaToBravoContext, ICryptoContext bravoToAlphaContext)
    {
        AlphaClient = alphaClient ?? throw new ArgumentNullException(nameof(alphaClient));

        BravoClient = bravoClient ?? throw new ArgumentNullException(nameof(bravoClient));

        AlphaToBravoContext = alphaToBravoContext ?? throw new ArgumentNullException(
                                  nameof(alphaToBravoContext));

        BravoToAlphaContext = bravoToAlphaContext ?? throw new ArgumentNullException(
                                  nameof(bravoToAlphaContext));
    }

    /// <summary>
    /// Gets the Alpha Mercury client.
    /// </summary>
    public IMercuryClient AlphaClient { get; }

    /// <summary>
    /// Gets the Bravo Mercury client.
    /// </summary>
    public IMercuryClient BravoClient { get; }

    /// <summary>
    /// Gets the crypto context used to send from Alpha to Bravo.
    /// </summary>
    public ICryptoContext AlphaToBravoContext { get; }

    /// <summary>
    /// Gets the crypto context used to send from Bravo to Alpha.
    /// </summary>
    public ICryptoContext BravoToAlphaContext { get; }
}