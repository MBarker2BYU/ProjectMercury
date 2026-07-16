// ***********************************************************************
// Assembly     : Mercury.Providers.AesGcm
// Author         : Kim Brown
// Created        : 07-16-2026
//
// Last Modified By : Kim Brown
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="FileDropTransport.cs">
//     Copyright (c) Kim Brown. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;

namespace Mercury.Transport.FileDrop;

/// <summary>
/// Moves protected Mercury frames through inbox and outbox directories.
/// </summary>
/// <remarks>
/// Frames are treated as opaque bytes. A temporary file and atomic rename
/// prevent receivers from observing partially written frames.
/// </remarks>
public sealed class FileDropTransport : ITransport
{
    private const string FRAME_EXTENSION = ".mercury";
    private const string TEMP_EXTENSION = ".tmp";
    private const string CLAIM_EXTENSION = ".processing";

    private readonly string m_InboxPath;
    private readonly string m_OutboxPath;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="FileDropTransport"/> class.
    /// </summary>
    /// <param name="inboxPath">
    /// Directory from which protected frames are received.
    /// </param>
    /// <param name="outboxPath">
    /// Directory to which protected frames are sent.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The inbox or outbox path is empty or contains only whitespace.
    /// </exception>
    public FileDropTransport(
        string inboxPath,
        string outboxPath)
    {
        m_InboxPath =
            NormalizePath(inboxPath, nameof(inboxPath));

        m_OutboxPath =
            NormalizePath(outboxPath, nameof(outboxPath));

        Directory.CreateDirectory(m_InboxPath);
        Directory.CreateDirectory(m_OutboxPath);
    }

    /// <summary>
    /// Gets a value indicating whether the inbox and outbox
    /// directories are available.
    /// </summary>
    /// <value>
    /// <c>true</c> when both directories exist; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool IsConnected =>
        Directory.Exists(m_InboxPath) &&
        Directory.Exists(m_OutboxPath);

    /// <summary>
    /// Writes an unchanged protected frame to the outbox.
    /// </summary>
    /// <param name="frame">
    /// The already-protected frame bytes.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous send operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The supplied frame is empty.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was cancelled.
    /// </exception>
    public async Task SendAsync(
        ReadOnlyMemory frame,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (frame.IsEmpty)
        {
            throw new ArgumentException(
                "Frame cannot be empty.",
                nameof(frame));
        }

        Directory.CreateDirectory(m_OutboxPath);

        var stem =
            $"{DateTime.UtcNow.Ticks:D19}-{Guid.NewGuid():N}";

        var temporaryPath =
            Path.Combine(
                m_OutboxPath,
                stem + TEMP_EXTENSION);

        var publishedPath =
            Path.Combine(
                m_OutboxPath,
                stem + FRAME_EXTENSION);

        var frameBytes = frame.ToArray();

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                FileOptions.Asynchronous))
            {
                await stream
                    .WriteAsync(
                        frameBytes,
                        cancellationToken)
                    .ConfigureAwait(false);

                await stream
                    .FlushAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Renaming within the same directory publishes
            // the complete file atomically.
            File.Move(temporaryPath, publishedPath);
        }
        finally
        {
            TryDelete(temporaryPath);
        }
    }

    /// <summary>
    /// Reads and removes the oldest available protected frame
    /// from the inbox.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task containing the unchanged protected frame bytes,
    /// or <see cref="ReadOnlyMemory.Empty"/> when no complete
    /// frame is available.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was cancelled.
    /// </exception>
    public async Task<ReadOnlyMemory> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Directory.CreateDirectory(m_InboxPath);

        foreach (var framePath in EnumerateFramesOldestFirst())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var claimedPath =
                framePath
                + "."
                + Guid.NewGuid().ToString("N")
                + CLAIM_EXTENSION;

            try
            {
                // Claim the file so another receiver
                // cannot process the same frame.
                File.Move(framePath, claimedPath);
            }
            catch (FileNotFoundException)
            {
                // Another receiver claimed or removed
                // the frame first.
                continue;
            }
            catch (IOException) when (!File.Exists(framePath))
            {
                // Some platforms report a missing source
                // as a general I/O error.
                continue;
            }

            try
            {
                var bytes =
                    await File
                        .ReadAllBytesAsync(
                            claimedPath,
                            cancellationToken)
                        .ConfigureAwait(false);

                return new ReadOnlyMemory(bytes);
            }
            finally
            {
                // Receiving successfully consumes the frame.
                File.Delete(claimedPath);
            }
        }

        return ReadOnlyMemory.Empty;
    }

    private IEnumerable<string> EnumerateFramesOldestFirst()
    {
        return Directory
            .EnumerateFiles(
                m_InboxPath,
                "*" + FRAME_EXTENSION,
                SearchOption.TopDirectoryOnly)
            .OrderBy(
                path => Path.GetFileName(path),
                StringComparer.Ordinal);
    }

    private static string NormalizePath(
        string path,
        string parameterName)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException(
                "A directory path is required.",
                parameterName);
        }

        return Path.GetFullPath(path);
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            // Do not hide the original send result
            // if best-effort cleanup fails.
        }
        catch (UnauthorizedAccessException)
        {
            // Do not hide the original send result
            // if best-effort cleanup fails.
        }
    }
}
