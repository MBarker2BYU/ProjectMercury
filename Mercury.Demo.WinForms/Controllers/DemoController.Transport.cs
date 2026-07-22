// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.Transport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Transport;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Proxy;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;
using System.Net;
using System.Net.Sockets;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// The m alpha capture transport
    /// </summary>
    private DemoCaptureTransport? m_AlphaCaptureTransport;
    /// <summary>
    /// The m alpha transport
    /// </summary>
    private ITransport? m_AlphaTransport;
    /// <summary>
    /// The m bravo transport
    /// </summary>
    private ITransport? m_BravoTransport;

    /// <summary>
    /// Build transport as an asynchronous operation.
    /// </summary>
    /// <param name="transportName">Name of the transport.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">The selected transport is not supported.</exception>
    private async Task<(ITransport Alpha, ITransport Bravo)> BuildTransportAsync(string transportName, CancellationToken cancellationToken)
    {
        switch (transportName)
        {
            case DemoConstants.IN_MEMORY_TRANSPORT:
                return InMemoryDuplexTransport
                    .CreateConnectedPair();

            case DemoConstants.TCP_TRANSPORT:
                return await BuildTcpTransportAsync(cancellationToken)
                    .ConfigureAwait(false);

            default:
                throw new InvalidOperationException("The selected transport is not supported.");
        }
    }

    /// <summary>
    /// Build TCP transport as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    private async Task<(ITransport Alpha, ITransport Bravo)> BuildTcpTransportAsync(CancellationToken cancellationToken)
    {
        TcpListener? realListener = null;

        try
        {
            realListener =
                new TcpListener(IPAddress.Loopback, 0);

            realListener.Start();

            var realPort = ((IPEndPoint)realListener.LocalEndpoint).Port;

            var proxyPort = ReserveTcpPort();

            m_AttackSimulator = new TcpAttackSimulator(proxyPort, realPort);

            await m_AttackSimulator
                .StartAsync()
                .ConfigureAwait(false);

            var bravoTask =
                TcpTransport.AcceptAsync(realListener, cancellationToken);

            var alphaTask =
                TcpTransport.ConnectAsync(IPAddress.Loopback.ToString(),
                    proxyPort, cancellationToken);

            await Task
                .WhenAll(alphaTask, bravoTask)
                .ConfigureAwait(false);

            return (await alphaTask.ConfigureAwait(false), await bravoTask.ConfigureAwait(false));
        }
        catch
        {
            await DisposeAttackSimulatorAsync()
                .ConfigureAwait(false);

            throw;
        }
        finally
        {
            realListener?.Stop();
        }
    }

    /// <summary>
    /// Reserves the TCP port.
    /// </summary>
    /// <returns>System.Int32.</returns>
    private static int ReserveTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);

        try
        {
            listener.Start();

            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }

    /// <summary>
    /// Dispose transports as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task DisposeTransportsAsync()
    {
        await DisposeAttackSimulatorAsync()
            .ConfigureAwait(false);

        await DisposeTransportAsync(
                m_AlphaTransport)
            .ConfigureAwait(false);

        await DisposeTransportAsync(
                m_BravoTransport)
            .ConfigureAwait(false);

        m_AlphaClient = null;
        m_BravoClient = null;

        m_AlphaCaptureTransport = null;
        m_AlphaTransport = null;
        m_BravoTransport = null;
    }

    /// <summary>
    /// Dispose attack simulator as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task DisposeAttackSimulatorAsync()
    {
        var attackSimulator = m_AttackSimulator;

        m_AttackSimulator = null;

        if (attackSimulator is null)
            return;

        await attackSimulator
            .DisposeAsync()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Dispose transport as an asynchronous operation.
    /// </summary>
    /// <param name="transport">The transport.</param>
    /// <returns>A Task&lt;ValueTask&gt; representing the asynchronous operation.</returns>
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
}