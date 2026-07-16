// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="TelemetryStore.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.Console.Models;

namespace Mercury.Demo.Console;

/// <summary>
/// Class TelemetryStore. This class cannot be inherited.
/// </summary>
internal sealed class TelemetryStore
{
    /// <summary>
    /// The m entries
    /// </summary>
    private readonly List<TelemetryEntry> m_Entries = [];

    /// <summary>
    /// Gets the entries.
    /// </summary>
    /// <value>The entries.</value>
    public IReadOnlyList<TelemetryEntry> Entries => m_Entries;

    /// <summary>
    /// Adds the specified entry.
    /// </summary>
    /// <param name="entry">The entry.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Add(TelemetryEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        m_Entries.Add(entry);
    }

    /// <summary>
    /// Clears this instance.
    /// </summary>
    public void Clear()
        => m_Entries.Clear();
}
