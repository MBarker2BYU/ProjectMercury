// ***********************************************************************
// Assembly       : Mercury.Provider.AesGcm
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
// ***********************************************************************
// <copyright file="AesGcmCryptoProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Provider.AesGcm.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Mercury.Core.Factories;

namespace Mercury.Provider.AesGcm;

/// <summary>
/// AES-GCM Crypto Provider.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AesGcmCryptoProvider"/> class.
/// </remarks>
/// <param name="keys">The keys.</param>
/// <exception cref="System.ArgumentNullException">keys</exception>
public sealed class AesGcmCryptoProvider(IAesKeyProvider keys) : ICryptoProvider
{
    /// <summary>
    /// The keys
    /// </summary>
    private readonly IAesKeyProvider m_Keys = keys ?? throw new ArgumentNullException(nameof(keys));

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name => "aes-gcm-256";

    /// <summary>
    /// Seals the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ISecureEnvelope&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentException">Payload must not be empty. - request</exception>
    public async Task<ISecureEnvelope> SealAsync(ISealRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        if (request.Payload.IsEmpty)
            throw new ArgumentException("Payload must not be empty.", nameof(request));

        ValidateContext(request.CryptoContext);

        // Resolve key
        var keyMem = await m_Keys.GetKeyAsync(request.CryptoContext.RecipientKeyId.Value, ct).ConfigureAwait(false);
        var key = keyMem.ToArray();
        ValidateKeySize(key);

        var nonce = RandomNumberGenerator.GetBytes(AesGcmPayloadFormat.NONCE_SIZE);
        var tag = new byte[AesGcmPayloadFormat.TAG_SIZE];
        var ciphertext = new byte[request.Payload.Length];

        var aad = BuildAad(
            request.CryptoContext.SenderKeyId.Value,
            request.CryptoContext.RecipientKeyId.Value,
            request.CryptoContext.Encryption.Value);

        using (var aes = new System.Security.Cryptography.AesGcm(key, AesGcmPayloadFormat.TAG_SIZE))
        {
            aes.Encrypt(nonce, request.Payload.ToArray(), ciphertext, tag, aad);
        }

        var replayToken = RandomNumberGenerator.GetBytes(16);

        var header = MercuryFactory.BuildEnvelopeHeader(
            new KeyId(Guid.NewGuid().ToString("N")),
            DateTimeOffset.UtcNow,
            request.CryptoContext.SenderKeyId,
            request.CryptoContext.RecipientKeyId,
            request.CryptoContext.Encryption,
            request.CryptoContext.Signature,
            new ReadOnlyMemory(replayToken),
            request.HeaderMeta);

        var footer = MercuryFactory.BuildEnvelopeFooter(request.FooterMeta);

        var protectedPayload = AesGcmPayloadFormat.Pack(nonce, tag, ciphertext);

        return MercuryFactory.BuildSecureEnvelope(
            new FrameworkVersion(1, 0),
            header,
            new ReadOnlyMemory(protectedPayload),
            footer
        );
    }

    /// <summary>
    /// Unseals the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IMercuryResult&gt; representing the asynchronous operation.</returns>
    public async Task<IMercuryResult> UnsealAsync(IUnsealRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var env = request.SecureEnvelope;

        try
        {
            ValidateEnvelope(env);

            var keyMem = await m_Keys.GetKeyAsync(env.Header.RecipientKeyId.Value, ct).ConfigureAwait(false);
            var key = keyMem.ToArray();
            ValidateKeySize(key);

            var (nonce, tag, ciphertext) = AesGcmPayloadFormat.Unpack(env.Payload.ToArray());

            var plaintext = new byte[ciphertext.Length];

            var aad = BuildAad(
                env.Header.SenderKeyId.Value,
                env.Header.RecipientKeyId.Value,
                env.Header.Encryption.Value);

            using (var aes = new System.Security.Cryptography.AesGcm(key, AesGcmPayloadFormat.TAG_SIZE))
            {
                aes.Decrypt(nonce, ciphertext, tag, plaintext, aad);
            }

            return  Factory.BuildMercuryResult(true, new ReadOnlyMemory(plaintext), env, FailureReason.None);
        }
        catch (CryptographicException)
        {
            return Factory.BuildMercuryResult(false, ReadOnlyMemory.Empty, null, FailureReason.AuthenticationFailed, "Decrypt failed (authentication tag mismatch).");
        }
        catch (Exception ex)
        {
            return Factory.BuildMercuryResult(false, ReadOnlyMemory.Empty, null, FailureReason.InternalError, ex.Message);
        }
    }

    /// <summary>
    /// Validates the context.
    /// </summary>
    /// <param name="ctx">The CTX.</param>
    /// <exception cref="System.ArgumentException">SenderKeyId is required. - ctx</exception>
    /// <exception cref="System.ArgumentException">RecipientKeyId is required. - ctx</exception>
    /// <exception cref="System.NotSupportedException">Encryption '{ctx.Encryption.Value}' not supported by AES-GCM provider.</exception>
    /// <exception cref="System.NotSupportedException">Signature '{ctx.Signature.Value}' not supported by AES-GCM provider.</exception>
    private static void ValidateContext(ICryptoContext ctx)
    {
        if (string.IsNullOrWhiteSpace(ctx.SenderKeyId.Value))
            throw new ArgumentException("SenderKeyId is required.", nameof(ctx));

        if (string.IsNullOrWhiteSpace(ctx.RecipientKeyId.Value))
            throw new ArgumentException("RecipientKeyId is required.", nameof(ctx));

        if (!string.Equals(ctx.Encryption.Value, "aes-gcm-256", StringComparison.Ordinal))
            throw new NotSupportedException($"Encryption '{ctx.Encryption.Value}' not supported by AES-GCM provider.");

        if (!string.Equals(ctx.Signature.Value, "none", StringComparison.Ordinal))
            throw new NotSupportedException($"Signature '{ctx.Signature.Value}' not supported by AES-GCM provider.");
    }

    /// <summary>
    /// Validates the envelope.
    /// </summary>
    /// <param name="env">The env.</param>
    /// <exception cref="System.ArgumentNullException">env</exception>
    /// <exception cref="System.InvalidOperationException">ProtectedPayload is empty.</exception>
    /// <exception cref="System.InvalidOperationException">Header is missing.</exception>
    private static void ValidateEnvelope(ISecureEnvelope env)
    {
        ArgumentNullException.ThrowIfNull(env);

        if (env.Payload.IsEmpty) throw new InvalidOperationException("ProtectedPayload is empty.");
        if (env.Header is null) throw new InvalidOperationException("Header is missing.");
    }

    /// <summary>
    /// Validates the size of the key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <exception cref="System.Security.Cryptography.CryptographicException">AES-GCM-256 requires a 32-byte key. Actual: {key.Length} bytes.</exception>
    private static void ValidateKeySize(byte[] key)
    {
        if (key.Length != 32)
            throw new CryptographicException($"AES-GCM-256 requires a 32-byte key. Actual: {key.Length} bytes.");
    }

    /// <summary>
    /// Builds the aad.
    /// </summary>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    /// <param name="encryption">The encryption.</param>
    /// <returns>System.Byte[].</returns>
    private static byte[] BuildAad(string senderKeyId, string recipientKeyId, string encryption)
    {
        var s = $"{senderKeyId}|{recipientKeyId}|{encryption}";
        return Encoding.UTF8.GetBytes(s);
    }

    /// <summary>
    /// Gets the factory.
    /// </summary>
    /// <value>The factory.</value>
    private MercuryFactory Factory 
        => MercuryFactory.Instance;
}