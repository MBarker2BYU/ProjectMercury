// ***********************************************************************
// Assembly       : Mercury.Abstractions
// Author           : Matthew D. Barker
// Created          : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="AuthenticatedPayloadFormat.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Shared;

/// <summary>
/// Defines the binary format used to store an authenticated payload.
/// </summary>
/// <remarks>
/// Layout:
/// [1-byte nonce length][nonce][authentication tag][protected payload]
/// </remarks>
public sealed class AuthenticatedPayloadFormat
{
    /// <summary>
    /// Gets the nonce size in bytes.
    /// </summary>
    /// <value>The nonce size.</value>
    public int NonceSize { get; }

    /// <summary>
    /// Gets the authentication tag size in bytes.
    /// </summary>
    /// <value>The authentication tag size.</value>
    public int TagSize { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticatedPayloadFormat"/> class.
    /// </summary>
    /// <param name="nonceSize">The nonce size in bytes.</param>
    /// <param name="tagSize">The authentication tag size in bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The nonce size or tag size is invalid.
    /// </exception>
    public AuthenticatedPayloadFormat(
        int nonceSize,
        int tagSize)
    {
        if (nonceSize is <= 0 or > byte.MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                nameof(nonceSize),
                "Nonce size must be between 1 and 255 bytes.");
        }

        if (tagSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tagSize),
                "Tag size must be greater than zero.");
        }

        NonceSize = nonceSize;
        TagSize = tagSize;
    }

    /// <summary>
    /// Packs the nonce, authentication tag, and protected payload.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    /// <param name="authenticationTag">
    /// The authentication tag.
    /// </param>
    /// <param name="protectedPayload">
    /// The protected payload.
    /// </param>
    /// <returns>The packed payload.</returns>
    /// <exception cref="ArgumentException">
    /// The nonce or authentication tag has an invalid size.
    /// </exception>
    public byte[] Pack(
        ReadOnlyMemory nonce,
        ReadOnlyMemory authenticationTag,
        ReadOnlyMemory protectedPayload)
    {
        if (nonce.Length != NonceSize)
        {
            throw new ArgumentException($"Nonce must be {NonceSize} bytes.", nameof(nonce));
        }

        if (authenticationTag.Length != TagSize)
        {
            throw new ArgumentException($"Authentication tag must be {TagSize} bytes.", nameof(authenticationTag));
        }

        var nonceBytes = nonce.ToArray();

        var tagBytes = authenticationTag.ToArray();

        var payloadBytes = protectedPayload.ToArray();

        var resultLength = checked(1 + NonceSize + TagSize + payloadBytes.Length);

        var result = new byte[resultLength];

        result[0] = (byte)NonceSize;

        Buffer.BlockCopy(nonceBytes, 0, result, 1, NonceSize);

        Buffer.BlockCopy(tagBytes, 0, result, 1 + NonceSize, TagSize);

        if (payloadBytes.Length > 0)
        {
            Buffer.BlockCopy(payloadBytes, 0, result, 1 + NonceSize + TagSize, payloadBytes.Length);
        }

        return result;
    }

    /// <summary>
    /// Unpacks an authenticated payload.
    /// </summary>
    /// <param name="payload">The packed payload.</param>
    /// <param name="nonce">The extracted nonce.</param>
    /// <param name="authenticationTag">
    /// The extracted authentication tag.
    /// </param>
    /// <param name="protectedPayload">
    /// The extracted protected payload.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The supplied payload is empty.
    /// </exception>
    /// <exception cref="FormatException">
    /// The packed payload is invalid.
    /// </exception>
    public void Unpack(ReadOnlyMemory payload, out byte[] nonce, out byte[] authenticationTag, out byte[] protectedPayload)
    {
        if (payload.IsEmpty)
        {
            throw new ArgumentException("Payload cannot be empty.", nameof(payload));
        }

        var payloadBytes = payload.ToArray();

        var minimumLength = 1 + NonceSize + TagSize;

        if (payloadBytes.Length < minimumLength)
        {
            throw new FormatException("The protected payload is too small.");
        }

        int nonceLength = payloadBytes[0];

        if (nonceLength != NonceSize)
        {
            throw new FormatException($"Unsupported nonce length {nonceLength}. Expected {NonceSize}.");
        }

        nonce = new byte[NonceSize];

        authenticationTag = new byte[TagSize];

        var protectedPayloadLength = payloadBytes.Length - minimumLength;

        protectedPayload = new byte[protectedPayloadLength];

        Buffer.BlockCopy(payloadBytes, 1, nonce, 0, NonceSize);

        Buffer.BlockCopy(payloadBytes, 1 + NonceSize, authenticationTag, 0, TagSize);

        if (protectedPayloadLength > 0)
        {
            Buffer.BlockCopy(payloadBytes, minimumLength, protectedPayload, 0, protectedPayloadLength);
        }
    }
}