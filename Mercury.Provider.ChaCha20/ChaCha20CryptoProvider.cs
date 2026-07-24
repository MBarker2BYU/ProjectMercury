// ***********************************************************************
// Assembly     : Mercury.Provider.ChaCha20
// Author         : Matthew D. Barker
// Created        : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="ChaCha20CryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Security.Cryptography;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using Mercury.Abstractions.Shared;

namespace Mercury.Provider.ChaCha20;

/// <summary>
/// ChaCha20-Poly1305 crypto provider.
/// Implements the <see cref="ICryptoProvider" />.
/// </summary>
/// <seealso cref="ICryptoProvider" />
public sealed class ChaCha20CryptoProvider : ICryptoProvider
{
    /// <summary>
    /// The algorithm name.
    /// </summary>
    private const string ALGORITHM_NAME = "chacha20-poly1305";

    /// <summary>
    /// The algorithm identifier.
    /// </summary>
    private static readonly AlgorithmId sm_AlgorithmId = new(ALGORITHM_NAME);

    /// <summary>
    /// The shared authenticated payload format.
    /// </summary>
    private static readonly AuthenticatedPayloadFormat sm_PayloadFormat = new(12, 16);

    /// <summary>
    /// The symmetric key provider.
    /// </summary>
    private readonly ISymmetricKeyProvider m_Keys;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ChaCha20CryptoProvider"/> class.
    /// </summary>
    /// <param name="keys">
    /// The symmetric key provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The symmetric key provider cannot be null.
    /// </exception>
    public ChaCha20CryptoProvider(ISymmetricKeyProvider keys)
    {
        m_Keys = keys
                 ?? throw new ArgumentNullException(nameof(keys));
    }

    /// <summary>
    /// Gets the provider name.
    /// </summary>
    /// <value>The provider name.</value>
    public string Name => sm_AlgorithmId.Value;

    /// <summary>
    /// Seals the requested payload into a protected Mercury envelope.
    /// </summary>
    /// <param name="request">
    /// The seal request.
    /// </param>
    /// <param name="envelopeService">
    /// The envelope service.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the crypto provider result.
    /// </returns>
    public async Task<ICryptoProviderResult> SealAsync(ISealRequest request, IEnvelopeService envelopeService, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(request);

        ArgumentNullException.ThrowIfNull(envelopeService);

        if (request.Payload.IsEmpty)
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, 
                null, FailureReason.Custom, "The payload cannot be empty.");
        

        if (request.CryptoContext.IsEmpty)
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, null,
                FailureReason.Custom, "The crypto context cannot be empty.");
        

        if (request.CryptoContext.SenderKeyId.IsEmpty)
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, null, 
                FailureReason.Custom, "The sender key identifier cannot be empty.");
        

        if (request.CryptoContext.RecipientKeyId.IsEmpty)
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, null,
                FailureReason.Custom, "The recipient key identifier cannot be empty.");
        

        byte[]? key = null;

        try
        {
            var keyMemory = await m_Keys
                    .GetKeyAsync(request.CryptoContext.RecipientKeyId, cancellationToken)
                    .ConfigureAwait(false);

            key = keyMemory.ToArray();

            ValidateKeySize(key);

            var nonce = RandomNumberGenerator.GetBytes(sm_PayloadFormat.NonceSize);

            var authenticationTag = new byte[sm_PayloadFormat.TagSize];

            var sourcePayload = request.Payload.ToArray();

            var ciphertext = new byte[sourcePayload.Length];

            var replayToken = RandomNumberGenerator.GetBytes(16);

            var replayTokenMemory = new ReadOnlyMemory(replayToken);

            /*
             * Build the header and footer before encryption because
             * their complete contents are authenticated as additional data.
             */
            var header = envelopeService.BuildEnvelopeHeader(new KeyId(Guid.NewGuid().ToString("N")), DateTimeOffset.UtcNow,
                    request.CryptoContext.SenderKeyId, request.CryptoContext.RecipientKeyId, sm_AlgorithmId, AlgorithmId.Empty,
                    replayTokenMemory, request.HeaderMeta.Clone());

            var footer = envelopeService.BuildEnvelopeFooter(request.FooterMeta.Clone());

            var additionalAuthenticatedData = AuthenticatedEnvelopeData.Build(FrameworkVersion.V1.Major,
                FrameworkVersion.V1.Minor, header, footer);

            using (var chaCha20 = new ChaCha20Poly1305(key))
            {
                chaCha20.Encrypt(nonce, sourcePayload, ciphertext, authenticationTag, additionalAuthenticatedData);
            }

            var protectedPayload = sm_PayloadFormat.Pack(new ReadOnlyMemory(nonce),
                    new ReadOnlyMemory(authenticationTag), new ReadOnlyMemory(ciphertext));

            return envelopeService.PackEnvelope(header, new ReadOnlyMemory(protectedPayload), footer);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, null,
                FailureReason.InternalError, exception.Message);
        }
        finally
        {
            if (key != null)
                CryptographicOperations.ZeroMemory(key);
        }
    }

    /// <summary>
    /// Opens and authenticates the requested Mercury envelope.
    /// </summary>
    /// <param name="request">
    /// The open request.
    /// </param>
    /// <param name="envelopeService">
    /// The envelope service.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the crypto provider result.
    /// </returns>
    public async Task<ICryptoProviderResult> OpenAsync(IOpenRequest request, IEnvelopeService envelopeService, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(request);

        ArgumentNullException.ThrowIfNull(envelopeService);

        var secureEnvelope = request.SecureEnvelope;

        byte[]? key = null;

        try
        {
            ValidateEnvelope(secureEnvelope);

            if (!string.Equals(secureEnvelope.Header.Encryption.Value, Name, StringComparison.Ordinal))
            {
                return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                    FailureReason.AuthenticationFailed, "The secure envelope was not created with the ChaCha20-Poly1305 provider.");
            }

            var keyMemory = await m_Keys
                    .GetKeyAsync(secureEnvelope.Header.RecipientKeyId, cancellationToken)
                    .ConfigureAwait(false);

            key = keyMemory.ToArray();

            ValidateKeySize(key);

            var payload = DecryptPayload(key, secureEnvelope);

            return envelopeService.BuildCryptoProviderResult(true, new ReadOnlyMemory(payload),
                secureEnvelope, FailureReason.None);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (CryptographicException)
        {
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                FailureReason.AuthenticationFailed, "ChaCha20-Poly1305 authentication failed.");
        }
        catch (Exception exception)
        {
            return envelopeService.BuildCryptoProviderResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                FailureReason.InternalError, exception.Message);
        }
        finally
        {
            if (key != null)
                CryptographicOperations.ZeroMemory(key);
            
        }
    }

    /// <summary>
    /// Authenticates and decrypts the protected envelope payload.
    /// </summary>
    /// <param name="key">
    /// The symmetric key.
    /// </param>
    /// <param name="secureEnvelope">
    /// The secure envelope.
    /// </param>
    /// <returns>
    /// The opened payload.
    /// </returns>
    private static byte[] DecryptPayload(byte[] key, ISecureEnvelope secureEnvelope)
    {
        sm_PayloadFormat.Unpack(secureEnvelope.Payload, out var nonce, out var authenticationTag,
            out var ciphertext);

        var payload = new byte[ciphertext.Length];

        var additionalAuthenticatedData = AuthenticatedEnvelopeData.Build(secureEnvelope.Version.Major, secureEnvelope.Version.Minor,
                secureEnvelope.Header, secureEnvelope.Footer);

        using var chaCha20 = new ChaCha20Poly1305(key);

        chaCha20.Decrypt(nonce, ciphertext, authenticationTag, payload, additionalAuthenticatedData);

        return payload;
    }

    /// <summary>
    /// Validates the secure envelope.
    /// </summary>
    /// <param name="secureEnvelope">
    /// The secure envelope.
    /// </param>
    private static void ValidateEnvelope(ISecureEnvelope secureEnvelope)
    {
        ArgumentNullException.ThrowIfNull(secureEnvelope);

        if (secureEnvelope.Payload.IsEmpty)
            throw new InvalidOperationException("The secure envelope payload is empty.");
        

        if (secureEnvelope.Header.SenderKeyId.IsEmpty)
            throw new InvalidOperationException("The sender key identifier is missing.");
        

        if (secureEnvelope.Header.RecipientKeyId.IsEmpty)
            throw new InvalidOperationException("The recipient key identifier is missing.");
        

        if (secureEnvelope.Header.Encryption.IsEmpty)
            throw new InvalidOperationException("The encryption algorithm identifier is missing.");
        

        if (secureEnvelope.Header.ReplayToken.IsEmpty)
            throw new InvalidOperationException("The replay token is missing.");
        
    }

    /// <summary>
    /// Validates the ChaCha20-Poly1305 key size.
    /// </summary>
    /// <param name="key">
    /// The symmetric key.
    /// </param>
    private static void ValidateKeySize(byte[] key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key.Length != 32)
            throw new CryptographicException($"ChaCha20-Poly1305 requires a 32-byte key. Actual: {key.Length} bytes.");
    }
}