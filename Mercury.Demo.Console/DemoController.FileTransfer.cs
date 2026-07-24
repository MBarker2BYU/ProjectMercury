// ***********************************************************************
// Assembly     : Mercury.Demo.Console
// Author         : Matthew D. Barker
// Created        : 07-16-2026
//
// ***********************************************************************
// <copyright file="DemoController.FileTransfer">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
//
// ***********************************************************************

using Mercury.Demo.Console.Interface;
using Mercury.Demo.Console.Models;
using System.Security.Cryptography;

using Terminal = System.Console;

namespace Mercury.Demo.Console;


/// <summary>
/// Coordinates the Mercury cross-platform demonstration host.
/// </summary>
internal sealed partial class DemoController
{
    /// <summary>
    /// Run protected file transfer as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidDataException">The received file hash does not match the source file hash.</exception>
    private async Task RunProtectedFileTransferAsync()
    {
        ConsoleScreen.Clear();
        ConsoleScreen.DisplayHeader();
        ConsoleScreen.WriteSection("Protected File Transfer");

        var sourcePath = ReadExistingFilePath();
        var started = DateTimeOffset.Now;
        var sourceFileName = Path.GetFileName(sourcePath);
        var sourceFileBytes = await File.ReadAllBytesAsync(sourcePath);
        var sourceHash = SHA256.HashData(sourceFileBytes);
        var transferPayload = new FileTransferPayload(
            sourceFileName,
            sourceFileBytes).Encode();

        try
        {
            Terminal.WriteLine();
            await ApplyConfigurationAsync();

            ConsoleScreen.WriteStatus("ALPHA", "FILE", $"Loaded {sourceFileName} ({sourceFileBytes.LongLength:N0} bytes)",
                ConsoleTheme.SUCCESS);

            ConsoleScreen.WriteStatus("ALPHA", "HASH", Convert.ToHexString(sourceHash),
                ConsoleTheme.MUTED);

            var result = await ExecuteExchangeAsync(transferPayload);

            DisplayTransportInspection(sourceFileBytes);

            if (!result.Success)
            {
                throw new InvalidOperationException(FormatFailure(result));
            }

            DisplayValidatedEnvelope(result);

            var receivedFile = FileTransferPayload.Decode(result.Payload.ToArray());
            var receivedHash = SHA256.HashData(receivedFile.FileBytes);
            var hashesMatch = CryptographicOperations.FixedTimeEquals(sourceHash, receivedHash);

            if (!hashesMatch)
                throw new InvalidDataException("The received file hash does not match the source file hash.");

            var destinationPath = BuildReceivedFilePath(sourcePath, receivedFile.FileName);

            await File.WriteAllBytesAsync(destinationPath, receivedFile.FileBytes);

            ConsoleScreen.WriteStatus("BRAVO", "OK", "SecureEnvelope received and validated",
                ConsoleTheme.SUCCESS);

            ConsoleScreen.WriteStatus("BRAVO", "FILE",
                $"Restored {receivedFile.FileName} ({receivedFile.FileBytes.LongLength:N0} bytes)",
                ConsoleTheme.SUCCESS);

            ConsoleScreen.WriteStatus("BRAVO", "HASH",
                Convert.ToHexString(receivedHash),
                ConsoleTheme.MUTED);

            Terminal.WriteLine();
            
            ConsoleTheme.WriteLine("  RESULT: FILE VERIFIED", ConsoleTheme.SUCCESS);
            ConsoleScreen.WriteLabel("Saved To", destinationPath, ConsoleTheme.PRIMARY);
            ConsoleScreen.WriteLabel("Hash Match", "YES", ConsoleTheme.SUCCESS);

            RecordTelemetry(started, "Protected File Transfer",
                transferPayload.Length, true,
                $"{sourceFileName} verified and saved to {destinationPath}");
        }
        catch (Exception exception)
        {
            Terminal.WriteLine();
            ConsoleTheme.WriteLine("  RESULT: FILE TRANSFER FAILURE", ConsoleTheme.FAILURE);
            ConsoleTheme.WriteLine($"  {exception.Message}", ConsoleTheme.FAILURE);

            RecordTelemetry(started, "Protected File Transfer", transferPayload.Length,
                false, exception.Message);
        }
        finally
        {
            ConsoleScreen.WriteSectionEnd();
            ConsoleScreen.Pause();
        }
    }

    /// <summary>
    /// Reads the existing file path.
    /// </summary>
    /// <returns>System.String.</returns>
    private static string ReadExistingFilePath()
    {
        while (true)
        {
            var enteredPath = ConsoleScreen.ReadRequiredLine(
                "  Enter file path (drag and drop is supported): ");

            var normalizedPath = NormalizeEnteredPath(enteredPath);

            if (File.Exists(normalizedPath))
                return Path.GetFullPath(normalizedPath);

            ConsoleTheme.WriteLine(
                "  The selected file does not exist.",
                ConsoleTheme.WARNING);

            Terminal.WriteLine();
        }
    }

    /// <summary>
    /// Normalizes the entered path.
    /// </summary>
    /// <param name="enteredPath">The entered path.</param>
    /// <returns>System.String.</returns>
    private static string NormalizeEnteredPath(string enteredPath)
    {
        var normalizedPath = enteredPath.Trim();

        if (normalizedPath.Length >= 2 &&
            ((normalizedPath[0] == '\"' && normalizedPath[^1] == '\"') ||
             (normalizedPath[0] == '\'' && normalizedPath[^1] == '\'')))
        {
            normalizedPath = normalizedPath[1..^1];
        }

        if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            normalizedPath = normalizedPath
                .Replace("\\ ", " ")
                .Replace("\\(", "(")
                .Replace("\\)", ")");
        }

        return normalizedPath;
    }

    /// <summary>
    /// Builds the received file path.
    /// </summary>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="receivedFileName">Name of the received file.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="IOException">A unique destination file name could not be created.</exception>
    private static string BuildReceivedFilePath(string sourcePath, string receivedFileName)
    {
        var sourceDirectory = Path.GetDirectoryName(sourcePath)
            ?? Environment.CurrentDirectory;

        var outputDirectory = Path.Combine(sourceDirectory, "Mercury-Received");

        Directory.CreateDirectory(outputDirectory);

        var safeFileName = Path.GetFileName(receivedFileName);
        var baseName = Path.GetFileNameWithoutExtension(safeFileName);
        var extension = Path.GetExtension(safeFileName);
        var candidate = Path.Combine(outputDirectory,
            $"{baseName}.received{extension}");

        if (!File.Exists(candidate))
            return candidate;

        for (var sequence = 2; sequence < int.MaxValue; sequence++)
        {
            candidate = Path.Combine(
                outputDirectory,
                $"{baseName}.received-{sequence}{extension}");

            if (!File.Exists(candidate))
                return candidate;
        }

        throw new IOException("A unique destination file name could not be created.");
    }
}
