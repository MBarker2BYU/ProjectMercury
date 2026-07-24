// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-24-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="DemoController.Transport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Transport;
using Mercury.Demo.Console.Proxy;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;
using System.Net;
using System.Net.Sockets;

namespace Mercury.Demo.Console;

/// <summary>
/// Coordinates the Mercury RC 1.2 console demonstration application.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Builds the configured transport pair.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The connected Alpha and Bravo transports.</returns>
    private async Task<(ITransport Alpha, ITransport Bravo)> BuildTransportAsync(CancellationToken cancellationToken = default)
    {
        return m_Configuration.Transport switch
        {
            IN_MEMORY_TRANSPORT => InMemoryDuplexTransport.CreateConnectedPair(),
            TCP_TRANSPORT => await BuildTcpTransportAsync(cancellationToken).ConfigureAwait(false),
            _ => throw new ArgumentException("The selected transport is not supported.")
        };
    }

    /// <summary>
    /// Builds a TCP transport pair routed through the replay and tamper simulator.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The connected Alpha and Bravo transports.</returns>
    private async Task<(ITransport Alpha, ITransport Bravo)> BuildTcpTransportAsync(CancellationToken cancellationToken)
    {
        TcpListener? realListener = null;

        try
        {
            realListener = new TcpListener(IPAddress.Loopback, 0);
            realListener.Start();

            var realPort = ((IPEndPoint)realListener.LocalEndpoint).Port;
            var proxyPort = ReserveTcpPort();

            m_AttackSimulator = new TcpAttackSimulator(proxyPort, realPort);
            await m_AttackSimulator.StartAsync().ConfigureAwait(false);

            var bravoTask = TcpTransport.AcceptAsync(realListener, cancellationToken);
            var alphaTask = TcpTransport.ConnectAsync(IPAddress.Loopback.ToString(), proxyPort, cancellationToken);

            await Task.WhenAll(alphaTask, bravoTask).ConfigureAwait(false);

            return (await alphaTask.ConfigureAwait(false), await bravoTask.ConfigureAwait(false));
        }
        catch
        {
            await DisposeAttackSimulatorAsync().ConfigureAwait(false);
            throw;
        }
        finally
        {
            realListener?.Stop();
        }
    }

    /// <summary>
    /// Reserves an available loopback TCP port.
    /// </summary>
    /// <returns>The reserved port number.</returns>
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
    /// Disposes the active clients, transports, and TCP attack simulator.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DisposeTransportsAsync()
    {
        await DisposeAttackSimulatorAsync().ConfigureAwait(false);
        await DisposeTransportAsync(m_AlphaTransport).ConfigureAwait(false);
        await DisposeTransportAsync(m_BravoTransport).ConfigureAwait(false);

        m_AlphaClient = null;
        m_BravoClient = null;
        m_AlphaCaptureTransport = null;
        m_AlphaTransport = null;
        m_BravoTransport = null;
    }

    /// <summary>
    /// Disposes the TCP attack simulator.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DisposeAttackSimulatorAsync()
    {
        var attackSimulator = m_AttackSimulator;
        m_AttackSimulator = null;

        if (attackSimulator is null)
            return;

        await attackSimulator.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Disposes one transport when it supports disposal.
    /// </summary>
    /// <param name="transport">The transport.</param>
    /// <returns>A value task representing the asynchronous operation.</returns>
    private static async ValueTask DisposeTransportAsync(ITransport? transport)
    {
        if (transport is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            return;
        }

        if (transport is IDisposable disposable)
            disposable.Dispose();
    }
}
