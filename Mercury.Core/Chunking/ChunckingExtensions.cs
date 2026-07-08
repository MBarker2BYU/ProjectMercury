using Mercury.Abstractions;
using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Primitives;

namespace Mercury.Core.Chunking;

public static class ChunckingExtensions
{
    /// <summary>
    /// The chunk identifier key
    /// </summary>
    private const string CHUNK_ID_KEY = "m.chunk.id";
    /// <summary>
    /// The chunk index key
    /// </summary>
    private const string CHUNK_INDEX_KEY = "m.chunk.index";
    /// <summary>
    /// The chunk total key
    /// </summary>
    private const string CHUNK_TOTAL_KEY = "m.chunk.total";
    /// <summary>
    /// The chunk length key
    /// </summary>
    private const string CHUNK_LEN_KEY = "m.chunk.len";

    public static async Task SendChunkedAsync(this IMercuryClient client, ICryptoContext cryptoContext,
        ReadOnlyMemory payload, int chunkSizeBytes = 64 * 1024, CancellationToken cancellationToken = default)
    {
        if (client is null)
            throw new ArgumentException(nameof(client));

        if (chunkSizeBytes < 1024) 
            throw new ArgumentOutOfRangeException(nameof(chunkSizeBytes));

        if (payload.IsEmpty) 
            throw new ArgumentException("Payload must not be empty.", nameof(payload));

        // Require concrete MercuryClient for meta overload (keeps contracts unchanged)
        if (client is not MercuryClient mercuryClient)
            throw new NotSupportedException("Chunked send requires SecureCommunicationsFrameworkClient concrete type (meta overload).");

        var msgId = Guid.NewGuid().ToString("N");
        var totalLen = payload.Length;
        var totalChunks = (totalLen + chunkSizeBytes - 1) / chunkSizeBytes;

        for (var i = 0; i < totalChunks; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var offset = i * chunkSizeBytes;
            var len = Math.Min(chunkSizeBytes, totalLen - offset);
            var slice = payload.Slice(offset, len);

            var meta = new Metadata
            {
                {CHUNK_ID_KEY, msgId},
                {CHUNK_INDEX_KEY, i.ToString()},
                { CHUNK_TOTAL_KEY, totalChunks.ToString() },
                { CHUNK_LEN_KEY, totalLen.ToString() }
            };

            await mercuryClient.SendAsync(cryptoContext, slice, headerMeta: meta, footerMeta: null, cancellationToken).ConfigureAwait(false);
        }
    }
}