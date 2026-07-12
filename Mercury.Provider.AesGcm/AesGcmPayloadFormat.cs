// ***********************************************************************
// Assembly       : Mercury.Providers.AesGcm
// Author         : Matthew D. Barker
// Created        : 07-11-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-11-2026
// ***********************************************************************
// <copyright file="AesGcmPayloadFormat.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Provider.AesGcm;

/// <summary>
/// Defines the protected payload format:
/// [1-byte nonce length][nonce][16-byte authentication tag][ciphertext].
/// </summary>
public static class AesGcmPayloadFormat
{
    /// <summary>
    /// The required AES-GCM nonce size.
    /// </summary>
    public const int NONCE_SIZE = 12;

    /// <summary>
    /// The required AES-GCM authentication tag size.
    /// </summary>
    public const int TAG_SIZE = 16;

    /// <summary>
    /// Packs the nonce, authentication tag, and ciphertext into one payload.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    /// <param name="tag">The authentication tag.</param>
    /// <param name="ciphertext">The ciphertext.</param>
    /// <returns>The packed protected payload.</returns>
    /// <exception cref="ArgumentException">
    /// The nonce or authentication tag has an invalid length.
    /// </exception>
    public static byte[] Pack(
        ReadOnlySpan<byte> nonce,
        ReadOnlySpan<byte> tag,
        ReadOnlySpan<byte> ciphertext)
    {
        if (nonce.Length != NONCE_SIZE)
        {
            throw new ArgumentException(
                $"Nonce must be {NONCE_SIZE} bytes.",
                nameof(nonce));
        }

        if (tag.Length != TAG_SIZE)
        {
            throw new ArgumentException(
                $"Authentication tag must be {TAG_SIZE} bytes.",
                nameof(tag));
        }

        var result =
            new byte[
                1 +
                NONCE_SIZE +
                TAG_SIZE +
                ciphertext.Length];

        result[0] = NONCE_SIZE;

        nonce.CopyTo(
            result.AsSpan(
                1,
                NONCE_SIZE));

        tag.CopyTo(
            result.AsSpan(
                1 + NONCE_SIZE,
                TAG_SIZE));

        ciphertext.CopyTo(
            result.AsSpan(
                1 + NONCE_SIZE + TAG_SIZE));

        return result;
    }

    /// <summary>
    /// Unpacks a protected payload into its nonce, authentication tag,
    /// and ciphertext components.
    /// </summary>
    /// <param name="payload">The packed protected payload.</param>
    /// <param name="nonce">The extracted nonce.</param>
    /// <param name="tag">The extracted authentication tag.</param>
    /// <param name="ciphertext">The extracted ciphertext.</param>
    /// <exception cref="FormatException">
    /// The protected payload is invalid or uses an unsupported nonce length.
    /// </exception>
    public static void Unpack(
        ReadOnlySpan<byte> payload,
        out ReadOnlySpan<byte> nonce,
        out ReadOnlySpan<byte> tag,
        out ReadOnlySpan<byte> ciphertext)
    {
        if (payload.Length <
            1 + NONCE_SIZE + TAG_SIZE)
        {
            throw new FormatException(
                "The protected payload is too small.");
        }

        int nonceLength =
            payload[0];

        if (nonceLength != NONCE_SIZE)
        {
            throw new FormatException(
                $"Unsupported nonce length {nonceLength}. Expected {NONCE_SIZE}.");
        }

        nonce =
            payload.Slice(
                1,
                NONCE_SIZE);

        tag =
            payload.Slice(
                1 + NONCE_SIZE,
                TAG_SIZE);

        ciphertext =
            payload.Slice(
                1 + NONCE_SIZE + TAG_SIZE);
    }
}