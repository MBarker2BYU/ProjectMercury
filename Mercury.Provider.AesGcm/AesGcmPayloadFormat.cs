namespace Mercury.Provider.AesGcm;

/// <summary>
/// Class AesGcmPayloadFormat.
/// </summary>
public static class AesGcmPayloadFormat
{
    /// <summary>
    /// The nonce size
    /// </summary>
    public const int NONCE_SIZE = 12;
    /// <summary>
    /// The tag size
    /// </summary>
    public const int TAG_SIZE = 16;
    /// <summary>
    /// Packs the specified nonce.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="ciphertext">The ciphertext.</param>
    /// <returns>System.Byte[].</returns>
    /// <exception cref="System.ArgumentException">Nonce must be {NONCE_SIZE} bytes. - nonce</exception>
    /// <exception cref="System.ArgumentException">Tag must be {TAG_SIZE} bytes. - tag</exception>
    public static byte[] Pack(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> tag, ReadOnlySpan<byte> ciphertext)
    {
        if (nonce.Length != NONCE_SIZE) throw new ArgumentException($"Nonce must be {NONCE_SIZE} bytes.", nameof(nonce));
        if (tag.Length != TAG_SIZE) throw new ArgumentException($"Tag must be {TAG_SIZE} bytes.", nameof(tag));

        var result = new byte[1 + NONCE_SIZE + TAG_SIZE + ciphertext.Length];
        result[0] = NONCE_SIZE;

        nonce.CopyTo(result.AsSpan(1, NONCE_SIZE));
        tag.CopyTo(result.AsSpan(1 + NONCE_SIZE, TAG_SIZE));
        ciphertext.CopyTo(result.AsSpan(1 + NONCE_SIZE + TAG_SIZE));

        return result;
    }

    /// <summary>
    /// Unpacks the specified payload.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>System.ValueTuple&lt;System.Byte[], System.Byte[], System.Byte[]&gt;.</returns>
    /// <exception cref="System.FormatException">ProtectedPayload is too small.</exception>
    /// <exception cref="System.FormatException">Unsupported nonce length {nonceLen}. Expected {NONCE_SIZE}.</exception>
    public static (byte[] nonce, byte[] tag, byte[] ciphertext) Unpack(ReadOnlySpan<byte> payload)
    {
        if (payload.Length < 1 + NONCE_SIZE + TAG_SIZE)
            throw new FormatException("ProtectedPayload is too small.");

        var nonceLen = payload[0];
        if (nonceLen != NONCE_SIZE)
            throw new FormatException($"Unsupported nonce length {nonceLen}. Expected {NONCE_SIZE}.");

        var nonce = payload.Slice(1, NONCE_SIZE).ToArray();
        var tag = payload.Slice(1 + NONCE_SIZE, TAG_SIZE).ToArray();
        var ciphertext = payload[(1 + NONCE_SIZE + TAG_SIZE)..].ToArray();

        return (nonce, tag, ciphertext);
    }
}
