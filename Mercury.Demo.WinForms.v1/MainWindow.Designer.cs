using Mercury.Demo.WinForms.v1.Controls;

namespace Mercury.Demo.WinForms.v1
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
            pictureBox1 = new PictureBox();
            mgpConfiguration = new MercuryGlassPanel();
            chkChunkEnabled = new CheckBox();
            lblChunking = new Label();
            lblLogging = new Label();
            lblChunkSize = new Label();
            lblEnvelopeCodec = new Label();
            lblTransport = new Label();
            mgbApplyConfiguration = new MercuryGlassButton();
            mgcLogging = new MercuryGlassComboBox();
            mgcChunkSize = new MercuryGlassComboBox();
            mgcEnvelopeCodec = new MercuryGlassComboBox();
            mgcTransport = new MercuryGlassComboBox();
            mgcCryptoProvider = new MercuryGlassComboBox();
            lblCryptoProvider = new Label();
            mercuryGlassPanel1 = new MercuryGlassPanel();
            mgbSendPayload = new MercuryGlassButton();
            lblRecipientTag = new Label();
            lblSenderTag = new Label();
            lblPayloadSizeReceive = new Label();
            lblPayloadSizeSend = new Label();
            mgbClearRecipient = new MercuryGlassButton();
            mgbClearSender = new MercuryGlassButton();
            txtRecipient = new TextBox();
            txtSender = new TextBox();
            mmlSender = new MercuryMarqueeLabel();
            mmlRecipient = new MercuryMarqueeLabel();
            mercuryGlassPanel2 = new MercuryGlassPanel();
            mgbClearLog = new MercuryGlassButton();
            mgbTamperPayload = new MercuryGlassToggleButton();
            mgbTamperAuthTag = new MercuryGlassToggleButton();
            mgbTamperReplayToken = new MercuryGlassToggleButton();
            mgbReplayLastFrame = new MercuryGlassToggleButton();
            mercuryGlassPanel6 = new MercuryGlassPanel();
            textBox1 = new TextBox();
            lblVersion = new Label();
            mercuryGlassPanel7 = new MercuryGlassPanel();
            richTextBox1 = new RichTextBox();
            mercuryGlassPanel8 = new MercuryGlassPanel();
            mercuryGlassPanel9 = new MercuryGlassPanel();
            lblStatusConnection = new Label();
            mercuryGlassPanel10 = new MercuryGlassPanel();
            lblStatusFramesSent = new Label();
            mercuryGlassPanel11 = new MercuryGlassPanel();
            lblStatusFramesReceived = new Label();
            mercuryGlassPanel12 = new MercuryGlassPanel();
            lblStatusAuthFailures = new Label();
            mercuryGlassPanel13 = new MercuryGlassPanel();
            lblStatusReplayAttemps = new Label();
            mercuryGlassPanel14 = new MercuryGlassPanel();
            lblStatusChunkCount = new Label();
            mercuryGlassPanel15 = new MercuryGlassPanel();
            lblStatusAverageSize = new Label();
            mgcEnvelopeInspector = new MercuryGlassButton();
            mgbProtectedPayload = new MercuryGlassButton();
            mgbHeaderMetadata = new MercuryGlassButton();
            mgbFooterMetadata = new MercuryGlassButton();
            mpgEnvelopeInspectionWorkspace = new MercuryGlassPanel();
            lblMercury = new Label();
            lblSercureCommunicationsFramework = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            mgpConfiguration.SuspendLayout();
            mercuryGlassPanel1.SuspendLayout();
            mercuryGlassPanel2.SuspendLayout();
            mercuryGlassPanel6.SuspendLayout();
            mercuryGlassPanel7.SuspendLayout();
            mercuryGlassPanel9.SuspendLayout();
            mercuryGlassPanel10.SuspendLayout();
            mercuryGlassPanel11.SuspendLayout();
            mercuryGlassPanel12.SuspendLayout();
            mercuryGlassPanel13.SuspendLayout();
            mercuryGlassPanel14.SuspendLayout();
            mercuryGlassPanel15.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.MercuryV3;
            pictureBox1.Location = new Point(23, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(75, 75);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // mgpConfiguration
            // 
            mgpConfiguration.AlignTitle = ContentAlignment.TopLeft;
            mgpConfiguration.BackColor = Color.FromArgb(26, 30, 33);
            mgpConfiguration.BorderColor = Color.FromArgb(46, 50, 54);
            mgpConfiguration.Controls.Add(chkChunkEnabled);
            mgpConfiguration.Controls.Add(lblChunking);
            mgpConfiguration.Controls.Add(lblLogging);
            mgpConfiguration.Controls.Add(lblChunkSize);
            mgpConfiguration.Controls.Add(lblEnvelopeCodec);
            mgpConfiguration.Controls.Add(lblTransport);
            mgpConfiguration.Controls.Add(mgbApplyConfiguration);
            mgpConfiguration.Controls.Add(mgcLogging);
            mgpConfiguration.Controls.Add(mgcChunkSize);
            mgpConfiguration.Controls.Add(mgcEnvelopeCodec);
            mgpConfiguration.Controls.Add(mgcTransport);
            mgpConfiguration.Controls.Add(mgcCryptoProvider);
            mgpConfiguration.Controls.Add(lblCryptoProvider);
            mgpConfiguration.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mgpConfiguration.ForeColor = Color.FromArgb(225, 232, 238);
            mgpConfiguration.Location = new Point(12, 89);
            mgpConfiguration.Name = "mgpConfiguration";
            mgpConfiguration.Padding = new Padding(16);
            mgpConfiguration.Size = new Size(292, 293);
            mgpConfiguration.TabIndex = 1;
            mgpConfiguration.Title = "CONFIGURATION";
            // 
            // chkChunkEnabled
            // 
            chkChunkEnabled.AutoSize = true;
            chkChunkEnabled.Checked = true;
            chkChunkEnabled.CheckState = CheckState.Checked;
            chkChunkEnabled.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkChunkEnabled.Location = new Point(126, 147);
            chkChunkEnabled.Name = "chkChunkEnabled";
            chkChunkEnabled.Size = new Size(68, 19);
            chkChunkEnabled.TabIndex = 13;
            chkChunkEnabled.Text = "Enabled";
            chkChunkEnabled.UseVisualStyleBackColor = true;
            // 
            // lblChunking
            // 
            lblChunking.AutoSize = true;
            lblChunking.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblChunking.Location = new Point(10, 148);
            lblChunking.Name = "lblChunking";
            lblChunking.Size = new Size(59, 15);
            lblChunking.TabIndex = 12;
            lblChunking.Text = "Chunking";
            // 
            // lblLogging
            // 
            lblLogging.AutoSize = true;
            lblLogging.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLogging.Location = new Point(10, 216);
            lblLogging.Name = "lblLogging";
            lblLogging.Size = new Size(51, 15);
            lblLogging.TabIndex = 11;
            lblLogging.Text = "Logging";
            // 
            // lblChunkSize
            // 
            lblChunkSize.AutoSize = true;
            lblChunkSize.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblChunkSize.Location = new Point(10, 181);
            lblChunkSize.Name = "lblChunkSize";
            lblChunkSize.Size = new Size(65, 15);
            lblChunkSize.TabIndex = 10;
            lblChunkSize.Text = "Chunk Size";
            // 
            // lblEnvelopeCodec
            // 
            lblEnvelopeCodec.AutoSize = true;
            lblEnvelopeCodec.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEnvelopeCodec.Location = new Point(10, 114);
            lblEnvelopeCodec.Name = "lblEnvelopeCodec";
            lblEnvelopeCodec.Size = new Size(92, 15);
            lblEnvelopeCodec.TabIndex = 9;
            lblEnvelopeCodec.Text = "Envelope Codec";
            // 
            // lblTransport
            // 
            lblTransport.AutoSize = true;
            lblTransport.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTransport.Location = new Point(10, 78);
            lblTransport.Name = "lblTransport";
            lblTransport.Size = new Size(57, 15);
            lblTransport.TabIndex = 8;
            lblTransport.Text = "Transport";
            // 
            // mgbApplyConfiguration
            // 
            mgbApplyConfiguration.BackColor = Color.Transparent;
            mgbApplyConfiguration.Font = new Font("Segoe UI", 9F);
            mgbApplyConfiguration.ForeColor = Color.FromArgb(55, 145, 235);
            mgbApplyConfiguration.ImageAlign = ContentAlignment.MiddleLeft;
            mgbApplyConfiguration.Location = new Point(10, 245);
            mgbApplyConfiguration.Name = "mgbApplyConfiguration";
            mgbApplyConfiguration.Padding = new Padding(10, 0, 10, 0);
            mgbApplyConfiguration.Size = new Size(267, 36);
            mgbApplyConfiguration.TabIndex = 7;
            mgbApplyConfiguration.Text = "Apply Configuration";
            mgbApplyConfiguration.UseVisualStyleBackColor = false;
            // 
            // mgcLogging
            // 
            mgcLogging.BackColor = Color.FromArgb(42, 42, 46);
            mgcLogging.Font = new Font("Segoe UI", 9F);
            mgcLogging.ForeColor = Color.FromArgb(225, 232, 238);
            mgcLogging.Location = new Point(126, 209);
            mgcLogging.Name = "mgcLogging";
            mgcLogging.Padding = new Padding(12, 0, 38, 0);
            mgcLogging.Size = new Size(151, 30);
            mgcLogging.TabIndex = 6;
            // 
            // mgcChunkSize
            // 
            mgcChunkSize.BackColor = Color.FromArgb(42, 42, 46);
            mgcChunkSize.Font = new Font("Segoe UI", 9F);
            mgcChunkSize.ForeColor = Color.FromArgb(225, 232, 238);
            mgcChunkSize.Location = new Point(126, 173);
            mgcChunkSize.Name = "mgcChunkSize";
            mgcChunkSize.Padding = new Padding(12, 0, 38, 0);
            mgcChunkSize.Size = new Size(151, 30);
            mgcChunkSize.TabIndex = 5;
            // 
            // mgcEnvelopeCodec
            // 
            mgcEnvelopeCodec.BackColor = Color.FromArgb(42, 42, 46);
            mgcEnvelopeCodec.Font = new Font("Segoe UI", 9F);
            mgcEnvelopeCodec.ForeColor = Color.FromArgb(225, 232, 238);
            mgcEnvelopeCodec.Location = new Point(126, 107);
            mgcEnvelopeCodec.Name = "mgcEnvelopeCodec";
            mgcEnvelopeCodec.Padding = new Padding(12, 0, 38, 0);
            mgcEnvelopeCodec.Size = new Size(151, 30);
            mgcEnvelopeCodec.TabIndex = 4;
            // 
            // mgcTransport
            // 
            mgcTransport.BackColor = Color.FromArgb(42, 42, 46);
            mgcTransport.Font = new Font("Segoe UI", 9F);
            mgcTransport.ForeColor = Color.FromArgb(225, 232, 238);
            mgcTransport.Location = new Point(126, 71);
            mgcTransport.Name = "mgcTransport";
            mgcTransport.Padding = new Padding(12, 0, 38, 0);
            mgcTransport.Size = new Size(151, 30);
            mgcTransport.TabIndex = 3;
            // 
            // mgcCryptoProvider
            // 
            mgcCryptoProvider.BackColor = Color.FromArgb(42, 42, 46);
            mgcCryptoProvider.Font = new Font("Segoe UI", 9F);
            mgcCryptoProvider.ForeColor = Color.FromArgb(225, 232, 238);
            mgcCryptoProvider.Location = new Point(126, 35);
            mgcCryptoProvider.Name = "mgcCryptoProvider";
            mgcCryptoProvider.Padding = new Padding(12, 0, 38, 0);
            mgcCryptoProvider.Size = new Size(151, 30);
            mgcCryptoProvider.TabIndex = 2;
            // 
            // lblCryptoProvider
            // 
            lblCryptoProvider.AutoSize = true;
            lblCryptoProvider.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCryptoProvider.Location = new Point(10, 41);
            lblCryptoProvider.Name = "lblCryptoProvider";
            lblCryptoProvider.Size = new Size(90, 15);
            lblCryptoProvider.TabIndex = 0;
            lblCryptoProvider.Text = "Crypto Provider";
            // 
            // mercuryGlassPanel1
            // 
            mercuryGlassPanel1.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel1.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel1.Controls.Add(mgbSendPayload);
            mercuryGlassPanel1.Controls.Add(lblRecipientTag);
            mercuryGlassPanel1.Controls.Add(lblSenderTag);
            mercuryGlassPanel1.Controls.Add(lblPayloadSizeReceive);
            mercuryGlassPanel1.Controls.Add(lblPayloadSizeSend);
            mercuryGlassPanel1.Controls.Add(mgbClearRecipient);
            mercuryGlassPanel1.Controls.Add(mgbClearSender);
            mercuryGlassPanel1.Controls.Add(txtRecipient);
            mercuryGlassPanel1.Controls.Add(txtSender);
            mercuryGlassPanel1.Controls.Add(mmlSender);
            mercuryGlassPanel1.Controls.Add(mmlRecipient);
            mercuryGlassPanel1.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold);
            mercuryGlassPanel1.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel1.Location = new Point(310, 89);
            mercuryGlassPanel1.Name = "mercuryGlassPanel1";
            mercuryGlassPanel1.Padding = new Padding(16);
            mercuryGlassPanel1.Size = new Size(1020, 293);
            mercuryGlassPanel1.TabIndex = 2;
            mercuryGlassPanel1.Title = "";
            // 
            // mgbSendPayload
            // 
            mgbSendPayload.BackColor = Color.Transparent;
            mgbSendPayload.ButtonColor = Color.FromArgb(0, 192, 0);
            mgbSendPayload.Font = new Font("Segoe UI", 9F);
            mgbSendPayload.ForeColor = Color.Lime;
            mgbSendPayload.ImageAlign = ContentAlignment.MiddleLeft;
            mgbSendPayload.Location = new Point(464, 137);
            mgbSendPayload.Name = "mgbSendPayload";
            mgbSendPayload.Padding = new Padding(10, 0, 10, 0);
            mgbSendPayload.Size = new Size(101, 36);
            mgbSendPayload.TabIndex = 10;
            mgbSendPayload.Text = "SEND";
            mgbSendPayload.UseVisualStyleBackColor = false;
            // 
            // lblRecipientTag
            // 
            lblRecipientTag.AutoSize = true;
            lblRecipientTag.ForeColor = Color.FromArgb(0, 192, 0);
            lblRecipientTag.Location = new Point(595, 16);
            lblRecipientTag.Name = "lblRecipientTag";
            lblRecipientTag.Size = new Size(258, 18);
            lblRecipientTag.TabIndex = 9;
            lblRecipientTag.Text = "BRAVO ENDPOINT (RECIPIENT)";
            // 
            // lblSenderTag
            // 
            lblSenderTag.AutoSize = true;
            lblSenderTag.ForeColor = Color.FromArgb(55, 145, 235);
            lblSenderTag.Location = new Point(24, 16);
            lblSenderTag.Name = "lblSenderTag";
            lblSenderTag.Size = new Size(236, 18);
            lblSenderTag.TabIndex = 8;
            lblSenderTag.Text = "ALPHA ENDPOINT (SENDER)";
            // 
            // lblPayloadSizeReceive
            // 
            lblPayloadSizeReceive.AutoSize = true;
            lblPayloadSizeReceive.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPayloadSizeReceive.ForeColor = Color.FromArgb(0, 192, 0);
            lblPayloadSizeReceive.Location = new Point(595, 49);
            lblPayloadSizeReceive.Name = "lblPayloadSizeReceive";
            lblPayloadSizeReceive.Size = new Size(136, 16);
            lblPayloadSizeReceive.TabIndex = 5;
            lblPayloadSizeReceive.Text = "Payload Size: 0 bytes";
            // 
            // lblPayloadSizeSend
            // 
            lblPayloadSizeSend.AutoSize = true;
            lblPayloadSizeSend.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPayloadSizeSend.ForeColor = Color.FromArgb(55, 145, 235);
            lblPayloadSizeSend.Location = new Point(24, 49);
            lblPayloadSizeSend.Name = "lblPayloadSizeSend";
            lblPayloadSizeSend.Size = new Size(136, 16);
            lblPayloadSizeSend.TabIndex = 4;
            lblPayloadSizeSend.Text = "Payload Size: 0 bytes";
            // 
            // mgbClearRecipient
            // 
            mgbClearRecipient.BackColor = Color.Transparent;
            mgbClearRecipient.ButtonColor = Color.Gray;
            mgbClearRecipient.Font = new Font("Segoe UI", 9F);
            mgbClearRecipient.ForeColor = Color.FromArgb(225, 232, 238);
            mgbClearRecipient.ImageAlign = ContentAlignment.MiddleLeft;
            mgbClearRecipient.Location = new Point(903, 245);
            mgbClearRecipient.Name = "mgbClearRecipient";
            mgbClearRecipient.Padding = new Padding(10, 0, 10, 0);
            mgbClearRecipient.Size = new Size(98, 36);
            mgbClearRecipient.TabIndex = 3;
            mgbClearRecipient.Text = "Clear";
            mgbClearRecipient.UseVisualStyleBackColor = false;
            // 
            // mgbClearSender
            // 
            mgbClearSender.BackColor = Color.Transparent;
            mgbClearSender.ButtonColor = Color.Gray;
            mgbClearSender.Font = new Font("Segoe UI", 9F);
            mgbClearSender.ForeColor = Color.FromArgb(225, 232, 238);
            mgbClearSender.ImageAlign = ContentAlignment.MiddleLeft;
            mgbClearSender.Location = new Point(332, 245);
            mgbClearSender.Name = "mgbClearSender";
            mgbClearSender.Padding = new Padding(10, 0, 10, 0);
            mgbClearSender.Size = new Size(98, 36);
            mgbClearSender.TabIndex = 2;
            mgbClearSender.Text = "Clear";
            mgbClearSender.UseVisualStyleBackColor = false;
            // 
            // txtRecipient
            // 
            txtRecipient.BackColor = Color.FromArgb(26, 30, 33);
            txtRecipient.BorderStyle = BorderStyle.FixedSingle;
            txtRecipient.ForeColor = Color.Gainsboro;
            txtRecipient.Location = new Point(595, 71);
            txtRecipient.Multiline = true;
            txtRecipient.Name = "txtRecipient";
            txtRecipient.Size = new Size(406, 160);
            txtRecipient.TabIndex = 1;
            // 
            // txtSender
            // 
            txtSender.BackColor = Color.FromArgb(26, 30, 33);
            txtSender.BorderStyle = BorderStyle.FixedSingle;
            txtSender.ForeColor = Color.Gainsboro;
            txtSender.Location = new Point(24, 71);
            txtSender.Multiline = true;
            txtSender.Name = "txtSender";
            txtSender.Size = new Size(406, 160);
            txtSender.TabIndex = 0;
            // 
            // mmlSender
            // 
            mmlSender.AutoSize = true;
            mmlSender.Direction = MercuryMarqueeLabel.MarqueeDirection.Right;
            mmlSender.Font = new Font("Consolas", 8.25F, FontStyle.Bold);
            mmlSender.ForeColor = Color.FromArgb(55, 145, 235);
            mmlSender.Location = new Point(317, 150);
            mmlSender.MarqueeEnabled = false;
            mmlSender.MarqueeInterval = 90;
            mmlSender.Name = "mmlSender";
            mmlSender.Size = new Size(193, 13);
            mmlSender.TabIndex = 0;
            mmlSender.Text = "0010011 01110101 01110010 01111";
            mmlSender.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mmlRecipient
            // 
            mmlRecipient.AutoSize = true;
            mmlRecipient.Direction = MercuryMarqueeLabel.MarqueeDirection.Right;
            mmlRecipient.Font = new Font("Consolas", 8.25F, FontStyle.Bold);
            mmlRecipient.ForeColor = Color.FromArgb(0, 192, 0);
            mmlRecipient.Location = new Point(516, 150);
            mmlRecipient.MarqueeEnabled = false;
            mmlRecipient.MarqueeInterval = 90;
            mmlRecipient.Name = "mmlRecipient";
            mmlRecipient.Size = new Size(193, 13);
            mmlRecipient.TabIndex = 11;
            mmlRecipient.Text = "0010011 01110101 01110010 01111";
            mmlRecipient.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mercuryGlassPanel2
            // 
            mercuryGlassPanel2.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel2.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel2.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel2.Controls.Add(mgbClearLog);
            mercuryGlassPanel2.Controls.Add(mgbTamperPayload);
            mercuryGlassPanel2.Controls.Add(mgbTamperAuthTag);
            mercuryGlassPanel2.Controls.Add(mgbTamperReplayToken);
            mercuryGlassPanel2.Controls.Add(mgbReplayLastFrame);
            mercuryGlassPanel2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel2.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel2.Location = new Point(1336, 89);
            mercuryGlassPanel2.Name = "mercuryGlassPanel2";
            mercuryGlassPanel2.Padding = new Padding(16);
            mercuryGlassPanel2.Size = new Size(292, 293);
            mercuryGlassPanel2.TabIndex = 3;
            mercuryGlassPanel2.Title = "TEST PANEL";
            // 
            // mgbClearLog
            // 
            mgbClearLog.BackColor = Color.Transparent;
            mgbClearLog.ButtonColor = Color.FromArgb(192, 192, 0);
            mgbClearLog.Font = new Font("Segoe UI", 9F);
            mgbClearLog.ForeColor = Color.FromArgb(225, 232, 238);
            mgbClearLog.ImageAlign = ContentAlignment.MiddleLeft;
            mgbClearLog.Location = new Point(19, 245);
            mgbClearLog.Name = "mgbClearLog";
            mgbClearLog.Padding = new Padding(10, 0, 10, 0);
            mgbClearLog.Size = new Size(254, 36);
            mgbClearLog.TabIndex = 9;
            mgbClearLog.Text = "Clear Log";
            mgbClearLog.UseVisualStyleBackColor = false;
            // 
            // mgbTamperPayload
            // 
            mgbTamperPayload.BackColor = Color.Transparent;
            mgbTamperPayload.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbTamperPayload.Font = new Font("Segoe UI", 9F);
            mgbTamperPayload.ForeColor = Color.FromArgb(225, 232, 238);
            mgbTamperPayload.ImageAlign = ContentAlignment.MiddleLeft;
            mgbTamperPayload.Location = new Point(19, 165);
            mgbTamperPayload.Name = "mgbTamperPayload";
            mgbTamperPayload.Padding = new Padding(10, 0, 10, 0);
            mgbTamperPayload.Size = new Size(254, 36);
            mgbTamperPayload.TabIndex = 6;
            mgbTamperPayload.Text = "Tamper Payload";
            mgbTamperPayload.UseVisualStyleBackColor = false;
            // 
            // mgbTamperAuthTag
            // 
            mgbTamperAuthTag.BackColor = Color.Transparent;
            mgbTamperAuthTag.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbTamperAuthTag.Font = new Font("Segoe UI", 9F);
            mgbTamperAuthTag.ForeColor = Color.FromArgb(225, 232, 238);
            mgbTamperAuthTag.ImageAlign = ContentAlignment.MiddleLeft;
            mgbTamperAuthTag.Location = new Point(19, 123);
            mgbTamperAuthTag.Name = "mgbTamperAuthTag";
            mgbTamperAuthTag.Padding = new Padding(10, 0, 10, 0);
            mgbTamperAuthTag.Size = new Size(254, 36);
            mgbTamperAuthTag.TabIndex = 5;
            mgbTamperAuthTag.Text = "Tamper Auth Tag";
            mgbTamperAuthTag.UseVisualStyleBackColor = false;
            // 
            // mgbTamperReplayToken
            // 
            mgbTamperReplayToken.BackColor = Color.Transparent;
            mgbTamperReplayToken.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbTamperReplayToken.Font = new Font("Segoe UI", 9F);
            mgbTamperReplayToken.ForeColor = Color.FromArgb(225, 232, 238);
            mgbTamperReplayToken.ImageAlign = ContentAlignment.MiddleLeft;
            mgbTamperReplayToken.Location = new Point(19, 81);
            mgbTamperReplayToken.Name = "mgbTamperReplayToken";
            mgbTamperReplayToken.Padding = new Padding(10, 0, 10, 0);
            mgbTamperReplayToken.Size = new Size(254, 36);
            mgbTamperReplayToken.TabIndex = 4;
            mgbTamperReplayToken.Text = "Tamper Replay Token";
            mgbTamperReplayToken.UseVisualStyleBackColor = false;
            // 
            // mgbReplayLastFrame
            // 
            mgbReplayLastFrame.BackColor = Color.Transparent;
            mgbReplayLastFrame.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbReplayLastFrame.Font = new Font("Segoe UI", 9F);
            mgbReplayLastFrame.ForeColor = Color.FromArgb(225, 232, 238);
            mgbReplayLastFrame.ImageAlign = ContentAlignment.MiddleLeft;
            mgbReplayLastFrame.Location = new Point(19, 39);
            mgbReplayLastFrame.Name = "mgbReplayLastFrame";
            mgbReplayLastFrame.Padding = new Padding(10, 0, 10, 0);
            mgbReplayLastFrame.Size = new Size(254, 36);
            mgbReplayLastFrame.TabIndex = 3;
            mgbReplayLastFrame.Text = "Replay Last Frame";
            mgbReplayLastFrame.UseVisualStyleBackColor = false;
            // 
            // mercuryGlassPanel6
            // 
            mercuryGlassPanel6.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel6.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel6.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel6.Controls.Add(textBox1);
            mercuryGlassPanel6.Controls.Add(lblVersion);
            mercuryGlassPanel6.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel6.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel6.Location = new Point(12, 569);
            mercuryGlassPanel6.Name = "mercuryGlassPanel6";
            mercuryGlassPanel6.Padding = new Padding(16);
            mercuryGlassPanel6.Size = new Size(292, 182);
            mercuryGlassPanel6.TabIndex = 8;
            mercuryGlassPanel6.Title = "ABOUT MERCURY";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(26, 30, 33);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.Gainsboro;
            textBox1.Location = new Point(10, 36);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(267, 100);
            textBox1.TabIndex = 1;
            textBox1.Text = "Mercury is a secure communications framework that ensures no unprotected data is ever sent over any transport. \r\n\r\nBuilt for flexibility. Designed for trust.\r\n\r\nArchitected for the mission.";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblVersion.ForeColor = Color.FromArgb(55, 145, 235);
            lblVersion.Location = new Point(10, 151);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(75, 15);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "Version: 0.7.0";
            // 
            // mercuryGlassPanel7
            // 
            mercuryGlassPanel7.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel7.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel7.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel7.Controls.Add(richTextBox1);
            mercuryGlassPanel7.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel7.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel7.Location = new Point(12, 757);
            mercuryGlassPanel7.Name = "mercuryGlassPanel7";
            mercuryGlassPanel7.Padding = new Padding(16);
            mercuryGlassPanel7.Size = new Size(1059, 256);
            mercuryGlassPanel7.TabIndex = 9;
            mercuryGlassPanel7.Title = "MERCURY ACTIVITY";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(20, 24, 26);
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Location = new Point(10, 32);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(713, 209);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // mercuryGlassPanel8
            // 
            mercuryGlassPanel8.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel8.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel8.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel8.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel8.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel8.Location = new Point(1077, 757);
            mercuryGlassPanel8.Name = "mercuryGlassPanel8";
            mercuryGlassPanel8.Padding = new Padding(16);
            mercuryGlassPanel8.Size = new Size(551, 256);
            mercuryGlassPanel8.TabIndex = 10;
            mercuryGlassPanel8.Title = "COMMUNICATION TIMELINE";
            // 
            // mercuryGlassPanel9
            // 
            mercuryGlassPanel9.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel9.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel9.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel9.Controls.Add(lblStatusConnection);
            mercuryGlassPanel9.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel9.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel9.Location = new Point(405, 9);
            mercuryGlassPanel9.Name = "mercuryGlassPanel9";
            mercuryGlassPanel9.Padding = new Padding(16);
            mercuryGlassPanel9.Size = new Size(162, 71);
            mercuryGlassPanel9.TabIndex = 11;
            mercuryGlassPanel9.Title = "STATUS";
            // 
            // lblStatusConnection
            // 
            lblStatusConnection.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold);
            lblStatusConnection.ForeColor = Color.FromArgb(192, 192, 0);
            lblStatusConnection.Location = new Point(19, 32);
            lblStatusConnection.Name = "lblStatusConnection";
            lblStatusConnection.Size = new Size(124, 35);
            lblStatusConnection.TabIndex = 0;
            lblStatusConnection.Text = "Offline";
            lblStatusConnection.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mercuryGlassPanel10
            // 
            mercuryGlassPanel10.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel10.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel10.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel10.Controls.Add(lblStatusFramesSent);
            mercuryGlassPanel10.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel10.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel10.Location = new Point(573, 9);
            mercuryGlassPanel10.Name = "mercuryGlassPanel10";
            mercuryGlassPanel10.Padding = new Padding(16);
            mercuryGlassPanel10.Size = new Size(162, 71);
            mercuryGlassPanel10.TabIndex = 12;
            mercuryGlassPanel10.Title = "FRAMES SENT";
            // 
            // lblStatusFramesSent
            // 
            lblStatusFramesSent.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblStatusFramesSent.ForeColor = Color.FromArgb(55, 145, 235);
            lblStatusFramesSent.Location = new Point(19, 32);
            lblStatusFramesSent.Name = "lblStatusFramesSent";
            lblStatusFramesSent.Size = new Size(124, 35);
            lblStatusFramesSent.TabIndex = 1;
            lblStatusFramesSent.Text = "0";
            lblStatusFramesSent.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mercuryGlassPanel11
            // 
            mercuryGlassPanel11.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel11.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel11.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel11.Controls.Add(lblStatusFramesReceived);
            mercuryGlassPanel11.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel11.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel11.Location = new Point(741, 9);
            mercuryGlassPanel11.Name = "mercuryGlassPanel11";
            mercuryGlassPanel11.Padding = new Padding(16);
            mercuryGlassPanel11.Size = new Size(162, 71);
            mercuryGlassPanel11.TabIndex = 13;
            mercuryGlassPanel11.Title = "FRAMES RECEIVED";
            // 
            // lblStatusFramesReceived
            // 
            lblStatusFramesReceived.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblStatusFramesReceived.ForeColor = Color.FromArgb(0, 192, 0);
            lblStatusFramesReceived.Location = new Point(19, 32);
            lblStatusFramesReceived.Name = "lblStatusFramesReceived";
            lblStatusFramesReceived.Size = new Size(124, 35);
            lblStatusFramesReceived.TabIndex = 1;
            lblStatusFramesReceived.Text = "0";
            lblStatusFramesReceived.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mercuryGlassPanel12
            // 
            mercuryGlassPanel12.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel12.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel12.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel12.Controls.Add(lblStatusAuthFailures);
            mercuryGlassPanel12.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel12.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel12.Location = new Point(909, 9);
            mercuryGlassPanel12.Name = "mercuryGlassPanel12";
            mercuryGlassPanel12.Padding = new Padding(16);
            mercuryGlassPanel12.Size = new Size(162, 71);
            mercuryGlassPanel12.TabIndex = 14;
            mercuryGlassPanel12.Title = "AUTH FAILURES";
            // 
            // lblStatusAuthFailures
            // 
            lblStatusAuthFailures.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblStatusAuthFailures.ForeColor = Color.FromArgb(192, 0, 0);
            lblStatusAuthFailures.Location = new Point(19, 32);
            lblStatusAuthFailures.Name = "lblStatusAuthFailures";
            lblStatusAuthFailures.Size = new Size(124, 35);
            lblStatusAuthFailures.TabIndex = 1;
            lblStatusAuthFailures.Text = "0";
            lblStatusAuthFailures.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mercuryGlassPanel13
            // 
            mercuryGlassPanel13.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel13.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel13.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel13.Controls.Add(lblStatusReplayAttemps);
            mercuryGlassPanel13.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel13.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel13.Location = new Point(1077, 9);
            mercuryGlassPanel13.Name = "mercuryGlassPanel13";
            mercuryGlassPanel13.Padding = new Padding(16);
            mercuryGlassPanel13.Size = new Size(162, 71);
            mercuryGlassPanel13.TabIndex = 12;
            mercuryGlassPanel13.Title = "REPLAY ATTEMPS";
            // 
            // lblStatusReplayAttemps
            // 
            lblStatusReplayAttemps.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblStatusReplayAttemps.ForeColor = Color.FromArgb(192, 192, 0);
            lblStatusReplayAttemps.Location = new Point(19, 32);
            lblStatusReplayAttemps.Name = "lblStatusReplayAttemps";
            lblStatusReplayAttemps.Size = new Size(124, 35);
            lblStatusReplayAttemps.TabIndex = 1;
            lblStatusReplayAttemps.Text = "0";
            lblStatusReplayAttemps.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mercuryGlassPanel14
            // 
            mercuryGlassPanel14.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel14.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel14.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel14.Controls.Add(lblStatusChunkCount);
            mercuryGlassPanel14.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel14.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel14.Location = new Point(1248, 9);
            mercuryGlassPanel14.Name = "mercuryGlassPanel14";
            mercuryGlassPanel14.Padding = new Padding(16);
            mercuryGlassPanel14.Size = new Size(162, 71);
            mercuryGlassPanel14.TabIndex = 12;
            mercuryGlassPanel14.Title = "CHUNK COUNT";
            // 
            // lblStatusChunkCount
            // 
            lblStatusChunkCount.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblStatusChunkCount.ForeColor = Color.FromArgb(55, 145, 235);
            lblStatusChunkCount.Location = new Point(19, 32);
            lblStatusChunkCount.Name = "lblStatusChunkCount";
            lblStatusChunkCount.Size = new Size(124, 35);
            lblStatusChunkCount.TabIndex = 1;
            lblStatusChunkCount.Text = "0";
            lblStatusChunkCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mercuryGlassPanel15
            // 
            mercuryGlassPanel15.AlignTitle = ContentAlignment.TopCenter;
            mercuryGlassPanel15.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel15.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel15.Controls.Add(lblStatusAverageSize);
            mercuryGlassPanel15.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel15.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel15.Location = new Point(1416, 9);
            mercuryGlassPanel15.Name = "mercuryGlassPanel15";
            mercuryGlassPanel15.Padding = new Padding(16);
            mercuryGlassPanel15.Size = new Size(212, 71);
            mercuryGlassPanel15.TabIndex = 15;
            mercuryGlassPanel15.Title = "AVG PAYLOAD SIZE";
            // 
            // lblStatusAverageSize
            // 
            lblStatusAverageSize.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblStatusAverageSize.ForeColor = Color.FromArgb(55, 145, 235);
            lblStatusAverageSize.Location = new Point(11, 32);
            lblStatusAverageSize.Name = "lblStatusAverageSize";
            lblStatusAverageSize.Size = new Size(187, 35);
            lblStatusAverageSize.TabIndex = 1;
            lblStatusAverageSize.Text = "0 B";
            lblStatusAverageSize.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mgcEnvelopeInspector
            // 
            mgcEnvelopeInspector.BackColor = Color.Transparent;
            mgcEnvelopeInspector.ButtonColor = Color.FromArgb(46, 50, 54);
            mgcEnvelopeInspector.FillAlpha = 75;
            mgcEnvelopeInspector.Font = new Font("Segoe UI", 9F);
            mgcEnvelopeInspector.ForeColor = Color.FromArgb(225, 232, 238);
            mgcEnvelopeInspector.ImageAlign = ContentAlignment.MiddleLeft;
            mgcEnvelopeInspector.Location = new Point(310, 388);
            mgcEnvelopeInspector.Name = "mgcEnvelopeInspector";
            mgcEnvelopeInspector.Padding = new Padding(10, 0, 10, 0);
            mgcEnvelopeInspector.Size = new Size(206, 36);
            mgcEnvelopeInspector.TabIndex = 16;
            mgcEnvelopeInspector.Text = "ENVELOPE INSPECTOR";
            mgcEnvelopeInspector.UseVisualStyleBackColor = false;
            // 
            // mgbProtectedPayload
            // 
            mgbProtectedPayload.BackColor = Color.Transparent;
            mgbProtectedPayload.ButtonColor = Color.FromArgb(46, 50, 54);
            mgbProtectedPayload.FillAlpha = 75;
            mgbProtectedPayload.Font = new Font("Segoe UI", 9F);
            mgbProtectedPayload.ForeColor = Color.FromArgb(225, 232, 238);
            mgbProtectedPayload.ImageAlign = ContentAlignment.MiddleLeft;
            mgbProtectedPayload.Location = new Point(522, 388);
            mgbProtectedPayload.Name = "mgbProtectedPayload";
            mgbProtectedPayload.Padding = new Padding(10, 0, 10, 0);
            mgbProtectedPayload.Size = new Size(206, 36);
            mgbProtectedPayload.TabIndex = 17;
            mgbProtectedPayload.Text = "PROTECTED PAYLOAD";
            mgbProtectedPayload.UseVisualStyleBackColor = false;
            // 
            // mgbHeaderMetadata
            // 
            mgbHeaderMetadata.BackColor = Color.Transparent;
            mgbHeaderMetadata.ButtonColor = Color.FromArgb(46, 50, 54);
            mgbHeaderMetadata.FillAlpha = 75;
            mgbHeaderMetadata.Font = new Font("Segoe UI", 9F);
            mgbHeaderMetadata.ForeColor = Color.FromArgb(225, 232, 238);
            mgbHeaderMetadata.ImageAlign = ContentAlignment.MiddleLeft;
            mgbHeaderMetadata.Location = new Point(734, 388);
            mgbHeaderMetadata.Name = "mgbHeaderMetadata";
            mgbHeaderMetadata.Padding = new Padding(10, 0, 10, 0);
            mgbHeaderMetadata.Size = new Size(206, 36);
            mgbHeaderMetadata.TabIndex = 18;
            mgbHeaderMetadata.Text = "HEADER METADATA";
            mgbHeaderMetadata.UseVisualStyleBackColor = false;
            // 
            // mgbFooterMetadata
            // 
            mgbFooterMetadata.BackColor = Color.Transparent;
            mgbFooterMetadata.ButtonColor = Color.FromArgb(46, 50, 54);
            mgbFooterMetadata.FillAlpha = 75;
            mgbFooterMetadata.Font = new Font("Segoe UI", 9F);
            mgbFooterMetadata.ForeColor = Color.FromArgb(225, 232, 238);
            mgbFooterMetadata.ImageAlign = ContentAlignment.MiddleLeft;
            mgbFooterMetadata.Location = new Point(946, 388);
            mgbFooterMetadata.Name = "mgbFooterMetadata";
            mgbFooterMetadata.Padding = new Padding(10, 0, 10, 0);
            mgbFooterMetadata.Size = new Size(206, 36);
            mgbFooterMetadata.TabIndex = 19;
            mgbFooterMetadata.Text = "FOOTER METADATA";
            mgbFooterMetadata.UseVisualStyleBackColor = false;
            // 
            // mpgEnvelopeInspectionWorkspace
            // 
            mpgEnvelopeInspectionWorkspace.BackColor = Color.FromArgb(26, 30, 33);
            mpgEnvelopeInspectionWorkspace.BorderColor = Color.FromArgb(46, 50, 54);
            mpgEnvelopeInspectionWorkspace.ForeColor = Color.FromArgb(225, 232, 238);
            mpgEnvelopeInspectionWorkspace.Location = new Point(310, 428);
            mpgEnvelopeInspectionWorkspace.Name = "mpgEnvelopeInspectionWorkspace";
            mpgEnvelopeInspectionWorkspace.Padding = new Padding(16);
            mpgEnvelopeInspectionWorkspace.Size = new Size(1020, 323);
            mpgEnvelopeInspectionWorkspace.TabIndex = 20;
            mpgEnvelopeInspectionWorkspace.Title = "";
            // 
            // lblMercury
            // 
            lblMercury.AutoSize = true;
            lblMercury.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMercury.ForeColor = Color.FromArgb(55, 145, 235);
            lblMercury.Location = new Point(95, 0);
            lblMercury.Name = "lblMercury";
            lblMercury.Size = new Size(256, 65);
            lblMercury.TabIndex = 21;
            lblMercury.Text = "MERCURY";
            // 
            // lblSercureCommunicationsFramework
            // 
            lblSercureCommunicationsFramework.AutoSize = true;
            lblSercureCommunicationsFramework.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSercureCommunicationsFramework.ForeColor = Color.Gainsboro;
            lblSercureCommunicationsFramework.Location = new Point(104, 60);
            lblSercureCommunicationsFramework.Name = "lblSercureCommunicationsFramework";
            lblSercureCommunicationsFramework.Size = new Size(250, 20);
            lblSercureCommunicationsFramework.TabIndex = 22;
            lblSercureCommunicationsFramework.Text = "Sercure Communications Framework";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 27, 29);
            ClientSize = new Size(1640, 1023);
            Controls.Add(lblSercureCommunicationsFramework);
            Controls.Add(mpgEnvelopeInspectionWorkspace);
            Controls.Add(mgbFooterMetadata);
            Controls.Add(mgbHeaderMetadata);
            Controls.Add(mgbProtectedPayload);
            Controls.Add(mgcEnvelopeInspector);
            Controls.Add(mercuryGlassPanel15);
            Controls.Add(mercuryGlassPanel14);
            Controls.Add(mercuryGlassPanel13);
            Controls.Add(mercuryGlassPanel12);
            Controls.Add(mercuryGlassPanel11);
            Controls.Add(mercuryGlassPanel10);
            Controls.Add(mercuryGlassPanel9);
            Controls.Add(mercuryGlassPanel8);
            Controls.Add(mercuryGlassPanel7);
            Controls.Add(mercuryGlassPanel6);
            Controls.Add(mercuryGlassPanel2);
            Controls.Add(mercuryGlassPanel1);
            Controls.Add(mgpConfiguration);
            Controls.Add(pictureBox1);
            Controls.Add(lblMercury);
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mercury Demo";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            mgpConfiguration.ResumeLayout(false);
            mgpConfiguration.PerformLayout();
            mercuryGlassPanel1.ResumeLayout(false);
            mercuryGlassPanel1.PerformLayout();
            mercuryGlassPanel2.ResumeLayout(false);
            mercuryGlassPanel6.ResumeLayout(false);
            mercuryGlassPanel6.PerformLayout();
            mercuryGlassPanel7.ResumeLayout(false);
            mercuryGlassPanel9.ResumeLayout(false);
            mercuryGlassPanel10.ResumeLayout(false);
            mercuryGlassPanel11.ResumeLayout(false);
            mercuryGlassPanel12.ResumeLayout(false);
            mercuryGlassPanel13.ResumeLayout(false);
            mercuryGlassPanel14.ResumeLayout(false);
            mercuryGlassPanel15.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Controls.MercuryGlassPanel mgpConfiguration;
        private Controls.MercuryGlassComboBox mgcCryptoProvider;
        private Label lblCryptoProvider;
        private Controls.MercuryGlassPanel mercuryGlassPanel1;
        private Controls.MercuryGlassPanel mercuryGlassPanel2;
        private Controls.MercuryGlassPanel mercuryGlassPanel6;
        private Controls.MercuryGlassButton mgbApplyConfiguration;
        private Controls.MercuryGlassComboBox mgcLogging;
        private Controls.MercuryGlassComboBox mgcChunkSize;
        private Controls.MercuryGlassComboBox mgcEnvelopeCodec;
        private Controls.MercuryGlassComboBox mgcTransport;
        private Controls.MercuryGlassPanel mercuryGlassPanel7;
        private Controls.MercuryGlassPanel mercuryGlassPanel8;
        private TextBox textBox1;
        private Label lblVersion;
        private Controls.MercuryGlassButton mgbClearLog;
        private Controls.MercuryGlassToggleButton mgbTamperPayload;
        private Controls.MercuryGlassToggleButton mgbTamperAuthTag;
        private Controls.MercuryGlassToggleButton mgbTamperReplayToken;
        private Controls.MercuryGlassToggleButton mgbReplayLastFrame;
        private Controls.MercuryGlassPanel mercuryGlassPanel9;
        private Controls.MercuryGlassPanel mercuryGlassPanel10;
        private Controls.MercuryGlassPanel mercuryGlassPanel11;
        private Controls.MercuryGlassPanel mercuryGlassPanel12;
        private Controls.MercuryGlassPanel mercuryGlassPanel13;
        private Controls.MercuryGlassPanel mercuryGlassPanel14;
        private Controls.MercuryGlassPanel mercuryGlassPanel15;
        private CheckBox chkChunkEnabled;
        private Label lblChunking;
        private Label lblLogging;
        private Label lblChunkSize;
        private Label lblEnvelopeCodec;
        private Label lblTransport;
        private Controls.MercuryGlassButton mgcEnvelopeInspector;
        private Controls.MercuryGlassButton mgbProtectedPayload;
        private Controls.MercuryGlassButton mgbHeaderMetadata;
        private Controls.MercuryGlassButton mgbFooterMetadata;
        private Controls.MercuryGlassPanel mpgEnvelopeInspectionWorkspace;
        private Label lblStatusConnection;
        private Label lblStatusFramesSent;
        private Label lblStatusFramesReceived;
        private Label lblStatusAuthFailures;
        private Label lblStatusReplayAttemps;
        private Label lblStatusChunkCount;
        private Label lblStatusAverageSize;
        private Label lblPayloadSizeSend;
        private Controls.MercuryGlassButton mgbClearRecipient;
        private Controls.MercuryGlassButton mgbClearSender;
        private TextBox txtRecipient;
        private TextBox txtSender;
        private Label lblPayloadSizeReceive;
        private Label lblSenderTag;
        private Controls.MercuryGlassButton mgbSendPayload;
        private Label lblRecipientTag;
        private Label lblMercury;
        private Label lblSercureCommunicationsFramework;
        private RichTextBox richTextBox1;
        private Controls.MercuryMarqueeLabel mmlRecipient;
        private Controls.MercuryMarqueeLabel mmlSender;
    }
}
