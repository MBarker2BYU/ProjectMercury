// ***********************************************************************
// Assembly       : Mercury.Core
// Author         : Matthew D. Barker
// Created        : 07-02-2026
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
/// Coordinates protected communication between a crypto provider,
/// envelope codec, transport, and replay protector.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="MercuryClient"/> is the primary runtime implementation of
/// <see cref="IMercuryClient"/>.
/// </para>
/// <para>
/// Sending follows this sequence:
/// </para>
/// <list type="number">
///     <item>
///         <description>
///         Pass the payload to the configured crypto provider.
///         </description>
///     </item>
///     <item>
///         <description>
///         Receive a validated <see cref="ISecureEnvelope"/> containing
///         the protected payload.
///         </description>
///     </item>
///     <item>
///         <description>
///         Encode the secure envelope into a transportable frame.
///         </description>
///     </item>
///     <item>
///         <description>
///         Send the encoded frame through the configured transport.
///         </description>
///     </item>
/// </list>
/// <para>
/// Receiving follows the reverse sequence:
/// </para>
/// <list type="number">
///     <item>
///         <description>
///         Receive an encoded frame from the transport.
///         </description>
///     </item>
///     <item>
///         <description>
///         Decode the frame into a secure envelope.
///         </description>
///     </item>
///     <item>
///         <description>
///         Authenticate and open the secure envelope through the
///         configured crypto provider.
///         </description>
///     </item>
///     <item>
///         <description>
///         Verify that the authenticated replay token has not already
///         been accepted.
///         </description>
///     </item>
///     <item>
///         <description>
///         Return the recovered payload through an
///         <see cref="IMercuryResult"/>.
///         </description>
///     </item>
/// </list>
/// <para>
/// The transport never receives an unprotected payload. Only encoded
/// secure-envelope frames are passed to the transport boundary.
/// </para>
/// </remarks>
/// <seealso cref="IMercuryClient"/>
internal sealed class MercuryClient(IMercuryClientDependencies dependencies, IEnvelopeService envelopeService) : IMercuryClient
{
    public readonly KeyId m_ClientId = dependencies.ClientId.IsEmpty 
            ? throw new ArgumentNullException(nameof(dependencies.ClientId))
            : dependencies.ClientId;

    /// <summary>
    /// Protects outgoing payloads and authenticates incoming secure envelopes.
    /// </summary>
    private readonly ICryptoProvider m_CryptoProvider =
        dependencies.CryptoProvider ?? throw new ArgumentNullException(nameof(dependencies.CryptoProvider));

    /// <summary>
    /// Sends and receives encoded secure-envelope frames.
    /// </summary>
    private readonly ITransport m_Transport =
        dependencies.Transport ?? throw new ArgumentNullException(nameof(dependencies.Transport));

    /// <summary>
    /// Builds and validates Mercury envelope components.
    /// </summary>
    private readonly IEnvelopeService m_EnvelopeService =
        envelopeService ?? throw new ArgumentNullException(nameof(envelopeService));

    /// <summary>
    /// Converts secure envelopes to and from transportable frames.
    /// </summary>
    private readonly IEnvelopeCodec m_EnvelopeCodec =
        dependencies.EnvelopeCodec;

    /// <summary>
    /// Detects secure envelopes that reuse a previously accepted replay token.
    /// </summary>
    private readonly IReplayProtector m_ReplayProtector =
        dependencies.ReplayProtector ?? throw new ArgumentNullException(nameof(dependencies.ReplayProtector));

    /// <summary>
    /// Records Mercury activity when logging is enabled.
    /// </summary>
    /// <remarks>
    /// A no-operation logger is used when no active logger is supplied.
    /// This allows Mercury to call the logger without repeatedly checking
    /// for a null value.
    /// </remarks>
    private readonly IMercuryLogger m_Logger =
        dependencies.Logger ?? NoOpMercuryLogger.Instance;

    /// <summary>
    /// Sends a payload with header and footer metadata.
    /// </summary>
    /// <remarks>
    /// This overload is used internally by features such as chunking.
    /// It allows Mercury Core to attach framework metadata without adding
    /// metadata parameters to the public <see cref="IMercuryClient"/> API.
    /// </remarks>
    /// <param name="context">
    /// Identifies the sender, recipient, encryption algorithm, and other
    /// cryptographic information required by the provider.
    /// </param>
    /// <param name="payload">
    /// The payload to protect and send.
    /// </param>
    /// <param name="headerMeta">
    /// Metadata to place in the secure-envelope header.
    /// </param>
    /// <param name="footerMeta">
    /// Metadata to place in the secure-envelope footer.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the send operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous send operation.
    /// </returns>
    internal Task SendAsync(ICryptoContext context, ReadOnlyMemory payload, Metadata headerMeta,
        Metadata footerMeta, CancellationToken cancellationToken = default)
        => SendInternalAsync(context, payload, headerMeta, footerMeta, cancellationToken);

    /// <summary>
    /// Protects and sends a payload through the configured Mercury pipeline.
    /// </summary>
    /// <remarks>
    /// This is the normal public send operation. Empty metadata collections
    /// are supplied automatically.
    /// </remarks>
    /// <param name="cryptoContext">
    /// Identifies the sender, recipient, encryption algorithm, and other
    /// cryptographic information required by the provider.
    /// </param>
    /// <param name="payload">
    /// The payload to protect and send.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the send operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous send operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The payload is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The crypto provider fails to create a validated secure envelope.
    /// </exception>
    public Task SendAsync(ICryptoContext cryptoContext, ReadOnlyMemory payload,
        CancellationToken cancellationToken = default)
        => SendInternalAsync(cryptoContext, payload, Metadata.Empty, 
            Metadata.Empty, cancellationToken);

    /// <summary>
    /// Performs the complete protected send operation.
    /// </summary>
    /// <param name="context">
    /// Identifies the sender, recipient, encryption algorithm, and other
    /// cryptographic information required by the provider.
    /// </param>
    /// <param name="payload">
    /// The payload to protect and send.
    /// </param>
    /// <param name="headerMeta">
    /// Metadata to include in the secure-envelope header.
    /// </param>
    /// <param name="footerMeta">
    /// Metadata to include in the secure-envelope footer.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the send operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous send operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The payload is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The crypto provider fails to create a validated secure envelope.
    /// </exception>
    private async Task SendInternalAsync(ICryptoContext context, ReadOnlyMemory payload,
        Metadata headerMeta, Metadata footerMeta, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (payload.IsEmpty)
        {
            throw new ArgumentException("Payload must not be empty.", nameof(payload));
        }

        // Package everything the crypto provider needs to protect the payload.
        ISealRequest sealRequest =
            new SealRequest(context, payload, headerMeta, footerMeta);

        // Protect the payload and build a validated secure envelope.
        var providerResult =
            await m_CryptoProvider
                .SealAsync(sealRequest, m_EnvelopeService, cancellationToken)
                .ConfigureAwait(false);

        // Mercury cannot send anything unless the provider successfully
        // returns a validated secure envelope.
        if (!providerResult.Success ||
            providerResult.ValidatedEnvelope == null)
        {
            throw new InvalidOperationException(
                providerResult.Message
                ?? "The crypto provider failed to create a secure envelope.");
        }

        // Convert the secure envelope into the configured wire format.
        var frame =
            m_EnvelopeCodec.Encode(providerResult.ValidatedEnvelope);

        // Only the encoded protected frame crosses the transport boundary.
        await m_Transport
            .SendAsync(frame, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives, decodes, authenticates, and opens one Mercury frame.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A failed receive operation is returned as an
    /// <see cref="IMercuryResult"/> rather than thrown, except when the
    /// operation is explicitly canceled.
    /// </para>
    /// <para>
    /// Replay protection is performed only after the crypto provider has
    /// successfully authenticated the secure envelope. This prevents
    /// unauthenticated header data from being recorded by the replay
    /// protector.
    /// </para>
    /// </remarks>
    /// <param name="cancellationToken">
    /// A token used to cancel the receive operation.
    /// </param>
    /// <returns>
    /// A result containing the recovered payload and validated secure
    /// envelope when successful, or failure information when unsuccessful.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation is canceled through the supplied cancellation token.
    /// </exception>
    public async Task<IMercuryResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        ISecureEnvelope? secureEnvelope = null;

        try
        {
            // Wait for one encoded frame from the configured transport.
            var frame =
                await m_Transport
                    .ReceiveAsync(cancellationToken)
                    .ConfigureAwait(false);

            // A transport must never return an empty frame as a valid exchange.
            if (frame.IsEmpty)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, null,
                    FailureReason.InternalError, "The transport returned an empty frame.");
            }

            try
            {
                // Reconstruct the secure envelope from the received frame.
                secureEnvelope =
                    m_EnvelopeCodec.Decode(frame);
            }
            catch (Exception exception)
            {
                // A frame that cannot be decoded is rejected before it reaches
                // the crypto provider.
                return new MercuryResult(false, ReadOnlyMemory.Empty, null,
                    FailureReason.DecodeFailed, exception.Message);
            }

            // Package the decoded secure envelope for the crypto provider.
            IOpenRequest openRequest =
                new OpenRequest(secureEnvelope);

            // Authenticate the secure envelope and recover the payload.
            var cryptoProviderResult =
                await m_CryptoProvider
                    .OpenAsync(openRequest, m_EnvelopeService, cancellationToken)
                    .ConfigureAwait(false);

            // Authentication, decryption, or provider validation failed.
            if (!cryptoProviderResult.Success)
            {
                return new MercuryResult(cryptoProviderResult);
            }

            // A provider must never report success without returning the
            // validated secure envelope that produced the recovered payload.
            if (cryptoProviderResult.ValidatedEnvelope == null)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, null,
                    FailureReason.InternalError, "The crypto provider reported success without returning a validated secure envelope.");
            }

            // Retrieve the authenticated header from the secure envelope returned
            // by the crypto provider.
            var validatedHeader = cryptoProviderResult.ValidatedEnvelope.Header;

            // Reject messages not addressed to this client
            if (!validatedHeader.RecipientKeyId.Equals(m_ClientId))
            {
                return new MercuryResult( false, ReadOnlyMemory.Empty, cryptoProviderResult.ValidatedEnvelope,
                    FailureReason.AuthenticationFailed, "Message was not addressed to this client.");
            }

            // Replay protection requires both the sender identity and a replay token.
            // An authenticated envelope missing either value is rejected before its
            // recovered payload can be delivered.
            if (validatedHeader.SenderKeyId.IsEmpty || validatedHeader.ReplayToken.IsEmpty)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, cryptoProviderResult.ValidatedEnvelope,
                    FailureReason.AuthenticationFailed, "The validated secure envelope is missing required replay information.");
            }

            // The replay token is checked only after the envelope has been
            // authenticated by the crypto provider.
            var replayAccepted = await m_ReplayProtector
                    .TryAcceptAsync(validatedHeader, cancellationToken)
                    .ConfigureAwait(false);

            // A previously accepted replay token means the frame is a replay
            // and its recovered payload must not be delivered.
            if (!replayAccepted)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, cryptoProviderResult.ValidatedEnvelope,
                    FailureReason.ReplayDetected, "Replay detected.");
            }

            // The frame was decoded, authenticated, opened, and accepted by
            // replay protection. Return the provider's successful result.
            return new MercuryResult(
                cryptoProviderResult);
        }
        catch (OperationCanceledException)
        {
            // Cancellation is intentionally allowed to propagate to the caller.
            throw;
        }
        catch (Exception exception)
        {
            // Unexpected failures are converted into a safe Mercury result.
            // If decoding succeeded, include the envelope for diagnostics.
            return new MercuryResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                FailureReason.InternalError, exception.Message);
        }
    }
}