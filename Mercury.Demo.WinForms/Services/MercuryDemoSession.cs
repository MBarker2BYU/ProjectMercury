// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-19-2026
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Abstractions.Transport;
using Mercury.Core.Factories;
using Mercury.Demo.WinForms.Demo;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using Mercury.Demo.WinForms.Proxy;

namespace Mercury.Demo.WinForms.Services;

internal sealed class MercuryDemoSession(Action<DemoLogEntry> log) : IAsyncDisposable
{
    private readonly SemaphoreSlim m_OperationLock = new(1, 1);

    private DemoConfiguration m_Configuration;
    private IMercuryClient? m_AlphaClient;
    private IMercuryClient? m_BravoClient;
    private DemoCaptureTransport? m_AlphaCaptureTransport;
    private ITransport? m_AlphaTransport;
    private ITransport? m_BravoTransport;

    public DemoConfiguration Configuration => m_Configuration;

    public static bool WrongKeyEnabled { get; set; } = true;

    public bool IsConnected =>
        m_AlphaCaptureTransport?.IsConnected == true && m_BravoTransport?.IsConnected == true;

    public async Task ConfigureAsync(DemoConfiguration configuration, CancellationToken cancellationToken = default)
    {
        await m_OperationLock
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            await DisposeTransportsAsync().ConfigureAwait(false);

            m_Configuration = configuration;

            var keys = new Dictionary<KeyId, byte[]>
            {
                [DemoConstants.ALPHA_NODE] =
                    RandomNumberGenerator.GetBytes(32),

                [DemoConstants.BRAVO_NODE] =
                    RandomNumberGenerator.GetBytes(32),

                [DemoConstants.CHARLIE_NODE] =
                    RandomNumberGenerator.GetBytes(32)
            };

            var keyProvider =
                new SymmetricKeyProviderDictionary(keys);

            var alphaCryptoProvider = BuildCryptoProvider(keyProvider, configuration.CryptoProvider);

            var bravoCryptoProvider = BuildCryptoProvider(keyProvider, configuration.CryptoProvider);

            var transports = await BuildTransportAsync(configuration.Transport, cancellationToken)
                .ConfigureAwait(false);

            m_AlphaTransport = transports.Alpha;
            m_BravoTransport = transports.Bravo;

            m_AlphaCaptureTransport = new DemoCaptureTransport(m_AlphaTransport);

            var envelopeCodec = BuildEnvelopeCodec(configuration.EnvelopeCodec);

            var alphaDependencies = MercuryFactory.Instance.BuildDependencies(keys[DemoConstants.ALPHA_NODE], alphaCryptoProvider, envelopeCodec, m_AlphaCaptureTransport);

            var bravoDependencies = MercuryFactory.Instance.BuildDependencies(keys[DemoConstants.BRAVO_NODE], bravoCryptoProvider, envelopeCodec, m_BravoTransport);

            m_AlphaClient =
                MercuryFactory.Instance.BuildClient(alphaDependencies);

            m_BravoClient =
                MercuryFactory.Instance.BuildClient(bravoDependencies);

            Log("INFO", $"Provider {configuration.CryptoProvider} loaded");

            Log("INFO", $"Transport {configuration.Transport} connected");

            Log("INFO", $"Envelope codec {configuration.EnvelopeCodec} active");

            Log("INFO", configuration.ChunkingEnabled
                    ? $"Chunking enabled at {FormatByteSize(configuration.ChunkSize)}"
                    : "Chunking disabled");
        }
        finally
        {
            m_OperationLock.Release();
        }
    }

    private static ICryptoProvider BuildCryptoProvider(
        SymmetricKeyProviderDictionary keys,
        string providerName)
    {
        return providerName switch
        {
            DemoConstants.AES_GCM =>
                new AesGcmCryptoProvider(keys),

            DemoConstants.CHA_CHA_20 =>
                new ChaCha20CryptoProvider(keys),

            _ => throw new InvalidOperationException(
                "The selected crypto provider is not supported.")
        };
    }

    private static EnvelopeCodec BuildEnvelopeCodec(string codecName)
    {
        return codecName switch
        {
            DemoConstants.BINARY_CODEC =>
                EnvelopeCodec.Binary,

            DemoConstants.JSON_CODEC =>
                EnvelopeCodec.Json,

            _ => throw new InvalidOperationException(
                "The selected envelope codec is not supported.")
        };
    }

    private static async Task<(ITransport Alpha, ITransport Bravo)> BuildTransportAsync(string transportName,
            CancellationToken cancellationToken)
    {
        switch (transportName)
        {
            case DemoConstants.IN_MEMORY_TRANSPORT:
                return InMemoryDuplexTransport.CreateConnectedPair();

            case DemoConstants.TCP_TRANSPORT:
                {
                    TcpListener? realListener = null;

                    if (TcpAttackSimulatorProxy != null)
                        await TcpAttackSimulatorProxy.DisposeAsync();

                    TcpAttackSimulatorProxy = null;

                    try
                    {
                        //var endpoint =
                        //    (IPEndPoint)listener.LocalEndpoint;

                        realListener = new TcpListener(IPAddress.Loopback, 0);
                        realListener.Start();
                        var realPort = ((IPEndPoint)realListener.LocalEndpoint).Port;

                        var proxyListener = new TcpListener(IPAddress.Loopback, 0);
                        proxyListener.Start();
                        var proxyPort = ((IPEndPoint)proxyListener.LocalEndpoint).Port;
                        proxyListener.Stop();

                        TcpAttackSimulatorProxy = new TcpAttackSimulator(proxyPort, realPort);
                        await TcpAttackSimulatorProxy.StartAsync();

                        //TcpAttackSimulatorProxy.ReplayEnabled = true;

                        var bravoTask =
                            TcpTransport.AcceptAsync(realListener, cancellationToken);

                        var alphaTask =
                            TcpTransport.ConnectAsync(IPAddress.Loopback.ToString(),
                                proxyPort, cancellationToken);

                        await Task
                            .WhenAll(alphaTask, bravoTask)
                            .ConfigureAwait(false);

                        return (
                            await alphaTask.ConfigureAwait(false),
                            await bravoTask.ConfigureAwait(false));
                    }
                    finally
                    {
                        realListener?.Stop();
                    }
                }

            default:
                throw new InvalidOperationException(
                    "The selected transport is not supported.");
        }
    }

    private void Log(string level, string entry)
    {
        log(new DemoLogEntry(level, entry));
    }

    private static string FormatByteSize(int bytes)
    {
        if (bytes >= 1024 * 1024)
            return $"{bytes / (1024 * 1024):N0} MB";

        return $"{bytes / 1024:N0} KB";
    }

    public static TcpAttackSimulator? TcpAttackSimulatorProxy { get; private set; }

    #region IAsyncDisposable Implementation

    public async ValueTask DisposeAsync()
    {
        await m_OperationLock.WaitAsync().ConfigureAwait(false);

        try
        {
            await DisposeTransportsAsync().ConfigureAwait(false);
        }
        finally
        {
            m_OperationLock.Release();
        }

        m_OperationLock.Dispose();
    }

    private async Task DisposeTransportsAsync()
    {
        await DisposeTransportAsync(m_AlphaTransport)
            .ConfigureAwait(false);

        await DisposeTransportAsync(m_BravoTransport)
            .ConfigureAwait(false);

        m_AlphaClient = null;
        m_BravoClient = null;
        m_AlphaCaptureTransport = null;
        m_AlphaTransport = null;
        m_BravoTransport = null;
    }

    public async Task<IMercuryResult> SendAsync(ReadOnlyMemory payload, CancellationToken cancellationToken = default)
    {
        if (payload.IsEmpty)
            throw new ArgumentException(
                "Payload must not be empty.",
                nameof(payload));

        await m_OperationLock
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            if (m_AlphaClient == null || m_BravoClient == null)
            {
                throw new InvalidOperationException(
                    "Mercury has not been configured.");
            }

            //var context =
            //    MercuryFactory.Instance.BuildCryptoContext(
            //        DemoConstants.ALPHA_NODE,
            //        DemoConstants.BRAVO_NODE);

            ICryptoContext context;

            if (WrongKeyEnabled)
            {
                context = MercuryFactory.Instance.BuildCryptoContext(
                    DemoConstants.ALPHA_NODE,
                    DemoConstants.CHARLIE_NODE);   // ← this is the important one

                Log("WARN", "Sending with WRONG KEY");
            }
            else
            {
                context = MercuryFactory.Instance.BuildCryptoContext(
                    DemoConstants.ALPHA_NODE,
                    DemoConstants.BRAVO_NODE);
            }

            Log(
                "INFO",
                $"Sending {payload.Length:N0} payload bytes");

            var receiveTask =
                m_BravoClient.ReceiveAsync(
                    cancellationToken);

            var sendTask =
                m_AlphaClient.SendAsync(context, payload, cancellationToken);

            await Task
                .WhenAll(receiveTask, sendTask)
                .ConfigureAwait(false);

            var result =
                await receiveTask.ConfigureAwait(false);

            Log(
                result.Success ? "INFO" : "ERROR",
                result.Success
                    ? "Payload received and authenticated"
                    : result.Message ??
                      result.FailureReason.ToString());

            return result;
        }
        finally
        {
            m_OperationLock.Release();
        }
    }

    private static async ValueTask DisposeTransportAsync(ITransport? transport)
    {
        if (transport is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable
                .DisposeAsync()
                .ConfigureAwait(false);

            return;
        }

        if (transport is IDisposable disposable)
            disposable.Dispose();
    }

    #endregion
}