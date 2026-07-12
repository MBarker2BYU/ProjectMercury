// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 02-19-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="MercuryChunkingExtensions.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Chunking;

/// <summary>
/// Class MercuryChunkingExtensions.
/// </summary>
public static class MercuryChunkingExtensions
{
    /// <summary>
    /// The maximum chunks
    /// </summary>
    private const int MAX_CHUNKS = 4096;

    /// <summary>
    /// The maximum payload length
    /// </summary>
    private const int MAX_PAYLOAD_LENGTH = 256 * 1024 * 1024;

    /// <summary>
    /// The chunk identifier key
    /// </summary>
    private const string CHUNK_ID_KEY = "mercury.chunk.id";
    /// <summary>
    /// The chunk index key
    /// </summary>
    private const string CHUNK_INDEX_KEY = "mercury.chunk.index";
    /// <summary>
    /// The chunk total key
    /// </summary>
    private const string CHUNK_TOTAL_KEY = "mercury.chunk.total";
    /// <summary>
    /// The chunk length key
    /// </summary>
    private const string CHUNK_LEN_KEY = "mercury.chunk.len";

    // Opt-in chunked send (does not affect normal SendAsync)
    /// <summary>
    /// Send chunked as an asynchronous operation.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="context">The context.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="chunkSizeBytes">The chunk size bytes.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">client</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">chunkSizeBytes</exception>
    /// <exception cref="System.ArgumentException">Payload must not be empty. - payload</exception>
    /// <exception cref="System.NotSupportedException">Chunked send requires SecureCommunicationsFrameworkClient concrete type (meta overload).</exception>
    public static async Task SendChunkedAsync(this IMercuryClient client, ICryptoContext context,
        ReadOnlyMemory payload, int chunkSizeBytes = 64 * 1024, CancellationToken cancellationToken = default)
    {
        if (client is null) 
            throw new ArgumentNullException(nameof(client));

        if (context is null)
            throw new ArgumentNullException(nameof(context));

        if (chunkSizeBytes < 1024) 
            throw new ArgumentOutOfRangeException(nameof(chunkSizeBytes));

        if (payload.IsEmpty) 
            throw new ArgumentException("Payload must not be empty.", nameof(payload));

        if (payload.Length > MAX_PAYLOAD_LENGTH)
        {
            throw new ArgumentOutOfRangeException(nameof(payload),
                $"Payload cannot exceed {MAX_PAYLOAD_LENGTH} bytes.");
        }
        
        // Require concrete MercuryClient for meta overload (keeps contracts unchanged)
        if (client is not MercuryClient mercuryClient)
            throw new NotSupportedException("Chunked send requires MercuryClient concrete type (meta overload).");

        var msgId = Guid.NewGuid().ToString("N");
        var totalLen = payload.Length;
        var totalChunks = (totalLen + chunkSizeBytes - 1) / chunkSizeBytes;

        if (totalChunks > MAX_CHUNKS)
        {
            throw new InvalidOperationException(
                $"The payload requires more than {MAX_CHUNKS} chunks.");
        }

        for (var i = 0; i < totalChunks; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var offset = i * chunkSizeBytes;
            var len = Math.Min(chunkSizeBytes, totalLen - offset);
            var slice = payload.Slice(offset, len);
            
            var meta = new Metadata()
            {
                {CHUNK_ID_KEY, msgId},
                {CHUNK_INDEX_KEY, i.ToString()},
                {CHUNK_TOTAL_KEY, totalChunks.ToString()},
                {CHUNK_LEN_KEY, totalLen.ToString()}
            };

            await mercuryClient.SendAsync(context, slice, meta, Metadata.Empty, cancellationToken).ConfigureAwait(false);
        }
    }

    // Simple “receive one full chunked message” helper.
    // It blocks (awaits) until it has all chunks for one message id.
    /// <summary>
    /// Receive chunked as an asynchronous operation.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IReceiveResult&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">client</exception>
    public static async Task<IMercuryResult> ReceiveChunkedAsync(this IMercuryClient client, CancellationToken cancellationToken = default)
    {
        if (client is null) throw new ArgumentNullException(nameof(client));

        string? currentId = null;
        var expectedTotal = -1;
        var expectedLen = -1;
        byte[]?[]? chunks = null;
        var received = 0;
        var receivedLength = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await client.ReceiveAsync(cancellationToken).ConfigureAwait(false);
            if (!result.Success)
                return result;

            var secureEnvelope = result.ValidatedEnvelope;

            if (secureEnvelope is null)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, null,
                    FailureReason.InternalError, "The received result did not contain a validated envelope.");
            }

            if (!secureEnvelope.Header.Meta.TryGetValue(CHUNK_ID_KEY, out var id) ||
                !secureEnvelope.Header.Meta.TryGetValue(CHUNK_INDEX_KEY, out var indexString) ||
                !secureEnvelope.Header.Meta.TryGetValue(CHUNK_TOTAL_KEY, out var totalString) ||
                !secureEnvelope.Header.Meta.TryGetValue(CHUNK_LEN_KEY, out var lengthString) ||
                !int.TryParse(indexString, out var index) ||
                !int.TryParse(totalString, out var total) ||
                !int.TryParse(lengthString, out var msgLen))
            {
                // Not a valid chunk marker — treat as normal
                return result;
            }

            if (currentId is null)
            {
                if (total is <= 0 or > MAX_CHUNKS)
                {
                    return new MercuryResult(false, ReadOnlyMemory.Empty,
                        secureEnvelope, FailureReason.InternalError, "The chunk count is invalid.");
                }

                if (msgLen is <= 0 or > MAX_PAYLOAD_LENGTH)
                {
                    return new MercuryResult(false, ReadOnlyMemory.Empty,
                        secureEnvelope, FailureReason.InternalError, "The chunked payload length is invalid.");
                }

                currentId = id;
                expectedTotal = total;
                expectedLen = msgLen;
                chunks = new byte[expectedTotal][];
            }

            if (total != expectedTotal || msgLen != expectedLen)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                    FailureReason.InternalError, "The chunk metadata does not match the current payload.");
            }

            if (!string.Equals(currentId, id, StringComparison.Ordinal))
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                    FailureReason.InternalError, "A chunk from another payload was received before reassembly completed.");
            }

            if (index < 0 || index >= expectedTotal)
            {
                return new MercuryResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                    FailureReason.InternalError, "The chunk index is invalid.");
            }

            if (chunks![index] is null)
            {
                var chunk = result.Payload.ToArray();

                if (receivedLength + chunk.Length > expectedLen)
                {
                    return new MercuryResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                        FailureReason.InternalError, "The received chunks exceed the declared payload length.");
                }

                chunks[index] = chunk;
                received++;
                receivedLength += chunk.Length;
            }

            if (received == expectedTotal)
            {

                if (receivedLength != expectedLen)
                {
                    return new MercuryResult(false, ReadOnlyMemory.Empty, secureEnvelope,
                        FailureReason.InternalError, "The rebuilt payload length does not match the declared length.");
                }

                var reassembled = new byte[expectedLen];
                var pos = 0;

                for (var i = 0; i < expectedTotal; i++)
                {
                    if (chunks[i] == null)
                    {
                        return new MercuryResult(false, ReadOnlyMemory.Empty,
                            secureEnvelope, FailureReason.InternalError, "A required chunk is missing.");
                    }

                    var part = chunks[i]!;

                    Buffer.BlockCopy(part, 0, reassembled, pos, part.Length);

                    pos += part.Length;
                }

                return new MercuryResult(
                     true,
                    new ReadOnlyMemory(reassembled),
                     secureEnvelope,
                     FailureReason.None
                );
            }
        }
    }
}
