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

namespace Mercury.Transport.FileDrop.Tests;

public sealed class FileDropTransportTests : IDisposable
{
    private readonly string m_RootPath =
        Path.Combine(
            Path.GetTempPath(),
            "Mercury.FileDrop.Tests",
            Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task SendAsyncCreatesOnePublishedFrameFile()
    {
        var inboxPath =
            Path.Combine(m_RootPath, "inbox");

        var outboxPath =
            Path.Combine(m_RootPath, "outbox");

        var transport =
            new FileDropTransport(inboxPath, outboxPath);

        byte[] frame = [0x00, 0x7F, 0x80, 0xFF];

        await transport.SendAsync(frame);

        var files =
            Directory.GetFiles(outboxPath, "*.mercury");

        Assert.Single(files);

        Assert.Equal(
            frame,
            await File.ReadAllBytesAsync(files[0]));

        Assert.Empty(
            Directory.GetFiles(outboxPath, "*.tmp"));
    }

    [Fact]
    public async Task ReceiveAsyncReturnsExactlyTheSentBytes()
    {
        var sideAPath =
            Path.Combine(m_RootPath, "side-a");

        var sideBPath =
            Path.Combine(m_RootPath, "side-b");

        var sender =
            new FileDropTransport(sideAPath, sideBPath);

        var receiver =
            new FileDropTransport(sideBPath, sideAPath);

        byte[] frame =
            [0x01, 0x02, 0x00, 0xFE, 0xFF];

        await sender.SendAsync(frame);

        var received =
            await receiver.ReceiveAsync();

        Assert.Equal(frame, received.ToArray());
    }

    [Fact]
    public async Task ReceiveAsyncReturnsEmptyMemoryForEmptyInbox()
    {
        var transport =
            new FileDropTransport(
                Path.Combine(m_RootPath, "inbox"),
                Path.Combine(m_RootPath, "outbox"));

        var received =
            await transport.ReceiveAsync();

        Assert.True(received.IsEmpty);
    }

    [Fact]
    public async Task ReceiveAsyncRemovesTheConsumedFrameFile()
    {
        var inboxPath =
            Path.Combine(m_RootPath, "inbox");

        var outboxPath =
            Path.Combine(m_RootPath, "outbox");

        var sender =
            new FileDropTransport(outboxPath, inboxPath);

        var receiver =
            new FileDropTransport(inboxPath, outboxPath);

        await sender.SendAsync(
            new byte[] { 1, 2, 3 });

        await receiver.ReceiveAsync();

        Assert.Empty(
            Directory.EnumerateFiles(inboxPath));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_RootPath))
        {
            Directory.Delete(
                m_RootPath,
                recursive: true);
        }
    }
}