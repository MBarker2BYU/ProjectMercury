// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
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
using Mercury.Abstractions.Logging;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Replay;
using Mercury.Abstractions.Services;
using Mercury.Abstractions.Transport;
using Mercury.Core.Cryptography;

namespace Mercury.Core;

/// <summary>
/// Class MercuryClient. This class cannot be inherited.
/// Implements the <see cref="IMercuryClient" />
/// </summary>
/// <seealso cref="IMercuryClient" />
internal sealed class MercuryClient(IMercuryClientDependencies dependencies, IEnvelopeService envelopeService) : IMercuryClient
{

    /// <summary>
    /// The crypto provider
    /// </summary>
    private readonly ICryptoProvider m_CryptoProvider = dependencies.CryptoProvider
                                                        ?? throw new ArgumentNullException(nameof(dependencies.CryptoProvider));

    /// <summary>
    /// The transport
    /// </summary>
    private readonly ITransport m_Transport = dependencies.Transport
                                              ?? throw new ArgumentNullException(nameof(dependencies.Transport));

    /// <summary>
    /// The envelope service
    /// </summary>
    private readonly IEnvelopeService m_EnvelopeService = envelopeService
                                                          ?? throw new ArgumentNullException(nameof(envelopeService));

    /// <summary>
    /// The envelope codec
    /// </summary>
    private readonly IEnvelopeCodec m_EnvelopeCodec = dependencies.EnvelopeCodec;

    /// <summary>
    /// The replay protector
    /// </summary>
    private readonly IReplayProtector m_ReplayProtector = dependencies.ReplayProtector
                                                          ?? throw new ArgumentNullException(nameof(dependencies.ReplayProtector));

    /// <summary>
    /// The client logger
    /// </summary>
    private readonly IMercuryLogger m_Logger = dependencies.Logger 
                                               ?? NoOpMercuryLogger.Instance;

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="headerMeta">The header meta.</param>
    /// <param name="footerMeta">The footer meta.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    internal Task SendAsync(ICryptoContext context, ReadOnlyMemory payload, Metadata headerMeta,
        Metadata footerMeta, CancellationToken cancellationToken = default)
            => SendInternalAsync(context, payload, headerMeta,
                footerMeta, cancellationToken);

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="cryptoContext"></param>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SendAsync(ICryptoContext cryptoContext, ReadOnlyMemory payload,
        CancellationToken cancellationToken = default)
        => SendInternalAsync(cryptoContext, payload, Metadata.Empty, Metadata.Empty, cancellationToken);

    /// <summary>
    /// Send internal as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="headerMeta">The header meta.</param>
    /// <param name="footerMeta">The footer meta.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Payload must not be empty. - payload</exception>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task SendInternalAsync(ICryptoContext context, ReadOnlyMemory payload,
        Metadata headerMeta, Metadata footerMeta, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (payload.IsEmpty)
        {
            throw new ArgumentException(
                "Payload must not be empty.",
                nameof(payload));
        }

        ISealRequest sealRequest =
            new SealRequest(
                context,
                payload,
                headerMeta,
                footerMeta);

        var providerResult =
            await m_CryptoProvider
                .SealAsync(
                    sealRequest,
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

            try
            {
                secureEnvelope =
                    m_EnvelopeCodec.Decode(frame);
            }
            catch (Exception ex)
            {
                return new MercuryResult(
                    false,
                    ReadOnlyMemory.Empty,
                    null,
                    FailureReason.DecodeFailed,
                    ex.Message);
            }
            
            IOpenRequest openRequest = new OpenRequest(secureEnvelope);

            var cryptoProviderResult =
                await m_CryptoProvider
                    .OpenAsync(openRequest, m_EnvelopeService, cancellationToken)
                    .ConfigureAwait(false);

            if (!cryptoProviderResult.Success || cryptoProviderResult.ValidatedEnvelope == null)
            {
                return new MercuryResult(cryptoProviderResult);
            }

            // Replay Protection
            var replayAccepted =
                await m_ReplayProtector
                    .TryAcceptAsync(cryptoProviderResult.ValidatedEnvelope.Header, cancellationToken)
                    .ConfigureAwait(false);

            if (!replayAccepted)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty,
                    cryptoProviderResult.ValidatedEnvelope, FailureReason.ReplayDetected,
                    "Replay detected.");
            }

            return new MercuryResult(cryptoProviderResult);
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
