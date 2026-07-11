using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;

namespace Mercury.Abstractions.Cryptograph;

public interface ICryptoProviderResult
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IMercuryResult"/> is success.
    /// </summary>
    /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
    bool Success { get; }
    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    ReadOnlyMemory Payload { get; }
    /// <summary>
    /// Gets the validated envelope.
    /// </summary>
    /// <value>The validated envelope.</value>
    ISecureEnvelope? ValidatedEnvelope { get; }
    /// <summary>
    /// Gets the failure reason.
    /// </summary>
    /// <value>The failure reason.</value>
    FailureReason FailureReason { get; }
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    string? Message { get; }
}