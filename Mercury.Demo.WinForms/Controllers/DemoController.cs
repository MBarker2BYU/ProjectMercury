// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoController.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.WinForms.Controllers;

/// <summary>
/// Class DemoController. This class cannot be inherited.
/// Implements the <see cref="System.IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IAsyncDisposable" />
internal sealed partial class DemoController : IAsyncDisposable
{

    #region Methods

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoController"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DemoController(
        MainWindow view)
    {
        ArgumentNullException.ThrowIfNull(view);

        m_View = view;

        m_CancellationTokenSource =
            new CancellationTokenSource();

        m_OperationLock =
            new SemaphoreSlim(1, 1);
    }

    #endregion

    /// <summary>
    /// Updates the view.
    /// </summary>
    /// <param name="action">The action.</param>
    private void UpdateView(Action<MainWindow> action)
    {
        m_View.RunOnUiThread(() => action(m_View));
    }

    /// <summary>
    /// Reads the view.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="function">The function.</param>
    /// <returns>T.</returns>
    private T ReadView<T>(Func<MainWindow, T> function)
    {
        return m_View.RunOnUiThread(() => function(m_View));
    }

    #endregion

    #region Property and Fields

    /// <summary>
    /// The m view
    /// </summary>
    private readonly MainWindow m_View;
    /// <summary>
    /// The m cancellation token source
    /// </summary>
    private readonly CancellationTokenSource m_CancellationTokenSource;
    /// <summary>
    /// The m operation lock
    /// </summary>
    private readonly SemaphoreSlim m_OperationLock;

    /// <summary>
    /// The m is closing
    /// </summary>
    private bool m_IsClosing;

    #endregion

    #region IAsyncDisposable Implementation

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.
    /// </summary>
    /// <returns>A Task&lt;ValueTask&gt; representing the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (!m_CancellationTokenSource.IsCancellationRequested)
        {
            await m_CancellationTokenSource
                .CancelAsync()
                .ConfigureAwait(false);
        }

        await m_OperationLock
            .WaitAsync()
            .ConfigureAwait(false);

        try
        {
            await DisposeTransportsAsync()
                .ConfigureAwait(false);
        }
        finally
        {
            m_OperationLock.Release();
        }

        m_OperationLock.Dispose();
        m_CancellationTokenSource.Dispose();
    }

    #endregion
}