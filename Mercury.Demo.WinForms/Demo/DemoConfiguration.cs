// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-19-2026
// ***********************************************************************

namespace Mercury.Demo.WinForms.Demo;

internal readonly struct DemoConfiguration(string cryptoProvider, string transport, string envelopeCodec, bool chunkingEnabled, int chunkSize, string loggingLevel)
{
    public static DemoConfiguration Default 
        => new DemoConfiguration();

    public DemoConfiguration() : this(DemoConstants.AES_GCM, DemoConstants.IN_MEMORY_TRANSPORT, 
        DemoConstants.BINARY_CODEC, true, DemoConstants.DEFAULT_CHUNK_SIZE, DemoConstants.VERBOSE_LOGGING)
    {}
    
    public string CryptoProvider { get; } = cryptoProvider;

    public string Transport { get; } = transport;

    public string EnvelopeCodec { get; } = envelopeCodec;

    public bool ChunkingEnabled { get;  } = chunkingEnabled;

    public int ChunkSize { get;  } = chunkSize;

    public string LoggingLevel { get;  } = loggingLevel;
}