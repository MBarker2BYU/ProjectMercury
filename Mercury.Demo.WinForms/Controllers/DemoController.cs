// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-19-2026
// ***********************************************************************

using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Services;

namespace Mercury.Demo.WinForms.Controllers;

internal sealed class DemoController(Action<DemoLogEntry> log) : IAsyncDisposable
{
    private readonly MercuryDemoSession m_Session = new(log);
    private readonly CancellationTokenSource m_CancellationTokenSource = new();

    public static IReadOnlyList<string> CryptoProviders =>
    [
        DemoConstants.AES_GCM,
        DemoConstants.CHA_CHA_20
    ];

    public static IReadOnlyList<string> Transports =>
    [
        DemoConstants.IN_MEMORY_TRANSPORT,
        DemoConstants.TCP_TRANSPORT
    ];

    public static IReadOnlyList<string> EnvelopeCodecs =>
    [
        DemoConstants.BINARY_CODEC,
        DemoConstants.JSON_CODEC
    ];

    public static IReadOnlyList<string> LoggingLevels =>
    [
        DemoConstants.QUIET_LOGGING,
        DemoConstants.NORMAL_LOGGING,
        DemoConstants.VERBOSE_LOGGING
    ];


    public async Task<(DemoConfiguration Configuration, bool IsConnected)> ConfigureAsync(DemoConfiguration configuration)
    {

        ValidateConfiguration(configuration);

        await m_Session
            .ConfigureAsync(configuration, m_CancellationTokenSource.Token)
            .ConfigureAwait(true);

        return (
            m_Session.Configuration,
            m_Session.IsConnected);
    }

    public static IReadOnlyList<int> ChunkSizes => BuildChunkSizes();

    private static IReadOnlyList<int> BuildChunkSizes()
    {
        var chunkSizes = new List<int>();

        for (var sizeKilobytes = 1;
             sizeKilobytes <= 1024;
             sizeKilobytes *= 2)
        {
            chunkSizes.Add(sizeKilobytes * 1024);
        }

        return chunkSizes;
    }

    private static void ValidateConfiguration(
        DemoConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.CryptoProvider))
            throw new InvalidOperationException("Select a crypto provider.");

        if (string.IsNullOrWhiteSpace(configuration.Transport))
            throw new InvalidOperationException("Select a transport.");

        if (string.IsNullOrWhiteSpace(configuration.EnvelopeCodec))
            throw new InvalidOperationException("Select an envelope codec.");

        if (configuration is { ChunkingEnabled: true, ChunkSize: < 1024 })
        {
            throw new InvalidOperationException("Select a valid chunk size.");
        }
    }

    #region IAsyncDisposable Implementation

    public async ValueTask DisposeAsync()
    {
        if (!m_CancellationTokenSource.IsCancellationRequested)
            await m_CancellationTokenSource.CancelAsync();

        await m_Session.DisposeAsync().ConfigureAwait(false);

        m_CancellationTokenSource.Dispose();
    }

    #endregion
}