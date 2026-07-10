using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Transport;
using System.Threading.Channels;

namespace Mercury.Transport.InMemory;

/// <summary>
/// Class InMemoryDuplexTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
public sealed class InMemoryDuplexTransport : ITransport
{
    private readonly ChannelReader<ISecureEnvelope> m_Inbound;
    private readonly ChannelWriter<ISecureEnvelope> m_Outbound;

    private InMemoryDuplexTransport(
        ChannelReader<ISecureEnvelope> inbound,
        ChannelWriter<ISecureEnvelope> outbound)
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
            Channel.CreateBounded<ISecureEnvelope>(
                new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = false,
                    SingleWriter = false
                });

        var bravoToAlpha =
            Channel.CreateBounded<ISecureEnvelope>(
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
    /// <param name="secureEnvelope">The secure envelope.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task SendAsync(
        ISecureEnvelope secureEnvelope,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(secureEnvelope);

        await m_Outbound
            .WriteAsync(secureEnvelope, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ISecureEnvelope&gt; representing the asynchronous operation.</returns>
    public async Task<ISecureEnvelope> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await m_Inbound
            .ReadAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}