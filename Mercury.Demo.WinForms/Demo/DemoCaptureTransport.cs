// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-19-2026
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Demo.WinForms.Demo;

internal sealed class DemoCaptureTransport(ITransport transport) : ITransport
{
    private readonly ITransport m_Transport = transport ?? throw new ArgumentNullException(nameof(transport));
    private ReadOnlyMemory m_LastFrame = ReadOnlyMemory.Empty;

    public bool IsConnected => m_Transport.IsConnected;

    public bool HasLastFrame => !m_LastFrame.IsEmpty;

    public ReadOnlyMemory LastFrame => m_LastFrame.Clone();

    public async Task SendAsync(ReadOnlyMemory frame, CancellationToken cancellationToken = default)
    {
        m_LastFrame = frame.Clone();

        await m_Transport
            .SendAsync(frame, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task<ReadOnlyMemory> ReceiveAsync(CancellationToken cancellationToken = default)
        => m_Transport.ReceiveAsync(cancellationToken);

    public Task ReplayLastFrameAsync(CancellationToken cancellationToken = default)
        => SendModifiedFrameAsync(static frame => frame, cancellationToken);

    public Task SendProtectedPayloadTamperedFrameAsync(CancellationToken cancellationToken = default)
        => SendModifiedFrameAsync(static frame 
            => TamperRegion(frame, FrameRegion.Payload), cancellationToken);

    private Task SendModifiedFrameAsync(Func<byte[], byte[]> modification, CancellationToken cancellationToken)
    {
        if (m_LastFrame.IsEmpty)
            throw new InvalidOperationException("No protected frame is available.");

        var frame = modification(m_LastFrame.ToArray());

        return m_Transport.SendAsync(new ReadOnlyMemory(frame), cancellationToken);
    }

    private static byte[] TamperRegion(byte[] frame, FrameRegion region)
    {
        if (frame.Length == 0)
            throw new InvalidOperationException("The captured frame is empty.");

        var index = region switch
        {
            FrameRegion.Payload => Math.Min(frame.Length - 1, Math.Max(0, frame.Length / 2)),
            _ => throw new InvalidOperationException("Unsupported frame region.")
        };

        frame[index] ^= 0x01;

        return frame;
    }

    private enum FrameRegion
    {
        Payload
    }
}
