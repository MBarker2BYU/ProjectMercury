// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="MercuryClient.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using Mercury.Abstractions.Transport;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClient. This class cannot be inherited.
/// Implements the <see cref="IMercuryClient" />
/// </summary>
/// <seealso cref="IMercuryClient" />
internal sealed class MercuryClient(IMercuryClientDependencies dependencies, IEnvelopeService envelopeService) : IMercuryClient
{

    private readonly ICryptoProvider m_CryptoProvider = dependencies.CryptoProvider
                                                        ?? throw new ArgumentNullException(nameof(dependencies.CryptoProvider));

    private readonly ITransport m_Transport = dependencies.Transport
                                              ?? throw new ArgumentNullException(nameof(dependencies.Transport));

    private readonly IEnvelopeService m_EnvelopeService = envelopeService
                                                          ?? throw new ArgumentNullException(nameof(envelopeService));

    private readonly IEnvelopeCodec m_EnvelopeCodec = dependencies.EnvelopeCodec;



    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task SendAsync(ReadOnlyMemory payload, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (payload.IsEmpty)
        {
            throw new ArgumentException(
                "Payload must not be empty.",
                nameof(payload));
        }

        var providerResult =
            await m_CryptoProvider
                .ProtectAsync(
                    payload,
                    m_EnvelopeService,
                    cancellationToken)
                .ConfigureAwait(false);

        if (!providerResult.Success ||
            providerResult.ValidatedEnvelope == null)
        {
            throw new InvalidOperationException(
                providerResult.Message ??
                "The crypto provider failed to create a secure envelope.");
        }

        var frame =
            m_EnvelopeCodec.Encode(
                providerResult.ValidatedEnvelope);

        if (frame.IsEmpty)
        {
            throw new InvalidOperationException(
                "The envelope codec returned an empty frame.");
        }

        await m_Transport
            .SendAsync(
                frame,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IMercuryResult&gt; representing the asynchronous operation.</returns>
    public async Task<IMercuryResult> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        ISecureEnvelope? secureEnvelope = null;

        try
        {
            var frame =
                await m_Transport
                    .ReceiveAsync(cancellationToken)
                    .ConfigureAwait(false);

            if (frame.IsEmpty)
            {
                return new MercuryResult(
                    false,
                    ReadOnlyMemory.Empty,
                    null,
                    FailureReason.InternalError,
                    "The transport returned an empty frame.");
            }

            secureEnvelope =
                m_EnvelopeCodec.Decode(frame);

            var cryptoProviderResult =
                await m_CryptoProvider
                    .UnprotectAsync(secureEnvelope, m_EnvelopeService, cancellationToken)
                    .ConfigureAwait(false);

            return new MercuryResult(
                cryptoProviderResult);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return new MercuryResult(
                false,
                ReadOnlyMemory.Empty,
                secureEnvelope,
                FailureReason.InternalError,
                exception.Message);
        }
    }
}
