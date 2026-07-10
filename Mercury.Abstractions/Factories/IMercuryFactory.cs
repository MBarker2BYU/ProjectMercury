// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="IMercuryFactory.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Transport;

namespace Mercury.Abstractions.Factories;
/// <summary>
/// Interface IMercuryFactory
/// </summary>
public interface IMercuryFactory
{
    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <returns>IMercuryClient.</returns>
    IMercuryClient BuildClient();

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <param name="transport">The transport.</param>
    /// <returns>IMercuryClient.</returns>
    IMercuryClient BuildClient(ITransport transport);
}