// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.Configuration.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Core.Factories;
using Mercury.Demo.WinForms.Demo;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using System.Security.Cryptography;

namespace Mercury.Demo.WinForms.Controllers;

internal sealed partial class DemoController
{
    private DemoConfiguration m_Configuration;

    public DemoConfiguration Configuration =>
        m_Configuration;

    public bool IsConnected =>
        m_AlphaCaptureTransport?.IsConnected == true &&
        m_BravoTransport?.IsConnected == true;

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

    public static IReadOnlyList<int> ChunkSizes =>
        BuildChunkSizes();

    public async Task<(
        DemoConfiguration Configuration,
        bool IsConnected)> ConfigureAsync(
            DemoConfiguration configuration)
    {
        ValidateConfiguration(configuration);

        var cancellationToken =
            m_CancellationTokenSource.Token;

        await m_OperationLock
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            await DisposeTransportsAsync()
                .ConfigureAwait(false);

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

            var alphaCryptoProvider =
                BuildCryptoProvider(
                    keyProvider,
                    configuration.CryptoProvider);

            var bravoCryptoProvider =
                BuildCryptoProvider(
                    keyProvider,
                    configuration.CryptoProvider);

            var transports =
                await BuildTransportAsync(
                        configuration.Transport,
                        cancellationToken)
                    .ConfigureAwait(false);

            m_AlphaTransport = transports.Alpha;
            m_BravoTransport = transports.Bravo;

            m_AlphaCaptureTransport =
                new DemoCaptureTransport(
                    m_AlphaTransport);

            var envelopeCodec =
                BuildEnvelopeCodec(
                    configuration.EnvelopeCodec);

            var alphaDependencies =
                MercuryFactory.Instance.BuildDependencies(
                    DemoConstants.ALPHA_NODE,
                    alphaCryptoProvider,
                    envelopeCodec,
                    m_AlphaCaptureTransport);

            var bravoDependencies =
                MercuryFactory.Instance.BuildDependencies(
                    DemoConstants.BRAVO_NODE,
                    bravoCryptoProvider,
                    envelopeCodec,
                    m_BravoTransport);

            m_AlphaClient =
                MercuryFactory.Instance.BuildClient(
                    alphaDependencies);

            m_BravoClient =
                MercuryFactory.Instance.BuildClient(
                    bravoDependencies);

            Log(
                "INFO",
                $"Provider {configuration.CryptoProvider} loaded");

            Log(
                "INFO",
                $"Transport {configuration.Transport} connected");

            Log(
                "INFO",
                $"Envelope codec {configuration.EnvelopeCodec} active");

            Log(
                "INFO",
                configuration.ChunkingEnabled
                    ? $"Chunking enabled at " +
                      $"{FormatByteSize(configuration.ChunkSize)}"
                    : "Chunking disabled");
        }
        finally
        {
            m_OperationLock.Release();
        }

        return (
            m_Configuration,
            IsConnected);
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

    private static EnvelopeCodec BuildEnvelopeCodec(
        string codecName)
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

    private static IReadOnlyList<int> BuildChunkSizes()
    {
        var chunkSizes = new List<int>();

        for (
            var sizeKilobytes = 1;
            sizeKilobytes <= 1024;
            sizeKilobytes *= 2)
        {
            chunkSizes.Add(
                sizeKilobytes * 1024);
        }

        return chunkSizes;
    }

    private static void ValidateConfiguration(
        DemoConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(
               configuration.CryptoProvider))
        {
            throw new InvalidOperationException(
                "Select a crypto provider.");
        }

        if (string.IsNullOrWhiteSpace(
               configuration.Transport))
        {
            throw new InvalidOperationException(
                "Select a transport.");
        }

        if (string.IsNullOrWhiteSpace(
               configuration.EnvelopeCodec))
        {
            throw new InvalidOperationException(
                "Select an envelope codec.");
        }

        if (configuration is
            {
                ChunkingEnabled: true,
                ChunkSize: < 1024
            })
        {
            throw new InvalidOperationException(
                "Select a valid chunk size.");
        }
    }

    private static string FormatByteSize(
        int bytes)
    {
        if (bytes >= 1024 * 1024)
        {
            return
                $"{bytes / (1024 * 1024):N0} MB";
        }

        return
            $"{bytes / 1024:N0} KB";
    }
}