// ***********************************************************************
// Assembly       : Mercury.Demo.Console
// Author           : Matthew D. Barker
// Created          : 07-16-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-16-2026
// ***********************************************************************
// <copyright file="ConsoleMenu.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Terminal = System.Console;

namespace Mercury.Demo.Console.Interface;

/// <summary>
/// Class ConsoleMenu. This class cannot be inherited.
/// </summary>
/// <typeparam name="TSelection">The type of the t selection.</typeparam>
internal sealed class ConsoleMenu<TSelection>
    where TSelection : notnull
{
    /// <summary>
    /// The m title
    /// </summary>
    private readonly string m_Title;
    /// <summary>
    /// The m items
    /// </summary>
    private readonly IReadOnlyList<ConsoleMenuItem<TSelection>> m_Items;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenu{TSelection}"/> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="items">The items.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException">A console menu requires at least one item. - items</exception>
    public ConsoleMenu(
        string title,
        params ConsoleMenuItem<TSelection>[] items)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(items);

        if (items.Length == 0)
        {
            throw new ArgumentException(
                "A console menu requires at least one item.",
                nameof(items));
        }

        m_Title = title;
        m_Items = items;
    }

    /// <summary>
    /// Displays this instance.
    /// </summary>
    /// <returns>TSelection.</returns>
    public TSelection Display()
    {
        var selectedIndex = 0;
        var menuTop = Terminal.CursorTop;

        Terminal.CursorVisible = false;

        try
        {
            while (true)
            {
                Draw(menuTop, selectedIndex);

                var key = Terminal.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex == 0
                            ? m_Items.Count - 1
                            : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex == m_Items.Count - 1
                            ? 0
                            : selectedIndex + 1;
                        break;

                    case ConsoleKey.Home:
                        selectedIndex = 0;
                        break;

                    case ConsoleKey.End:
                        selectedIndex = m_Items.Count - 1;
                        break;

                    case ConsoleKey.Enter:
                        return m_Items[selectedIndex].Selection;

                    case ConsoleKey.Escape:
                    {
                        var escapeItem = m_Items.FirstOrDefault(item => item.IsEscapeSelection);

                        if (escapeItem is not null)
                            return escapeItem.Selection;

                        break;
                    }
                }
            }
        }
        finally
        {
            Terminal.ResetColor();
            Terminal.CursorVisible = true;
        }
    }

    /// <summary>
    /// Draws the specified menu top.
    /// </summary>
    /// <param name="menuTop">The menu top.</param>
    /// <param name="selectedIndex">Index of the selected.</param>
    private void Draw(int menuTop, int selectedIndex)
    {
        Terminal.SetCursorPosition(0, menuTop);
        ClearMenuArea();
        Terminal.SetCursorPosition(0, menuTop);

        ConsoleScreen.WriteSection(m_Title);

        for (var index = 0; index < m_Items.Count; index++)
            DrawItem(m_Items[index], index == selectedIndex);

        ConsoleScreen.WriteSectionEnd();
        ConsoleScreen.DisplayFooter();
    }

    /// <summary>
    /// Draws the item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="selected">if set to <c>true</c> [selected].</param>
    private void DrawItem(ConsoleMenuItem<TSelection> item, bool selected)
    {
        var marker = selected ? " ▶ " : "   ";
        var availableWidth = Math.Max(1, ConsoleScreen.Width - marker.Length - 2);
        var description = item.Description;

        if (description.Length > availableWidth)
            description = description[..availableWidth];

        Terminal.BackgroundColor = selected
            ? ConsoleTheme.HIGHLIGHT_BACKGROUND
            : ConsoleTheme.BACKGROUND;

        Terminal.ForegroundColor = selected
            ? ConsoleTheme.HIGHLIGHT_FOREGROUND
            : ConsoleTheme.NORMAL;

        var line = $"{marker}{description}".PadRight(ConsoleScreen.Width - 1);
        Terminal.WriteLine(line);

        Terminal.BackgroundColor = ConsoleTheme.BACKGROUND;
        Terminal.ForegroundColor = ConsoleTheme.NORMAL;
    }

    /// <summary>
    /// Clears the menu area.
    /// </summary>
    private void ClearMenuArea()
    {
        var requiredLines = m_Items.Count + 7;

        for (var line = 0; line < requiredLines; line++)
        {
            Terminal.Write(new string(' ', ConsoleScreen.Width));
            Terminal.WriteLine();
        }
    }
}
