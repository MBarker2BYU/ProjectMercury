using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Mercury.Demo.WinForms.Controls;

public partial class MercuryTitleBar : UserControl
{
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int HTCAPTION = 0x0002;

    private Form? m_TargetForm;
    private string m_Title = "Mercury";
    private Image? m_TitleIcon;

    public MercuryTitleBar()
    {
        InitializeComponent();

        DoubleBuffered = true;

        m_MinimizeButton.ImageAlign = ContentAlignment.MiddleCenter;
        m_MinimizeButton.Text = string.Empty;

        m_MaximizeButton.ImageAlign = ContentAlignment.MiddleCenter;
        m_MaximizeButton.Text = string.Empty;

        m_CloseButton.ImageAlign = ContentAlignment.MiddleCenter;
        m_CloseButton.Text = string.Empty;
    }

    [Category("Mercury")]
    [DefaultValue("Mercury")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Title
    {
        get => m_Title;
        set
        {
            var newValue = value ?? string.Empty;

            if (m_Title == newValue)
                return;

            m_Title = newValue;
            m_TitleLabel.Text = newValue;
        }
    }

    [Category("Mercury")]
    [DefaultValue(null)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Image? TitleIcon
    {
        get => m_TitleIcon;
        set
        {
            if (ReferenceEquals(m_TitleIcon, value))
                return;

            m_TitleIcon = value;
            m_IconPictureBox.Image = value;
        }
    }

    [Category("Mercury")]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowMinimizeButton
    {
        get => m_MinimizeButton.Visible;
        set => m_MinimizeButton.Visible = value;
    }

    [Category("Mercury")]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowMaximizeButton
    {
        get => m_MaximizeButton.Visible;
        set => m_MaximizeButton.Visible = value;
    }

    [Category("Mercury")]
    [DefaultValue(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowCloseButton
    {
        get => m_CloseButton.Visible;
        set => m_CloseButton.Visible = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Form? TargetForm
    {
        get => m_TargetForm;
        set
        {
            if (ReferenceEquals(m_TargetForm, value))
                return;

            if (m_TargetForm != null)
                m_TargetForm.Resize -= TargetForm_Resize;

            m_TargetForm = value;

            if (m_TargetForm != null)
                m_TargetForm.Resize += TargetForm_Resize;

            UpdateMaximizeButton();
        }
    }

    protected override void OnParentChanged(
        EventArgs e)
    {
        base.OnParentChanged(e);

        if (DesignMode)
            return;

        TargetForm ??= FindForm();
    }

    private void m_TitleBarPanel_MouseDown(
        object? sender,
        MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            return;

        var targetForm = GetTargetForm();

        if (targetForm == null)
            return;

        ReleaseCapture();

        SendMessage(
            targetForm.Handle,
            WM_NCLBUTTONDOWN,
            new IntPtr(HTCAPTION),
            IntPtr.Zero);
    }

    private void m_TitleBarPanel_MouseDoubleClick(
        object? sender,
        MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            return;

        ToggleMaximize();
    }

    private void m_MinimizeButton_Click(
        object? sender,
        EventArgs e)
    {
        var targetForm = GetTargetForm();

        if (targetForm == null)
            return;

        targetForm.WindowState = FormWindowState.Minimized;
    }

    private void m_MaximizeButton_Click(
        object? sender,
        EventArgs e)
    {
        ToggleMaximize();
    }

    private void m_CloseButton_Click(
        object? sender,
        EventArgs e)
    {
        var targetForm = GetTargetForm();

        if (targetForm == null)
            return;

        targetForm.Close();
    }

    private void TargetForm_Resize(
        object? sender,
        EventArgs e)
    {
        UpdateMaximizeButton();
    }

    private Form? GetTargetForm()
    {
        return m_TargetForm ?? FindForm();
    }

    private void ToggleMaximize()
    {
        var targetForm = GetTargetForm();

        if (targetForm == null)
            return;

        targetForm.WindowState =
            targetForm.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;

        UpdateMaximizeButton();
    }

    private void UpdateMaximizeButton()
    {
        var targetForm = GetTargetForm();

        m_MaximizeButton.Image =
            targetForm?.WindowState == FormWindowState.Maximized
                ? Properties.Resources.Restore 
                : Properties.Resources.Maximize;
    }

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(
        IntPtr hWnd,
        int message,
        IntPtr wParam,
        IntPtr lParam);
}