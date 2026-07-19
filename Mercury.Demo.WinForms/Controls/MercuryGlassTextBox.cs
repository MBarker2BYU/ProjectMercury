// ***********************************************************************
// Assembly       : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-18-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-18-2026
// ***********************************************************************
// <copyright file="MercuryGlassTextBox.cs">
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
/// Displays a custom Mercury glass-style text box.
/// </summary>
[DefaultEvent(nameof(TextChanged))]
[DefaultProperty(nameof(Text))]
public sealed class MercuryGlassTextBox : UserControl
{
    private readonly TextBox m_TextBox;

    private Color m_BorderColor =
        Color.FromArgb(55, 145, 235);

    private Color m_FocusedBorderColor =
        Color.FromArgb(70, 180, 255);

    private Color m_DisabledForeColor =
        Color.FromArgb(105, 110, 115);

    private Color m_PlaceholderColor =
        Color.FromArgb(105, 110, 115);

    private string m_PlaceholderText =
        string.Empty;

    private int m_BorderWidth = 1;
    private int m_CornerSize = 6;

    private CornerStyle m_CornerStyle =
        CornerStyle.Clipped;

    private RenderedCorners m_RenderedCorners =
        RenderedCorners.All;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MercuryGlassTextBox"/> class.
    /// </summary>
    public MercuryGlassTextBox()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint |
            ControlStyles.SupportsTransparentBackColor,
            true);

        DoubleBuffered = true;

        BackColor =
            Color.FromArgb(26, 30, 33);

        ForeColor =
            Color.FromArgb(225, 232, 238);

        Font =
            new Font(
                "Segoe UI",
                9F,
                FontStyle.Regular);

        Padding =
            new Padding(10, 6, 10, 6);

        MinimumSize =
            new Size(50, 32);

        Size =
            new Size(220, 34);

        m_TextBox =
            new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = BackColor,
                ForeColor = ForeColor,
                Font = Font,
                Location = Point.Empty,
                TabIndex = 0
            };

        m_TextBox.TextChanged +=
            HandleInnerTextChanged;

        m_TextBox.GotFocus +=
            HandleInnerFocusChanged;

        m_TextBox.LostFocus +=
            HandleInnerFocusChanged;

        m_TextBox.KeyDown +=
            HandleInnerKeyDown;

        Controls.Add(m_TextBox);

        UpdateInnerTextBoxBounds();
    }

    /// <summary>
    /// Gets or sets the text contained by the control.
    /// </summary>
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    [Bindable(true)]
    public override string Text
    {
        get => m_TextBox.Text;
        set
        {
            var newValue =
                value ?? string.Empty;

            if (m_TextBox.Text == newValue)
                return;

            m_TextBox.Text = newValue;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the placeholder displayed when
    /// the control is empty.
    /// </summary>
    [DefaultValue("")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public string PlaceholderText
    {
        get => m_PlaceholderText;
        set
        {
            var newValue =
                value ?? string.Empty;

            if (m_PlaceholderText == newValue)
                return;

            m_PlaceholderText = newValue;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the placeholder color.
    /// </summary>
    [DefaultValue(typeof(Color), "105, 110, 115")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color PlaceholderColor
    {
        get => m_PlaceholderColor;
        set
        {
            if (m_PlaceholderColor == value)
                return;

            m_PlaceholderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the normal border color.
    /// </summary>
    [DefaultValue(typeof(Color), "55, 145, 235")]
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
    /// Gets or sets the border color used while focused.
    /// </summary>
    [DefaultValue(typeof(Color), "70, 180, 255")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color FocusedBorderColor
    {
        get => m_FocusedBorderColor;
        set
        {
            if (m_FocusedBorderColor == value)
                return;

            m_FocusedBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the disabled foreground color.
    /// </summary>
    [DefaultValue(typeof(Color), "105, 110, 115")]
    [DesignerSerializationVisibility(
        DesignerSerializationVisibility.Visible)]
    public Color DisabledForeColor
    {
        get => m_DisabledForeColor;
        set
        {
            if (m_DisabledForeColor == value)
                return;

            m_DisabledForeColor = value;
            UpdateInnerTextBoxColors();
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets whether multiple lines are allowed.
    /// </summary>
    [DefaultValue(false)]
    public bool Multiline
    {
        get => m_TextBox.Multiline;
        set
        {
            if (m_TextBox.Multiline == value)
                return;

            m_TextBox.Multiline = value;
            UpdateInnerTextBoxBounds();
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets whether the text is read-only.
    /// </summary>
    [DefaultValue(false)]
    public bool ReadOnly
    {
        get => m_TextBox.ReadOnly;
        set
        {
            if (m_TextBox.ReadOnly == value)
                return;

            m_TextBox.ReadOnly = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets whether a password character is used.
    /// </summary>
    [DefaultValue(false)]
    public bool UseSystemPasswordChar
    {
        get => m_TextBox.UseSystemPasswordChar;
        set =>
            m_TextBox.UseSystemPasswordChar = value;
    }

    /// <summary>
    /// Gets or sets the maximum permitted text length.
    /// </summary>
    [DefaultValue(32767)]
    public int MaxLength
    {
        get => m_TextBox.MaxLength;
        set =>
            m_TextBox.MaxLength =
                Math.Max(0, value);
    }

    /// <summary>
    /// Gets or sets the horizontal text alignment.
    /// </summary>
    [DefaultValue(HorizontalAlignment.Left)]
    public HorizontalAlignment TextAlign
    {
        get => m_TextBox.TextAlign;
        set =>
            m_TextBox.TextAlign = value;
    }

    /// <summary>
    /// Gets or sets whether scroll bars are displayed.
    /// </summary>
    [DefaultValue(ScrollBars.None)]
    public ScrollBars ScrollBars
    {
        get => m_TextBox.ScrollBars;
        set =>
            m_TextBox.ScrollBars = value;
    }

    /// <summary>
    /// Gets or sets whether the Return key creates a
    /// new line in a multiline control.
    /// </summary>
    [DefaultValue(false)]
    public bool AcceptsReturn
    {
        get => m_TextBox.AcceptsReturn;
        set =>
            m_TextBox.AcceptsReturn = value;
    }

    /// <summary>
    /// Gets or sets whether the Tab key inserts a tab.
    /// </summary>
    [DefaultValue(false)]
    public bool AcceptsTab
    {
        get => m_TextBox.AcceptsTab;
        set =>
            m_TextBox.AcceptsTab = value;
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
            var borderWidth =
                Math.Max(0, value);

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
            var cornerSize =
                Math.Max(0, value);

            if (m_CornerSize == cornerSize)
                return;

            m_CornerSize = cornerSize;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the corner style.
    /// </summary>
    [DefaultValue(CornerStyle.Clipped)]
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
    /// Gets or sets the corners rendered by the control.
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
    /// Selects all text.
    /// </summary>
    public void SelectAll()
    {
        m_TextBox.SelectAll();
    }

    /// <summary>
    /// Appends text to the existing content.
    /// </summary>
    public void AppendText(string value)
    {
        m_TextBox.AppendText(value);
    }

    /// <summary>
    /// Gives focus to the inner text box.
    /// </summary>
    public new void Focus()
    {
        m_TextBox.Focus();
    }

    /// <summary>
    /// Paints the Mercury glass frame and placeholder.
    /// </summary>
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

        var pixelOffsetMode =
            graphics.PixelOffsetMode;

        graphics.SmoothingMode =
            SmoothingMode.AntiAlias;

        graphics.PixelOffsetMode =
            PixelOffsetMode.HighQuality;

        var parentColor =
            Parent?.BackColor ??
            SystemColors.Control;

        graphics.ClearBackground(
            ClientRectangle,
            parentColor);

        graphics.RenderBackground(
            bounds,
            BackColor,
            m_CornerStyle,
            m_RenderedCorners,
            m_CornerSize);

        graphics.RenderBorder(
            bounds,
            GetCurrentBorderColor(),
            m_BorderWidth,
            m_CornerStyle,
            m_RenderedCorners,
            m_CornerSize);

        RenderPlaceholder(
            graphics);

        graphics.SmoothingMode =
            smoothingMode;

        graphics.PixelOffsetMode =
            pixelOffsetMode;
    }

    /// <summary>
    /// Redirects focus to the native text box.
    /// </summary>
    protected override void OnEnter(
        EventArgs eventArgs)
    {
        base.OnEnter(eventArgs);

        if (!m_TextBox.Focused)
            m_TextBox.Focus();
    }

    /// <summary>
    /// Handles mouse clicks on the outer control.
    /// </summary>
    protected override void OnMouseDown(
        MouseEventArgs mouseEvent)
    {
        base.OnMouseDown(mouseEvent);

        if (mouseEvent.Button == MouseButtons.Left)
            m_TextBox.Focus();
    }

    /// <summary>
    /// Updates layout when the control size changes.
    /// </summary>
    protected override void OnSizeChanged(
        EventArgs eventArgs)
    {
        base.OnSizeChanged(eventArgs);

        UpdateInnerTextBoxBounds();
        Invalidate();
    }

    /// <summary>
    /// Synchronizes the inner control font.
    /// </summary>
    protected override void OnFontChanged(
        EventArgs eventArgs)
    {
        base.OnFontChanged(eventArgs);

        if (m_TextBox == null)
            return;

        m_TextBox.Font = Font;

        UpdateInnerTextBoxBounds();
        Invalidate();
    }

    /// <summary>
    /// Synchronizes the inner foreground color.
    /// </summary>
    protected override void OnForeColorChanged(
        EventArgs eventArgs)
    {
        base.OnForeColorChanged(eventArgs);

        UpdateInnerTextBoxColors();
        Invalidate();
    }

    /// <summary>
    /// Synchronizes the inner background color.
    /// </summary>
    protected override void OnBackColorChanged(
        EventArgs eventArgs)
    {
        base.OnBackColorChanged(eventArgs);

        UpdateInnerTextBoxColors();
        Invalidate();
    }

    /// <summary>
    /// Synchronizes enabled state.
    /// </summary>
    protected override void OnEnabledChanged(
        EventArgs eventArgs)
    {
        base.OnEnabledChanged(eventArgs);

        m_TextBox.Enabled = Enabled;

        UpdateInnerTextBoxColors();
        Invalidate();
    }

    /// <summary>
    /// Updates layout when padding changes.
    /// </summary>
    protected override void OnPaddingChanged(
        EventArgs eventArgs)
    {
        base.OnPaddingChanged(eventArgs);

        UpdateInnerTextBoxBounds();
        Invalidate();
    }

    private void HandleInnerTextChanged(
        object? sender,
        EventArgs eventArgs)
    {
        OnTextChanged(eventArgs);
        Invalidate();
    }

    private void HandleInnerFocusChanged(
        object? sender,
        EventArgs eventArgs)
    {
        Invalidate();
    }

    private void HandleInnerKeyDown(
        object? sender,
        KeyEventArgs keyEvent)
    {
        OnKeyDown(keyEvent);
    }

    private void UpdateInnerTextBoxBounds()
    {
        if (m_TextBox == null)
            return;

        var contentBounds =
            ClientRectangle;

        contentBounds.X += Padding.Left;
        contentBounds.Y += Padding.Top;

        contentBounds.Width -=
            Padding.Horizontal;

        contentBounds.Height -=
            Padding.Vertical;

        if (contentBounds.Width <= 0 ||
            contentBounds.Height <= 0)
        {
            return;
        }

        if (!m_TextBox.Multiline)
        {
            var preferredHeight =
                m_TextBox.PreferredHeight;

            contentBounds.Y +=
                Math.Max(
                    0,
                    (contentBounds.Height -
                     preferredHeight) / 2);

            contentBounds.Height =
                preferredHeight;
        }

        m_TextBox.Bounds =
            contentBounds;
    }

    private void UpdateInnerTextBoxColors()
    {
        if (m_TextBox == null)
            return;

        m_TextBox.BackColor =
            BackColor;

        m_TextBox.ForeColor =
            Enabled
                ? ForeColor
                : m_DisabledForeColor;
    }

    private Color GetCurrentBorderColor()
    {
        if (!Enabled)
            return CreateMutedColor(m_BorderColor);

        return m_TextBox.Focused
            ? m_FocusedBorderColor
            : m_BorderColor;
    }

    private void RenderPlaceholder(
        Graphics graphics)
    {
        if (!string.IsNullOrEmpty(m_TextBox.Text) ||
            m_TextBox.Focused ||
            string.IsNullOrEmpty(m_PlaceholderText))
        {
            return;
        }

        var bounds =
            m_TextBox.Bounds;

        graphics.RenderText(
            m_PlaceholderText,
            Font,
            bounds,
            TextFormatFlags.Left |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPadding,
            m_PlaceholderColor,
            m_PlaceholderColor,
            true);
    }

    private static Color CreateMutedColor(
        Color color)
    {
        const int neutral = 105;

        return Color.FromArgb(
            (color.R + (neutral * 2)) / 3,
            (color.G + (neutral * 2)) / 3,
            (color.B + (neutral * 2)) / 3);
    }
}