using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Demo.WinForms.Wrappers;

internal sealed class DemoCaptureTransport(
    ITransport transport) : ITransport
{
    private readonly ITransport m_Transport =
        transport ?? throw new ArgumentNullException(nameof(transport));

    private ReadOnlyMemory m_LastFrame = ReadOnlyMemory.Empty;

    public bool IsConnected => m_Transport.IsConnected;

    public bool HasLastFrame =>
        !m_LastFrame.IsEmpty;

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
    {
        if (m_LastFrame.IsEmpty)
        {
            throw new InvalidOperationException("No protected frame is available to replay.");
        }

        return m_Transport.SendAsync(m_LastFrame, cancellationToken);
    }
}