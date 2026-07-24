// ***********************************************************************
// Assembly     : Mercury.Transport.InMemory
// Author         : Matthew D. Barker
// Created        : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-23-2026
// ***********************************************************************
// <copyright file="InMemoryDuplexTransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Threading.Channels;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Transport.InMemory;

/// <summary>
/// Class InMemoryDuplexTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />.
/// </summary>
/// <seealso cref="ITransport" />
public sealed class InMemoryDuplexTransport : ITransport
{
    /// <summary>
    /// The maximum permitted frame size.
    /// </summary>
    private const int MAX_FRAME_BYTES =
        8 * 1024 * 1024;

    private readonly ChannelReader<byte[]> m_Inbound;
    private readonly ChannelWriter<byte[]> m_Outbound;

    private InMemoryDuplexTransport(ChannelReader<byte[]> inbound, ChannelWriter<byte[]> outbound)
    {
        m_Inbound = inbound;
        m_Outbound = outbound;
    }

    /// <summary>
    /// Creates the connected pair.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    /// <returns>
    /// System.ValueTuple&lt;ITransport, ITransport&gt;.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Capacity must be greater than zero.
    /// </exception>
    public static (ITransport A, ITransport B) CreateConnectedPair(int capacity = 128)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        

        var alphaToBravo = Channel.CreateBounded<byte[]>(new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = false,
                    SingleWriter = false
                });

        var bravoToAlpha =
            Channel.CreateBounded<byte[]>(new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = false,
                    SingleWriter = false
                });

        ITransport transportA = new InMemoryDuplexTransport(bravoToAlpha.Reader, alphaToBravo.Writer);

        ITransport transportB = new InMemoryDuplexTransport(alphaToBravo.Reader, bravoToAlpha.Writer);

        return (transportA, transportB);
    }

    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    /// <value>
    /// <c>true</c> if the transport is connected; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool IsConnected => true;

    /// <summary>
    /// Sends one complete frame.
    /// </summary>
    /// <param name="frame">The protected frame.</param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The frame is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The frame exceeds the maximum permitted size.
    /// </exception>
    public async Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (frame.IsEmpty)
            throw new ArgumentException("Frame cannot be empty.", nameof(frame));
        
        if (frame.Length > MAX_FRAME_BYTES)
            throw new InvalidOperationException($"Frame is too large: {frame.Length} bytes.");
        

        await m_Outbound
            .WriteAsync(frame.ToArray(), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives one complete frame.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the received frame.
    /// </returns>
    public async Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        var frame = await m_Inbound
                .ReadAsync(cancellationToken)
                .ConfigureAwait(false);

        return new ReadOnlyMemory(frame);
    }
}