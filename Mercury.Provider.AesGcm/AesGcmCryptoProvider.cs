// ***********************************************************************
// Assembly       : Mercury.Providers.AesGcm
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="AesGcmCryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Security.Cryptography;
using System.Text;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using Mercury.Provider.AesGcm.Interfaces;

namespace Mercury.Provider.AesGcm;

/// <summary>
/// Class AesGcmCryptoProvider. This class cannot be inherited.
/// Implements the <see cref="ICryptoProvider" />.
/// </summary>
/// <seealso cref="ICryptoProvider" />
public sealed class AesGcmCryptoProvider : ICryptoProvider
{
    /// <summary>
    /// The AES key provider.
    /// </summary>
    private readonly IAesKeyProvider m_Keys;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AesGcmCryptoProvider"/> class.
    /// </summary>
    /// <param name="keys">The AES key provider.</param>
    /// <exception cref="ArgumentNullException">
    /// The AES key provider cannot be null.
    /// </exception>
    public AesGcmCryptoProvider(
        IAesKeyProvider keys)
    {
        m_Keys = keys
            ?? throw new ArgumentNullException(nameof(keys));
    }

    /// <summary>
    /// Gets the provider name.
    /// </summary>
    /// <value>The provider name.</value>
    public string Name => "aes-gcm-256";

    /// <summary>
    /// Seals the requested payload into a protected Mercury envelope.
    /// </summary>
    /// <param name="request">The seal request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the crypto provider result.
    /// </returns>
    public async Task<ICryptoProviderResult> SealAsync(
        ISealRequest request,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (envelopeService == null)
        {
            throw new ArgumentNullException(nameof(envelopeService));
        }

        if (request.Payload.IsEmpty)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.Custom,
                "The payload cannot be empty.");
        }

        if (request.CryptoContext.SenderKeyId.IsEmpty)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                null,
                FailureReason.Custom,
                "The sender key identifier cannot be empty.");
        }

        if (request.CryptoContext.RecipientKeyId.IsEmpty)
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
            ReadOnlyMemory keyMemory =
                await m_Keys
                    .GetKeyAsync(
                        request.CryptoContext.RecipientKeyId,
                        cancellationToken)
                    .ConfigureAwait(false);

            key = keyMemory.ToArray();

            ValidateKeySize(key);

            byte[] nonce =
                RandomNumberGenerator.GetBytes(
                    AesGcmPayloadFormat.NONCE_SIZE);

            byte[] authenticationTag =
                new byte[AesGcmPayloadFormat.TAG_SIZE];

            byte[] sourcePayload =
                request.Payload.ToArray();

            byte[] ciphertext =
                new byte[sourcePayload.Length];

            byte[] additionalAuthenticatedData =
                BuildAdditionalAuthenticatedData(
                    request.CryptoContext.SenderKeyId,
                    request.CryptoContext.RecipientKeyId,
                    Name);

            using (var aesGcm =
                   new System.Security.Cryptography.AesGcm(
                       key,
                       AesGcmPayloadFormat.TAG_SIZE))
            {
                aesGcm.Encrypt(
                    nonce,
                    sourcePayload,
                    ciphertext,
                    authenticationTag,
                    additionalAuthenticatedData);
            }

            byte[] replayToken =
                RandomNumberGenerator.GetBytes(16);

            IEnvelopeHeader header =
                envelopeService.BuildEnvelopeHeader(
                    new KeyId(
                        Guid.NewGuid().ToString("N")),
                    DateTimeOffset.UtcNow,
                    request.CryptoContext.SenderKeyId,
                    request.CryptoContext.RecipientKeyId,
                    new AlgorithmId(Name),
                    AlgorithmId.Empty,
                    new ReadOnlyMemory(replayToken),
                    CloneMetadata(request.HeaderMeta));

            IEnvelopeFooter footer =
                envelopeService.BuildEnvelopeFooter(
                    CloneMetadata(request.FooterMeta));

            byte[] protectedPayload =
                AesGcmPayloadFormat.Pack(
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
                FailureReason.Custom,
                exception.Message);
        }
        finally
        {
            if (key != null)
            {
                CryptographicOperations.ZeroMemory(key);
            }
        }
    }

    /// <summary>
    /// Opens and authenticates the requested Mercury envelope.
    /// </summary>
    /// <param name="request">The open request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the crypto provider result.
    /// </returns>
    public async Task<ICryptoProviderResult> OpenAsync(
        IOpenRequest request,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (envelopeService == null)
        {
            throw new ArgumentNullException(nameof(envelopeService));
        }

        ISecureEnvelope secureEnvelope =
            request.SecureEnvelope;

        byte[]? key = null;

        try
        {
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
                    FailureReason.Custom,
                    "The secure envelope was not created with the AES-GCM-256 provider.");
            }

            ReadOnlyMemory keyMemory =
                await m_Keys
                    .GetKeyAsync(
                        secureEnvelope.Header.RecipientKeyId,
                        cancellationToken)
                    .ConfigureAwait(false);

            key = keyMemory.ToArray();

            ValidateKeySize(key);

            byte[] payload =
                DecryptPayload(
                    key,
                    secureEnvelope);

            return envelopeService.BuildCryptoProviderResult(
                true,
                new ReadOnlyMemory(payload),
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
                FailureReason.Custom,
                "AES-GCM authentication failed.");
        }
        catch (Exception exception)
        {
            return envelopeService.BuildCryptoProviderResult(
                false,
                ReadOnlyMemory.Empty,
                secureEnvelope,
                FailureReason.Custom,
                exception.Message);
        }
        finally
        {
            if (key != null)
            {
                CryptographicOperations.ZeroMemory(key);
            }
        }
    }

    /// <summary>
    /// Authenticates and decrypts the protected envelope payload.
    /// </summary>
    /// <param name="key">The AES key.</param>
    /// <param name="secureEnvelope">The secure envelope.</param>
    /// <returns>The opened payload.</returns>
    private static byte[] DecryptPayload(
        byte[] key,
        ISecureEnvelope secureEnvelope)
    {
        byte[] protectedPayload =
            secureEnvelope.Payload.ToArray();

        AesGcmPayloadFormat.Unpack(
            protectedPayload,
            out ReadOnlySpan<byte> nonce,
            out ReadOnlySpan<byte> authenticationTag,
            out ReadOnlySpan<byte> ciphertext);

        byte[] payload =
            new byte[ciphertext.Length];

        byte[] additionalAuthenticatedData =
            BuildAdditionalAuthenticatedData(
                secureEnvelope.Header.SenderKeyId,
                secureEnvelope.Header.RecipientKeyId,
                secureEnvelope.Header.Encryption.Value);

        using (var aesGcm =
               new System.Security.Cryptography.AesGcm(
                   key,
                   AesGcmPayloadFormat.TAG_SIZE))
        {
            aesGcm.Decrypt(
                nonce,
                ciphertext,
                authenticationTag,
                payload,
                additionalAuthenticatedData);
        }

        return payload;
    }

    /// <summary>
    /// Validates the secure envelope.
    /// </summary>
    /// <param name="secureEnvelope">The secure envelope.</param>
    private static void ValidateEnvelope(
        ISecureEnvelope secureEnvelope)
    {
        if (secureEnvelope == null)
        {
            throw new ArgumentNullException(
                nameof(secureEnvelope));
        }

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
    /// Validates the AES key size.
    /// </summary>
    /// <param name="key">The AES key.</param>
    private static void ValidateKeySize(
        byte[] key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key.Length != 32)
        {
            throw new CryptographicException(
                $"AES-GCM-256 requires a 32-byte key. Actual: {key.Length} bytes.");
        }
    }

    /// <summary>
    /// Builds the additional authenticated data.
    /// </summary>
    /// <param name="senderKeyId">
    /// The sender key identifier.
    /// </param>
    /// <param name="recipientKeyId">
    /// The recipient key identifier.
    /// </param>
    /// <param name="algorithmName">
    /// The algorithm name.
    /// </param>
    /// <returns>The additional authenticated data.</returns>
    private static byte[] BuildAdditionalAuthenticatedData(
        KeyId senderKeyId,
        KeyId recipientKeyId,
        string algorithmName)
    {
        string value =
            $"{senderKeyId.Value}|{recipientKeyId.Value}|{algorithmName}";

        return Encoding.UTF8.GetBytes(value);
    }

    /// <summary>
    /// Creates an isolated metadata copy.
    /// </summary>
    /// <param name="source">The source metadata.</param>
    /// <returns>The copied metadata.</returns>
    private static Metadata CloneMetadata(
        Metadata? source)
    {
        Metadata copy =
            new Metadata();

        if (source == null)
        {
            return copy;
        }

        foreach (KeyValuePair<string, string> item in source)
        {
            copy.Add(
                item.Key,
                item.Value);
        }

        return copy;
    }
}