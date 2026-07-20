namespace Mercury.Demo.WinForms.Demo;

internal readonly struct ChunkSizeOption(int bytes)
{
    public int Bytes { get; } = bytes;

    public override string ToString()
    {
        if (Bytes >= 1024 * 1024)
            return $"{Bytes / (1024 * 1024):N0} MB";

        return $"{Bytes / 1024:N0} KB";
    }
}