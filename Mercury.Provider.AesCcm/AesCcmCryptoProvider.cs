// ***********************************************************************
// Assembly     : Mercury.Providers.AesGcm
// Author         : {Your Name Here} 
// Created        : 07-14-2026
//
// Last Modified By : {Your Name Here}
// Last Modified On : 07-14-2026
// ***********************************************************************
// <copyright file="AesCcmCryptoProvider.cs">
//     Copyright (c) {Your Name Here}. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Services;

namespace Mercury.Provider.AesCcm
{
    /// <summary>
    /// Class AesCcmCryptoProvider.
    /// Implements the <see cref="ICryptoProvider" />
    /// </summary>
    /// <seealso cref="ICryptoProvider" />
    public class AesCcmCryptoProvider : ICryptoProvider
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Seals the asynchronous.
        /// </summary>
        /// <param name="sealRequest">The protect request.</param>
        /// <param name="envelopeService">The envelope service.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ICryptoProviderResult> SealAsync(ISealRequest sealRequest, IEnvelopeService envelopeService,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="openRequest">The unprotect request.</param>
        /// <param name="envelopeService">The envelope service.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ICryptoProviderResult> OpenAsync(IOpenRequest openRequest, IEnvelopeService envelopeService,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
