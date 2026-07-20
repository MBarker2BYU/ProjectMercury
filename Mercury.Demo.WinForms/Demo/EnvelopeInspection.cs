// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-19-2026
// ***********************************************************************

namespace Mercury.Demo.WinForms.Demo;

internal sealed class EnvelopeInspection
{
    public static EnvelopeInspection Empty { get; } = new();

    public string Version { get; init; } = "-";

    public string EnvelopeId { get; init; } = "-";

    public string Timestamp { get; init; } = "-";

    public string SenderKeyId { get; init; } = "-";

    public string RecipientKeyId { get; init; } = "-";

    public string Algorithm { get; init; } = "-";

    public string ReplayToken { get; init; } = "-";

    public string HeaderMetadata { get; init; } = "-";

    public string FooterMetadata { get; init; } = "-";

    public int ProtectedPayloadSize { get; init; }

    public int TotalFrameSize { get; init; }

    public string HexPreview { get; init; } = string.Empty;
}