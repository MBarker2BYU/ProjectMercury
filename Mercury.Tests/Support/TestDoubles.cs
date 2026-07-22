// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="TestSymmetricKeyProvider.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Collections.Concurrent;
using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Logging;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Replay;
using Mercury.Abstractions.Services;
using Mercury.Abstractions.Transport;

using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Support;

/// <summary>
/// Class TestSymmetricKeyProvider. This class cannot be inherited.
/// Implements the <see cref="ISymmetricKeyProvider" />
/// </summary>
/// <seealso cref="ISymmetricKeyProvider" />
internal sealed class TestSymmetricKeyProvider : ISymmetricKeyProvider
{
    /// <summary>
    /// The m keys
    /// </summary>
    private readonly IReadOnlyDictionary<string, byte[]> m_Keys;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSymmetricKeyProvider"/> class.
    /// </summary>
    /// <param name="keys">The keys.</param>
    public TestSymmetricKeyProvider(params (string KeyId, byte[] Key)[] keys)
    {
        m_Keys = keys.ToDictionary(
            pair => pair.KeyId,
            pair => (byte[])pair.Key.Clone(),
            StringComparer.Ordinal);
    }

    /// <summary>
    /// Gets the symmetric key associated with the specified key identifier.
    /// </summary>
    /// <param name="keyId">The key identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.</param>
    /// <returns>A task containing the symmetric key.</returns>
    /// <exception cref="KeyNotFoundException">No symmetric key is registered for '{keyId.Value}'.</exception>
    public Task<MercuryMemory> GetKeyAsync(
        KeyId keyId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!m_Keys.TryGetValue(keyId.Value, out var key))
        {
            throw new KeyNotFoundException(
                $"No symmetric key is registered for '{keyId.Value}'.");
        }

        return Task.FromResult(new MercuryMemory(key));
    }
}

/// <summary>
/// Class TestCryptoContext. This class cannot be inherited.
/// Implements the <see cref="ICryptoContext" />
/// </summary>
/// <seealso cref="ICryptoContext" />
internal sealed class TestCryptoContext : ICryptoContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestCryptoContext"/> class.
    /// </summary>
    /// <param name="senderKeyId">The sender key identifier.</param>
    /// <param name="recipientKeyId">The recipient key identifier.</param>
    public TestCryptoContext(KeyId senderKeyId, KeyId recipientKeyId)
    {
        SenderKeyId = senderKeyId;
        RecipientKeyId = recipientKeyId;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
    public bool IsEmpty => SenderKeyId.IsEmpty && RecipientKeyId.IsEmpty;

    /// <summary>
    /// Gets the sender key identifier.
    /// </summary>
    /// <value>The sender key identifier.</value>
    public KeyId SenderKeyId { get; }

    /// <summary>
    /// Gets the recipient key identifier.
    /// </summary>
    /// <value>The recipient key identifier.</value>
    public KeyId RecipientKeyId { get; }
}

/// <summary>
/// Class TestSealRequest. This class cannot be inherited.
/// Implements the <see cref="ISealRequest" />
/// </summary>
/// <seealso cref="ISealRequest" />
internal sealed class TestSealRequest : ISealRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestSealRequest"/> class.
    /// </summary>
    /// <param name="cryptoContext">The crypto context.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="headerMeta">The header meta.</param>
    /// <param name="footerMeta">The footer meta.</param>
    public TestSealRequest(
        ICryptoContext cryptoContext,
        MercuryMemory payload,
        Metadata? headerMeta = null,
        Metadata? footerMeta = null)
    {
        CryptoContext = cryptoContext;
        Payload = payload;
        HeaderMeta = headerMeta ?? Metadata.Empty;
        FooterMeta = footerMeta ?? Metadata.Empty;
    }

    /// <summary>
    /// Gets the crypto context.
    /// </summary>
    /// <value>The crypto context.</value>
    public ICryptoContext CryptoContext { get; }

    /// <summary>
    /// Gets the payload to protect.
    /// </summary>
    /// <value>The payload.</value>
    public MercuryMemory Payload { get; }

    /// <summary>
    /// Gets the envelope header metadata.
    /// </summary>
    /// <value>The header meta.</value>
    public Metadata HeaderMeta { get; }

    /// <summary>
    /// Gets the envelope footer metadata.
    /// </summary>
    /// <value>The footer meta.</value>
    public Metadata FooterMeta { get; }
}

/// <summary>
/// Class TestOpenRequest. This class cannot be inherited.
/// Implements the <see cref="IOpenRequest" />
/// </summary>
/// <seealso cref="IOpenRequest" />
internal sealed class TestOpenRequest : IOpenRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestOpenRequest"/> class.
    /// </summary>
    /// <param name="secureEnvelope">The secure envelope.</param>
    public TestOpenRequest(ISecureEnvelope secureEnvelope)
    {
        SecureEnvelope = secureEnvelope;
    }

    /// <summary>
    /// Gets the secure envelope.
    /// </summary>
    /// <value>The secure envelope.</value>
    public ISecureEnvelope SecureEnvelope { get; }
}

/// <summary>
/// Class TestSecureEnvelope. This class cannot be inherited.
/// Implements the <see cref="ISecureEnvelope" />
/// </summary>
/// <seealso cref="ISecureEnvelope" />
internal sealed class TestSecureEnvelope : ISecureEnvelope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestSecureEnvelope"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="header">The header.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="footer">The footer.</param>
    public TestSecureEnvelope(
        FrameworkVersion version,
        IEnvelopeHeader header,
        MercuryMemory payload,
        IEnvelopeFooter footer)
    {
        Version = version;
        Header = header;
        Payload = payload;
        Footer = footer;
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <value>The version.</value>
    public FrameworkVersion Version { get; }

    /// <summary>
    /// Gets the header.
    /// </summary>
    /// <value>The header.</value>
    public IEnvelopeHeader Header { get; }

    /// <summary>
    /// Gets the payload.
    /// </summary>
    /// <value>The payload.</value>
    public MercuryMemory Payload { get; }

    /// <summary>
    /// Gets the footer.
    /// </summary>
    /// <value>The footer.</value>
    public IEnvelopeFooter Footer { get; }
}

/// <summary>
/// Class TestDependencies. This class cannot be inherited.
/// Implements the <see cref="IMercuryClientDependencies" />
/// </summary>
/// <seealso cref="IMercuryClientDependencies" />
internal sealed class TestDependencies : IMercuryClientDependencies
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestDependencies"/> class.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <param name="envelopeCodec">The envelope codec.</param>
    /// <param name="transport">The transport.</param>
    /// <param name="replayProtector">The replay protector.</param>
    /// <param name="logger">The logger.</param>
    public TestDependencies(
        KeyId clientId,
        ICryptoProvider cryptoProvider,
        IEnvelopeCodec envelopeCodec,
        ITransport transport,
        IReplayProtector? replayProtector = null,
        IMercuryLogger? logger = null)
    {
        ClientId = clientId;
        CryptoProvider = cryptoProvider;
        EnvelopeCodec = envelopeCodec;
        Transport = transport;
        ReplayProtector = replayProtector ?? new AcceptAllReplayProtector();
        Logger = logger ?? NoOpMercuryLogger.Instance;
    }

    public KeyId ClientId { get; }

    /// <summary>
    /// Gets the crypto provider.
    /// </summary>
    /// <value>The crypto.</value>
    public ICryptoProvider CryptoProvider { get; }

    /// <summary>
    /// Gets the envelope codec.
    /// </summary>
    /// <value>The envelope codec.</value>
    public IEnvelopeCodec EnvelopeCodec { get; }

    /// <summary>
    /// Gets the transport.
    /// </summary>
    /// <value>The transport.</value>
    public ITransport Transport { get; }

    /// <summary>
    /// Gets the replay protector.
    /// </summary>
    /// <value>The replay protector.</value>
    public IReplayProtector ReplayProtector { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    public IMercuryLogger Logger { get; }
}

/// <summary>
/// Class QueueTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
internal sealed class QueueTransport : ITransport
{
    /// <summary>
    /// The m inbound
    /// </summary>
    private readonly ConcurrentQueue<MercuryMemory> m_Inbound = new();
    /// <summary>
    /// The m signal
    /// </summary>
    private readonly SemaphoreSlim m_Signal = new(0);
    /// <summary>
    /// The m sent lock
    /// </summary>
    private readonly object m_SentLock = new();
    /// <summary>
    /// The m sent frames
    /// </summary>
    private readonly List<MercuryMemory> m_SentFrames = [];

    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    /// <value><c>true</c> if the transport is connected; otherwise, <c>false</c>.</value>
    public bool IsConnected { get; set; } = true;

    /// <summary>
    /// Gets the sent frames.
    /// </summary>
    /// <value>The sent frames.</value>
    public IReadOnlyList<MercuryMemory> SentFrames
    {
        get
        {
            lock (m_SentLock)
            {
                return m_SentFrames.Select(frame => frame.Clone()).ToArray();
            }
        }
    }

    /// <summary>
    /// Gets the last sent frame.
    /// </summary>
    /// <value>The last sent frame.</value>
    public MercuryMemory LastSentFrame
    {
        get
        {
            lock (m_SentLock)
            {
                return m_SentFrames.Count == 0
                    ? MercuryMemory.Empty
                    : m_SentFrames[^1].Clone();
            }
        }
    }

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="ArgumentException">Frame cannot be empty. - frame</exception>
    public Task SendAsync(
        MercuryMemory frame,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (frame.IsEmpty)
        {
            throw new ArgumentException("Frame cannot be empty.", nameof(frame));
        }

        var copy = frame.Clone();

        lock (m_SentLock)
        {
            m_SentFrames.Add(copy);
        }

        Inject(copy);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;MercuryMemory&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">The queue transport was signaled without a frame.</exception>
    public async Task<MercuryMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        await m_Signal.WaitAsync(cancellationToken).ConfigureAwait(false);

        if (m_Inbound.TryDequeue(out var frame))
        {
            return frame.Clone();
        }

        throw new InvalidOperationException(
            "The queue transport was signaled without a frame.");
    }

    /// <summary>
    /// Injects the specified frame.
    /// </summary>
    /// <param name="frame">The frame.</param>
    public void Inject(MercuryMemory frame)
    {
        m_Inbound.Enqueue(frame.Clone());
        m_Signal.Release();
    }
}

/// <summary>
/// Class EmptyFrameTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
internal sealed class EmptyFrameTransport : ITransport
{
    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    /// <value><c>true</c> if the transport is connected; otherwise, <c>false</c>.</value>
    public bool IsConnected => true;

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendAsync(
        MercuryMemory frame,
        CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    public Task<MercuryMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
        => Task.FromResult(MercuryMemory.Empty);
}

/// <summary>
/// Class ThrowingTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
internal sealed class ThrowingTransport : ITransport
{
    /// <summary>
    /// The m send exception
    /// </summary>
    private readonly Exception m_SendException;
    /// <summary>
    /// The m receive exception
    /// </summary>
    private readonly Exception m_ReceiveException;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowingTransport"/> class.
    /// </summary>
    /// <param name="sendException">The send exception.</param>
    /// <param name="receiveException">The receive exception.</param>
    public ThrowingTransport(
        Exception? sendException = null,
        Exception? receiveException = null)
    {
        m_SendException = sendException ?? new IOException("Send failed.");
        m_ReceiveException = receiveException ?? new IOException("Receive failed.");
    }

    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    /// <value><c>true</c> if the transport is connected; otherwise, <c>false</c>.</value>
    public bool IsConnected => false;

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendAsync(
        MercuryMemory frame,
        CancellationToken cancellationToken = default)
        => Task.FromException(m_SendException);

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    public Task<MercuryMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
        => Task.FromException<MercuryMemory>(m_ReceiveException);
}

/// <summary>
/// Class RecordingTransport. This class cannot be inherited.
/// Implements the <see cref="ITransport" />
/// </summary>
/// <seealso cref="ITransport" />
internal sealed class RecordingTransport : ITransport
{
    /// <summary>
    /// Gets a value indicating whether the transport is connected.
    /// </summary>
    /// <value><c>true</c> if the transport is connected; otherwise, <c>false</c>.</value>
    public bool IsConnected => true;

    /// <summary>
    /// Gets the send count.
    /// </summary>
    /// <value>The send count.</value>
    public int SendCount { get; private set; }

    /// <summary>
    /// Gets the last frame.
    /// </summary>
    /// <value>The last frame.</value>
    public MercuryMemory LastFrame { get; private set; } = MercuryMemory.Empty;

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="frame">The frame.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task SendAsync(
        MercuryMemory frame,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        SendCount++;
        LastFrame = frame.Clone();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Receives the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ReadOnlyMemory&gt;.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public Task<MercuryMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
        => throw new NotSupportedException();
}

/// <summary>
/// Class AcceptAllReplayProtector. This class cannot be inherited.
/// Implements the <see cref="IReplayProtector" />
/// </summary>
/// <seealso cref="IReplayProtector" />
internal sealed class AcceptAllReplayProtector : IReplayProtector
{
    /// <summary>
    /// Gets the call count.
    /// </summary>
    /// <value>The call count.</value>
    public int CallCount { get; private set; }

    /// <summary>
    /// Attempts to accept and record the envelope header.
    /// </summary>
    /// <param name="header">The validated envelope header.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.</param>
    /// <returns>A task containing <c>true</c> if the header was accepted;
    /// otherwise, <c>false</c> if it was previously recorded.</returns>
    public Task<bool> TryAcceptAsync(
        IEnvelopeHeader header,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CallCount++;
        return Task.FromResult(true);
    }
}

/// <summary>
/// Class RejectAllReplayProtector. This class cannot be inherited.
/// Implements the <see cref="IReplayProtector" />
/// </summary>
/// <seealso cref="IReplayProtector" />
internal sealed class RejectAllReplayProtector : IReplayProtector
{
    /// <summary>
    /// Gets the call count.
    /// </summary>
    /// <value>The call count.</value>
    public int CallCount { get; private set; }

    /// <summary>
    /// Attempts to accept and record the envelope header.
    /// </summary>
    /// <param name="header">The validated envelope header.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.</param>
    /// <returns>A task containing <c>true</c> if the header was accepted;
    /// otherwise, <c>false</c> if it was previously recorded.</returns>
    public Task<bool> TryAcceptAsync(
        IEnvelopeHeader header,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CallCount++;
        return Task.FromResult(false);
    }
}

/// <summary>
/// Class ThrowingReplayProtector. This class cannot be inherited.
/// Implements the <see cref="IReplayProtector" />
/// </summary>
/// <seealso cref="IReplayProtector" />
internal sealed class ThrowingReplayProtector : IReplayProtector
{
    /// <summary>
    /// Attempts to accept and record the envelope header.
    /// </summary>
    /// <param name="header">The validated envelope header.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads
    /// to receive notice of cancellation.</param>
    /// <returns>A task containing <c>true</c> if the header was accepted;
    /// otherwise, <c>false</c> if it was previously recorded.</returns>
    public Task<bool> TryAcceptAsync(
        IEnvelopeHeader header,
        CancellationToken cancellationToken = default)
        => Task.FromException<bool>(new InvalidOperationException("Replay check failed."));
}

/// <summary>
/// Class DelegatingCryptoProvider. This class cannot be inherited.
/// Implements the <see cref="ICryptoProvider" />
/// </summary>
/// <seealso cref="ICryptoProvider" />
internal sealed class DelegatingCryptoProvider : ICryptoProvider
{
    /// <summary>
    /// The m seal
    /// </summary>
    private readonly Func<ISealRequest, IEnvelopeService, CancellationToken, Task<ICryptoProviderResult>> m_Seal;
    /// <summary>
    /// The m open
    /// </summary>
    private readonly Func<IOpenRequest, IEnvelopeService, CancellationToken, Task<ICryptoProviderResult>> m_Open;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegatingCryptoProvider"/> class.
    /// </summary>
    /// <param name="seal">The seal.</param>
    /// <param name="open">The open.</param>
    /// <param name="name">The name.</param>
    public DelegatingCryptoProvider(
        Func<ISealRequest, IEnvelopeService, CancellationToken, Task<ICryptoProviderResult>>? seal = null,
        Func<IOpenRequest, IEnvelopeService, CancellationToken, Task<ICryptoProviderResult>>? open = null,
        string name = "test-provider")
    {
        Name = name;
        m_Seal = seal ?? ((_, service, _) => Task.FromResult(
            service.BuildCryptoProviderResult(
                false,
                MercuryMemory.Empty,
                null,
                FailureReason.InternalError,
                "Seal was not configured.")));
        m_Open = open ?? ((_, service, _) => Task.FromResult(
            service.BuildCryptoProviderResult(
                false,
                MercuryMemory.Empty,
                null,
                FailureReason.InternalError,
                "Open was not configured.")));
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>
    /// Seals the asynchronous.
    /// </summary>
    /// <param name="sealRequest">The protect request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
    public Task<ICryptoProviderResult> SealAsync(
        ISealRequest sealRequest,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
        => m_Seal(sealRequest, envelopeService, cancellationToken);

    /// <summary>
    /// Opens the asynchronous.
    /// </summary>
    /// <param name="openRequest">The unprotect request.</param>
    /// <param name="envelopeService">The envelope service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ICryptoProviderResult&gt;.</returns>
    public Task<ICryptoProviderResult> OpenAsync(
        IOpenRequest openRequest,
        IEnvelopeService envelopeService,
        CancellationToken cancellationToken = default)
        => m_Open(openRequest, envelopeService, cancellationToken);
}

/// <summary>
/// Class DelegatingCodec. This class cannot be inherited.
/// Implements the <see cref="IEnvelopeCodec" />
/// </summary>
/// <seealso cref="IEnvelopeCodec" />
internal sealed class DelegatingCodec : IEnvelopeCodec
{
    /// <summary>
    /// The m encode
    /// </summary>
    private readonly Func<ISecureEnvelope, MercuryMemory> m_Encode;
    /// <summary>
    /// The m decode
    /// </summary>
    private readonly Func<MercuryMemory, ISecureEnvelope> m_Decode;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegatingCodec"/> class.
    /// </summary>
    /// <param name="encode">The encode.</param>
    /// <param name="decode">The decode.</param>
    public DelegatingCodec(
        Func<ISecureEnvelope, MercuryMemory>? encode = null,
        Func<MercuryMemory, ISecureEnvelope>? decode = null)
    {
        m_Encode = encode ?? (_ => throw new NotSupportedException());
        m_Decode = decode ?? (_ => throw new NotSupportedException());
    }

    /// <summary>
    /// Encodes the specified envelope.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>ReadOnlyMemory.</returns>
    public MercuryMemory Encode(ISecureEnvelope envelope)
        => m_Encode(envelope);

    /// <summary>
    /// Decodes the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>ISecureEnvelope.</returns>
    public ISecureEnvelope Decode(MercuryMemory data)
        => m_Decode(data);
}

/// <summary>
/// Class ByteSearch.
/// </summary>
internal static class ByteSearch
{
    /// <summary>
    /// Determines whether the specified source contains subsequence.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the specified source contains subsequence; otherwise, <c>false</c>.</returns>
    public static bool ContainsSubsequence(byte[] source, byte[] value)
    {
        if (value.Length == 0)
        {
            return true;
        }

        if (value.Length > source.Length)
        {
            return false;
        }

        for (var sourceIndex = 0;
             sourceIndex <= source.Length - value.Length;
             sourceIndex++)
        {
            var match = true;

            for (var valueIndex = 0;
                 valueIndex < value.Length;
                 valueIndex++)
            {
                if (source[sourceIndex + valueIndex] == value[valueIndex])
                {
                    continue;
                }

                match = false;
                break;
            }

            if (match)
            {
                return true;
            }
        }

        return false;
    }
}
