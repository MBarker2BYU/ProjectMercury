// ***********************************************************************
// Assembly       : Mercury.Demo
// Author         : Matthew D. Barker
// Created        : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
// <copyright file="MercuryGlassButton.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.ComponentModel;
using System.Drawing.Drawing2D;
using Mercury.Demo.WinForms.Enums;
using Mercury.Demo.WinForms.Rendering;

namespace Mercury.Demo.WinForms.Controls;

/// <summary>
/// Displays a custom glass-style button.
/// </summary>
public sealed class MercuryGlassButton : ButtonBase
{
    private const int HOVER_ALPHA_INCREASE = 26;
    private const int PRESSED_ALPHA_INCREASE = 51;

    private bool m_MouseOver;
    private bool m_MouseDown;

    private Color m_ButtonColor = Color.FromArgb(55, 145, 235);
    private Color m_DisabledForeColor = Color.FromArgb(105, 110, 115);

    private int m_FillAlpha = 35;
    private int m_BorderWidth = 1;
    private int m_CornerSize = 6;

    private CornerStyle m_CornerStyle = CornerStyle.Rounded;
    private RenderedCorners m_RenderedCorners = RenderedCorners.All;

    /// <summary>
    /// Initializes a new instance of the <see cref="MercuryGlassButton"/> class.
    /// </summary>
    public MercuryGlassButton()
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

        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(225, 232, 238);

        Font = new Font("Segoe UI", 9F, FontStyle.Regular);

        Cursor = Cursors.Hand;
        Size = new Size(140, 36);
        TextAlign = ContentAlignment.MiddleCenter;
        ImageAlign = ContentAlignment.MiddleLeft;
        Padding = new Padding(10, 0, 10, 0);

        TabStop = true;
    }

    /// <summary>
    /// Gets or sets the primary button color.
    /// </summary>
    [DefaultValue(typeof(Color), "55, 145, 235")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color ButtonColor
    {
        get => m_ButtonColor;
        set
        {
            if (m_ButtonColor == value)
                return;

            m_ButtonColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the normal background alpha.
    /// </summary>
    [DefaultValue(35)]
    public int FillAlpha
    {
        get => m_FillAlpha;
        set
        {
            var fillAlpha = Math.Clamp(value, 0, 255);

            if (m_FillAlpha == fillAlpha)
                return;

            m_FillAlpha = fillAlpha;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the disabled foreground color.
    /// </summary>
    [DefaultValue(typeof(Color), "105, 110, 115")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color DisabledForeColor
    {
        get => m_DisabledForeColor;
        set
        {
            if (m_DisabledForeColor == value)
                return;

            m_DisabledForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the border width.
    /// </summary>
    [DefaultValue(1)]
    public int BorderWidth
    {
        get => m_BorderWidth;
        set
        {
            var borderWidth = Math.Max(0, value);

            if (m_BorderWidth == borderWidth)
                return;

            m_BorderWidth = borderWidth;
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
            var cornerSize = Math.Max(0, value);

            if (m_CornerSize == cornerSize)
                return;

            m_CornerSize = cornerSize;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the corner style.
    /// </summary>
    [DefaultValue(CornerStyle.Rounded)]
    public CornerStyle CornerStyle
    {
        get => m_CornerStyle;
        set
        {
            if (m_CornerStyle == value)
                return;

            m_CornerStyle = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the corners rendered by the button.
    /// </summary>
    [DefaultValue(RenderedCorners.All)]
    public RenderedCorners RenderedCorners
    {
        get => m_RenderedCorners;
        set
        {
            if (m_RenderedCorners == value)
                return;

            m_RenderedCorners = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Paints the button.
    /// </summary>
    /// <param name="paintEvent">The paint event arguments.</param>
    protected override void OnPaint(PaintEventArgs paintEvent)
    {
        var graphics = paintEvent.Graphics;
        var bounds = ClientRectangle;

        bounds.Width -= 1;
        bounds.Height -= 1;

        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        var smoothingMode = graphics.SmoothingMode;
        var pixelOffsetMode = graphics.PixelOffsetMode;

        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        var parentColor =
            Parent?.BackColor ?? SystemColors.Control;

        graphics.ClearBackground(
            ClientRectangle,
            parentColor);

        graphics.RenderBackground(
            bounds,
            GetBackgroundColor(),
            m_CornerStyle,
            m_RenderedCorners,
            m_CornerSize);

        graphics.RenderBorder(
            bounds,
            GetBorderColor(),
            m_BorderWidth,
            m_CornerStyle,
            m_RenderedCorners,
            m_CornerSize);

        if (Focused && ShowFocusCues && Enabled)
        {
            var focusBounds = bounds;

            focusBounds.Inflate(-3, -3);

            graphics.RenderBorder(
                focusBounds,
                Color.FromArgb(140, m_ButtonColor),
                1,
                m_CornerStyle,
                m_RenderedCorners,
                Math.Max(0, m_CornerSize - 2));
        }

        RenderButtonContent(
            graphics,
            bounds);

        graphics.SmoothingMode = smoothingMode;
        graphics.PixelOffsetMode = pixelOffsetMode;
    }

    /// <summary>
    /// Handles pointer entry.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnMouseEnter(System.EventArgs eventArgs)
    {
        base.OnMouseEnter(eventArgs);

        m_MouseOver = true;
        Invalidate();
    }

    /// <summary>
    /// Handles pointer exit.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnMouseLeave(EventArgs eventArgs)
    {
        base.OnMouseLeave(eventArgs);

        m_MouseOver = false;
        m_MouseDown = false;

        Invalidate();
    }

    /// <summary>
    /// Handles a mouse button press.
    /// </summary>
    /// <param name="mouseEvent">The mouse event arguments.</param>
    protected override void OnMouseDown(MouseEventArgs mouseEvent)
    {
        base.OnMouseDown(mouseEvent);

        if (mouseEvent.Button != MouseButtons.Left)
            return;

        m_MouseDown = true;

        Focus();
        Invalidate();
    }

    /// <summary>
    /// Handles a mouse button release.
    /// </summary>
    /// <param name="mouseEvent">The mouse event arguments.</param>
    protected override void OnMouseUp(MouseEventArgs mouseEvent)
    {
        base.OnMouseUp(mouseEvent);

        m_MouseDown = false;
        Invalidate();
    }

    /// <summary>
    /// Handles a keyboard button press.
    /// </summary>
    /// <param name="keyEvent">The key event arguments.</param>
    protected override void OnKeyDown(KeyEventArgs keyEvent)
    {
        base.OnKeyDown(keyEvent);

        if (keyEvent.KeyCode is not Keys.Space and not Keys.Enter)
            return;

        m_MouseDown = true;
        Invalidate();
    }

    /// <summary>
    /// Handles a keyboard button release.
    /// </summary>
    /// <param name="keyEvent">The key event arguments.</param>
    protected override void OnKeyUp(KeyEventArgs keyEvent)
    {
        base.OnKeyUp(keyEvent);

        if (keyEvent.KeyCode is not Keys.Space and not Keys.Enter)
            return;

        m_MouseDown = false;
        Invalidate();
    }

    /// <summary>
    /// Handles focus changes.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnGotFocus(EventArgs eventArgs)
    {
        base.OnGotFocus(eventArgs);
        Invalidate();
    }

    /// <summary>
    /// Handles focus changes.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnLostFocus(EventArgs eventArgs)
    {
        base.OnLostFocus(eventArgs);

        m_MouseDown = false;
        Invalidate();
    }

    /// <summary>
    /// Handles enabled-state changes.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnEnabledChanged(EventArgs eventArgs)
    {
        base.OnEnabledChanged(eventArgs);

        if (!Enabled)
        {
            m_MouseOver = false;
            m_MouseDown = false;
        }

        Invalidate();
    }

    /// <summary>
    /// Handles font changes.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnFontChanged(EventArgs eventArgs)
    {
        base.OnFontChanged(eventArgs);
        Invalidate();
    }

    /// <summary>
    /// Handles foreground-color changes.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnForeColorChanged(EventArgs eventArgs)
    {
        base.OnForeColorChanged(eventArgs);
        Invalidate();
    }

    /// <summary>
    /// Handles background-color changes.
    /// </summary>
    /// <param name="eventArgs">The event arguments.</param>
    protected override void OnBackColorChanged(EventArgs eventArgs)
    {
        base.OnBackColorChanged(eventArgs);
        Invalidate();
    }

    private Color GetBackgroundColor()
    {
        if (!Enabled)
        {
            return Color.FromArgb(
                Math.Min(m_FillAlpha, 25),
                CreateMutedColor(m_ButtonColor));
        }

        var alpha = m_FillAlpha;

        if (m_MouseDown)
        {
            alpha += PRESSED_ALPHA_INCREASE;
        }
        else if (m_MouseOver)
        {
            alpha += HOVER_ALPHA_INCREASE;
        }

        return Color.FromArgb(
            Math.Min(alpha, 255),
            m_ButtonColor);
    }

    private Color GetBorderColor()
    {
        if (!Enabled)
        {
            return CreateMutedColor(m_ButtonColor);
        }

        return m_ButtonColor;
    }

    private void RenderButtonContent(
        Graphics graphics,
        Rectangle bounds)
    {
        var contentBounds = bounds;

        contentBounds.X += Padding.Left;
        contentBounds.Y += Padding.Top;
        contentBounds.Width -= Padding.Horizontal;
        contentBounds.Height -= Padding.Vertical;

        if (contentBounds.Width <= 0 || contentBounds.Height <= 0)
            return;

        if (m_MouseDown)
        {
            contentBounds.Offset(1, 1);
        }

        if (Image != null)
        {
            RenderImage(
                graphics,
                ref contentBounds);
        }

        graphics.RenderText(
            Text,
            Font,
            contentBounds,
            TextAlign.ToTextFormatFlags(),
            ForeColor,
            m_DisabledForeColor,
            Enabled);
    }

    private void RenderImage(
        Graphics graphics,
        ref Rectangle contentBounds)
    {
        if (Image == null)
            return;

        var imageSize = Image.Size;

        var imageY =
            contentBounds.Top +
            ((contentBounds.Height - imageSize.Height) / 2);

        var imageX = contentBounds.Left;

        if (ImageAlign is ContentAlignment.TopCenter or
            ContentAlignment.MiddleCenter or
            ContentAlignment.BottomCenter)
        {
            imageX =
                contentBounds.Left +
                ((contentBounds.Width - imageSize.Width) / 2);
        }
        else if (ImageAlign is ContentAlignment.TopRight or
                 ContentAlignment.MiddleRight or
                 ContentAlignment.BottomRight)
        {
            imageX =
                contentBounds.Right -
                imageSize.Width;
        }

        if (Enabled)
        {
            graphics.DrawImage(
                Image,
                imageX,
                imageY,
                imageSize.Width,
                imageSize.Height);
        }
        else
        {
            ControlPaint.DrawImageDisabled(
                graphics,
                Image,
                imageX,
                imageY,
                CreateMutedColor(m_ButtonColor));
        }

        if (ImageAlign is ContentAlignment.TopLeft or
            ContentAlignment.MiddleLeft or
            ContentAlignment.BottomLeft)
        {
            contentBounds.X += imageSize.Width + 8;
            contentBounds.Width -= imageSize.Width + 8;
        }
        else if (ImageAlign is ContentAlignment.TopRight or
                 ContentAlignment.MiddleRight or
                 ContentAlignment.BottomRight)
        {
            contentBounds.Width -= imageSize.Width + 8;
        }
    }

    private static Color CreateMutedColor(Color color)
    {
        const int neutral = 105;

        return Color.FromArgb(
            (color.R + (neutral * 2)) / 3,
            (color.G + (neutral * 2)) / 3,
            (color.B + (neutral * 2)) / 3);
    }
}