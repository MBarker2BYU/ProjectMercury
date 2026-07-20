// ***********************************************************************
// Assembly       : Mercury.Demo
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="MercuryGlassComboBox.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.ComponentModel;
using System.Drawing.Drawing2D;
using Mercury.Demo.WinForms.Controls.Enums;
using Mercury.Demo.WinForms.Controls.Rendering;

namespace Mercury.Demo.WinForms.Controls;

/// <summary>
/// Displays a custom glass-style drop-down list.
/// </summary>
[DefaultEvent(nameof(SelectedIndexChanged))]
public sealed partial class MercuryGlassComboBox : UserControl
{
    private const int HOVER_ALPHA_INCREASE = 20;
    private const int OPEN_ALPHA_INCREASE = 35;

    private readonly ListBox m_ListBox;
    private readonly ToolStripControlHost m_ListHost;
    private readonly ToolStripDropDown m_DropDown;

    private bool m_MouseOver;
    private bool m_DropDownOpen;

    private Color m_AccentColor =
        Color.FromArgb(78, 82, 87);

    private Color m_SelectionColor =
        Color.FromArgb(55, 145, 235);

    private int m_FillAlpha = 255;
    private int m_CornerSize = 6;
    private int m_BorderWidth = 1;
    private int m_DropDownHeight = 200;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MercuryGlassComboBox"/> class.
    /// </summary>
    public MercuryGlassComboBox()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint |
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.Selectable,
            true);

        DoubleBuffered = true;

        BackColor = Color.FromArgb(42, 42, 46);
        ForeColor = Color.FromArgb(225, 232, 238);

        Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Regular);

        Cursor = Cursors.Hand;
        Padding = new Padding(12, 0, 38, 0);
        Size = new Size(230, 35);
        TabStop = true;

        m_ListBox = new ListBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(32, 35, 39),
            ForeColor = ForeColor,
            Font = Font,
            IntegralHeight = false,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 28
        };

        m_ListBox.DrawItem += ListBox_DrawItem;
        m_ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
        m_ListBox.MouseClick += ListBox_MouseClick;

        m_ListHost = new ToolStripControlHost(m_ListBox)
        {
            AutoSize = false,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };

        m_DropDown = new ToolStripDropDown
        {
            AutoSize = false,
            Padding = Padding.Empty,
            Margin = Padding.Empty,
            DropShadowEnabled = true
        };

        m_DropDown.Items.Add(m_ListHost);

        m_DropDown.Closed += DropDown_Closed;

        if (m_ListBox.Items.Count > 0)
        {
            m_ListBox.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Occurs when the selected item changes.
    /// </summary>
    public event EventHandler? SelectedIndexChanged;

    /// <summary>
    /// Gets the items contained in the drop-down list.
    /// </summary>
    [Category("Data")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Content)]
    public ListBox.ObjectCollection Items =>
        m_ListBox.Items;

    /// <summary>
    /// Gets or sets the selected item index.
    /// </summary>
    [Category("Data")]
    [DefaultValue(-1)]
    public int SelectedIndex
    {
        get => m_ListBox.SelectedIndex;
        set
        {
            if (m_ListBox.SelectedIndex == value)
                return;

            m_ListBox.SelectedIndex = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Hidden)]
    public object? SelectedItem
    {
        get => m_ListBox.SelectedItem;
        set
        {
            if (Equals(m_ListBox.SelectedItem, value))
                return;

            m_ListBox.SelectedItem = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the color used by the border.
    /// </summary>
    [Category("Mercury")]
    [DefaultValue(typeof(Color), "78, 82, 87")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color AccentColor
    {
        get => m_AccentColor;
        set
        {
            if (m_AccentColor == value)
                return;

            m_AccentColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the drop-down selection color.
    /// </summary>
    [Category("Mercury")]
    [DefaultValue(typeof(Color), "55, 145, 235")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color SelectionColor
    {
        get => m_SelectionColor;
        set
        {
            if (m_SelectionColor == value)
                return;

            m_SelectionColor = value;
            m_ListBox.Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the background alpha.
    /// </summary>
    [Category("Mercury")]
    [DefaultValue(255)]
    public int FillAlpha
    {
        get => m_FillAlpha;
        set
        {
            var alpha =
                Math.Clamp(value, 0, 255);

            if (m_FillAlpha == alpha)
                return;

            m_FillAlpha = alpha;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the corner size.
    /// </summary>
    [Category("Mercury")]
    [DefaultValue(6)]
    public int CornerSize
    {
        get => m_CornerSize;
        set
        {
            var cornerSize =
                Math.Max(0, value);

            if (m_CornerSize == cornerSize)
                return;

            m_CornerSize = cornerSize;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the border width.
    /// </summary>
    [Category("Mercury")]
    [DefaultValue(1)]
    public int BorderWidth
    {
        get => m_BorderWidth;
        set
        {
            var borderWidth =
                Math.Max(0, value);

            if (m_BorderWidth == borderWidth)
                return;

            m_BorderWidth = borderWidth;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the maximum drop-down height.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(200)]
    public int DropDownHeight
    {
        get => m_DropDownHeight;
        set => m_DropDownHeight = Math.Max(40, value);
    }

    /// <summary>
    /// Gets the display text.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Text
    {
        get
        {
            if (m_ListBox.SelectedIndex >= 0)
            {
                return m_ListBox.GetItemText(m_ListBox.SelectedItem);
            }
            return m_ListBox.Items.Count > 0
                ? m_ListBox.GetItemText(m_ListBox.Items[0])
                : string.Empty;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                return;

            var index = m_ListBox.FindStringExact(value);
            if (index >= 0)
            {
                m_ListBox.SelectedIndex = index;
            }
            Invalidate();
        }
    }

    /// <summary>
    /// Paints the control.
    /// </summary>
    /// <param name="paintEvent">
    /// The paint event arguments.
    /// </param>
    protected override void OnPaint(
        PaintEventArgs paintEvent)
    {
        var graphics =
            paintEvent.Graphics;

        var bounds =
            ClientRectangle;

        bounds.Width -= 1;
        bounds.Height -= 1;

        if (bounds.Width <= 0 ||
            bounds.Height <= 0)
        {
            return;
        }

        var smoothingMode =
            graphics.SmoothingMode;

        try
        {
            graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            graphics.ClearBackground(
                ClientRectangle,
                Parent?.BackColor ??
                SystemColors.Control);

            graphics.RenderBackground(
                bounds,
                GetBackgroundColor(),
                CornerStyle.Rounded,
                RenderedCorners.All,
                m_CornerSize);

            graphics.RenderBorder(
                bounds,
                GetBorderColor(),
                m_BorderWidth,
                CornerStyle.Rounded,
                RenderedCorners.All,
                m_CornerSize);

            RenderSelectedText(
                graphics,
                bounds);

            RenderArrow(
                graphics,
                bounds);
        }
        finally
        {
            graphics.SmoothingMode =
                smoothingMode;
        }
    }

    /// <summary>
    /// Opens or closes the drop-down list.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnClick(
        EventArgs eventArgs)
    {
        base.OnClick(eventArgs);

        Focus();

        if (m_DropDownOpen)
        {
            m_DropDown.Close();
        }
        else
        {
            ShowDropDown();
        }
    }

    /// <summary>
    /// Handles mouse entry.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnMouseEnter(
        EventArgs eventArgs)
    {
        base.OnMouseEnter(eventArgs);

        m_MouseOver = true;
        Invalidate();
    }

    /// <summary>
    /// Handles mouse exit.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnMouseLeave(
        EventArgs eventArgs)
    {
        base.OnMouseLeave(eventArgs);

        m_MouseOver = false;
        Invalidate();
    }

    /// <summary>
    /// Handles keyboard navigation.
    /// </summary>
    /// <param name="keyEvent">
    /// The key event arguments.
    /// </param>
    protected override void OnKeyDown(
        KeyEventArgs keyEvent)
    {
        base.OnKeyDown(keyEvent);

        if (keyEvent.Alt &&
            keyEvent.KeyCode == Keys.Down)
        {
            ShowDropDown();
            keyEvent.Handled = true;
            return;
        }

        switch (keyEvent.KeyCode)
        {
            case Keys.Enter:
            case Keys.Space:
                ShowDropDown();
                keyEvent.Handled = true;
                break;

            case Keys.Up:
                MoveSelection(-1);
                keyEvent.Handled = true;
                break;

            case Keys.Down:
                MoveSelection(1);
                keyEvent.Handled = true;
                break;

            case Keys.Escape:
                m_DropDown.Close();
                keyEvent.Handled = true;
                break;
        }
    }

    /// <summary>
    /// Handles font changes.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnFontChanged(
        EventArgs eventArgs)
    {
        base.OnFontChanged(eventArgs);

        if (m_ListBox != null)
        {
            m_ListBox.Font = Font;
        }

        Invalidate();
    }

    /// <summary>
    /// Handles foreground-color changes.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnForeColorChanged(
        EventArgs eventArgs)
    {
        base.OnForeColorChanged(eventArgs);

        if (m_ListBox != null)
        {
            m_ListBox.ForeColor = ForeColor;
        }

        Invalidate();
    }

    /// <summary>
    /// Handles focus changes.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnGotFocus(
        EventArgs eventArgs)
    {
        base.OnGotFocus(eventArgs);
        Invalidate();
    }

    /// <summary>
    /// Handles focus changes.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnLostFocus(
        EventArgs eventArgs)
    {
        base.OnLostFocus(eventArgs);
        Invalidate();
    }

    /// <summary>
    /// Releases owned resources.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> to release managed resources.
    /// </param>
    protected override void Dispose(
        bool disposing)
    {
        if (disposing)
        {
            m_DropDown.Dispose();
            m_ListHost.Dispose();
            m_ListBox.Dispose();
        }

        base.Dispose(disposing);
    }

    private void ShowDropDown()
    {
        if (m_DropDownOpen ||
            m_ListBox.Items.Count == 0)
        {
            return;
        }

        var visibleItems =
            Math.Min(
                m_ListBox.Items.Count,
                8);

        var listHeight =
            Math.Min(
                m_DropDownHeight,
                (visibleItems *
                 m_ListBox.ItemHeight) + 2);

        m_ListBox.Size =
            new Size(
                Width,
                listHeight);

        m_ListHost.Size =
            m_ListBox.Size;

        m_DropDown.Size =
            m_ListBox.Size;

        m_DropDownOpen = true;

        m_DropDown.Show(
            this,
            new Point(
                0,
                Height + 2));

        m_ListBox.Focus();
        Invalidate();
    }

    private void MoveSelection(
        int amount)
    {
        if (m_ListBox.Items.Count == 0)
            return;

        var index =
            m_ListBox.SelectedIndex;

        if (index < 0)
        {
            index = amount > 0
                ? 0
                : m_ListBox.Items.Count - 1;
        }
        else
        {
            index =
                Math.Clamp(
                    index + amount,
                    0,
                    m_ListBox.Items.Count - 1);
        }

        m_ListBox.SelectedIndex = index;
    }

    private void ListBox_DrawItem(
        object? sender,
        DrawItemEventArgs drawItemEvent)
    {
        if (drawItemEvent.Index < 0)
            return;

        var selected =
            (drawItemEvent.State &
             DrawItemState.Selected) ==
            DrawItemState.Selected;

        var backgroundColor =
            selected
                ? Color.FromArgb(
                    70,
                    m_SelectionColor)
                : m_ListBox.BackColor;

        using (var brush =
               new SolidBrush(backgroundColor))
        {
            drawItemEvent.Graphics.FillRectangle(
                brush,
                drawItemEvent.Bounds);
        }

        if (selected)
        {
            using var accentBrush =
                new SolidBrush(m_SelectionColor);

            drawItemEvent.Graphics.FillRectangle(
                accentBrush,
                drawItemEvent.Bounds.Left,
                drawItemEvent.Bounds.Top,
                3,
                drawItemEvent.Bounds.Height);
        }

        var itemText =
            m_ListBox.GetItemText(
                m_ListBox.Items[
                    drawItemEvent.Index]);

        var textBounds =
            new Rectangle(
                drawItemEvent.Bounds.Left + 10,
                drawItemEvent.Bounds.Top,
                drawItemEvent.Bounds.Width - 20,
                drawItemEvent.Bounds.Height);

        drawItemEvent.Graphics.RenderText(
            itemText,
            Font,
            textBounds,
            ContentAlignment.MiddleLeft
                .ToTextFormatFlags(),
            ForeColor,
            SystemColors.GrayText,
            Enabled);
    }

    private void ListBox_SelectedIndexChanged(
        object? sender,
        EventArgs eventArgs)
    {
        SelectedIndexChanged?.Invoke(
            this,
            EventArgs.Empty);

        Invalidate();
    }

    private void ListBox_MouseClick(
        object? sender,
        MouseEventArgs mouseEvent)
    {
        if (mouseEvent.Button !=
            MouseButtons.Left)
        {
            return;
        }

        m_DropDown.Close();
    }

    private void DropDown_Closed(
        object? sender,
        ToolStripDropDownClosedEventArgs eventArgs)
    {
        m_DropDownOpen = false;

        Focus();
        Invalidate();
    }

    private void RenderSelectedText(
        Graphics graphics,
        Rectangle bounds)
    {
        var textBounds =
            new Rectangle(
                bounds.Left + Padding.Left,
                bounds.Top,
                bounds.Width -
                Padding.Horizontal,
                bounds.Height);

        graphics.RenderText(
            Text,
            Font,
            textBounds,
            ContentAlignment.MiddleLeft
                .ToTextFormatFlags(),
            ForeColor,
            SystemColors.GrayText,
            Enabled);
    }

    private void RenderArrow(
        Graphics graphics,
        Rectangle bounds)
    {
        var centerX =
            bounds.Right - 16;

        var centerY =
            bounds.Top +
            (bounds.Height / 2);

        var arrowColor =
            Enabled
                ? ForeColor
                : SystemColors.GrayText;

        using var arrowPen =
            new Pen(
                arrowColor,
                2F)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

        if (m_DropDownOpen)
        {
            graphics.DrawLine(
                arrowPen,
                centerX - 5,
                centerY + 2,
                centerX,
                centerY - 3);

            graphics.DrawLine(
                arrowPen,
                centerX,
                centerY - 3,
                centerX + 5,
                centerY + 2);
        }
        else
        {
            graphics.DrawLine(
                arrowPen,
                centerX - 5,
                centerY - 2,
                centerX,
                centerY + 3);

            graphics.DrawLine(
                arrowPen,
                centerX,
                centerY + 3,
                centerX + 5,
                centerY - 2);
        }
    }

    private Color GetBackgroundColor()
    {
        var alpha =
            m_FillAlpha;

        if (m_DropDownOpen)
        {
            alpha += OPEN_ALPHA_INCREASE;
        }
        else if (m_MouseOver)
        {
            alpha += HOVER_ALPHA_INCREASE;
        }

        return Color.FromArgb(
            Math.Min(alpha, 255),
            BackColor);
    }

    private Color GetBorderColor()
    {
        if (!Enabled)
        {
            return SystemColors.GrayText;
        }

        if (Focused ||
            m_MouseOver ||
            m_DropDownOpen)
        {
            return m_SelectionColor;
        }

        return m_AccentColor;
    }
}