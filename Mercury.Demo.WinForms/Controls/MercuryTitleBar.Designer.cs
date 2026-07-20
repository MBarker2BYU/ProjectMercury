namespace Mercury.Demo.WinForms.Controls;

partial class MercuryTitleBar
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(
        bool disposing)
    {
        if (disposing)
            components?.Dispose();

        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        m_TitleBarPanel = new Panel();
        m_IconPictureBox = new PictureBox();
        m_TitleLabel = new Label();
        m_MinimizeButton = new MercuryGlassButton();
        m_MaximizeButton = new MercuryGlassButton();
        m_CloseButton = new MercuryGlassButton();
        m_TitleBarPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)m_IconPictureBox).BeginInit();
        SuspendLayout();
        // 
        // m_TitleBarPanel
        // 
        m_TitleBarPanel.BackColor = Color.FromArgb(0, 12, 25);
        m_TitleBarPanel.Controls.Add(m_IconPictureBox);
        m_TitleBarPanel.Controls.Add(m_TitleLabel);
        m_TitleBarPanel.Controls.Add(m_MinimizeButton);
        m_TitleBarPanel.Controls.Add(m_MaximizeButton);
        m_TitleBarPanel.Controls.Add(m_CloseButton);
        m_TitleBarPanel.Dock = DockStyle.Fill;
        m_TitleBarPanel.Location = new Point(0, 0);
        m_TitleBarPanel.Name = "m_TitleBarPanel";
        m_TitleBarPanel.Size = new Size(900, 40);
        m_TitleBarPanel.TabIndex = 0;
        m_TitleBarPanel.MouseDoubleClick += m_TitleBarPanel_MouseDoubleClick;
        m_TitleBarPanel.MouseDown += m_TitleBarPanel_MouseDown;
        // 
        // m_IconPictureBox
        // 
        m_IconPictureBox.BackColor = Color.FromArgb(0, 12, 25);
        m_IconPictureBox.Location = new Point(10, 8);
        m_IconPictureBox.Name = "m_IconPictureBox";
        m_IconPictureBox.Size = new Size(24, 24);
        m_IconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        m_IconPictureBox.TabIndex = 0;
        m_IconPictureBox.TabStop = false;
        m_IconPictureBox.MouseDoubleClick += m_TitleBarPanel_MouseDoubleClick;
        m_IconPictureBox.MouseDown += m_TitleBarPanel_MouseDown;
        // 
        // m_TitleLabel
        // 
        m_TitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        m_TitleLabel.ForeColor = Color.FromArgb(225, 232, 238);
        m_TitleLabel.Location = new Point(42, 0);
        m_TitleLabel.Name = "m_TitleLabel";
        m_TitleLabel.Size = new Size(716, 40);
        m_TitleLabel.TabIndex = 1;
        m_TitleLabel.Text = "Mercury";
        m_TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        m_TitleLabel.MouseDoubleClick += m_TitleBarPanel_MouseDoubleClick;
        m_TitleLabel.MouseDown += m_TitleBarPanel_MouseDown;
        // 
        // m_MinimizeButton
        // 
        m_MinimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_MinimizeButton.BackColor = Color.Transparent;
        m_MinimizeButton.ButtonColor = Color.Transparent;
        m_MinimizeButton.FillAlpha = 0;
        m_MinimizeButton.Font = new Font("Segoe UI", 9F);
        m_MinimizeButton.ForeColor = Color.FromArgb(225, 232, 238);
        m_MinimizeButton.Image = Properties.Resources.Minimize;
        m_MinimizeButton.ImageAlign = ContentAlignment.MiddleLeft;
        m_MinimizeButton.Location = new Point(770, 4);
        m_MinimizeButton.Name = "m_MinimizeButton";
        m_MinimizeButton.Padding = new Padding(10, 0, 10, 0);
        m_MinimizeButton.Size = new Size(40, 32);
        m_MinimizeButton.TabIndex = 2;
        m_MinimizeButton.UseVisualStyleBackColor = false;
        m_MinimizeButton.Click += m_MinimizeButton_Click;
        // 
        // m_MaximizeButton
        // 
        m_MaximizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_MaximizeButton.BackColor = Color.Transparent;
        m_MaximizeButton.ButtonColor = Color.Transparent;
        m_MaximizeButton.FillAlpha = 0;
        m_MaximizeButton.Font = new Font("Segoe UI", 9F);
        m_MaximizeButton.ForeColor = Color.FromArgb(225, 232, 238);
        m_MaximizeButton.Image = Properties.Resources.Maximize;
        m_MaximizeButton.ImageAlign = ContentAlignment.MiddleLeft;
        m_MaximizeButton.Location = new Point(812, 4);
        m_MaximizeButton.Name = "m_MaximizeButton";
        m_MaximizeButton.Padding = new Padding(10, 0, 10, 0);
        m_MaximizeButton.Size = new Size(40, 32);
        m_MaximizeButton.TabIndex = 3;
        m_MaximizeButton.UseVisualStyleBackColor = false;
        m_MaximizeButton.Click += m_MaximizeButton_Click;
        // 
        // m_CloseButton
        // 
        m_CloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_CloseButton.BackColor = Color.Transparent;
        m_CloseButton.ButtonColor = Color.Transparent;
        m_CloseButton.FillAlpha = 0;
        m_CloseButton.Font = new Font("Segoe UI", 9F);
        m_CloseButton.ForeColor = Color.FromArgb(225, 232, 238);
        m_CloseButton.Image = Properties.Resources.Close;
        m_CloseButton.Location = new Point(854, 4);
        m_CloseButton.Name = "m_CloseButton";
        m_CloseButton.Padding = new Padding(10, 0, 10, 0);
        m_CloseButton.Size = new Size(40, 32);
        m_CloseButton.TabIndex = 4;
        m_CloseButton.UseVisualStyleBackColor = false;
        m_CloseButton.Click += m_CloseButton_Click;
        // 
        // MercuryTitleBar
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 12, 25);
        Controls.Add(m_TitleBarPanel);
        Name = "MercuryTitleBar";
        Size = new Size(900, 40);
        m_TitleBarPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)m_IconPictureBox).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel m_TitleBarPanel;
    private PictureBox m_IconPictureBox;
    private Label m_TitleLabel;
    private MercuryGlassButton m_MinimizeButton;
    private MercuryGlassButton m_MaximizeButton;
    private MercuryGlassButton m_CloseButton;
}