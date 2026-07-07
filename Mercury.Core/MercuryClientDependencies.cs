using Mercury.Abstractions;
using Mercury.Abstractions.Cryptography;
using Mercury.Abstractions.Detection;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Logging;
using Mercury.Abstractions.Transport;

namespace Mercury.Core;

internal class MercuryClientDependencies(ICryptoProvider cryptoProvider, IEnvelopeCodec envelopeCodec, ITransport transport, IReplayProtector replayProtector, ILogger? logger) : IMercuryClientDependencies
{
    public ICryptoProvider CryptoProvider { get; } = cryptoProvider;
    public IEnvelopeCodec EnvelopeCodec { get; } = envelopeCodec;       
    public ITransport Transport { get; } = transport;   
    public IReplayProtector ReplayProtector { get; } = replayProtector;    
    public ILogger? EffectiveLogger { get; } = logger;    
}