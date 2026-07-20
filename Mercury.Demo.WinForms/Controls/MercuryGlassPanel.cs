using System.ComponentModel;
using System.Drawing.Drawing2D;
using Mercury.Demo.WinForms.Controls.Enums;
using Mercury.Demo.WinForms.Controls.Rendering;

namespace Mercury.Demo.WinForms.Controls;

public sealed class MercuryGlassPanel : Panel
{
    public MercuryGlassPanel()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint |
            ControlStyles.SupportsTransparentBackColor,
            true);

        DoubleBuffered = true;
        BackColor = Color.FromArgb(26, 30, 33);
        ForeColor = Color.FromArgb(225, 232, 238);

        Padding = new Padding(16, 16, 16, 16);
        Size = new Size(320, 220);
    }

    private string m_Title = "Title";

    [DefaultValue("Title")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Title
    {
        get => m_Title;
        set
        {
            var newValue = value ?? string.Empty;

            if(m_Title == newValue)
                return;

            m_Title = newValue;
            Invalidate();
        }
    }


    private Color m_BorderColor = Color.FromArgb(46, 50, 54);

    /// <summary>
    /// Gets or sets the color of the border.
    /// </summary>
    /// <value>The color of the border.</value>
    [DefaultValue(ContentAlignment.TopLeft)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

    private ContentAlignment m_AlignTitle = ContentAlignment.TopLeft;

    /// <summary>
    /// Gets or sets the align title.
    /// </summary>
    /// <value>The align title.</value>
    [DefaultValue(ContentAlignment.MiddleLeft)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ContentAlignment AlignTitle
    {
        get => m_AlignTitle;
        set
        {
            if (m_AlignTitle == value)
                return;

            m_AlignTitle = value;
            Invalidate();
        }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Obsolete("Use Title", true)]
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var graphics = e.Graphics;
        var bounds = ClientRectangle;

        bounds.Width -= 2;
        bounds.Height -= 2;

        var smoothingMode = graphics.SmoothingMode;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        var clearColor = Color.Transparent;

        graphics.ClearBackground(bounds, clearColor);

        graphics.RenderBackground(bounds, BackColor, m_CornerStyle, m_RenderedCorners, m_CornerSize);

        graphics.RenderBorder(bounds, BorderColor, 1, m_CornerStyle, m_RenderedCorners, m_CornerSize);

        bounds.Width -= 20;
        bounds.Height -= 10;

        bounds.X += 10;
        bounds.Y += 5;    

        graphics.RenderText(Title, Font, bounds, AlignTitle.ToTextFormatFlags(), ForeColor, SystemColors.GrayText, Enabled);

        graphics.SmoothingMode = smoothingMode;
    }

    private CornerStyle m_CornerStyle = CornerStyle.Clipped;

    /// <summary>
    /// Gets or sets the corner style.
    /// </summary>
    /// <value>The corner style.</value>
    [DefaultValue(CornerStyle.Clipped)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public CornerStyle CornerStyle
    {
        get => m_CornerStyle;
        set
        {
            if(m_CornerStyle == value)
                return;

            m_CornerStyle = value;
            Invalidate();
        }
    }

    private RenderedCorners m_RenderedCorners = RenderedCorners.All;

    /// <summary>
    /// Gets or sets the rendered corners.
    /// </summary>
    /// <value>The rendered corners.</value>
    [DefaultValue(RenderedCorners.All)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public RenderedCorners RenderedCorners
    {
        get => m_RenderedCorners;
        set
        {
            if(m_RenderedCorners == value)
                return;

            m_RenderedCorners = value;
            Invalidate();
        }
    }


    private int m_CornerSize = 8;

    /// <summary>
    /// Gets or sets the size of the corner.
    /// </summary>
    /// <value>The size of the corner.</value>
    [DefaultValue(8)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CornerSize
    {
        get => m_CornerSize;
        set
        {
            if(m_CornerSize == value)
                return;

            m_CornerSize = value;
            Invalidate();
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Invalidate();
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        Invalidate();
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        Invalidate();
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        Invalidate();
    }
}