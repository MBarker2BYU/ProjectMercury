
// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.Builders.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Abstractions.Transport;
using Mercury.Core.Factories;
using Mercury.Demo.Console.Wrappers;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Apply configuration as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task ApplyConfigurationAsync()
    {
        var keys = new Dictionary<KeyId, byte[]>
        {
            [ALPHA_NODE] = RandomNumberGenerator.GetBytes(32),
            [BRAVO_NODE] = RandomNumberGenerator.GetBytes(32)
        };

        var keyProvider = new SymmetricKeyProviderDictionary(keys);
        var alphaCryptoProvider = BuildCryptoProvider(keyProvider);
        var bravoCryptoProvider = BuildCryptoProvider(keyProvider);
        var (alphaTransport, bravoTransport) = await BuildTransportAsync();
        var envelopeCodec = BuildEnvelopeCodec();

        m_AlphaCaptureTransport = new CaptureTransport(alphaTransport);

        var alphaDependencies = MercuryFactory.Instance.BuildDependencies(
            alphaCryptoProvider,
            envelopeCodec,
            m_AlphaCaptureTransport);

        var bravoDependencies = MercuryFactory.Instance.BuildDependencies(
            bravoCryptoProvider,
            envelopeCodec,
            bravoTransport);

        m_AlphaClient = MercuryFactory.Instance.BuildClient(alphaDependencies);
        m_BravoClient = MercuryFactory.Instance.BuildClient(bravoDependencies);
    }

    /// <summary>
    /// Builds the crypto provider.
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <returns>ICryptoProvider.</returns>
    private ICryptoProvider BuildCryptoProvider(SymmetricKeyProviderDictionary keys)
    {
        return m_Configuration.CryptoProvider switch
        {
            AES_GCM => new AesGcmCryptoProvider(keys),
            CHA_CHA_20 => new ChaCha20CryptoProvider(keys),
            _ => throw new ArgumentException("The selected crypto provider is not supported.")
        };
    }

    /// <summary>
    /// Build transport as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">The selected transport is not supported.</exception>
    private async Task<(ITransport alphaTransport, ITransport bravoTransport)> BuildTransportAsync(
        CancellationToken cancellationToken = default)
    {
        switch (m_Configuration.Transport)
        {
            case IN_MEMORY_TRANSPORT:
                return InMemoryDuplexTransport.CreateConnectedPair();

            case TCP_TRANSPORT:
            {
                var listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();

                try
                {
                    var localEndpoint = (IPEndPoint)listener.LocalEndpoint;

                    var bravoTransportTask = TcpTransport.AcceptAsync(listener, cancellationToken);
                    var alphaTransportTask = TcpTransport.ConnectAsync(IPAddress.Loopback.ToString(),
                        localEndpoint.Port, cancellationToken);

                    await Task.WhenAll(alphaTransportTask, bravoTransportTask).ConfigureAwait(false);

                    return (
                        await alphaTransportTask.ConfigureAwait(false),
                        await bravoTransportTask.ConfigureAwait(false));
                }
                finally
                {
                    listener.Stop();
                }
            }

            default:
                throw new ArgumentException("The selected transport is not supported.");
        }
    }

    /// <summary>
    /// Builds the envelope codec.
    /// </summary>
    /// <returns>EnvelopeCodec.</returns>
    private EnvelopeCodec BuildEnvelopeCodec()
    {
        return m_Configuration.EnvelopeCodec switch
        {
            BINARY_CODEC => EnvelopeCodec.Binary,
            JSON_CODEC => EnvelopeCodec.Json,
            _ => throw new ArgumentException("The selected envelope codec is not supported.")
        };
    }
}
