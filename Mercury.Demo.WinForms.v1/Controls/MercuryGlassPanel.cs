using System.ComponentModel;
using System.Drawing.Drawing2D;
using Mercury.Demo.WinForms.v1.Enums;
using Mercury.Demo.WinForms.v1.Rendering;

namespace Mercury.Demo.WinForms.v1.Controls;

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
    [DefaultValue(ContentAlignment.MiddleLeft)]
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

    private ContentAlignment m_AlignTitle = ContentAlignment.MiddleLeft;

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

        graphics.RenderBackground(bounds, BackColor, CornerStyle.Rounded, RenderedCorners.All, 6);

        graphics.RenderBorder(bounds, BorderColor, 1, CornerStyle.Rounded, RenderedCorners.All, 6);

        bounds.Width -= 10;
        bounds.Height -= 10;

        bounds.X += 5;
        bounds.Y += 5;    

        graphics.RenderText(Title, Font, bounds, AlignTitle.ToTextFormatFlags(), Color.White, SystemColors.GrayText, Enabled);

        graphics.SmoothingMode = smoothingMode;
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