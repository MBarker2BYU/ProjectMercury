// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-24-2026
// ***********************************************************************
// <copyright file="CaptureTransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Demo.Console.Wrappers;

/// <summary>
/// Captures the most recently transmitted Mercury frame for display while
/// delegating transport behavior to the configured transport.
/// </summary>
/// <param name="transport">The underlying transport.</param>
internal sealed class CaptureTransport(ITransport transport) : ITransport
{
    /// <summary>
    /// The underlying transport.
    /// </summary>
    private readonly ITransport m_Transport = transport ?? throw new ArgumentNullException(nameof(transport));

    /// <summary>
    /// The last transmitted frame.
    /// </summary>
    private ReadOnlyMemory m_LastFrame = ReadOnlyMemory.Empty;

    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    public bool IsConnected => m_Transport.IsConnected;

    /// <summary>
    /// Gets a value indicating whether a frame has been captured.
    /// </summary>
    public bool HasLastFrame => !m_LastFrame.IsEmpty;

    /// <summary>
    /// Gets a clone of the last captured frame.
    /// </summary>
    public ReadOnlyMemory LastFrame => m_LastFrame.Clone();

    /// <summary>
    /// Captures and sends a complete Mercury frame.
    /// </summary>
    public async Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default)
    {
        m_LastFrame = frame.Clone();

        await m_Transport.SendAsync(frame, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Receives a complete Mercury frame.
    /// </summary>
    public Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default)
        => m_Transport.ReceiveAsync(cancellationToken);
}
