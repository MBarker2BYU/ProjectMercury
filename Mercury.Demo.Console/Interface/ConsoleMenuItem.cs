// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="ConsoleMenuItem.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.Console.Interface;

/// <summary>
/// Class ConsoleMenuItem. This class cannot be inherited.
/// </summary>
/// <typeparam name="TSelection">The type of the t selection.</typeparam>
internal sealed class ConsoleMenuItem<TSelection>
    where TSelection : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenuItem{TSelection}"/> class.
    /// </summary>
    /// <param name="selection">The selection.</param>
    /// <param name="description">The description.</param>
    /// <param name="isEscapeSelection">if set to <c>true</c> [is escape selection].</param>
    public ConsoleMenuItem(TSelection selection, string description, bool isEscapeSelection = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Selection = selection;
        Description = description;
        IsEscapeSelection = isEscapeSelection;
    }

    /// <summary>
    /// Gets the selection.
    /// </summary>
    /// <value>The selection.</value>
    public TSelection Selection { get; }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is escape selection.
    /// </summary>
    /// <value><c>true</c> if this instance is escape selection; otherwise, <c>false</c>.</value>
    public bool IsEscapeSelection { get; }
}
