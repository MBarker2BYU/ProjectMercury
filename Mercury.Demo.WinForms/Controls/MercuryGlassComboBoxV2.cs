// ***********************************************************************
// Assembly       : Mercury.Demo
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="MercuryGlassComboBoxV2.cs">
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
/// Displays a custom-drawn glass-style combo box.
/// </summary>
public sealed class MercuryGlassComboBoxV2 : ComboBox
{
    private const int WM_PAINT = 0x000F;

    private bool m_MouseOver;

    private Color m_BorderColor =
        Color.FromArgb(78, 82, 87);

    private Color m_AccentColor =
        Color.FromArgb(55, 145, 235);

    private Color m_ArrowColor =
        Color.FromArgb(225, 232, 238);

    private Color m_DropDownBackColor =
        Color.FromArgb(38, 40, 44);

    private int m_CornerSize = 6;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MercuryGlassComboBoxV2"/> class.
    /// </summary>
    public MercuryGlassComboBoxV2()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        DrawMode = DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;
        FlatStyle = FlatStyle.Flat;

        BackColor = Color.FromArgb(42, 42, 46);
        ForeColor = Color.FromArgb(225, 232, 238);

        Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Regular);

        ItemHeight = 26;
        IntegralHeight = false;
        DropDownHeight = 220;

        Size = new Size(230, 34);
        Cursor = Cursors.Hand;
    }

    /// <summary>
    /// Gets or sets the border color.
    /// </summary>
    [DefaultValue(typeof(Color), "78, 82, 87")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color BorderColor
    {
        get => m_BorderColor;
        set
        {
            if (m_BorderColor == value)
                return;

            m_BorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the accent color used for focus and selection.
    /// </summary>
    [DefaultValue(typeof(Color), "55, 145, 235")]
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
    /// Gets or sets the arrow color.
    /// </summary>
    [DefaultValue(typeof(Color), "225, 232, 238")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color ArrowColor
    {
        get => m_ArrowColor;
        set
        {
            if (m_ArrowColor == value)
                return;

            m_ArrowColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the drop-down background color.
    /// </summary>
    [DefaultValue(typeof(Color), "38, 40, 44")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color DropDownBackColor
    {
        get => m_DropDownBackColor;
        set
        {
            if (m_DropDownBackColor == value)
                return;

            m_DropDownBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the corner size.
    /// </summary>
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
    /// Draws an item in the drop-down list.
    /// </summary>
    /// <param name="drawItemEvent">
    /// The draw item event arguments.
    /// </param>
    protected override void OnDrawItem(
        DrawItemEventArgs drawItemEvent)
    {
        if (drawItemEvent.Index < 0)
            return;

        var graphics =
            drawItemEvent.Graphics;

        var bounds =
            drawItemEvent.Bounds;

        var selected =
            (drawItemEvent.State &
             DrawItemState.Selected) ==
            DrawItemState.Selected;

        var backgroundColor =
            selected
                ? Color.FromArgb(
                    70,
                    m_AccentColor)
                : m_DropDownBackColor;

        using (var backgroundBrush =
               new SolidBrush(backgroundColor))
        {
            graphics.FillRectangle(
                backgroundBrush,
                bounds);
        }

        var item =
            GetItemText(
                Items[drawItemEvent.Index]);

        var textBounds =
            new Rectangle(
                bounds.Left + 10,
                bounds.Top,
                bounds.Width - 20,
                bounds.Height);

        graphics.RenderText(
            item,
            Font,
            textBounds,
            ContentAlignment.MiddleLeft
                .ToTextFormatFlags(),
            ForeColor,
            SystemColors.GrayText,
            Enabled);

        if (selected)
        {
            using var accentBrush =
                new SolidBrush(m_AccentColor);

            graphics.FillRectangle(
                accentBrush,
                bounds.Left,
                bounds.Top,
                3,
                bounds.Height);
        }
    }

    /// <summary>
    /// Paints the closed combo box.
    /// </summary>
    /// <param name="message">
    /// The Windows message.
    /// </param>
    protected override void WndProc(
        ref Message message)
    {
        base.WndProc(ref message);

        if (message.Msg != WM_PAINT)
            return;

        using var graphics =
            CreateGraphics();

        PaintComboBox(graphics);
    }

    /// <summary>
    /// Handles pointer entry.
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
    /// Handles pointer exit.
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
    /// Handles selected-item changes.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnSelectedIndexChanged(
        EventArgs eventArgs)
    {
        base.OnSelectedIndexChanged(eventArgs);
        Invalidate();
    }

    /// <summary>
    /// Handles enabled-state changes.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments.
    /// </param>
    protected override void OnEnabledChanged(
        EventArgs eventArgs)
    {
        base.OnEnabledChanged(eventArgs);
        Invalidate();
    }

    private void PaintComboBox(
        Graphics graphics)
    {
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

            var clearColor =
                Parent?.BackColor ??
                SystemColors.Control;

            graphics.ClearBackground(
                ClientRectangle,
                clearColor);

            graphics.RenderBackground(
                bounds,
                BackColor,
                CornerStyle.Rounded,
                RenderedCorners.All,
                m_CornerSize);

            var borderColor =
                Focused || m_MouseOver
                    ? m_AccentColor
                    : m_BorderColor;

            graphics.RenderBorder(
                bounds,
                borderColor,
                1,
                CornerStyle.Rounded,
                RenderedCorners.All,
                m_CornerSize);

            RenderSelectedItem(
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

    private void RenderSelectedItem(
        Graphics graphics,
        Rectangle bounds)
    {
        var text =
            SelectedIndex >= 0
                ? GetItemText(
                    Items[SelectedIndex])
                : Text;

        var textBounds =
            new Rectangle(
                bounds.Left + 14,
                bounds.Top,
                bounds.Width - 54,
                bounds.Height);

        graphics.RenderText(
            text,
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
            bounds.Right - 22;

        var centerY =
            bounds.Top +
            (bounds.Height / 2);

        var arrowColor =
            Enabled
                ? m_ArrowColor
                : SystemColors.GrayText;

        using var arrowPen =
            new Pen(
                arrowColor,
                2F);

        arrowPen.StartCap =
            LineCap.Round;

        arrowPen.EndCap =
            LineCap.Round;

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