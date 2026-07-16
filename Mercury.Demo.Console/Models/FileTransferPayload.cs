// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="FileTransferPayload.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Text;

namespace Mercury.Demo.Console.Models;

/// <summary>
/// Class FileTransferPayload. This class cannot be inherited.
/// </summary>
/// <param name="FileName">Name of the file.</param>
/// <param name="FileBytes">The file bytes.</param>
internal sealed record FileTransferPayload(string FileName, byte[] FileBytes)
{
    /// <summary>
    /// The magic
    /// </summary>
    private const uint MAGIC = 0x4D46494C; // MFIL
    /// <summary>
    /// The version
    /// </summary>
    private const byte VERSION = 1;

    /// <summary>
    /// Encodes this instance.
    /// </summary>
    /// <returns>System.Byte[].</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public byte[] Encode()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(FileName);
        ArgumentNullException.ThrowIfNull(FileBytes);

        var safeFileName = Path.GetFileName(FileName);
        var fileNameBytes = Encoding.UTF8.GetBytes(safeFileName);

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

        writer.Write(MAGIC);
        writer.Write(VERSION);
        writer.Write(fileNameBytes.Length);
        writer.Write(FileBytes.LongLength);
        writer.Write(fileNameBytes);
        writer.Write(FileBytes);
        writer.Flush();

        return stream.ToArray();
    }

    /// <summary>
    /// Decodes the specified payload.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>FileTransferPayload.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidDataException">The received payload is not a Mercury file transfer.</exception>
    /// <exception cref="InvalidDataException">The Mercury file-transfer version is not supported.</exception>
    /// <exception cref="InvalidDataException">The received file name is invalid.</exception>
    /// <exception cref="InvalidDataException">The received file size is not supported by this demo host.</exception>
    /// <exception cref="InvalidDataException">The received file-transfer payload contains trailing data.</exception>
    /// <exception cref="InvalidDataException">The received file name is empty.</exception>
    /// <exception cref="EndOfStreamException">The received file name is incomplete.</exception>
    /// <exception cref="EndOfStreamException">The received file is incomplete.</exception>
    public static FileTransferPayload Decode(byte[] payload)
    {
        ArgumentNullException.ThrowIfNull(payload);

        using var stream = new MemoryStream(payload, writable: false);
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: false);

        if (reader.ReadUInt32() != MAGIC)
            throw new InvalidDataException("The received payload is not a Mercury file transfer.");

        if (reader.ReadByte() != VERSION)
            throw new InvalidDataException("The Mercury file-transfer version is not supported.");

        var fileNameLength = reader.ReadInt32();
        var fileLength = reader.ReadInt64();

        if (fileNameLength <= 0 || fileNameLength > 4096)
            throw new InvalidDataException("The received file name is invalid.");

        if (fileLength < 0 || fileLength > int.MaxValue)
            throw new InvalidDataException("The received file size is not supported by this demo host.");

        var fileNameBytes = reader.ReadBytes(fileNameLength);

        if (fileNameBytes.Length != fileNameLength)
            throw new EndOfStreamException("The received file name is incomplete.");

        var fileBytes = reader.ReadBytes((int)fileLength);

        if (fileBytes.LongLength != fileLength)
            throw new EndOfStreamException("The received file is incomplete.");

        if (stream.Position != stream.Length)
            throw new InvalidDataException("The received file-transfer payload contains trailing data.");

        var fileName = Path.GetFileName(Encoding.UTF8.GetString(fileNameBytes));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new InvalidDataException("The received file name is empty.");

        return new FileTransferPayload(fileName, fileBytes);
    }
}
