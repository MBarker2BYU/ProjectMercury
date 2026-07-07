namespace Mercury.Abstractions.Factories;

/// <summary>
/// Interface IMercuryFactory
/// </summary>
public interface IMercuryFactory
{
    /// <summary>
    /// Builds the client dependencies.
    /// </summary>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <param name="replayProtector">The replay protector.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>IMercuryClientDependencies.</returns>
    IMercuryClientDependencies BuildClientDependencies(ICryptoProvider cryptoProvider, IEnvelopeCodec envelopeCodec, ITransport transport, IReplayProtector replayProtector, ILogger? logger = null);
    
    /// <summary>
    /// Builds the mercury client.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <returns>IMercuryClient.</returns>
    IMercuryClient BuildClient(IMercuryClientDependencies dependencies);
}