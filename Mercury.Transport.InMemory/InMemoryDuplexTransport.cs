// ***********************************************************************
// Assembly       : Mercury.Transport.InMemory
// Author           : Matthew D. Barker
// Created          : 07-03-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-08-2026
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
/// Duplex in-memory transport. Create a connected pair for client/server demo.
/// </summary>
public sealed class InMemoryDuplexTransport : ITransport
{

    /// <summary>
    /// The inbound backing variable
    /// </summary>
    private readonly ChannelReader<byte[]> m_Inbound;

    /// <summary>
    /// The outbound backing variable
    /// </summary>
    private readonly ChannelWriter<byte[]> m_Outbound;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryDuplexTransport"/> class.
    /// </summary>
    /// <param name="inbound">The inbound.</param>
    /// <param name="outbound">The outbound.</param>
    private InMemoryDuplexTransport(ChannelReader<byte[]> inbound, ChannelWriter<byte[]> outbound)
    {
        m_Inbound = inbound;
        m_Outbound = outbound;
    }

    /// <summary>
    /// Creates a connected pair of transports.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    /// <returns>System.ValueTuple&lt;ITransport, ITransport&gt;.</returns>
    public static (ITransport A, ITransport B) CreateConnectedPair(int capacity = 128)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };

        var aToB = Channel.CreateBounded<byte[]>(options);
        var bToA = Channel.CreateBounded<byte[]>(options);

        var a = new InMemoryDuplexTransport(bToA.Reader, aToB.Writer);
        var b = new InMemoryDuplexTransport(aToB.Reader, bToA.Writer);

        return (a, b);
    }

    /// <summary>
    /// Send as an asynchronous operation.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Frame must not be empty. - frame</exception>
    public async Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default)
    {
        if (frame.IsEmpty)
            throw new ArgumentException("Frame must not be empty.", nameof(frame));

        var copy = frame.ToArray(); // defensive copy
        await m_Outbound.WriteAsync(copy, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Receive as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ReadOnlyMemory&gt; representing the asynchronous operation.</returns>
    public async Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        var data = await m_Inbound.ReadAsync(cancellationToken).ConfigureAwait(false);
        return new ReadOnlyMemory(data);
    }
}