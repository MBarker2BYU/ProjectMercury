using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Services;

public interface IEnvelopeService
{
    IEnvelopeHeader BuildEnvelopeHeader();

    IEnvelopeFooter BuildEnvelopeFooter();

    IMercuryResult PackEnvelope(IEnvelopeHeader header, ReadOnlyMemory payload, IEnvelopeFooter footer);

    IMercuryResult UnpackEnvelope(ISecureEnvelope secureEnvelope);
}