// ***********************************************************************
// Assembly     : Mercury.Providers.AesGcm
// Author         : {Your Name Here}
// Created        : 07-14-2026
//
// Last Modified By : {Your Name Here}
// Last Modified On : 07-14-2026
// ***********************************************************************
// <copyright file="FileDropTransport.cs">
//     Copyright (c) {Your Name Here}. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Transport.FileDrop
{
    /// <summary>
    /// Class FileDropTransport.
    /// Implements the <see cref="ITransport" />
    /// </summary>
    /// <seealso cref="ITransport" />
    public class FileDropTransport : ITransport
    {

        /// <summary>
        /// Gets a value indicating whether the transport is connected.
        /// </summary>
        /// <value><c>true</c> if the transport is connected; otherwise, <c>false</c>.</value>
        public bool IsConnected { get; }

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
