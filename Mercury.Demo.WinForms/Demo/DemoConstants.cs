// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-19-2026
// ***********************************************************************

namespace Mercury.Demo.WinForms.Demo;

internal static class DemoConstants
{
    public const string ALPHA_NODE = "Alpha Node";
    public const string BRAVO_NODE = "Bravo Node";

    public const string AES_GCM = "AES-GCM";
    public const string CHA_CHA_20 = "ChaCha20-Poly1305";

    public const string IN_MEMORY_TRANSPORT = "In-Memory";
    public const string TCP_TRANSPORT = "TCP";

    public const string BINARY_CODEC = "Binary";
    public const string JSON_CODEC = "Json";

    public const int DEFAULT_CHUNK_SIZE = 64 * 1024;

    public const string QUIET_LOGGING = "Quiet";
    public const string NORMAL_LOGGING = "Normal";
    public const string VERBOSE_LOGGING = "Verbose";

    public const string HEADER_METADATA_KEY = "demo.application.header";
    public const string FOOTER_METADATA_KEY = "demo.application.footer";
}