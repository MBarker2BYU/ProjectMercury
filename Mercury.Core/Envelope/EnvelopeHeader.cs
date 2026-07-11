using Mercury.Abstractions.Envelope;

namespace Mercury.Core.Envelope;

internal sealed class EnvelopeHeader : IEnvelopeHeader
{
    public static IEnvelopeHeader Empty = new EnvelopeHeader();
}