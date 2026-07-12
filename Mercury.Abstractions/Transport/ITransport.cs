// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="ITransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Transport;

/// <summary>
/// Interface ITransport
/// </summary>
public interface ITransport
{
    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default);

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default);
}