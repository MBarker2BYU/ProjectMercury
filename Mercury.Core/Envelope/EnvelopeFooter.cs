using Mercury.Abstractions.Envelope;

namespace Mercury.Core.Envelope;

internal sealed class EnvelopeFooter : IEnvelopeFooter
{
    public static IEnvelopeFooter Empty => new EnvelopeFooter();    
}