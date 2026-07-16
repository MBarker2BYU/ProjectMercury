// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
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
/// Class CaptureTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <param name="transport">The transport.</param>
/// <seealso cref="ITransport" />
internal sealed class CaptureTransport(ITransport transport) : ITransport
{
    /// <summary>
    /// The m transport
    /// </summary>
    private readonly ITransport m_Transport =
        transport ?? throw new ArgumentNullException(nameof(transport));

    /// <summary>
    /// The m last frame
    /// </summary>
    private ReadOnlyMemory m_LastFrame = ReadOnlyMemory.Empty;

    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    /// <value><c>true</c> if the transport is connected; otherwise, <c>false</c>.</value>
    public bool IsConnected => m_Transport.IsConnected;

    /// <summary>
    /// Gets a value indicating whether this instance has last frame.
    /// </summary>
    /// <value><c>true</c> if this instance has last frame; otherwise, <c>false</c>.</value>
    public bool HasLastFrame => !m_LastFrame.IsEmpty;

    /// <summary>
    /// Gets the last frame.
    /// </summary>
    /// <value>The last frame.</value>
    public ReadOnlyMemory LastFrame => m_LastFrame.Clone();

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task SendAsync(
        ReadOnlyMemory frame,
        CancellationToken cancellationToken = default)
    {
        m_LastFrame = frame.Clone();

        await m_Transport
            .SendAsync(frame, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    public Task<ReadOnlyMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
        => m_Transport.ReceiveAsync(cancellationToken);

    /// <summary>
    /// Replays the last frame asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task ReplayLastFrameAsync(
        CancellationToken cancellationToken = default)
        => SendModifiedFrameAsync(
            static frame => frame,
            cancellationToken);

    /// <summary>
    /// Sends the replay metadata tampered frame asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendReplayMetadataTamperedFrameAsync(
        CancellationToken cancellationToken = default)
        => SendModifiedFrameAsync(
            static frame => TamperRegion(frame, FrameRegion.Header),
            cancellationToken);

    /// <summary>
    /// Sends the authentication tag tampered frame asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendAuthenticationTagTamperedFrameAsync(
        CancellationToken cancellationToken = default)
        => SendModifiedFrameAsync(
            static frame => TamperRegion(frame, FrameRegion.Footer),
            cancellationToken);

    /// <summary>
    /// Sends the protected payload tampered frame asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendProtectedPayloadTamperedFrameAsync(
        CancellationToken cancellationToken = default)
        => SendModifiedFrameAsync(
            static frame => TamperRegion(frame, FrameRegion.Payload),
            cancellationToken);

    /// <summary>
    /// Sends the modified frame asynchronous.
    /// </summary>
    /// <param name="modification">The modification.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="InvalidOperationException">No protected frame is available.</exception>
    private Task SendModifiedFrameAsync(Func<byte[], byte[]> modification, CancellationToken cancellationToken)
    {
        if (m_LastFrame.IsEmpty)
        {
            throw new InvalidOperationException("No protected frame is available.");
        }

        var frame = modification(m_LastFrame.ToArray());

        return m_Transport.SendAsync(new ReadOnlyMemory(frame), cancellationToken);
    }

    /// <summary>
    /// Tampers the region.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="region">The region.</param>
    /// <returns>System.Byte[].</returns>
    /// <exception cref="InvalidOperationException">The captured frame is empty.</exception>
    private static byte[] TamperRegion(byte[] frame, FrameRegion region)
    {
        if (frame.Length == 0)
            throw new InvalidOperationException("The captured frame is empty.");

        var index = region switch
        {
            FrameRegion.Header => Math.Min(frame.Length - 1, Math.Max(0, frame.Length / 8)),
            FrameRegion.Payload => Math.Min(frame.Length - 1, Math.Max(0, frame.Length / 2)),
            FrameRegion.Footer => frame.Length - 1,
            _ => throw new InvalidOperationException("Unsupported frame region.")
        };

        frame[index] ^= 0x01;

        return frame;
    }

    /// <summary>
    /// Enum FrameRegion
    /// </summary>
    private enum FrameRegion
    {
        /// <summary>
        /// The header
        /// </summary>
        Header,
        /// <summary>
        /// The payload
        /// </summary>
        Payload,
        /// <summary>
        /// The footer
        /// </summary>
        Footer
    }
}
