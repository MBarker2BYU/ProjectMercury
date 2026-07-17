// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-17-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-17-2026
// ***********************************************************************
// <copyright file="FileDropTransportTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Transport.FileDrop;
using MercuryMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests.Transports;

/// <summary>
/// Class FileDropTransportTests. This class cannot be inherited.
/// </summary>
public sealed class FileDropTransportTests
{
    /// <summary>
    /// The frame extension
    /// </summary>
    private const string FRAME_EXTENSION = ".mercury";
    /// <summary>
    /// The temporary extension
    /// </summary>
    private const string TEMPORARY_EXTENSION = ".tmp";
    /// <summary>
    /// The processing extension
    /// </summary>
    private const string PROCESSING_EXTENSION = ".processing";

    /// <summary>
    /// Defines the test method Constructor_NullInboxPath_ThrowsArgumentException.
    /// </summary>
    [Fact]
    public void Constructor_NullInboxPath_ThrowsArgumentException()
    {
        using var directories = new TestDirectories();

        Assert.Throws<ArgumentException>(
            () => new FileDropTransport(
                null!,
                directories.OutboxPath));
    }

    /// <summary>
    /// Defines the test method Constructor_NullOutboxPath_ThrowsArgumentException.
    /// </summary>
    [Fact]
    public void Constructor_NullOutboxPath_ThrowsArgumentException()
    {
        using var directories = new TestDirectories();

        Assert.Throws<ArgumentException>(
            () => new FileDropTransport(
                directories.InboxPath,
                null!));
    }

    /// <summary>
    /// Defines the test method Constructor_BlankInboxPath_ThrowsArgumentException.
    /// </summary>
    /// <param name="inboxPath">The inbox path.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_BlankInboxPath_ThrowsArgumentException(
        string inboxPath)
    {
        using var directories = new TestDirectories();

        Assert.Throws<ArgumentException>(
            () => new FileDropTransport(
                inboxPath,
                directories.OutboxPath));
    }

    /// <summary>
    /// Defines the test method Constructor_BlankOutboxPath_ThrowsArgumentException.
    /// </summary>
    /// <param name="outboxPath">The outbox path.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_BlankOutboxPath_ThrowsArgumentException(
        string outboxPath)
    {
        using var directories = new TestDirectories();

        Assert.Throws<ArgumentException>(
            () => new FileDropTransport(
                directories.InboxPath,
                outboxPath));
    }

    /// <summary>
    /// Defines the test method Constructor_CreatesDirectoriesAndReportsConnected.
    /// </summary>
    [Fact]
    public void Constructor_CreatesDirectoriesAndReportsConnected()
    {
        using var directories = new TestDirectories();

        var transport = directories.BuildTransport();

        Assert.True(Directory.Exists(directories.InboxPath));
        Assert.True(Directory.Exists(directories.OutboxPath));
        Assert.True(transport.IsConnected);
    }

    /// <summary>
    /// Defines the test method IsConnected_WhenInboxIsRemoved_ReturnsFalse.
    /// </summary>
    [Fact]
    public void IsConnected_WhenInboxIsRemoved_ReturnsFalse()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        Directory.Delete(directories.InboxPath, recursive: true);

        Assert.False(transport.IsConnected);
    }

    /// <summary>
    /// Defines the test method IsConnected_WhenOutboxIsRemoved_ReturnsFalse.
    /// </summary>
    [Fact]
    public void IsConnected_WhenOutboxIsRemoved_ReturnsFalse()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        Directory.Delete(directories.OutboxPath, recursive: true);

        Assert.False(transport.IsConnected);
    }

    /// <summary>
    /// Defines the test method SendAsync_EmptyFrame_ThrowsArgumentException.
    /// </summary>
    [Fact]
    public async Task SendAsync_EmptyFrame_ThrowsArgumentException()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();

        await Assert.ThrowsAsync<ArgumentException>(
            () => transport.SendAsync(MercuryMemory.Empty));
    }

    /// <summary>
    /// Defines the test method SendAsync_CanceledToken_ThrowsOperationCanceledException.
    /// </summary>
    [Fact]
    public async Task SendAsync_CanceledToken_ThrowsOperationCanceledException()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => transport.SendAsync(
                new MercuryMemory(new byte[] { 1, 2, 3 }),
                cancellation.Token));

        Assert.Empty(Directory.EnumerateFiles(directories.OutboxPath));
    }

    /// <summary>
    /// Defines the test method SendAsync_WritesExactFrameAndPublishesMercuryFile.
    /// </summary>
    [Fact]
    public async Task SendAsync_WritesExactFrameAndPublishesMercuryFile()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var expected = Enumerable.Range(0, 256)
            .Select(value => (byte)value)
            .ToArray();

        await transport.SendAsync(new MercuryMemory(expected));

        var framePath = Assert.Single(
            Directory.EnumerateFiles(
                directories.OutboxPath,
                "*" + FRAME_EXTENSION));
        var actual = await File.ReadAllBytesAsync(framePath);

        Assert.Equal(expected, actual);
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.OutboxPath,
                "*" + TEMPORARY_EXTENSION));
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.OutboxPath,
                "*" + PROCESSING_EXTENSION));
    }

    /// <summary>
    /// Defines the test method SendAsync_WhenOutboxWasRemoved_RecreatesDirectory.
    /// </summary>
    [Fact]
    public async Task SendAsync_WhenOutboxWasRemoved_RecreatesDirectory()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        Directory.Delete(directories.OutboxPath, recursive: true);

        await transport.SendAsync(
            new MercuryMemory(new byte[] { 1, 2, 3 }));

        Assert.True(Directory.Exists(directories.OutboxPath));
        Assert.True(transport.IsConnected);
        Assert.Single(
            Directory.EnumerateFiles(
                directories.OutboxPath,
                "*" + FRAME_EXTENSION));
    }

    /// <summary>
    /// Defines the test method SendAsync_MultipleFrames_CreatesUniqueFiles.
    /// </summary>
    [Fact]
    public async Task SendAsync_MultipleFrames_CreatesUniqueFiles()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();

        for (var index = 0; index < 25; index++)
        {
            await transport.SendAsync(
                new MercuryMemory(new byte[] { (byte)index }));
        }

        var framePaths = Directory
            .EnumerateFiles(
                directories.OutboxPath,
                "*" + FRAME_EXTENSION)
            .ToArray();

        Assert.Equal(25, framePaths.Length);
        Assert.Equal(
            25,
            framePaths
                .Select(Path.GetFileName)
                .Distinct(StringComparer.Ordinal)
                .Count());
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.OutboxPath,
                "*" + TEMPORARY_EXTENSION));
    }

    /// <summary>
    /// Defines the test method SendAsync_ConcurrentFrames_DoesNotOverwriteFiles.
    /// </summary>
    [Fact]
    public async Task SendAsync_ConcurrentFrames_DoesNotOverwriteFiles()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var sends = Enumerable.Range(0, 50)
            .Select(index => transport.SendAsync(
                new MercuryMemory(new byte[] { (byte)index })))
            .ToArray();

        await Task.WhenAll(sends);

        var framePaths = Directory
            .EnumerateFiles(
                directories.OutboxPath,
                "*" + FRAME_EXTENSION)
            .ToArray();
        var values = new List<byte>();

        foreach (var framePath in framePaths)
        {
            var frame = await File.ReadAllBytesAsync(framePath);
            Assert.Single(frame);
            values.Add(frame[0]);
        }

        Assert.Equal(50, framePaths.Length);
        Assert.Equal(
            Enumerable.Range(0, 50).Select(value => (byte)value),
            values.OrderBy(value => value));
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.OutboxPath,
                "*" + TEMPORARY_EXTENSION));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_EmptyInbox_ReturnsEmptyFrame.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_EmptyInbox_ReturnsEmptyFrame()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();

        var actual = await transport.ReceiveAsync();

        Assert.True(actual.IsEmpty);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_WhenInboxWasRemoved_RecreatesDirectory.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_WhenInboxWasRemoved_RecreatesDirectory()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        Directory.Delete(directories.InboxPath, recursive: true);

        var actual = await transport.ReceiveAsync();

        Assert.True(actual.IsEmpty);
        Assert.True(Directory.Exists(directories.InboxPath));
        Assert.True(transport.IsConnected);
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_IgnoresTemporaryFiles.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_IgnoresTemporaryFiles()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var temporaryPath = Path.Combine(
            directories.InboxPath,
            "0000000000000000001-frame" + TEMPORARY_EXTENSION);
        await File.WriteAllBytesAsync(
            temporaryPath,
            new byte[] { 1, 2, 3 });

        var actual = await transport.ReceiveAsync();

        Assert.True(actual.IsEmpty);
        Assert.True(File.Exists(temporaryPath));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_IgnoresProcessingFiles.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_IgnoresProcessingFiles()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var processingPath = Path.Combine(
            directories.InboxPath,
            "0000000000000000001-frame.mercury.abc" +
            PROCESSING_EXTENSION);
        await File.WriteAllBytesAsync(
            processingPath,
            new byte[] { 1, 2, 3 });

        var actual = await transport.ReceiveAsync();

        Assert.True(actual.IsEmpty);
        Assert.True(File.Exists(processingPath));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_CompleteFrame_ReturnsExactBytesAndConsumesFile.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_CompleteFrame_ReturnsExactBytesAndConsumesFile()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var expected = Enumerable.Range(0, 1024)
            .Select(value => (byte)(value % 256))
            .ToArray();
        var framePath = Path.Combine(
            directories.InboxPath,
            "0000000000000000001-frame" + FRAME_EXTENSION);
        await File.WriteAllBytesAsync(framePath, expected);

        var actual = await transport.ReceiveAsync();

        Assert.Equal(expected, actual.ToArray());
        Assert.False(File.Exists(framePath));
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.InboxPath,
                "*" + PROCESSING_EXTENSION));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_MultipleFrames_ReturnsOldestFilenameFirst.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_MultipleFrames_ReturnsOldestFilenameFirst()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var firstPath = Path.Combine(
            directories.InboxPath,
            "0000000000000000001-first" + FRAME_EXTENSION);
        var secondPath = Path.Combine(
            directories.InboxPath,
            "0000000000000000002-second" + FRAME_EXTENSION);
        await File.WriteAllBytesAsync(
            secondPath,
            new byte[] { 2 });
        await File.WriteAllBytesAsync(
            firstPath,
            new byte[] { 1 });

        var first = await transport.ReceiveAsync();
        var second = await transport.ReceiveAsync();

        Assert.Equal(new byte[] { 1 }, first.ToArray());
        Assert.Equal(new byte[] { 2 }, second.ToArray());
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.InboxPath,
                "*" + FRAME_EXTENSION));
    }

    /// <summary>
    /// Defines the test method ReceiveAsync_CanceledToken_ThrowsAndDoesNotConsumeFrame.
    /// </summary>
    [Fact]
    public async Task ReceiveAsync_CanceledToken_ThrowsAndDoesNotConsumeFrame()
    {
        using var directories = new TestDirectories();
        var transport = directories.BuildTransport();
        var framePath = Path.Combine(
            directories.InboxPath,
            "0000000000000000001-frame" + FRAME_EXTENSION);
        await File.WriteAllBytesAsync(
            framePath,
            new byte[] { 1, 2, 3 });
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => transport.ReceiveAsync(cancellation.Token));

        Assert.True(File.Exists(framePath));
    }

    /// <summary>
    /// Defines the test method ConnectedPair_AlphaToBravo_ReturnsIdenticalFrame.
    /// </summary>
    [Fact]
    public async Task ConnectedPair_AlphaToBravo_ReturnsIdenticalFrame()
    {
        using var directories = new TestDirectories();
        var (alpha, bravo) = directories.BuildConnectedPair();
        var expected = Enumerable.Range(0, 4096)
            .Select(value => (byte)(value % 256))
            .ToArray();

        await alpha.SendAsync(new MercuryMemory(expected));
        var actual = await bravo.ReceiveAsync();

        Assert.Equal(expected, actual.ToArray());
        Assert.True((await alpha.ReceiveAsync()).IsEmpty);
    }

    /// <summary>
    /// Defines the test method ConnectedPair_BravoToAlpha_ReturnsIdenticalFrame.
    /// </summary>
    [Fact]
    public async Task ConnectedPair_BravoToAlpha_ReturnsIdenticalFrame()
    {
        using var directories = new TestDirectories();
        var (alpha, bravo) = directories.BuildConnectedPair();
        var expected = new byte[] { 9, 8, 7, 6, 5 };

        await bravo.SendAsync(new MercuryMemory(expected));
        var actual = await alpha.ReceiveAsync();

        Assert.Equal(expected, actual.ToArray());
        Assert.True((await bravo.ReceiveAsync()).IsEmpty);
    }

    /// <summary>
    /// Defines the test method ConnectedPair_BidirectionalExchange_PreservesBothFrames.
    /// </summary>
    [Fact]
    public async Task ConnectedPair_BidirectionalExchange_PreservesBothFrames()
    {
        using var directories = new TestDirectories();
        var (alpha, bravo) = directories.BuildConnectedPair();
        var alphaFrame = new byte[] { 1, 3, 5, 7 };
        var bravoFrame = new byte[] { 2, 4, 6, 8 };

        await Task.WhenAll(
            alpha.SendAsync(new MercuryMemory(alphaFrame)),
            bravo.SendAsync(new MercuryMemory(bravoFrame)));

        var alphaReceived = await alpha.ReceiveAsync();
        var bravoReceived = await bravo.ReceiveAsync();

        Assert.Equal(bravoFrame, alphaReceived.ToArray());
        Assert.Equal(alphaFrame, bravoReceived.ToArray());
    }

    /// <summary>
    /// Defines the test method ConcurrentReceivers_ConsumeFrameOnlyOnce.
    /// </summary>
    [Fact]
    public async Task ConcurrentReceivers_ConsumeFrameOnlyOnce()
    {
        using var directories = new TestDirectories();
        Directory.CreateDirectory(directories.InboxPath);
        var first = new FileDropTransport(
            directories.InboxPath,
            Path.Combine(directories.RootPath, "first-outbox"));
        var second = new FileDropTransport(
            directories.InboxPath,
            Path.Combine(directories.RootPath, "second-outbox"));
        var expected = new byte[] { 4, 3, 2, 1 };
        var framePath = Path.Combine(
            directories.InboxPath,
            "0000000000000000001-frame" + FRAME_EXTENSION);
        await File.WriteAllBytesAsync(framePath, expected);

        var receiveTasks = new[]
        {
            first.ReceiveAsync(),
            second.ReceiveAsync()
        };
        var results = await Task.WhenAll(receiveTasks);

        var delivered = results
            .Where(result => !result.IsEmpty)
            .ToArray();

        var received = Assert.Single(delivered);
        Assert.Equal(expected, received.ToArray());
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.InboxPath,
                "*" + FRAME_EXTENSION));
        Assert.Empty(
            Directory.EnumerateFiles(
                directories.InboxPath,
                "*" + PROCESSING_EXTENSION));
    }

    /// <summary>
    /// Defines the test method ConnectedPair_OneMegabyteFrame_RoundTripsExactly.
    /// </summary>
    [Fact]
    public async Task ConnectedPair_OneMegabyteFrame_RoundTripsExactly()
    {
        using var directories = new TestDirectories();
        var (alpha, bravo) = directories.BuildConnectedPair();
        var expected = new byte[1024 * 1024];
        Random.Shared.NextBytes(expected);

        await alpha.SendAsync(new MercuryMemory(expected));
        var actual = await bravo.ReceiveAsync();

        Assert.Equal(expected, actual.ToArray());
    }

    /// <summary>
    /// Class TestDirectories. This class cannot be inherited.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    private sealed class TestDirectories : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDirectories"/> class.
        /// </summary>
        public TestDirectories()
        {
            RootPath = Path.Combine(
                Path.GetTempPath(),
                "Mercury.Tests",
                Guid.NewGuid().ToString("N"));
            InboxPath = Path.Combine(RootPath, "inbox");
            OutboxPath = Path.Combine(RootPath, "outbox");

            Directory.CreateDirectory(RootPath);
        }

        /// <summary>
        /// Gets the root path.
        /// </summary>
        /// <value>The root path.</value>
        public string RootPath { get; }

        /// <summary>
        /// Gets the inbox path.
        /// </summary>
        /// <value>The inbox path.</value>
        public string InboxPath { get; }

        /// <summary>
        /// Gets the outbox path.
        /// </summary>
        /// <value>The outbox path.</value>
        public string OutboxPath { get; }

        /// <summary>
        /// Builds the transport.
        /// </summary>
        /// <returns>FileDropTransport.</returns>
        public FileDropTransport BuildTransport()
        {
            return new FileDropTransport(
                InboxPath,
                OutboxPath);
        }

        /// <summary>
        /// Builds the connected pair.
        /// </summary>
        /// <returns>System.ValueTuple&lt;FileDropTransport, FileDropTransport&gt;.</returns>
        public (FileDropTransport Alpha, FileDropTransport Bravo)
            BuildConnectedPair()
        {
            var alphaInbox = Path.Combine(RootPath, "alpha-inbox");
            var bravoInbox = Path.Combine(RootPath, "bravo-inbox");

            return (
                new FileDropTransport(alphaInbox, bravoInbox),
                new FileDropTransport(bravoInbox, alphaInbox));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!Directory.Exists(RootPath))
            {
                return;
            }

            Directory.Delete(RootPath, recursive: true);
        }
    }
}