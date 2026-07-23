namespace Mercury.Demo.WinForms
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mtbTitleBar = new Mercury.Demo.WinForms.Controls.MercuryTitleBar();
            pnlHeaderLogo = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblMotto = new Label();
            lblTitle = new Label();
            pictureBox1 = new PictureBox();
            pnlSystemStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblSystemTime = new Label();
            lblTimeCaption = new Label();
            lblSystemState = new Label();
            pnlConfiguration = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblLogging = new Label();
            lblChunkSize = new Label();
            lblChunking = new Label();
            lblEnvelopeCodec = new Label();
            lblTransport = new Label();
            btnApplyConfiguration = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            tglChunking = new Mercury.Demo.WinForms.Controls.MercuryGlassToggleButton();
            cboLogging = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            cboChunkSize = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            cboEnvelopeCodec = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            cboTransport = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            cboCryptoProvider = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            lblCryptoProvider = new Label();
            pnlSecurityStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            pnlTamperStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblLastCheck = new Label();
            lblTamperState = new Label();
            lblTamperCaption = new Label();
            picTamperCheck = new PictureBox();
            picTamperStatus = new PictureBox();
            pnlReplayStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblReplayWindow = new Label();
            lblReplayState = new Label();
            lblReplayCaption = new Label();
            picReplayCheck = new PictureBox();
            picReplayStatus = new PictureBox();
            pnlIntegrityStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblIntegrityState = new Label();
            lblIntegrityCaption = new Label();
            picIntegrityCheck = new PictureBox();
            picIntegrityStatus = new PictureBox();
            pnlTransportStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblTransportState = new Label();
            lblTransportValue = new Label();
            lblTransportCaption = new Label();
            picTransportCheck = new PictureBox();
            picTransportStatus = new PictureBox();
            pnlProviderStatus = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblProviderReady = new Label();
            lblProviderValue = new Label();
            picProviderCheck = new PictureBox();
            lblProviderCaption = new Label();
            picProviderStatus = new PictureBox();
            pnlExchangeFlow = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblFlowMercuryReceive = new Label();
            lblValidateEnveleope = new Label();
            lblFlowTransport = new Label();
            lblFlowProvider = new Label();
            lblFlowMercurySend = new Label();
            lblFlowApplication = new Label();
            lblFlowArrow5 = new PictureBox();
            lblFlowArrow4 = new PictureBox();
            lblFlowArrow3 = new PictureBox();
            lblFlowArrow2 = new PictureBox();
            lblFlowArrow1 = new PictureBox();
            picValidateEnveleope = new PictureBox();
            picFlowTransport = new PictureBox();
            picFlowProtected = new PictureBox();
            picFlowMercuryReceived = new PictureBox();
            picFlowMercurySend = new PictureBox();
            picFlowApplication = new PictureBox();
            pnlSendData = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            btnSendFile = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            lblSendPayload = new Label();
            txtSendPayload = new Mercury.Demo.WinForms.Controls.MercuryGlassTextBox();
            btnSend = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            pnlEventLog = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            btnClearLog = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            lblEventLog = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            rtbEventLog = new RichTextBox();
            pnlEnvelopeViewer = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblRawPayloadValue = new Label();
            lblFrameSizeValue = new Label();
            lblProtectedSizeValue = new Label();
            lblReplayTokenValue = new Label();
            lblAlgorithmValue = new Label();
            lblEnvelopeIdValue = new Label();
            lblEnvelopeVersionValue = new Label();
            lblRawPayload = new Label();
            lblFrameSize = new Label();
            lblProtectedSize = new Label();
            lblReplayToken = new Label();
            lblAlgorithm = new Label();
            lblEnvelopeId = new Label();
            lblEnvelopeVersion = new Label();
            lblHexPreview = new Label();
            pnlHexPreview = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            rtbHexPreview = new RichTextBox();
            pnlDemoActions = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            tglWrongKeyAttack = new Mercury.Demo.WinForms.Controls.MercuryGlassToggleButton();
            tglTamperAttack = new Mercury.Demo.WinForms.Controls.MercuryGlassToggleButton();
            tglReplayAttack = new Mercury.Demo.WinForms.Controls.MercuryGlassToggleButton();
            pnlReceivedData = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblReceivePayload = new Label();
            lblReceiveResult = new Label();
            txtReceivePayload = new Mercury.Demo.WinForms.Controls.MercuryGlassTextBox();
            pnlHeaderLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlSystemStatus.SuspendLayout();
            pnlConfiguration.SuspendLayout();
            pnlSecurityStatus.SuspendLayout();
            pnlTamperStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picTamperCheck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTamperStatus).BeginInit();
            pnlReplayStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picReplayCheck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picReplayStatus).BeginInit();
            pnlIntegrityStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picIntegrityCheck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picIntegrityStatus).BeginInit();
            pnlTransportStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picTransportCheck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTransportStatus).BeginInit();
            pnlProviderStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picProviderCheck).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picProviderStatus).BeginInit();
            pnlExchangeFlow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picValidateEnveleope).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFlowTransport).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFlowProtected).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFlowMercuryReceived).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFlowMercurySend).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFlowApplication).BeginInit();
            pnlSendData.SuspendLayout();
            pnlEventLog.SuspendLayout();
            lblEventLog.SuspendLayout();
            pnlEnvelopeViewer.SuspendLayout();
            pnlHexPreview.SuspendLayout();
            pnlDemoActions.SuspendLayout();
            pnlReceivedData.SuspendLayout();
            SuspendLayout();
            // 
            // mtbTitleBar
            // 
            mtbTitleBar.BackColor = Color.FromArgb(0, 12, 25);
            mtbTitleBar.Dock = DockStyle.Top;
            mtbTitleBar.Location = new Point(3, 3);
            mtbTitleBar.Name = "mtbTitleBar";
            mtbTitleBar.Size = new Size(1914, 40);
            mtbTitleBar.TabIndex = 0;
            mtbTitleBar.Title = "";
            // 
            // pnlHeaderLogo
            // 
            pnlHeaderLogo.AlignTitle = ContentAlignment.TopLeft;
            pnlHeaderLogo.BackColor = Color.FromArgb(0, 15, 28);
            pnlHeaderLogo.BorderColor = Color.FromArgb(34, 61, 84);
            pnlHeaderLogo.Controls.Add(lblMotto);
            pnlHeaderLogo.Controls.Add(lblTitle);
            pnlHeaderLogo.Controls.Add(pictureBox1);
            pnlHeaderLogo.ForeColor = Color.FromArgb(0, 135, 191);
            pnlHeaderLogo.Location = new Point(12, 46);
            pnlHeaderLogo.Name = "pnlHeaderLogo";
            pnlHeaderLogo.Padding = new Padding(16);
            pnlHeaderLogo.Size = new Size(302, 86);
            pnlHeaderLogo.TabIndex = 1;
            pnlHeaderLogo.Title = "";
            // 
            // lblMotto
            // 
            lblMotto.AutoSize = true;
            lblMotto.Location = new Point(88, 58);
            lblMotto.Name = "lblMotto";
            lblMotto.Size = new Size(189, 15);
            lblMotto.TabIndex = 2;
            lblMotto.Text = "MODULAR.  SECURE.  EXTENSIBLE.";
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Gainsboro;
            lblTitle.Location = new Point(80, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(203, 41);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Mercury";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.MercuryIcon128;
            pictureBox1.Location = new Point(10, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(64, 64);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pnlSystemStatus
            // 
            pnlSystemStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlSystemStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlSystemStatus.BackColor = Color.FromArgb(0, 15, 28);
            pnlSystemStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlSystemStatus.Controls.Add(lblSystemTime);
            pnlSystemStatus.Controls.Add(lblTimeCaption);
            pnlSystemStatus.Controls.Add(lblSystemState);
            pnlSystemStatus.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlSystemStatus.ForeColor = Color.FromArgb(0, 135, 191);
            pnlSystemStatus.Location = new Point(1527, 49);
            pnlSystemStatus.Name = "pnlSystemStatus";
            pnlSystemStatus.Padding = new Padding(16);
            pnlSystemStatus.Size = new Size(381, 86);
            pnlSystemStatus.TabIndex = 2;
            pnlSystemStatus.Title = "SYSTEM STATUS";
            // 
            // lblSystemTime
            // 
            lblSystemTime.ForeColor = Color.Gainsboro;
            lblSystemTime.Location = new Point(128, 47);
            lblSystemTime.Name = "lblSystemTime";
            lblSystemTime.Size = new Size(234, 23);
            lblSystemTime.TabIndex = 3;
            lblSystemTime.Text = "--:--:-- UTC";
            lblSystemTime.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTimeCaption
            // 
            lblTimeCaption.ForeColor = Color.Gainsboro;
            lblTimeCaption.Location = new Point(19, 47);
            lblTimeCaption.Name = "lblTimeCaption";
            lblTimeCaption.Size = new Size(103, 23);
            lblTimeCaption.TabIndex = 2;
            lblTimeCaption.Text = "TIME";
            lblTimeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSystemState
            // 
            lblSystemState.ForeColor = Color.FromArgb(192, 192, 0);
            lblSystemState.Location = new Point(19, 22);
            lblSystemState.Name = "lblSystemState";
            lblSystemState.Size = new Size(343, 25);
            lblSystemState.TabIndex = 0;
            lblSystemState.Text = "●  INITIALIZING";
            lblSystemState.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlConfiguration
            // 
            pnlConfiguration.AlignTitle = ContentAlignment.TopLeft;
            pnlConfiguration.BackColor = Color.FromArgb(0, 15, 28);
            pnlConfiguration.BorderColor = Color.FromArgb(34, 61, 84);
            pnlConfiguration.Controls.Add(lblLogging);
            pnlConfiguration.Controls.Add(lblChunkSize);
            pnlConfiguration.Controls.Add(lblChunking);
            pnlConfiguration.Controls.Add(lblEnvelopeCodec);
            pnlConfiguration.Controls.Add(lblTransport);
            pnlConfiguration.Controls.Add(btnApplyConfiguration);
            pnlConfiguration.Controls.Add(tglChunking);
            pnlConfiguration.Controls.Add(cboLogging);
            pnlConfiguration.Controls.Add(cboChunkSize);
            pnlConfiguration.Controls.Add(cboEnvelopeCodec);
            pnlConfiguration.Controls.Add(cboTransport);
            pnlConfiguration.Controls.Add(cboCryptoProvider);
            pnlConfiguration.Controls.Add(lblCryptoProvider);
            pnlConfiguration.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlConfiguration.ForeColor = Color.FromArgb(0, 135, 191);
            pnlConfiguration.Location = new Point(12, 138);
            pnlConfiguration.Name = "pnlConfiguration";
            pnlConfiguration.Padding = new Padding(16);
            pnlConfiguration.Size = new Size(302, 326);
            pnlConfiguration.TabIndex = 3;
            pnlConfiguration.Title = "CONFIGURATION";
            // 
            // lblLogging
            // 
            lblLogging.ForeColor = Color.Gainsboro;
            lblLogging.Location = new Point(19, 218);
            lblLogging.Name = "lblLogging";
            lblLogging.Size = new Size(107, 30);
            lblLogging.TabIndex = 12;
            lblLogging.Text = "Logging";
            lblLogging.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblChunkSize
            // 
            lblChunkSize.ForeColor = Color.Gainsboro;
            lblChunkSize.Location = new Point(19, 182);
            lblChunkSize.Name = "lblChunkSize";
            lblChunkSize.Size = new Size(107, 30);
            lblChunkSize.TabIndex = 11;
            lblChunkSize.Text = "Chunk Size";
            lblChunkSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblChunking
            // 
            lblChunking.ForeColor = Color.Gainsboro;
            lblChunking.Location = new Point(19, 146);
            lblChunking.Name = "lblChunking";
            lblChunking.Size = new Size(107, 30);
            lblChunking.TabIndex = 10;
            lblChunking.Text = "Chunking";
            lblChunking.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEnvelopeCodec
            // 
            lblEnvelopeCodec.ForeColor = Color.Gainsboro;
            lblEnvelopeCodec.Location = new Point(19, 110);
            lblEnvelopeCodec.Name = "lblEnvelopeCodec";
            lblEnvelopeCodec.Size = new Size(107, 30);
            lblEnvelopeCodec.TabIndex = 9;
            lblEnvelopeCodec.Text = "Envelope Codec";
            lblEnvelopeCodec.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTransport
            // 
            lblTransport.ForeColor = Color.Gainsboro;
            lblTransport.Location = new Point(19, 74);
            lblTransport.Name = "lblTransport";
            lblTransport.Size = new Size(107, 30);
            lblTransport.TabIndex = 8;
            lblTransport.Text = "Transport";
            lblTransport.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnApplyConfiguration
            // 
            btnApplyConfiguration.BackColor = Color.Transparent;
            btnApplyConfiguration.Font = new Font("Segoe UI", 9F);
            btnApplyConfiguration.ForeColor = Color.FromArgb(225, 232, 238);
            btnApplyConfiguration.ImageAlign = ContentAlignment.MiddleLeft;
            btnApplyConfiguration.Location = new Point(19, 264);
            btnApplyConfiguration.Name = "btnApplyConfiguration";
            btnApplyConfiguration.Padding = new Padding(10, 0, 10, 0);
            btnApplyConfiguration.Size = new Size(264, 49);
            btnApplyConfiguration.TabIndex = 7;
            btnApplyConfiguration.Text = "Apply Configuration";
            btnApplyConfiguration.UseVisualStyleBackColor = false;
            btnApplyConfiguration.Click += btnApplyConfiguration_Click;
            // 
            // tglChunking
            // 
            tglChunking.BackColor = Color.Transparent;
            tglChunking.Checked = true;
            tglChunking.Font = new Font("Segoe UI", 9F);
            tglChunking.ForeColor = Color.FromArgb(225, 232, 238);
            tglChunking.ImageAlign = ContentAlignment.MiddleLeft;
            tglChunking.Location = new Point(132, 146);
            tglChunking.Name = "tglChunking";
            tglChunking.Padding = new Padding(10, 0, 10, 0);
            tglChunking.Size = new Size(151, 30);
            tglChunking.TabIndex = 6;
            tglChunking.Text = "Enabled";
            tglChunking.UseVisualStyleBackColor = false;
            tglChunking.CheckedChanged += tglChunking_CheckedChanged;
            // 
            // cboLogging
            // 
            cboLogging.BackColor = Color.FromArgb(42, 42, 46);
            cboLogging.Font = new Font("Segoe UI", 9F);
            cboLogging.ForeColor = Color.FromArgb(225, 232, 238);
            cboLogging.Location = new Point(132, 218);
            cboLogging.Name = "cboLogging";
            cboLogging.Padding = new Padding(12, 0, 38, 0);
            cboLogging.SelectionColor = Color.FromArgb(0, 135, 191);
            cboLogging.Size = new Size(151, 30);
            cboLogging.TabIndex = 5;
            // 
            // cboChunkSize
            // 
            cboChunkSize.BackColor = Color.FromArgb(42, 42, 46);
            cboChunkSize.Font = new Font("Segoe UI", 9F);
            cboChunkSize.ForeColor = Color.FromArgb(225, 232, 238);
            cboChunkSize.Location = new Point(132, 182);
            cboChunkSize.Name = "cboChunkSize";
            cboChunkSize.Padding = new Padding(12, 0, 38, 0);
            cboChunkSize.SelectionColor = Color.FromArgb(0, 135, 191);
            cboChunkSize.Size = new Size(151, 30);
            cboChunkSize.TabIndex = 4;
            // 
            // cboEnvelopeCodec
            // 
            cboEnvelopeCodec.BackColor = Color.FromArgb(42, 42, 46);
            cboEnvelopeCodec.Font = new Font("Segoe UI", 9F);
            cboEnvelopeCodec.ForeColor = Color.FromArgb(225, 232, 238);
            cboEnvelopeCodec.Location = new Point(132, 110);
            cboEnvelopeCodec.Name = "cboEnvelopeCodec";
            cboEnvelopeCodec.Padding = new Padding(12, 0, 38, 0);
            cboEnvelopeCodec.SelectionColor = Color.FromArgb(0, 135, 191);
            cboEnvelopeCodec.Size = new Size(151, 30);
            cboEnvelopeCodec.TabIndex = 3;
            // 
            // cboTransport
            // 
            cboTransport.BackColor = Color.FromArgb(42, 42, 46);
            cboTransport.Font = new Font("Segoe UI", 9F);
            cboTransport.ForeColor = Color.FromArgb(225, 232, 238);
            cboTransport.Location = new Point(132, 74);
            cboTransport.Name = "cboTransport";
            cboTransport.Padding = new Padding(12, 0, 38, 0);
            cboTransport.SelectionColor = Color.FromArgb(0, 135, 191);
            cboTransport.Size = new Size(151, 30);
            cboTransport.TabIndex = 2;
            // 
            // cboCryptoProvider
            // 
            cboCryptoProvider.BackColor = Color.FromArgb(42, 42, 46);
            cboCryptoProvider.Font = new Font("Segoe UI", 9F);
            cboCryptoProvider.ForeColor = Color.FromArgb(225, 232, 238);
            cboCryptoProvider.Location = new Point(132, 38);
            cboCryptoProvider.Name = "cboCryptoProvider";
            cboCryptoProvider.Padding = new Padding(12, 0, 38, 0);
            cboCryptoProvider.SelectionColor = Color.FromArgb(0, 135, 191);
            cboCryptoProvider.Size = new Size(151, 30);
            cboCryptoProvider.TabIndex = 1;
            // 
            // lblCryptoProvider
            // 
            lblCryptoProvider.ForeColor = Color.Gainsboro;
            lblCryptoProvider.Location = new Point(19, 38);
            lblCryptoProvider.Name = "lblCryptoProvider";
            lblCryptoProvider.Size = new Size(107, 30);
            lblCryptoProvider.TabIndex = 0;
            lblCryptoProvider.Text = "Crypto Provider";
            lblCryptoProvider.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSecurityStatus
            // 
            pnlSecurityStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlSecurityStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlSecurityStatus.BackColor = Color.FromArgb(0, 15, 28);
            pnlSecurityStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlSecurityStatus.Controls.Add(pnlTamperStatus);
            pnlSecurityStatus.Controls.Add(pnlReplayStatus);
            pnlSecurityStatus.Controls.Add(pnlIntegrityStatus);
            pnlSecurityStatus.Controls.Add(pnlTransportStatus);
            pnlSecurityStatus.Controls.Add(pnlProviderStatus);
            pnlSecurityStatus.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlSecurityStatus.ForeColor = Color.FromArgb(0, 135, 191);
            pnlSecurityStatus.Location = new Point(1527, 141);
            pnlSecurityStatus.Name = "pnlSecurityStatus";
            pnlSecurityStatus.Padding = new Padding(16);
            pnlSecurityStatus.Size = new Size(381, 533);
            pnlSecurityStatus.TabIndex = 4;
            pnlSecurityStatus.Title = "SECURITY STATUS";
            // 
            // pnlTamperStatus
            // 
            pnlTamperStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlTamperStatus.BackColor = Color.FromArgb(0, 15, 25);
            pnlTamperStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlTamperStatus.Controls.Add(lblLastCheck);
            pnlTamperStatus.Controls.Add(lblTamperState);
            pnlTamperStatus.Controls.Add(lblTamperCaption);
            pnlTamperStatus.Controls.Add(picTamperCheck);
            pnlTamperStatus.Controls.Add(picTamperStatus);
            pnlTamperStatus.ForeColor = Color.FromArgb(225, 232, 238);
            pnlTamperStatus.Location = new Point(19, 422);
            pnlTamperStatus.Name = "pnlTamperStatus";
            pnlTamperStatus.Padding = new Padding(16);
            pnlTamperStatus.Size = new Size(343, 92);
            pnlTamperStatus.TabIndex = 4;
            pnlTamperStatus.Title = "";
            // 
            // lblLastCheck
            // 
            lblLastCheck.AutoSize = true;
            lblLastCheck.ForeColor = Color.DimGray;
            lblLastCheck.Location = new Point(175, 59);
            lblLastCheck.Name = "lblLastCheck";
            lblLastCheck.Size = new Size(152, 17);
            lblLastCheck.TabIndex = 7;
            lblLastCheck.Text = "LAST CHECK: --:--:-- UTC";
            lblLastCheck.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTamperState
            // 
            lblTamperState.AutoSize = true;
            lblTamperState.ForeColor = Color.FromArgb(0, 192, 0);
            lblTamperState.Location = new Point(84, 41);
            lblTamperState.Name = "lblTamperState";
            lblTamperState.Size = new Size(48, 17);
            lblTamperState.TabIndex = 5;
            lblTamperState.Text = "CLEAN";
            // 
            // lblTamperCaption
            // 
            lblTamperCaption.Location = new Point(84, 14);
            lblTamperCaption.Name = "lblTamperCaption";
            lblTamperCaption.Size = new Size(209, 23);
            lblTamperCaption.TabIndex = 4;
            lblTamperCaption.Text = "TAMPER DETECTION";
            lblTamperCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picTamperCheck
            // 
            picTamperCheck.Image = Properties.Resources.Check;
            picTamperCheck.Location = new Point(299, 14);
            picTamperCheck.Name = "picTamperCheck";
            picTamperCheck.Size = new Size(28, 28);
            picTamperCheck.SizeMode = PictureBoxSizeMode.StretchImage;
            picTamperCheck.TabIndex = 3;
            picTamperCheck.TabStop = false;
            // 
            // picTamperStatus
            // 
            picTamperStatus.Image = Properties.Resources.TamperDetection;
            picTamperStatus.Location = new Point(14, 14);
            picTamperStatus.Name = "picTamperStatus";
            picTamperStatus.Size = new Size(64, 64);
            picTamperStatus.TabIndex = 2;
            picTamperStatus.TabStop = false;
            // 
            // pnlReplayStatus
            // 
            pnlReplayStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlReplayStatus.BackColor = Color.FromArgb(0, 15, 25);
            pnlReplayStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlReplayStatus.Controls.Add(lblReplayWindow);
            pnlReplayStatus.Controls.Add(lblReplayState);
            pnlReplayStatus.Controls.Add(lblReplayCaption);
            pnlReplayStatus.Controls.Add(picReplayCheck);
            pnlReplayStatus.Controls.Add(picReplayStatus);
            pnlReplayStatus.ForeColor = Color.FromArgb(225, 232, 238);
            pnlReplayStatus.Location = new Point(19, 324);
            pnlReplayStatus.Name = "pnlReplayStatus";
            pnlReplayStatus.Padding = new Padding(16);
            pnlReplayStatus.Size = new Size(343, 92);
            pnlReplayStatus.TabIndex = 3;
            pnlReplayStatus.Title = "";
            // 
            // lblReplayWindow
            // 
            lblReplayWindow.AutoSize = true;
            lblReplayWindow.ForeColor = Color.DimGray;
            lblReplayWindow.Location = new Point(239, 61);
            lblReplayWindow.Name = "lblReplayWindow";
            lblReplayWindow.Size = new Size(88, 17);
            lblReplayWindow.TabIndex = 7;
            lblReplayWindow.Text = "WINDOW: 64";
            lblReplayWindow.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblReplayState
            // 
            lblReplayState.AutoSize = true;
            lblReplayState.ForeColor = Color.FromArgb(0, 192, 0);
            lblReplayState.Location = new Point(84, 46);
            lblReplayState.Name = "lblReplayState";
            lblReplayState.Size = new Size(51, 17);
            lblReplayState.TabIndex = 5;
            lblReplayState.Text = "ACTIVE";
            // 
            // lblReplayCaption
            // 
            lblReplayCaption.Location = new Point(84, 14);
            lblReplayCaption.Name = "lblReplayCaption";
            lblReplayCaption.Size = new Size(209, 23);
            lblReplayCaption.TabIndex = 4;
            lblReplayCaption.Text = "REPLAY PROTECTION";
            lblReplayCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picReplayCheck
            // 
            picReplayCheck.Image = Properties.Resources.Check;
            picReplayCheck.Location = new Point(299, 14);
            picReplayCheck.Name = "picReplayCheck";
            picReplayCheck.Size = new Size(28, 28);
            picReplayCheck.SizeMode = PictureBoxSizeMode.StretchImage;
            picReplayCheck.TabIndex = 3;
            picReplayCheck.TabStop = false;
            // 
            // picReplayStatus
            // 
            picReplayStatus.Image = Properties.Resources.Replay_Protection;
            picReplayStatus.Location = new Point(14, 14);
            picReplayStatus.Name = "picReplayStatus";
            picReplayStatus.Size = new Size(64, 64);
            picReplayStatus.TabIndex = 2;
            picReplayStatus.TabStop = false;
            // 
            // pnlIntegrityStatus
            // 
            pnlIntegrityStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlIntegrityStatus.BackColor = Color.FromArgb(0, 15, 25);
            pnlIntegrityStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlIntegrityStatus.Controls.Add(lblIntegrityState);
            pnlIntegrityStatus.Controls.Add(lblIntegrityCaption);
            pnlIntegrityStatus.Controls.Add(picIntegrityCheck);
            pnlIntegrityStatus.Controls.Add(picIntegrityStatus);
            pnlIntegrityStatus.ForeColor = Color.FromArgb(225, 232, 238);
            pnlIntegrityStatus.Location = new Point(19, 226);
            pnlIntegrityStatus.Name = "pnlIntegrityStatus";
            pnlIntegrityStatus.Padding = new Padding(16);
            pnlIntegrityStatus.Size = new Size(343, 92);
            pnlIntegrityStatus.TabIndex = 2;
            pnlIntegrityStatus.Title = "";
            // 
            // lblIntegrityState
            // 
            lblIntegrityState.AutoSize = true;
            lblIntegrityState.ForeColor = Color.FromArgb(0, 192, 0);
            lblIntegrityState.Location = new Point(84, 46);
            lblIntegrityState.Name = "lblIntegrityState";
            lblIntegrityState.Size = new Size(49, 17);
            lblIntegrityState.TabIndex = 5;
            lblIntegrityState.Text = "READY";
            // 
            // lblIntegrityCaption
            // 
            lblIntegrityCaption.Location = new Point(84, 14);
            lblIntegrityCaption.Name = "lblIntegrityCaption";
            lblIntegrityCaption.Size = new Size(209, 23);
            lblIntegrityCaption.TabIndex = 4;
            lblIntegrityCaption.Text = "INTEGRITY CHECK";
            lblIntegrityCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picIntegrityCheck
            // 
            picIntegrityCheck.Image = Properties.Resources.Check;
            picIntegrityCheck.Location = new Point(299, 14);
            picIntegrityCheck.Name = "picIntegrityCheck";
            picIntegrityCheck.Size = new Size(28, 28);
            picIntegrityCheck.SizeMode = PictureBoxSizeMode.StretchImage;
            picIntegrityCheck.TabIndex = 3;
            picIntegrityCheck.TabStop = false;
            // 
            // picIntegrityStatus
            // 
            picIntegrityStatus.Image = Properties.Resources.Shield;
            picIntegrityStatus.Location = new Point(14, 14);
            picIntegrityStatus.Name = "picIntegrityStatus";
            picIntegrityStatus.Size = new Size(64, 64);
            picIntegrityStatus.TabIndex = 2;
            picIntegrityStatus.TabStop = false;
            // 
            // pnlTransportStatus
            // 
            pnlTransportStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlTransportStatus.BackColor = Color.FromArgb(0, 15, 25);
            pnlTransportStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlTransportStatus.Controls.Add(lblTransportState);
            pnlTransportStatus.Controls.Add(lblTransportValue);
            pnlTransportStatus.Controls.Add(lblTransportCaption);
            pnlTransportStatus.Controls.Add(picTransportCheck);
            pnlTransportStatus.Controls.Add(picTransportStatus);
            pnlTransportStatus.ForeColor = Color.FromArgb(225, 232, 238);
            pnlTransportStatus.Location = new Point(19, 128);
            pnlTransportStatus.Name = "pnlTransportStatus";
            pnlTransportStatus.Padding = new Padding(16);
            pnlTransportStatus.Size = new Size(343, 92);
            pnlTransportStatus.TabIndex = 1;
            pnlTransportStatus.Title = "";
            // 
            // lblTransportState
            // 
            lblTransportState.AutoSize = true;
            lblTransportState.ForeColor = Color.DimGray;
            lblTransportState.Location = new Point(243, 58);
            lblTransportState.Name = "lblTransportState";
            lblTransportState.Size = new Size(84, 17);
            lblTransportState.TabIndex = 6;
            lblTransportState.Text = "CONNECTED";
            lblTransportState.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTransportValue
            // 
            lblTransportValue.AutoSize = true;
            lblTransportValue.ForeColor = Color.FromArgb(0, 192, 0);
            lblTransportValue.Location = new Point(84, 46);
            lblTransportValue.Name = "lblTransportValue";
            lblTransportValue.Size = new Size(84, 17);
            lblTransportValue.TabIndex = 5;
            lblTransportValue.Text = "IN-MEMORY";
            // 
            // lblTransportCaption
            // 
            lblTransportCaption.Location = new Point(84, 14);
            lblTransportCaption.Name = "lblTransportCaption";
            lblTransportCaption.Size = new Size(209, 23);
            lblTransportCaption.TabIndex = 4;
            lblTransportCaption.Text = "TRANSPORT STATUS";
            lblTransportCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picTransportCheck
            // 
            picTransportCheck.Image = Properties.Resources.Check;
            picTransportCheck.Location = new Point(299, 15);
            picTransportCheck.Name = "picTransportCheck";
            picTransportCheck.Size = new Size(28, 28);
            picTransportCheck.SizeMode = PictureBoxSizeMode.StretchImage;
            picTransportCheck.TabIndex = 3;
            picTransportCheck.TabStop = false;
            // 
            // picTransportStatus
            // 
            picTransportStatus.Image = Properties.Resources.Transport;
            picTransportStatus.Location = new Point(14, 14);
            picTransportStatus.Name = "picTransportStatus";
            picTransportStatus.Size = new Size(64, 64);
            picTransportStatus.TabIndex = 1;
            picTransportStatus.TabStop = false;
            // 
            // pnlProviderStatus
            // 
            pnlProviderStatus.AlignTitle = ContentAlignment.TopLeft;
            pnlProviderStatus.BackColor = Color.FromArgb(0, 15, 25);
            pnlProviderStatus.BorderColor = Color.FromArgb(34, 61, 84);
            pnlProviderStatus.Controls.Add(lblProviderReady);
            pnlProviderStatus.Controls.Add(lblProviderValue);
            pnlProviderStatus.Controls.Add(picProviderCheck);
            pnlProviderStatus.Controls.Add(lblProviderCaption);
            pnlProviderStatus.Controls.Add(picProviderStatus);
            pnlProviderStatus.ForeColor = Color.FromArgb(225, 232, 238);
            pnlProviderStatus.Location = new Point(19, 30);
            pnlProviderStatus.Name = "pnlProviderStatus";
            pnlProviderStatus.Padding = new Padding(16);
            pnlProviderStatus.Size = new Size(343, 92);
            pnlProviderStatus.TabIndex = 0;
            pnlProviderStatus.Title = "";
            // 
            // lblProviderReady
            // 
            lblProviderReady.AutoSize = true;
            lblProviderReady.ForeColor = Color.DimGray;
            lblProviderReady.Location = new Point(278, 61);
            lblProviderReady.Name = "lblProviderReady";
            lblProviderReady.Size = new Size(49, 17);
            lblProviderReady.TabIndex = 4;
            lblProviderReady.Text = "READY";
            lblProviderReady.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblProviderValue
            // 
            lblProviderValue.AutoSize = true;
            lblProviderValue.ForeColor = Color.FromArgb(0, 192, 0);
            lblProviderValue.Location = new Point(84, 46);
            lblProviderValue.Name = "lblProviderValue";
            lblProviderValue.Size = new Size(65, 17);
            lblProviderValue.TabIndex = 3;
            lblProviderValue.Text = "AES-GCM";
            // 
            // picProviderCheck
            // 
            picProviderCheck.Image = Properties.Resources.Check;
            picProviderCheck.Location = new Point(299, 14);
            picProviderCheck.Name = "picProviderCheck";
            picProviderCheck.Size = new Size(28, 28);
            picProviderCheck.SizeMode = PictureBoxSizeMode.StretchImage;
            picProviderCheck.TabIndex = 2;
            picProviderCheck.TabStop = false;
            // 
            // lblProviderCaption
            // 
            lblProviderCaption.Location = new Point(84, 14);
            lblProviderCaption.Name = "lblProviderCaption";
            lblProviderCaption.Size = new Size(209, 23);
            lblProviderCaption.TabIndex = 1;
            lblProviderCaption.Text = "PROVIDER STATUS";
            lblProviderCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picProviderStatus
            // 
            picProviderStatus.Image = Properties.Resources.Shield;
            picProviderStatus.Location = new Point(14, 14);
            picProviderStatus.Name = "picProviderStatus";
            picProviderStatus.Size = new Size(64, 64);
            picProviderStatus.TabIndex = 0;
            picProviderStatus.TabStop = false;
            // 
            // pnlExchangeFlow
            // 
            pnlExchangeFlow.AlignTitle = ContentAlignment.TopLeft;
            pnlExchangeFlow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlExchangeFlow.BackColor = Color.FromArgb(0, 15, 28);
            pnlExchangeFlow.BorderColor = Color.FromArgb(34, 61, 84);
            pnlExchangeFlow.Controls.Add(lblFlowMercuryReceive);
            pnlExchangeFlow.Controls.Add(lblValidateEnveleope);
            pnlExchangeFlow.Controls.Add(lblFlowTransport);
            pnlExchangeFlow.Controls.Add(lblFlowProvider);
            pnlExchangeFlow.Controls.Add(lblFlowMercurySend);
            pnlExchangeFlow.Controls.Add(lblFlowApplication);
            pnlExchangeFlow.Controls.Add(lblFlowArrow5);
            pnlExchangeFlow.Controls.Add(lblFlowArrow4);
            pnlExchangeFlow.Controls.Add(lblFlowArrow3);
            pnlExchangeFlow.Controls.Add(lblFlowArrow2);
            pnlExchangeFlow.Controls.Add(lblFlowArrow1);
            pnlExchangeFlow.Controls.Add(picValidateEnveleope);
            pnlExchangeFlow.Controls.Add(picFlowTransport);
            pnlExchangeFlow.Controls.Add(picFlowProtected);
            pnlExchangeFlow.Controls.Add(picFlowMercuryReceived);
            pnlExchangeFlow.Controls.Add(picFlowMercurySend);
            pnlExchangeFlow.Controls.Add(picFlowApplication);
            pnlExchangeFlow.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlExchangeFlow.ForeColor = Color.FromArgb(0, 135, 191);
            pnlExchangeFlow.Location = new Point(323, 49);
            pnlExchangeFlow.Name = "pnlExchangeFlow";
            pnlExchangeFlow.Padding = new Padding(16);
            pnlExchangeFlow.Size = new Size(1198, 173);
            pnlExchangeFlow.TabIndex = 5;
            pnlExchangeFlow.Title = "SECURE EXCHANGE FLOW";
            // 
            // lblFlowMercuryReceive
            // 
            lblFlowMercuryReceive.ForeColor = Color.Gainsboro;
            lblFlowMercuryReceive.Location = new Point(1066, 129);
            lblFlowMercuryReceive.Name = "lblFlowMercuryReceive";
            lblFlowMercuryReceive.Size = new Size(96, 38);
            lblFlowMercuryReceive.TabIndex = 16;
            lblFlowMercuryReceive.Text = "MERCURY RECEIVE SIDE";
            lblFlowMercuryReceive.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblValidateEnveleope
            // 
            lblValidateEnveleope.ForeColor = Color.Gainsboro;
            lblValidateEnveleope.Location = new Point(862, 129);
            lblValidateEnveleope.Name = "lblValidateEnveleope";
            lblValidateEnveleope.Size = new Size(96, 38);
            lblValidateEnveleope.TabIndex = 15;
            lblValidateEnveleope.Text = "VALIDATE ENVELOPE";
            lblValidateEnveleope.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFlowTransport
            // 
            lblFlowTransport.ForeColor = Color.Gainsboro;
            lblFlowTransport.Location = new Point(602, 129);
            lblFlowTransport.Name = "lblFlowTransport";
            lblFlowTransport.Size = new Size(204, 38);
            lblFlowTransport.TabIndex = 14;
            lblFlowTransport.Text = "IN-MEMORY";
            lblFlowTransport.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFlowProvider
            // 
            lblFlowProvider.ForeColor = Color.Gainsboro;
            lblFlowProvider.Location = new Point(406, 129);
            lblFlowProvider.Name = "lblFlowProvider";
            lblFlowProvider.Size = new Size(190, 38);
            lblFlowProvider.TabIndex = 13;
            lblFlowProvider.Text = "AES-GCM";
            lblFlowProvider.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFlowMercurySend
            // 
            lblFlowMercurySend.ForeColor = Color.Gainsboro;
            lblFlowMercurySend.Location = new Point(250, 129);
            lblFlowMercurySend.Name = "lblFlowMercurySend";
            lblFlowMercurySend.Size = new Size(96, 38);
            lblFlowMercurySend.TabIndex = 12;
            lblFlowMercurySend.Text = "MERCURY SEND SIDE";
            lblFlowMercurySend.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFlowApplication
            // 
            lblFlowApplication.ForeColor = Color.Gainsboro;
            lblFlowApplication.Location = new Point(46, 129);
            lblFlowApplication.Name = "lblFlowApplication";
            lblFlowApplication.Size = new Size(96, 38);
            lblFlowApplication.TabIndex = 11;
            lblFlowApplication.Text = "APPLICATION SENDER";
            lblFlowApplication.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFlowArrow5
            // 
            lblFlowArrow5.Image = Properties.Resources.GreenArrow;
            lblFlowArrow5.Location = new Point(994, 55);
            lblFlowArrow5.Name = "lblFlowArrow5";
            lblFlowArrow5.Size = new Size(40, 40);
            lblFlowArrow5.SizeMode = PictureBoxSizeMode.StretchImage;
            lblFlowArrow5.TabIndex = 10;
            lblFlowArrow5.TabStop = false;
            // 
            // lblFlowArrow4
            // 
            lblFlowArrow4.Image = Properties.Resources.GreenArrow;
            lblFlowArrow4.Location = new Point(790, 55);
            lblFlowArrow4.Name = "lblFlowArrow4";
            lblFlowArrow4.Size = new Size(40, 40);
            lblFlowArrow4.SizeMode = PictureBoxSizeMode.StretchImage;
            lblFlowArrow4.TabIndex = 9;
            lblFlowArrow4.TabStop = false;
            // 
            // lblFlowArrow3
            // 
            lblFlowArrow3.Image = Properties.Resources.GreenArrow;
            lblFlowArrow3.Location = new Point(584, 55);
            lblFlowArrow3.Name = "lblFlowArrow3";
            lblFlowArrow3.Size = new Size(40, 40);
            lblFlowArrow3.SizeMode = PictureBoxSizeMode.StretchImage;
            lblFlowArrow3.TabIndex = 8;
            lblFlowArrow3.TabStop = false;
            // 
            // lblFlowArrow2
            // 
            lblFlowArrow2.Image = Properties.Resources.GreenArrow;
            lblFlowArrow2.Location = new Point(381, 55);
            lblFlowArrow2.Name = "lblFlowArrow2";
            lblFlowArrow2.Size = new Size(40, 40);
            lblFlowArrow2.SizeMode = PictureBoxSizeMode.StretchImage;
            lblFlowArrow2.TabIndex = 7;
            lblFlowArrow2.TabStop = false;
            // 
            // lblFlowArrow1
            // 
            lblFlowArrow1.Image = Properties.Resources.GreenArrow;
            lblFlowArrow1.Location = new Point(180, 55);
            lblFlowArrow1.Name = "lblFlowArrow1";
            lblFlowArrow1.Size = new Size(40, 40);
            lblFlowArrow1.SizeMode = PictureBoxSizeMode.StretchImage;
            lblFlowArrow1.TabIndex = 6;
            lblFlowArrow1.TabStop = false;
            // 
            // picValidateEnveleope
            // 
            picValidateEnveleope.Image = Properties.Resources.EnvelopeVerify;
            picValidateEnveleope.Location = new Point(862, 30);
            picValidateEnveleope.Name = "picValidateEnveleope";
            picValidateEnveleope.Size = new Size(96, 96);
            picValidateEnveleope.SizeMode = PictureBoxSizeMode.StretchImage;
            picValidateEnveleope.TabIndex = 5;
            picValidateEnveleope.TabStop = false;
            // 
            // picFlowTransport
            // 
            picFlowTransport.Image = Properties.Resources.Transport128;
            picFlowTransport.Location = new Point(658, 30);
            picFlowTransport.Name = "picFlowTransport";
            picFlowTransport.Size = new Size(96, 96);
            picFlowTransport.SizeMode = PictureBoxSizeMode.StretchImage;
            picFlowTransport.TabIndex = 4;
            picFlowTransport.TabStop = false;
            // 
            // picFlowProtected
            // 
            picFlowProtected.Image = Properties.Resources.MercuryEnvelope;
            picFlowProtected.Location = new Point(454, 30);
            picFlowProtected.Name = "picFlowProtected";
            picFlowProtected.Size = new Size(96, 96);
            picFlowProtected.SizeMode = PictureBoxSizeMode.StretchImage;
            picFlowProtected.TabIndex = 3;
            picFlowProtected.TabStop = false;
            // 
            // picFlowMercuryReceived
            // 
            picFlowMercuryReceived.Image = Properties.Resources.MercuryIcon128;
            picFlowMercuryReceived.Location = new Point(1066, 30);
            picFlowMercuryReceived.Name = "picFlowMercuryReceived";
            picFlowMercuryReceived.Size = new Size(96, 96);
            picFlowMercuryReceived.SizeMode = PictureBoxSizeMode.StretchImage;
            picFlowMercuryReceived.TabIndex = 2;
            picFlowMercuryReceived.TabStop = false;
            // 
            // picFlowMercurySend
            // 
            picFlowMercurySend.Image = Properties.Resources.MercuryIcon128;
            picFlowMercurySend.Location = new Point(250, 30);
            picFlowMercurySend.Name = "picFlowMercurySend";
            picFlowMercurySend.Size = new Size(96, 96);
            picFlowMercurySend.SizeMode = PictureBoxSizeMode.StretchImage;
            picFlowMercurySend.TabIndex = 1;
            picFlowMercurySend.TabStop = false;
            // 
            // picFlowApplication
            // 
            picFlowApplication.Image = Properties.Resources.ApplicationOut128;
            picFlowApplication.Location = new Point(46, 30);
            picFlowApplication.Name = "picFlowApplication";
            picFlowApplication.Size = new Size(96, 96);
            picFlowApplication.SizeMode = PictureBoxSizeMode.StretchImage;
            picFlowApplication.TabIndex = 0;
            picFlowApplication.TabStop = false;
            // 
            // pnlSendData
            // 
            pnlSendData.AlignTitle = ContentAlignment.TopLeft;
            pnlSendData.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSendData.BackColor = Color.FromArgb(0, 15, 28);
            pnlSendData.BorderColor = Color.FromArgb(34, 61, 84);
            pnlSendData.Controls.Add(btnSendFile);
            pnlSendData.Controls.Add(lblSendPayload);
            pnlSendData.Controls.Add(txtSendPayload);
            pnlSendData.Controls.Add(btnSend);
            pnlSendData.ForeColor = Color.FromArgb(0, 135, 191);
            pnlSendData.Location = new Point(323, 232);
            pnlSendData.Name = "pnlSendData";
            pnlSendData.Padding = new Padding(16);
            pnlSendData.Size = new Size(596, 442);
            pnlSendData.TabIndex = 6;
            pnlSendData.Title = "SEND DATA";
            // 
            // btnSendFile
            // 
            btnSendFile.BackColor = Color.Transparent;
            btnSendFile.Font = new Font("Segoe UI", 9F);
            btnSendFile.ForeColor = Color.FromArgb(225, 232, 238);
            btnSendFile.ImageAlign = ContentAlignment.MiddleLeft;
            btnSendFile.Location = new Point(19, 378);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Padding = new Padding(10, 0, 10, 0);
            btnSendFile.Size = new Size(162, 45);
            btnSendFile.TabIndex = 5;
            btnSendFile.Text = "Send File";
            btnSendFile.UseVisualStyleBackColor = false;
            btnSendFile.Click += btnSendFile_Click;
            // 
            // lblSendPayload
            // 
            lblSendPayload.AutoSize = true;
            lblSendPayload.ForeColor = Color.Gainsboro;
            lblSendPayload.Location = new Point(19, 31);
            lblSendPayload.Name = "lblSendPayload";
            lblSendPayload.Size = new Size(49, 15);
            lblSendPayload.TabIndex = 5;
            lblSendPayload.Text = "Payload";
            // 
            // txtSendPayload
            // 
            txtSendPayload.BackColor = Color.FromArgb(0, 15, 28);
            txtSendPayload.BorderColor = Color.FromArgb(34, 61, 84);
            txtSendPayload.CornerStyle = WinForms.Controls.Enums.CornerStyle.Squared;
            txtSendPayload.Font = new Font("Segoe UI", 9F);
            txtSendPayload.ForeColor = Color.FromArgb(225, 232, 238);
            txtSendPayload.Location = new Point(19, 52);
            txtSendPayload.MinimumSize = new Size(50, 32);
            txtSendPayload.Multiline = true;
            txtSendPayload.Name = "txtSendPayload";
            txtSendPayload.Padding = new Padding(10, 6, 10, 6);
            txtSendPayload.Size = new Size(558, 316);
            txtSendPayload.TabIndex = 2;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.Transparent;
            btnSend.Font = new Font("Segoe UI", 9F);
            btnSend.ForeColor = Color.FromArgb(225, 232, 238);
            btnSend.ImageAlign = ContentAlignment.MiddleLeft;
            btnSend.Location = new Point(415, 378);
            btnSend.Name = "btnSend";
            btnSend.Padding = new Padding(10, 0, 10, 0);
            btnSend.Size = new Size(162, 45);
            btnSend.TabIndex = 4;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // pnlEventLog
            // 
            pnlEventLog.AlignTitle = ContentAlignment.TopLeft;
            pnlEventLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlEventLog.BackColor = Color.FromArgb(0, 15, 28);
            pnlEventLog.BorderColor = Color.FromArgb(34, 61, 84);
            pnlEventLog.Controls.Add(btnClearLog);
            pnlEventLog.Controls.Add(lblEventLog);
            pnlEventLog.ForeColor = Color.FromArgb(0, 135, 191);
            pnlEventLog.Location = new Point(12, 680);
            pnlEventLog.Name = "pnlEventLog";
            pnlEventLog.Padding = new Padding(16);
            pnlEventLog.Size = new Size(723, 387);
            pnlEventLog.TabIndex = 7;
            pnlEventLog.Title = "EVENT LOG";
            // 
            // btnClearLog
            // 
            btnClearLog.BackColor = Color.Transparent;
            btnClearLog.Font = new Font("Segoe UI", 9F);
            btnClearLog.ForeColor = Color.FromArgb(225, 232, 238);
            btnClearLog.ImageAlign = ContentAlignment.MiddleLeft;
            btnClearLog.Location = new Point(533, 326);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Padding = new Padding(10, 0, 10, 0);
            btnClearLog.Size = new Size(171, 49);
            btnClearLog.TabIndex = 8;
            btnClearLog.Text = "Clear Log";
            btnClearLog.UseVisualStyleBackColor = false;
            btnClearLog.Click += btnClearLog_Click;
            // 
            // lblEventLog
            // 
            lblEventLog.AlignTitle = ContentAlignment.TopLeft;
            lblEventLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblEventLog.BackColor = Color.FromArgb(0, 15, 28);
            lblEventLog.BorderColor = Color.FromArgb(34, 61, 84);
            lblEventLog.Controls.Add(rtbEventLog);
            lblEventLog.CornerStyle = WinForms.Controls.Enums.CornerStyle.Squared;
            lblEventLog.ForeColor = Color.FromArgb(225, 232, 238);
            lblEventLog.Location = new Point(19, 31);
            lblEventLog.Name = "lblEventLog";
            lblEventLog.Padding = new Padding(16);
            lblEventLog.Size = new Size(685, 284);
            lblEventLog.TabIndex = 0;
            lblEventLog.Title = "";
            // 
            // rtbEventLog
            // 
            rtbEventLog.BackColor = Color.FromArgb(0, 15, 28);
            rtbEventLog.BorderStyle = BorderStyle.None;
            rtbEventLog.Dock = DockStyle.Fill;
            rtbEventLog.Location = new Point(16, 16);
            rtbEventLog.Name = "rtbEventLog";
            rtbEventLog.ReadOnly = true;
            rtbEventLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbEventLog.Size = new Size(653, 252);
            rtbEventLog.TabIndex = 0;
            rtbEventLog.Text = "";
            // 
            // pnlEnvelopeViewer
            // 
            pnlEnvelopeViewer.AlignTitle = ContentAlignment.TopLeft;
            pnlEnvelopeViewer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pnlEnvelopeViewer.BackColor = Color.FromArgb(0, 15, 28);
            pnlEnvelopeViewer.BorderColor = Color.FromArgb(34, 61, 84);
            pnlEnvelopeViewer.Controls.Add(lblRawPayloadValue);
            pnlEnvelopeViewer.Controls.Add(lblFrameSizeValue);
            pnlEnvelopeViewer.Controls.Add(lblProtectedSizeValue);
            pnlEnvelopeViewer.Controls.Add(lblReplayTokenValue);
            pnlEnvelopeViewer.Controls.Add(lblAlgorithmValue);
            pnlEnvelopeViewer.Controls.Add(lblEnvelopeIdValue);
            pnlEnvelopeViewer.Controls.Add(lblEnvelopeVersionValue);
            pnlEnvelopeViewer.Controls.Add(lblRawPayload);
            pnlEnvelopeViewer.Controls.Add(lblFrameSize);
            pnlEnvelopeViewer.Controls.Add(lblProtectedSize);
            pnlEnvelopeViewer.Controls.Add(lblReplayToken);
            pnlEnvelopeViewer.Controls.Add(lblAlgorithm);
            pnlEnvelopeViewer.Controls.Add(lblEnvelopeId);
            pnlEnvelopeViewer.Controls.Add(lblEnvelopeVersion);
            pnlEnvelopeViewer.Controls.Add(lblHexPreview);
            pnlEnvelopeViewer.Controls.Add(pnlHexPreview);
            pnlEnvelopeViewer.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlEnvelopeViewer.ForeColor = Color.FromArgb(0, 135, 191);
            pnlEnvelopeViewer.Location = new Point(741, 680);
            pnlEnvelopeViewer.Name = "pnlEnvelopeViewer";
            pnlEnvelopeViewer.Padding = new Padding(16);
            pnlEnvelopeViewer.Size = new Size(1167, 387);
            pnlEnvelopeViewer.TabIndex = 8;
            pnlEnvelopeViewer.Title = "PROTECTED FRAME / SECUREENVELOPE VIEWER";
            // 
            // lblRawPayloadValue
            // 
            lblRawPayloadValue.ForeColor = Color.Gainsboro;
            lblRawPayloadValue.Location = new Point(187, 210);
            lblRawPayloadValue.Name = "lblRawPayloadValue";
            lblRawPayloadValue.Size = new Size(389, 30);
            lblRawPayloadValue.TabIndex = 18;
            lblRawPayloadValue.Text = "-";
            lblRawPayloadValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFrameSizeValue
            // 
            lblFrameSizeValue.ForeColor = Color.Gainsboro;
            lblFrameSizeValue.Location = new Point(187, 180);
            lblFrameSizeValue.Name = "lblFrameSizeValue";
            lblFrameSizeValue.Size = new Size(389, 30);
            lblFrameSizeValue.TabIndex = 17;
            lblFrameSizeValue.Text = "-";
            lblFrameSizeValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblProtectedSizeValue
            // 
            lblProtectedSizeValue.ForeColor = Color.Gainsboro;
            lblProtectedSizeValue.Location = new Point(187, 150);
            lblProtectedSizeValue.Name = "lblProtectedSizeValue";
            lblProtectedSizeValue.Size = new Size(389, 30);
            lblProtectedSizeValue.TabIndex = 16;
            lblProtectedSizeValue.Text = "-";
            lblProtectedSizeValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblReplayTokenValue
            // 
            lblReplayTokenValue.ForeColor = Color.Gainsboro;
            lblReplayTokenValue.Location = new Point(187, 120);
            lblReplayTokenValue.Name = "lblReplayTokenValue";
            lblReplayTokenValue.Size = new Size(389, 30);
            lblReplayTokenValue.TabIndex = 15;
            lblReplayTokenValue.Text = "-";
            lblReplayTokenValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAlgorithmValue
            // 
            lblAlgorithmValue.ForeColor = Color.Gainsboro;
            lblAlgorithmValue.Location = new Point(187, 90);
            lblAlgorithmValue.Name = "lblAlgorithmValue";
            lblAlgorithmValue.Size = new Size(389, 30);
            lblAlgorithmValue.TabIndex = 14;
            lblAlgorithmValue.Text = "-";
            lblAlgorithmValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEnvelopeIdValue
            // 
            lblEnvelopeIdValue.ForeColor = Color.Gainsboro;
            lblEnvelopeIdValue.Location = new Point(187, 60);
            lblEnvelopeIdValue.Name = "lblEnvelopeIdValue";
            lblEnvelopeIdValue.Size = new Size(389, 30);
            lblEnvelopeIdValue.TabIndex = 13;
            lblEnvelopeIdValue.Text = "-";
            lblEnvelopeIdValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEnvelopeVersionValue
            // 
            lblEnvelopeVersionValue.ForeColor = Color.Gainsboro;
            lblEnvelopeVersionValue.Location = new Point(187, 28);
            lblEnvelopeVersionValue.Name = "lblEnvelopeVersionValue";
            lblEnvelopeVersionValue.Size = new Size(389, 30);
            lblEnvelopeVersionValue.TabIndex = 12;
            lblEnvelopeVersionValue.Text = "-";
            lblEnvelopeVersionValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRawPayload
            // 
            lblRawPayload.ForeColor = Color.Gainsboro;
            lblRawPayload.Location = new Point(19, 210);
            lblRawPayload.Name = "lblRawPayload";
            lblRawPayload.Size = new Size(162, 30);
            lblRawPayload.TabIndex = 9;
            lblRawPayload.Text = "Raw Payload Visible";
            lblRawPayload.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFrameSize
            // 
            lblFrameSize.ForeColor = Color.Gainsboro;
            lblFrameSize.Location = new Point(19, 180);
            lblFrameSize.Name = "lblFrameSize";
            lblFrameSize.Size = new Size(162, 30);
            lblFrameSize.TabIndex = 8;
            lblFrameSize.Text = "Total Frame Size";
            lblFrameSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblProtectedSize
            // 
            lblProtectedSize.ForeColor = Color.Gainsboro;
            lblProtectedSize.Location = new Point(19, 150);
            lblProtectedSize.Name = "lblProtectedSize";
            lblProtectedSize.Size = new Size(162, 30);
            lblProtectedSize.TabIndex = 7;
            lblProtectedSize.Text = "Protected Payload Size";
            lblProtectedSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblReplayToken
            // 
            lblReplayToken.ForeColor = Color.Gainsboro;
            lblReplayToken.Location = new Point(19, 120);
            lblReplayToken.Name = "lblReplayToken";
            lblReplayToken.Size = new Size(162, 30);
            lblReplayToken.TabIndex = 6;
            lblReplayToken.Text = "Replay Token";
            lblReplayToken.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAlgorithm
            // 
            lblAlgorithm.ForeColor = Color.Gainsboro;
            lblAlgorithm.Location = new Point(19, 90);
            lblAlgorithm.Name = "lblAlgorithm";
            lblAlgorithm.Size = new Size(162, 30);
            lblAlgorithm.TabIndex = 5;
            lblAlgorithm.Text = "Algorithm";
            lblAlgorithm.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEnvelopeId
            // 
            lblEnvelopeId.ForeColor = Color.Gainsboro;
            lblEnvelopeId.Location = new Point(19, 60);
            lblEnvelopeId.Name = "lblEnvelopeId";
            lblEnvelopeId.Size = new Size(162, 30);
            lblEnvelopeId.TabIndex = 4;
            lblEnvelopeId.Text = "Envelope ID";
            lblEnvelopeId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEnvelopeVersion
            // 
            lblEnvelopeVersion.ForeColor = Color.Gainsboro;
            lblEnvelopeVersion.Location = new Point(19, 28);
            lblEnvelopeVersion.Name = "lblEnvelopeVersion";
            lblEnvelopeVersion.Size = new Size(162, 30);
            lblEnvelopeVersion.TabIndex = 3;
            lblEnvelopeVersion.Text = "Version";
            lblEnvelopeVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHexPreview
            // 
            lblHexPreview.AutoSize = true;
            lblHexPreview.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHexPreview.Location = new Point(597, 5);
            lblHexPreview.Name = "lblHexPreview";
            lblHexPreview.Size = new Size(213, 17);
            lblHexPreview.TabIndex = 2;
            lblHexPreview.Text = "PROTECTED FRAME HEX PREVIEW";
            // 
            // pnlHexPreview
            // 
            pnlHexPreview.AlignTitle = ContentAlignment.TopLeft;
            pnlHexPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlHexPreview.BackColor = Color.FromArgb(0, 15, 28);
            pnlHexPreview.BorderColor = Color.FromArgb(34, 61, 84);
            pnlHexPreview.Controls.Add(rtbHexPreview);
            pnlHexPreview.CornerStyle = WinForms.Controls.Enums.CornerStyle.Squared;
            pnlHexPreview.ForeColor = Color.FromArgb(0, 135, 191);
            pnlHexPreview.Location = new Point(595, 31);
            pnlHexPreview.Name = "pnlHexPreview";
            pnlHexPreview.Padding = new Padding(16, 32, 16, 16);
            pnlHexPreview.Size = new Size(553, 336);
            pnlHexPreview.TabIndex = 1;
            pnlHexPreview.Title = "";
            // 
            // rtbHexPreview
            // 
            rtbHexPreview.BackColor = Color.FromArgb(0, 15, 28);
            rtbHexPreview.BorderStyle = BorderStyle.None;
            rtbHexPreview.Dock = DockStyle.Fill;
            rtbHexPreview.ForeColor = Color.LimeGreen;
            rtbHexPreview.Location = new Point(16, 32);
            rtbHexPreview.Name = "rtbHexPreview";
            rtbHexPreview.ReadOnly = true;
            rtbHexPreview.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbHexPreview.Size = new Size(521, 288);
            rtbHexPreview.TabIndex = 0;
            rtbHexPreview.Text = "";
            // 
            // pnlDemoActions
            // 
            pnlDemoActions.AlignTitle = ContentAlignment.TopLeft;
            pnlDemoActions.BackColor = Color.FromArgb(0, 15, 28);
            pnlDemoActions.BorderColor = Color.FromArgb(34, 61, 84);
            pnlDemoActions.Controls.Add(tglWrongKeyAttack);
            pnlDemoActions.Controls.Add(tglTamperAttack);
            pnlDemoActions.Controls.Add(tglReplayAttack);
            pnlDemoActions.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlDemoActions.ForeColor = Color.FromArgb(0, 135, 191);
            pnlDemoActions.Location = new Point(12, 470);
            pnlDemoActions.Name = "pnlDemoActions";
            pnlDemoActions.Padding = new Padding(16);
            pnlDemoActions.Size = new Size(302, 204);
            pnlDemoActions.TabIndex = 10;
            pnlDemoActions.Title = "SECURITY DEMONSTRATIONS";
            // 
            // tglWrongKeyAttack
            // 
            tglWrongKeyAttack.BackColor = Color.Transparent;
            tglWrongKeyAttack.ButtonColor = Color.FromArgb(192, 0, 0);
            tglWrongKeyAttack.Font = new Font("Segoe UI", 9F);
            tglWrongKeyAttack.ForeColor = Color.Gainsboro;
            tglWrongKeyAttack.ImageAlign = ContentAlignment.MiddleLeft;
            tglWrongKeyAttack.Location = new Point(19, 142);
            tglWrongKeyAttack.Name = "tglWrongKeyAttack";
            tglWrongKeyAttack.Padding = new Padding(10, 0, 10, 0);
            tglWrongKeyAttack.Size = new Size(264, 49);
            tglWrongKeyAttack.TabIndex = 2;
            tglWrongKeyAttack.Text = "Wrong key Attack";
            tglWrongKeyAttack.UseVisualStyleBackColor = false;
            tglWrongKeyAttack.CheckedChanged += tglWrongKeyAttack_CheckedChanged;
            // 
            // tglTamperAttack
            // 
            tglTamperAttack.BackColor = Color.Transparent;
            tglTamperAttack.ButtonColor = Color.FromArgb(192, 0, 0);
            tglTamperAttack.Enabled = false;
            tglTamperAttack.Font = new Font("Segoe UI", 9F);
            tglTamperAttack.ForeColor = Color.Gainsboro;
            tglTamperAttack.ImageAlign = ContentAlignment.MiddleLeft;
            tglTamperAttack.Location = new Point(19, 87);
            tglTamperAttack.Name = "tglTamperAttack";
            tglTamperAttack.Padding = new Padding(10, 0, 10, 0);
            tglTamperAttack.Size = new Size(264, 49);
            tglTamperAttack.TabIndex = 1;
            tglTamperAttack.Text = "Tamper Attack";
            tglTamperAttack.UseVisualStyleBackColor = false;
            tglTamperAttack.CheckedChanged += tglTamperAttack_CheckedChanged;
            // 
            // tglReplayAttack
            // 
            tglReplayAttack.BackColor = Color.Transparent;
            tglReplayAttack.ButtonColor = Color.FromArgb(192, 0, 0);
            tglReplayAttack.Enabled = false;
            tglReplayAttack.Font = new Font("Segoe UI", 9F);
            tglReplayAttack.ForeColor = Color.Gainsboro;
            tglReplayAttack.ImageAlign = ContentAlignment.MiddleLeft;
            tglReplayAttack.Location = new Point(19, 32);
            tglReplayAttack.Name = "tglReplayAttack";
            tglReplayAttack.Padding = new Padding(10, 0, 10, 0);
            tglReplayAttack.Size = new Size(264, 49);
            tglReplayAttack.TabIndex = 0;
            tglReplayAttack.Text = "Replay Attack";
            tglReplayAttack.UseVisualStyleBackColor = false;
            tglReplayAttack.CheckedChanged += tglReplayAttack_CheckedChanged;
            // 
            // pnlReceivedData
            // 
            pnlReceivedData.AlignTitle = ContentAlignment.TopLeft;
            pnlReceivedData.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlReceivedData.BackColor = Color.FromArgb(0, 15, 28);
            pnlReceivedData.BorderColor = Color.FromArgb(34, 61, 84);
            pnlReceivedData.Controls.Add(lblReceivePayload);
            pnlReceivedData.Controls.Add(lblReceiveResult);
            pnlReceivedData.Controls.Add(txtReceivePayload);
            pnlReceivedData.ForeColor = Color.FromArgb(0, 135, 191);
            pnlReceivedData.Location = new Point(925, 232);
            pnlReceivedData.Name = "pnlReceivedData";
            pnlReceivedData.Padding = new Padding(16);
            pnlReceivedData.Size = new Size(596, 442);
            pnlReceivedData.TabIndex = 11;
            pnlReceivedData.Title = "RECEIVED DATA";
            // 
            // lblReceivePayload
            // 
            lblReceivePayload.AutoSize = true;
            lblReceivePayload.ForeColor = Color.Gainsboro;
            lblReceivePayload.Location = new Point(19, 31);
            lblReceivePayload.Name = "lblReceivePayload";
            lblReceivePayload.Size = new Size(150, 15);
            lblReceivePayload.TabIndex = 9;
            lblReceivePayload.Text = "Recovered Payload / Result";
            // 
            // lblReceiveResult
            // 
            lblReceiveResult.BackColor = Color.FromArgb(22, 153, 170, 187);
            lblReceiveResult.ForeColor = Color.Gainsboro;
            lblReceiveResult.Location = new Point(19, 378);
            lblReceiveResult.Name = "lblReceiveResult";
            lblReceiveResult.Size = new Size(558, 41);
            lblReceiveResult.TabIndex = 7;
            lblReceiveResult.Text = "WAITING FOR EXCHANGE";
            lblReceiveResult.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtReceivePayload
            // 
            txtReceivePayload.BackColor = Color.FromArgb(0, 15, 28);
            txtReceivePayload.BorderColor = Color.FromArgb(34, 61, 84);
            txtReceivePayload.CornerStyle = WinForms.Controls.Enums.CornerStyle.Squared;
            txtReceivePayload.Font = new Font("Segoe UI", 9F);
            txtReceivePayload.ForeColor = Color.FromArgb(225, 232, 238);
            txtReceivePayload.Location = new Point(19, 52);
            txtReceivePayload.MinimumSize = new Size(50, 32);
            txtReceivePayload.Multiline = true;
            txtReceivePayload.Name = "txtReceivePayload";
            txtReceivePayload.Padding = new Padding(10, 6, 10, 6);
            txtReceivePayload.ReadOnly = true;
            txtReceivePayload.Size = new Size(558, 316);
            txtReceivePayload.TabIndex = 5;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 12, 25);
            ClientSize = new Size(1920, 1080);
            Controls.Add(pnlReceivedData);
            Controls.Add(pnlDemoActions);
            Controls.Add(pnlEnvelopeViewer);
            Controls.Add(pnlEventLog);
            Controls.Add(pnlSendData);
            Controls.Add(pnlExchangeFlow);
            Controls.Add(pnlSecurityStatus);
            Controls.Add(pnlConfiguration);
            Controls.Add(pnlSystemStatus);
            Controls.Add(pnlHeaderLogo);
            Controls.Add(mtbTitleBar);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainWindow";
            Padding = new Padding(3);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainWindow";
            FormClosing += MainWindow_FormClosing;
            Load += MainWindow_Load;
            pnlHeaderLogo.ResumeLayout(false);
            pnlHeaderLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlSystemStatus.ResumeLayout(false);
            pnlConfiguration.ResumeLayout(false);
            pnlSecurityStatus.ResumeLayout(false);
            pnlTamperStatus.ResumeLayout(false);
            pnlTamperStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picTamperCheck).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTamperStatus).EndInit();
            pnlReplayStatus.ResumeLayout(false);
            pnlReplayStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picReplayCheck).EndInit();
            ((System.ComponentModel.ISupportInitialize)picReplayStatus).EndInit();
            pnlIntegrityStatus.ResumeLayout(false);
            pnlIntegrityStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picIntegrityCheck).EndInit();
            ((System.ComponentModel.ISupportInitialize)picIntegrityStatus).EndInit();
            pnlTransportStatus.ResumeLayout(false);
            pnlTransportStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picTransportCheck).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTransportStatus).EndInit();
            pnlProviderStatus.ResumeLayout(false);
            pnlProviderStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picProviderCheck).EndInit();
            ((System.ComponentModel.ISupportInitialize)picProviderStatus).EndInit();
            pnlExchangeFlow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow5).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow4).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow3).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow2).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblFlowArrow1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picValidateEnveleope).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFlowTransport).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFlowProtected).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFlowMercuryReceived).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFlowMercurySend).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFlowApplication).EndInit();
            pnlSendData.ResumeLayout(false);
            pnlSendData.PerformLayout();
            pnlEventLog.ResumeLayout(false);
            lblEventLog.ResumeLayout(false);
            pnlEnvelopeViewer.ResumeLayout(false);
            pnlEnvelopeViewer.PerformLayout();
            pnlHexPreview.ResumeLayout(false);
            pnlDemoActions.ResumeLayout(false);
            pnlReceivedData.ResumeLayout(false);
            pnlReceivedData.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Controls.MercuryTitleBar mtbTitleBar;
        private Controls.MercuryGlassPanel pnlHeaderLogo;
        private Controls.MercuryGlassPanel pnlSystemStatus;
        private Controls.MercuryGlassPanel pnlConfiguration;
        private Controls.MercuryGlassPanel pnlSecurityStatus;
        private Controls.MercuryGlassPanel pnlExchangeFlow;
        private Controls.MercuryGlassPanel pnlSendData;
        private Controls.MercuryGlassPanel pnlEventLog;
        private Controls.MercuryGlassPanel pnlEnvelopeViewer;
        private Controls.MercuryGlassPanel pnlDemoActions;
        private Label lblCryptoProvider;
        private Controls.MercuryGlassComboBox cboCryptoProvider;
        private Controls.MercuryGlassToggleButton tglChunking;
        private Controls.MercuryGlassComboBox cboLogging;
        private Controls.MercuryGlassComboBox cboChunkSize;
        private Controls.MercuryGlassComboBox cboEnvelopeCodec;
        private Controls.MercuryGlassComboBox cboTransport;
        private Controls.MercuryGlassButton btnApplyConfiguration;
        private Label lblTransport;
        private Label lblLogging;
        private Label lblChunkSize;
        private Label lblChunking;
        private Label lblEnvelopeCodec;
        private Controls.MercuryGlassPanel lblEventLog;
        private Controls.MercuryGlassButton btnClearLog;
        private RichTextBox rtbEventLog;
        private Controls.MercuryGlassPanel pnlReceivedData;
        private Controls.MercuryGlassPanel pnlHexPreview;
        private RichTextBox rtbHexPreview;
        private Label lblHexPreview;
        private Label lblRawPayload;
        private Label lblFrameSize;
        private Label lblProtectedSize;
        private Label lblReplayToken;
        private Label lblAlgorithm;
        private Label lblEnvelopeId;
        private Label lblEnvelopeVersion;
        private Label lblRawPayloadValue;
        private Label lblFrameSizeValue;
        private Label lblProtectedSizeValue;
        private Label lblReplayTokenValue;
        private Label lblAlgorithmValue;
        private Label lblEnvelopeIdValue;
        private Label lblEnvelopeVersionValue;
        private PictureBox picFlowApplication;
        private PictureBox picFlowMercuryReceived;
        private PictureBox picFlowMercurySend;
        private PictureBox picValidateEnveleope;
        private PictureBox picFlowTransport;
        private PictureBox picFlowProtected;
        private PictureBox lblFlowArrow5;
        private PictureBox lblFlowArrow4;
        private PictureBox lblFlowArrow3;
        private PictureBox lblFlowArrow2;
        private PictureBox lblFlowArrow1;
        private Label lblFlowApplication;
        private Label lblFlowMercuryReceive;
        private Label lblValidateEnveleope;
        private Label lblFlowTransport;
        private Label lblFlowProvider;
        private Label lblFlowMercurySend;
        private Controls.MercuryGlassPanel pnlTamperStatus;
        private Controls.MercuryGlassPanel pnlReplayStatus;
        private Controls.MercuryGlassPanel pnlIntegrityStatus;
        private Controls.MercuryGlassPanel pnlTransportStatus;
        private Controls.MercuryGlassPanel pnlProviderStatus;
        private PictureBox picReplayStatus;
        private PictureBox picIntegrityStatus;
        private PictureBox picTransportStatus;
        private PictureBox picProviderStatus;
        private PictureBox picTamperStatus;
        private Label lblProviderCaption;
        private Label lblTamperCaption;
        private PictureBox picTamperCheck;
        private Label lblReplayCaption;
        private PictureBox picReplayCheck;
        private Label lblIntegrityCaption;
        private PictureBox picIntegrityCheck;
        private Label lblTransportCaption;
        private PictureBox picTransportCheck;
        private PictureBox picProviderCheck;
        private Label lblTamperState;
        private Label lblReplayState;
        private Label lblIntegrityState;
        private Label lblTransportValue;
        private Label lblProviderValue;
        private Label lblLastCheck;
        private Label lblTransportState;
        private Label lblProviderReady;
        private Label lblReplayWindow;
        private Label lblSystemTime;
        private Label lblTimeCaption;
        private Label lblSystemState;
        private Controls.MercuryGlassButton btnSend;
        private Controls.MercuryGlassTextBox txtSendPayload;
        private Controls.MercuryGlassTextBox txtReceivePayload;
        private Label lblReceiveResult;
        private Label lblSendPayload;
        private Label lblReceivePayload;
        private Controls.MercuryGlassButton btnSendFile;
        private PictureBox pictureBox1;
        private Label lblTitle;
        private Label lblMotto;
        private Controls.MercuryGlassToggleButton tglWrongKeyAttack;
        private Controls.MercuryGlassToggleButton tglTamperAttack;
        private Controls.MercuryGlassToggleButton tglReplayAttack;
    }
}
