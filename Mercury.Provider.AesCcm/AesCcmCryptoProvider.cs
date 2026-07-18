// ***********************************************************************
// Assembly     : Mercury.Providers.AesCcm
// Author         : Kim K. Brown
// Created        : 07-18-2026
//
// Last Modified By : Kim K. Brown
// Last Modified On : 07-18-2026
// ***********************************************************************
// <copyright file="AesCcmCryptoProvider.cs">
//     Copyright (c) Kim K. Brown. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using Mercury.Abstractions.Shared;
using System.Security.Cryptography;
using System.Text;

namespace Mercury.Provider.AesCcm;

/// <summary>
/// Provides authenticated encryption using AES-CCM with a 256-bit key.
/// </summary>
public sealed class AesCcmCryptoProvider(
    ISymmetricKeyProvider keys) : ICryptoProvider
{
    private const string ALGORITHM_NAME = "aes-ccm-256";
    private const int KEY_SIZE = 32;
    private const int NONCE_SIZE = 12;
    private const int TAG_SIZE = 16;
    private const int REPLAY_TOKEN_SIZE = 16;

    private static readonly AlgorithmId sm_AlgorithmId =
        new(ALGORITHM_NAME);

    private static readonly AuthenticatedPayloadFormat sm_PayloadFormat =
        new(NONCE_SIZE, TAG_SIZE);

    private readonly ISymmetricKeyProvider m_Keys = keys
        ?? throw new ArgumentNullException(nameof(keys));

    /// <inheritdoc />
    public string Name => sm_AlgorithmId.Value;

    /// <inheritdoc />
    public async Task<ICryptoProviderResult> SealAsync(
        ISealRequest sealRequest,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(sealRequest);
        ArgumentNullException.ThrowIfNull(envelopeService);

        if (sealRequest.Payload.IsEmpty)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.Custom,
                "The payload cannot be empty.");
        }

        if (sealRequest.CryptoContext.IsEmpty)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.Custom,
                "The crypto context cannot be empty.");
        }

        if (sealRequest.CryptoContext.SenderKeyId.IsEmpty)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.Custom,
                "The sender key identifier cannot be empty.");
        }

        if (sealRequest.CryptoContext.RecipientKeyId.IsEmpty)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.Custom,
                "The recipient key identifier cannot be empty.");
        }

        byte[]? key = null;

        try
        {
            EnsureAlgorithmIsSupported();

            var keyMemory = await m_Keys.GetKeyAsync(
                    sealRequest.CryptoContext.RecipientKeyId,
                    cancellationToken)
                .ConfigureAwait(false);

            key = keyMemory.ToArray();

            ValidateKeySize(key);

            var nonce =
                RandomNumberGenerator.GetBytes(NONCE_SIZE);

            var authenticationTag =
                new byte[TAG_SIZE];

            var plaintext =
                sealRequest.Payload.ToArray();

            var ciphertext =
                new byte[plaintext.Length];

            var replayToken =
                RandomNumberGenerator.GetBytes(REPLAY_TOKEN_SIZE);

            var replayTokenMemory =
                new ReadOnlyMemory(replayToken);

            var additionalAuthenticatedData =
                BuildAdditionalAuthenticatedData(
                    sealRequest.CryptoContext.SenderKeyId,
                    sealRequest.CryptoContext.RecipientKeyId,
                    sm_AlgorithmId,
                    replayTokenMemory);

            using (var aesCcm =
                   new System.Security.Cryptography.AesCcm(key))
            {
                aesCcm.Encrypt(
                    nonce,
                    plaintext,
                    ciphertext,
                    authenticationTag,
                    additionalAuthenticatedData);
            }

            var header =
                envelopeService.BuildEnvelopeHeader(
                    new KeyId(Guid.NewGuid().ToString("N")),
                    DateTimeOffset.UtcNow,
                    sealRequest.CryptoContext.SenderKeyId,
                    sealRequest.CryptoContext.RecipientKeyId,
                    sm_AlgorithmId,
                    AlgorithmId.Empty,
                    replayTokenMemory,
                    sealRequest.HeaderMeta.Clone());

            var footer =
                envelopeService.BuildEnvelopeFooter(
                    sealRequest.FooterMeta.Clone());

            var protectedPayload =
                sm_PayloadFormat.Pack(
                    nonce,
                    authenticationTag,
                    ciphertext);

            return envelopeService.PackEnvelope(
                header,
                new ReadOnlyMemory(protectedPayload),
                footer);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.InternalError,
                exception.Message);
        }
        finally
        {
            if (key is not null)
            {
                CryptographicOperations.ZeroMemory(key);
            }
        }
    }

    /// <inheritdoc />
    public async Task<ICryptoProviderResult> OpenAsync(
        IOpenRequest openRequest,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(openRequest);
        ArgumentNullException.ThrowIfNull(envelopeService);

        var secureEnvelope =
            openRequest.SecureEnvelope;

        byte[]? key = null;

        try
        {
            EnsureAlgorithmIsSupported();
            ValidateEnvelope(secureEnvelope);

            if (!string.Equals(
                    secureEnvelope.Header.Encryption.Value,
                    Name,
                    StringComparison.Ordinal))
            {
                return envelopeService.BuildCryptoProviderResult(
                    false,
                    ReadOnlyMemory.Empty,
                    secureEnvelope,
                    FailureReason.AuthenticationFailed,
                    "The secure envelope was not created with " +
                    "the AES-CCM-256 provider.");
            }

            var keyMemory = await m_Keys.GetKeyAsync(
                    secureEnvelope.Header.RecipientKeyId,
                    cancellationToken)
                .ConfigureAwait(false);

            key = keyMemory.ToArray();

            ValidateKeySize(key);

            var plaintext =
                DecryptPayload(key, secureEnvelope);

            return envelopeService.BuildCryptoProviderResult(
                true,
                new ReadOnlyMemory(plaintext),
                secureEnvelope,
                FailureReason.None);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (CryptographicException)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                secureEnvelope,
                FailureReason.AuthenticationFailed,
                "AES-CCM authentication failed.");
        }
        catch (Exception exception)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                secureEnvelope,
                FailureReason.InternalError,
                exception.Message);
        }
        finally
        {
            if (key is not null)
            {
                CryptographicOperations.ZeroMemory(key);
            }
        }
    }

    /// <summary>
    /// Authenticates and decrypts a protected envelope payload.
    /// </summary>
    /// <param name="key">The AES-CCM key.</param>
    /// <param name="secureEnvelope">The protected envelope.</param>
    /// <returns>The decrypted plaintext.</returns>
    private static byte[] DecryptPayload(
        byte[] key,
        ISecureEnvelope secureEnvelope)
    {
        sm_PayloadFormat.Unpack(
            secureEnvelope.Payload,
            out var nonce,
            out var authenticationTag,
            out var ciphertext);

        var plaintext =
            new byte[ciphertext.Length];

        var additionalAuthenticatedData =
            BuildAdditionalAuthenticatedData(
                secureEnvelope.Header.SenderKeyId,
                secureEnvelope.Header.RecipientKeyId,
                secureEnvelope.Header.Encryption,
                secureEnvelope.Header.ReplayToken);

        using var aesCcm =
            new System.Security.Cryptography.AesCcm(key);

        aesCcm.Decrypt(
            nonce,
            ciphertext,
            authenticationTag,
            plaintext,
            additionalAuthenticatedData);

        return plaintext;
    }

    /// <summary>
    /// Validates the required secure-envelope fields.
    /// </summary>
    /// <param name="secureEnvelope">The envelope to validate.</param>
    private static void ValidateEnvelope(
        ISecureEnvelope secureEnvelope)
    {
        ArgumentNullException.ThrowIfNull(secureEnvelope);

        if (secureEnvelope.Payload.IsEmpty)
        {
            throw new InvalidOperationException(
                "The secure envelope payload is empty.");
        }

        if (secureEnvelope.Header.SenderKeyId.IsEmpty)
        {
            throw new InvalidOperationException(
                "The sender key identifier is missing.");
        }

        if (secureEnvelope.Header.RecipientKeyId.IsEmpty)
        {
            throw new InvalidOperationException(
                "The recipient key identifier is missing.");
        }

        if (secureEnvelope.Header.Encryption.IsEmpty)
        {
            throw new InvalidOperationException(
                "The encryption algorithm identifier is missing.");
        }

        if (secureEnvelope.Header.ReplayToken.IsEmpty)
        {
            throw new InvalidOperationException(
                "The replay token is missing.");
        }
    }

    /// <summary>
    /// Validates that the key is a 256-bit AES key.
    /// </summary>
    /// <param name="key">The key to validate.</param>
    private static void ValidateKeySize(byte[] key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key.Length != KEY_SIZE)
        {
            throw new CryptographicException(
                $"AES-CCM-256 requires a {KEY_SIZE}-byte key. " +
                $"Actual: {key.Length} bytes.");
        }
    }

    /// <summary>
    /// Verifies that AES-CCM is supported by the current platform.
    /// </summary>
    private static void EnsureAlgorithmIsSupported()
    {
        if (!System.Security.Cryptography.AesCcm.IsSupported)
        {
            throw new PlatformNotSupportedException(
                "AES-CCM is not supported on this platform.");
        }
    }

    /// <summary>
    /// Builds the additional authenticated data used by AES-CCM.
    /// </summary>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <param name="encryption">The encryption algorithm identifier.</param>
    /// <param name="replayToken">The replay-protection token.</param>
    /// <returns>The encoded additional authenticated data.</returns>
    private static byte[] BuildAdditionalAuthenticatedData(
        KeyId senderKeyId,
        KeyId recipientKeyId,
        AlgorithmId encryption,
        ReadOnlyMemory replayToken)
    {
        if (senderKeyId.IsEmpty)
        {
            throw new ArgumentException(
                "Sender key identifier cannot be empty.",
                nameof(senderKeyId));
        }

        if (recipientKeyId.IsEmpty)
        {
            throw new ArgumentException(
                "Recipient key identifier cannot be empty.",
                nameof(recipientKeyId));
        }

        if (encryption.IsEmpty)
        {
            throw new ArgumentException(
                "Encryption identifier cannot be empty.",
                nameof(encryption));
        }

        if (replayToken.IsEmpty)
        {
            throw new ArgumentException(
                "Replay token cannot be empty.",
                nameof(replayToken));
        }

        var replayTokenValue =
            Convert.ToBase64String(replayToken.ToArray());

        var value =
            $"{senderKeyId.Value}|" +
            $"{recipientKeyId.Value}|" +
            $"{encryption.Value}|" +
            $"{replayTokenValue}";

        return Encoding.UTF8.GetBytes(value);
    }
}