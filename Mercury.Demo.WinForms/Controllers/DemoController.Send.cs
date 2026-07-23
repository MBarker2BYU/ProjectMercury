// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.Send.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Enums;
using System.Text;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// The m alpha client
    /// </summary>
    private IMercuryClient? m_AlphaClient;
    /// <summary>
    /// The m bravo client
    /// </summary>
    private IMercuryClient? m_BravoClient;

    #region Methods

    /// <summary>
    /// Send once as an asynchronous operation.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IMercuryResult&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Mercury has not been configured.</exception>
    private async Task<IMercuryResult> SendOnceAsync(ReadOnlyMemory payload, CancellationToken cancellationToken)
    {
        if (m_AlphaClient is null ||
            m_BravoClient is null)
        {
            throw new InvalidOperationException(@"Mercury has not been configured.");
        }

        ICryptoContext context;

        if (m_WrongKeyEnabled)
        {
            context =
                MercuryFactory.Instance.BuildCryptoContext(DemoConstants.ALPHA_NODE, DemoConstants.CHARLIE_NODE);

            Log("WARN", "Sending with wrong key");
        }
        else
        {
            context =
                MercuryFactory.Instance.BuildCryptoContext(DemoConstants.ALPHA_NODE, DemoConstants.BRAVO_NODE);
        }

        Log("INFO", $"Sending {payload.Length:N0} payload bytes");

        var receiveTask =
            m_BravoClient.ReceiveAsync(cancellationToken);

        var sendTask =
            m_AlphaClient.SendAsync(context, payload, cancellationToken);

        await Task
            .WhenAll(receiveTask, sendTask)
            .ConfigureAwait(false);

        var result =
            await receiveTask
                .ConfigureAwait(false);

        Log(
            result.Success
                ? "INFO"
                : "ERROR",
            result.Success
                ? "Payload received and authenticated"
                : result.Message ??
                  result.FailureReason.ToString());

        return result;
    }

    /// <summary>
    /// Sends one Mercury exchange asynchronously.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>The Mercury result.</returns>
    /// <exception cref="ArgumentException">
    /// Payload must not be empty.
    /// </exception>
    public async Task<IMercuryResult> SendAsync(
        ReadOnlyMemory payload)
    {
        if (payload.IsEmpty)
        {
            throw new ArgumentException(
                @"Payload must not be empty.",
                nameof(payload));
        }

        var cancellationToken =
            m_CancellationTokenSource.Token;

        await m_OperationLock
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            return await SendOnceAsync(
                    payload,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            m_OperationLock.Release();
        }
    }

    /// <summary>
    /// Executes the payload-send demonstration asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendPayloadAsync()
    {
        try
        {
            UpdateView(
                view => view.SetBusy(true));

            var value =
                ReadView(
                    view => view.GetSendPayload());

            var payload =
                new ReadOnlyMemory(
                    Encoding.UTF8.GetBytes(value));

            var attackMode =
                CurrentAttackMode;

            /*
             * First exchange:
             * This must be displayed immediately. During the replay
             * demonstration, this is the valid frame that Mercury accepts.
             */
            var firstResult =
                await SendAsync(payload);

            if (!firstResult.Success)
            {
                var failureMessage =
                    GetFailureMessage(firstResult);

                UpdateView(
                    view => view.DisplayFailure(
                        failureMessage,
                        attackMode));

                ClearReplayFrame();

                return;
            }

            var firstEnvelope =
                BuildEnvelopeDisplay(firstResult);

            var recoveredPayload =
                Encoding.UTF8.GetString(
                    firstResult.Payload.ToArray());

            UpdateView(
                view => view.DisplaySuccessfulPayload(
                    recoveredPayload,
                    firstResult.Payload.Length,
                    firstEnvelope));

            if (attackMode != DemoAttackMode.Replay)
                return;

            try
            {
                /*
                 * Allow the successful first exchange to remain visible
                 * before the captured frame is submitted again.
                 */
                await Task.Delay(
                    500,
                    m_CancellationTokenSource.Token);

                var replayResult =
                    await SendAsync(payload);

                if (!replayResult.Success)
                {
                    var failureMessage =
                        GetFailureMessage(replayResult);

                    UpdateView(
                        view => view.DisplayFailure(
                            failureMessage,
                            DemoAttackMode.Replay));

                    return;
                }

                /*
                 * This should not normally happen. If the second frame is
                 * accepted, show the result honestly instead of manufacturing
                 * a replay failure.
                 */
                var replayEnvelope =
                    BuildEnvelopeDisplay(replayResult);

                var replayPayload =
                    Encoding.UTF8.GetString(
                        replayResult.Payload.ToArray());

                UpdateView(
                    view => view.DisplaySuccessfulPayload(
                        replayPayload,
                        replayResult.Payload.Length,
                        replayEnvelope));
            }
            finally
            {
                ClearReplayFrame();
            }
        }
        catch (OperationCanceledException)
            when (m_CancellationTokenSource.IsCancellationRequested)
        {
            // Application shutdown.
        }
        catch (Exception exception)
        {
            UpdateView(
                view => view.DisplayError(
                    exception.Message));
        }
        finally
        {
            UpdateView(
                view => view.SetBusy(false));
        }
    }

    /// <summary>
    /// Send file as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task SendFileAsync()
    {
        try
        {
            var filePath =
                ReadView(
                    view => view.SelectImageFile());

            if (string.IsNullOrWhiteSpace(filePath))
                return;

            UpdateView(
                view => view.SetBusy(true));

            var fileName =
                Path.GetFileName(filePath);

            var payload =
                await File.ReadAllBytesAsync(
                    filePath,
                    m_CancellationTokenSource.Token);

            Log(
                "INFO",
                $"Sending image {fileName} | " +
                FormatFileSize(payload.Length));

            var attackMode =
                CurrentAttackMode;

            var result =
                await SendAsync(
                    new ReadOnlyMemory(payload));

            if (!result.Success)
            {
                var failureMessage =
                    GetFailureMessage(result);

                UpdateView(
                    view => view.DisplayFailure(
                        failureMessage,
                        attackMode));

                return;
            }

            var envelopeDisplay =
                BuildEnvelopeDisplay(result);

            UpdateView(
                view => view.DisplaySuccessfulFile(
                    fileName,
                    result.Payload,
                    envelopeDisplay));
        }
        catch (Exception exception)
        {
            UpdateView(
                view => view.DisplayError(
                    exception.Message));
        }
        finally
        {
            UpdateView(
                view => view.SetBusy(false));
        }
    }

    /// <summary>
    /// Formats the size of the file.
    /// </summary>
    /// <param name="byteCount">The byte count.</param>
    /// <returns>System.String.</returns>
    private static string FormatFileSize(long byteCount)
    {
        const double kilobyte = 1024;
        const double megabyte =
            kilobyte * 1024;

        if (byteCount >= megabyte)
            return $"{byteCount / megabyte:N2} MB";


        return byteCount >= kilobyte 
            ? $"{byteCount / kilobyte:N2} KB" 
            : $"{byteCount:N0} bytes";
    }

    #endregion
}