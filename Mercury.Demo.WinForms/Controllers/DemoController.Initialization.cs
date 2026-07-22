// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.Initialization.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.Demo;

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController
{
    /// <summary>
    /// Initialize as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        try
        {
            UpdateView(view => view.SetBusy(true));

            m_View.BindConfigurationOptions();

            await ConfigureViewAsync();
        }
        catch (Exception exception)
        {
            m_View.DisplayError(exception.Message);
        }
        finally
        {
            UpdateView(view => view.SetBusy(false));
        }
    }

    /// <summary>
    /// Apply configuration as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task ApplyConfigurationAsync()
    {
        try
        {
            UpdateView(view => view.SetBusy(true));

            await ConfigureViewAsync();
        }
        catch (Exception exception)
        {
            m_View.DisplayError(exception.Message);
        }
        finally
        {
            UpdateView(view => view.SetBusy(false));
        }
    }

    /// <summary>
    /// Configure view as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task ConfigureViewAsync()
    {
        var configuration =
            m_View.BuildConfiguration();

        var result =
            await ConfigureAsync(configuration);

        m_View.ApplyConfiguration(result.Configuration, result.IsConnected);

        var tcpEnabled =
            result.Configuration.Transport ==
            DemoConstants.TCP_TRANSPORT;

        m_View.SetTcpAttackControlsEnabled(
            tcpEnabled);
    }

    /// <summary>
    /// Form closing as an asynchronous operation.
    /// </summary>
    /// <param name="eventArgs">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task FormClosingAsync(FormClosingEventArgs eventArgs)
    {
        if (m_IsClosing)
            return;

        if (!m_View.ConfirmClose())
        {
            eventArgs.Cancel = true;
            return;
        }

        eventArgs.Cancel = true;
        m_IsClosing = true;

        try
        {
            await DisposeAsync();

            m_View.CloseWindow();
        }
        catch (Exception exception)
        {
            m_IsClosing = false;

            m_View.DisplayError(exception.Message);
        }
    }
}