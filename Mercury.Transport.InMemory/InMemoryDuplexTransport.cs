// ***********************************************************************
// Assembly       : Mercury.Transport.InMemory
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-10-2026
// ***********************************************************************
// <copyright file="InMemoryDuplexTransport.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Transport;
using System.Threading.Channels;
using Mercury.Abstractions.Primitives;

namespace Mercury.Transport.InMemory;

/// <summary>
/// Class InMemoryDuplexTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
public sealed class InMemoryDuplexTransport : ITransport
{
    private readonly ChannelReader<byte[]> m_Inbound;
    private readonly ChannelWriter<byte[]> m_Outbound;

    private InMemoryDuplexTransport(
        ChannelReader<byte[]> inbound,
        ChannelWriter<byte[]> outbound)
    {
        m_Inbound = inbound;
        m_Outbound = outbound;
    }

    /// <summary>
    /// Creates the connected pair.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    /// <returns>System.ValueTuple&lt;ITransport, ITransport&gt;.</returns>
    /// <exception cref="ArgumentOutOfRangeException">capacity - Capacity must be greater than zero.</exception>
    public static (ITransport A, ITransport B) CreateConnectedPair(
        int capacity = 128)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity),
                "Capacity must be greater than zero.");
        }

        var alphaToBravo =
            Channel.CreateBounded<byte[]>(
                new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = false,
                    SingleWriter = false
                });

        var bravoToAlpha =
            Channel.CreateBounded<byte[]>(
                new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = false,
                    SingleWriter = false
                });

        ITransport transportA =
            new InMemoryDuplexTransport(
                bravoToAlpha.Reader,
                alphaToBravo.Writer);

        ITransport transportB =
            new InMemoryDuplexTransport(
                alphaToBravo.Reader,
                bravoToAlpha.Writer);

        return (transportA, transportB);
    }

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task SendAsync(
        ReadOnlyMemory frame,
        CancellationToken cancellationToken = default)
    {
        if (frame.IsEmpty)
        {
            throw new ArgumentException(
                "Frame cannot be empty.",
                nameof(frame));
        }

        await m_Outbound
            .WriteAsync(
                frame.ToArray(),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ReadOnlyMemory&gt; representing the asynchronous operation.</returns>
    public async Task<ReadOnlyMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
            var frame = await m_Inbound
            .ReadAsync(cancellationToken)
            .ConfigureAwait(false);

            return new ReadOnlyMemory(frame);
    }
}