using System.Data;
using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Services;
using Mercury.Core.Envelope;

namespace Mercury.Core.Services;

public sealed class EnvelopeService : IEnvelopeService
{
    /// <summary>
    /// The static Envelope Service
    /// </summary>
    private static readonly Lazy<EnvelopeService> sm_EnvelopeService = new(() => new EnvelopeService());

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static EnvelopeService Instance => sm_EnvelopeService.Value;

    public IEnvelopeHeader BuildEnvelopeHeader()
        => EnvelopeHeader.Empty;

    public IEnvelopeFooter BuildEnvelopeFooter()
        => EnvelopeFooter.Empty;

    public IMercuryResult PackEnvelope(IEnvelopeHeader header, ReadOnlyMemory payload, IEnvelopeFooter footer)
    {
        var success = !payload.IsEmpty;
        var message = !success ? @"Payload cannot be empty." : null ;
        var reason = payload.IsEmpty ? FailureReason.Custom : FailureReason.None;

        return new MercuryResult(success, payload, new SecureEnvelope(header, payload, footer), reason, message);
    }

    public IMercuryResult UnpackEnvelope(ISecureEnvelope secureEnvelope)
    {
        var success = !secureEnvelope.Payload.IsEmpty;
        var message = !success ? @"Payload cannot be empty." : null;
        var reason = secureEnvelope.Payload.IsEmpty ? FailureReason.Custom : FailureReason.None;

        return new MercuryResult(success, secureEnvelope.Payload, secureEnvelope, reason, message);
    }
}