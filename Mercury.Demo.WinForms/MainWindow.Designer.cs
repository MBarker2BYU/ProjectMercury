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
            pictureBox1 = new PictureBox();
            mgpConfiguration = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            ckbChunkEnabled = new CheckBox();
            lblChunking = new Label();
            lblLogging = new Label();
            lblChunkSize = new Label();
            lblEnvelopeCodec = new Label();
            lblTransport = new Label();
            mgbApplyConfiguration = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgcLogging = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            mgcChunkSize = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            mgcEnvelopeCodec = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            mgcTransport = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            mgcCryptoProvider = new Mercury.Demo.WinForms.Controls.MercuryGlassComboBox();
            lblCryptoProvider = new Label();
            mercuryGlassPanel1 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            mgbSendPayload = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            lblRecipientTag = new Label();
            lblSenderTag = new Label();
            lblPayloadReceived = new Label();
            lblPayloadSent = new Label();
            lblPayloadSizeReceive = new Label();
            lblPayloadSizeSend = new Label();
            mercuryGlassButton2 = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mercuryGlassButton1 = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            mercuryGlassPanel2 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            mgbClearLog = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbChunkedPayload = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbLargePayload = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbTamperPayload = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbTamperAuthTag = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbTamperReplayToken = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbReplayLastFrame = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbReceive = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbSend = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mercuryGlassPanel3 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            mgbLoadFrameFromFile = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbSaveLastFrame = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbViewBinaryFrame = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mercuryGlassPanel4 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            label2 = new Label();
            lblTransportCon = new Label();
            mgbDisconnect = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mercuryGlassPanel5 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            mgbSwapEndpoints = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbRemoteEndpoint = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbLocalEndpoint = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mercuryGlassPanel6 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            textBox1 = new TextBox();
            lblVersion = new Label();
            mercuryGlassPanel7 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            richTextBox1 = new RichTextBox();
            mercuryGlassPanel8 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            mercuryGlassPanel9 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusConnection = new Label();
            mercuryGlassPanel10 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusFramesSent = new Label();
            mercuryGlassPanel11 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusFramesReceived = new Label();
            mercuryGlassPanel12 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusAuthFailures = new Label();
            mercuryGlassPanel13 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusReplayAttemps = new Label();
            mercuryGlassPanel14 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusChunkCount = new Label();
            mercuryGlassPanel15 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblStatusAverageSize = new Label();
            mgcEnvelopeInspector = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbProtectedPayload = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbHeaderMetadata = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mgbFooterMetadata = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            mercuryGlassPanel16 = new Mercury.Demo.WinForms.Controls.MercuryGlassPanel();
            lblMercury = new Label();
            lblSercureCommunicationsFramework = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            mgpConfiguration.SuspendLayout();
            mercuryGlassPanel1.SuspendLayout();
            mercuryGlassPanel2.SuspendLayout();
            mercuryGlassPanel3.SuspendLayout();
            mercuryGlassPanel4.SuspendLayout();
            mercuryGlassPanel5.SuspendLayout();
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
            mgpConfiguration.Controls.Add(ckbChunkEnabled);
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
            // ckbChunkEnabled
            // 
            ckbChunkEnabled.AutoSize = true;
            ckbChunkEnabled.Checked = true;
            ckbChunkEnabled.CheckState = CheckState.Checked;
            ckbChunkEnabled.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ckbChunkEnabled.Location = new Point(126, 147);
            ckbChunkEnabled.Name = "ckbChunkEnabled";
            ckbChunkEnabled.Size = new Size(68, 19);
            ckbChunkEnabled.TabIndex = 13;
            ckbChunkEnabled.Text = "Enabled";
            ckbChunkEnabled.UseVisualStyleBackColor = true;
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
            mercuryGlassPanel1.Controls.Add(lblPayloadReceived);
            mercuryGlassPanel1.Controls.Add(lblPayloadSent);
            mercuryGlassPanel1.Controls.Add(lblPayloadSizeReceive);
            mercuryGlassPanel1.Controls.Add(lblPayloadSizeSend);
            mercuryGlassPanel1.Controls.Add(mercuryGlassButton2);
            mercuryGlassPanel1.Controls.Add(mercuryGlassButton1);
            mercuryGlassPanel1.Controls.Add(textBox3);
            mercuryGlassPanel1.Controls.Add(textBox2);
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
            mgbSendPayload.Location = new Point(461, 137);
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
            // lblPayloadReceived
            // 
            lblPayloadReceived.AutoSize = true;
            lblPayloadReceived.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPayloadReceived.Location = new Point(595, 43);
            lblPayloadReceived.Name = "lblPayloadReceived";
            lblPayloadReceived.Size = new Size(120, 16);
            lblPayloadReceived.TabIndex = 7;
            lblPayloadReceived.Text = "Payload Received";
            // 
            // lblPayloadSent
            // 
            lblPayloadSent.AutoSize = true;
            lblPayloadSent.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPayloadSent.Location = new Point(24, 43);
            lblPayloadSent.Name = "lblPayloadSent";
            lblPayloadSent.Size = new Size(58, 16);
            lblPayloadSent.TabIndex = 6;
            lblPayloadSent.Text = "Payload";
            // 
            // lblPayloadSizeReceive
            // 
            lblPayloadSizeReceive.AutoSize = true;
            lblPayloadSizeReceive.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPayloadSizeReceive.ForeColor = Color.FromArgb(0, 192, 0);
            lblPayloadSizeReceive.Location = new Point(595, 253);
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
            lblPayloadSizeSend.Location = new Point(24, 253);
            lblPayloadSizeSend.Name = "lblPayloadSizeSend";
            lblPayloadSizeSend.Size = new Size(136, 16);
            lblPayloadSizeSend.TabIndex = 4;
            lblPayloadSizeSend.Text = "Payload Size: 0 bytes";
            // 
            // mercuryGlassButton2
            // 
            mercuryGlassButton2.BackColor = Color.Transparent;
            mercuryGlassButton2.ButtonColor = Color.Gray;
            mercuryGlassButton2.Font = new Font("Segoe UI", 9F);
            mercuryGlassButton2.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassButton2.ImageAlign = ContentAlignment.MiddleLeft;
            mercuryGlassButton2.Location = new Point(903, 245);
            mercuryGlassButton2.Name = "mercuryGlassButton2";
            mercuryGlassButton2.Padding = new Padding(10, 0, 10, 0);
            mercuryGlassButton2.Size = new Size(98, 36);
            mercuryGlassButton2.TabIndex = 3;
            mercuryGlassButton2.Text = "Clear";
            mercuryGlassButton2.UseVisualStyleBackColor = false;
            // 
            // mercuryGlassButton1
            // 
            mercuryGlassButton1.BackColor = Color.Transparent;
            mercuryGlassButton1.ButtonColor = Color.Gray;
            mercuryGlassButton1.Font = new Font("Segoe UI", 9F);
            mercuryGlassButton1.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassButton1.ImageAlign = ContentAlignment.MiddleLeft;
            mercuryGlassButton1.Location = new Point(332, 245);
            mercuryGlassButton1.Name = "mercuryGlassButton1";
            mercuryGlassButton1.Padding = new Padding(10, 0, 10, 0);
            mercuryGlassButton1.Size = new Size(98, 36);
            mercuryGlassButton1.TabIndex = 2;
            mercuryGlassButton1.Text = "Clear";
            mercuryGlassButton1.UseVisualStyleBackColor = false;
            // 
            // textBox3
            // 
            textBox3.BackColor = Color.FromArgb(26, 30, 33);
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.ForeColor = Color.Gainsboro;
            textBox3.Location = new Point(595, 71);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(406, 160);
            textBox3.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(26, 30, 33);
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.ForeColor = Color.Gainsboro;
            textBox2.Location = new Point(24, 71);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(406, 160);
            textBox2.TabIndex = 0;
            // 
            // mercuryGlassPanel2
            // 
            mercuryGlassPanel2.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel2.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel2.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel2.Controls.Add(mgbClearLog);
            mercuryGlassPanel2.Controls.Add(mgbChunkedPayload);
            mercuryGlassPanel2.Controls.Add(mgbLargePayload);
            mercuryGlassPanel2.Controls.Add(mgbTamperPayload);
            mercuryGlassPanel2.Controls.Add(mgbTamperAuthTag);
            mercuryGlassPanel2.Controls.Add(mgbTamperReplayToken);
            mercuryGlassPanel2.Controls.Add(mgbReplayLastFrame);
            mercuryGlassPanel2.Controls.Add(mgbReceive);
            mercuryGlassPanel2.Controls.Add(mgbSend);
            mercuryGlassPanel2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel2.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel2.Location = new Point(1336, 89);
            mercuryGlassPanel2.Name = "mercuryGlassPanel2";
            mercuryGlassPanel2.Padding = new Padding(16);
            mercuryGlassPanel2.Size = new Size(292, 381);
            mercuryGlassPanel2.TabIndex = 3;
            mercuryGlassPanel2.Title = "TEST PANEL";
            // 
            // mgbClearLog
            // 
            mgbClearLog.BackColor = Color.Transparent;
            mgbClearLog.Font = new Font("Segoe UI", 9F);
            mgbClearLog.ForeColor = Color.FromArgb(225, 232, 238);
            mgbClearLog.ImageAlign = ContentAlignment.MiddleLeft;
            mgbClearLog.Location = new Point(19, 339);
            mgbClearLog.Name = "mgbClearLog";
            mgbClearLog.Padding = new Padding(10, 0, 10, 0);
            mgbClearLog.Size = new Size(254, 32);
            mgbClearLog.TabIndex = 9;
            mgbClearLog.Text = "Clear Log";
            mgbClearLog.UseVisualStyleBackColor = false;
            // 
            // mgbChunkedPayload
            // 
            mgbChunkedPayload.BackColor = Color.Transparent;
            mgbChunkedPayload.Font = new Font("Segoe UI", 9F);
            mgbChunkedPayload.ForeColor = Color.FromArgb(225, 232, 238);
            mgbChunkedPayload.ImageAlign = ContentAlignment.MiddleLeft;
            mgbChunkedPayload.Location = new Point(19, 301);
            mgbChunkedPayload.Name = "mgbChunkedPayload";
            mgbChunkedPayload.Padding = new Padding(10, 0, 10, 0);
            mgbChunkedPayload.Size = new Size(254, 32);
            mgbChunkedPayload.TabIndex = 8;
            mgbChunkedPayload.Text = "Chunked Payload (50 KB)";
            mgbChunkedPayload.UseVisualStyleBackColor = false;
            // 
            // mgbLargePayload
            // 
            mgbLargePayload.BackColor = Color.Transparent;
            mgbLargePayload.Font = new Font("Segoe UI", 9F);
            mgbLargePayload.ForeColor = Color.FromArgb(225, 232, 238);
            mgbLargePayload.ImageAlign = ContentAlignment.MiddleLeft;
            mgbLargePayload.Location = new Point(19, 263);
            mgbLargePayload.Name = "mgbLargePayload";
            mgbLargePayload.Padding = new Padding(10, 0, 10, 0);
            mgbLargePayload.Size = new Size(254, 32);
            mgbLargePayload.TabIndex = 7;
            mgbLargePayload.Text = "Large Payload (10 KB)";
            mgbLargePayload.UseVisualStyleBackColor = false;
            // 
            // mgbTamperPayload
            // 
            mgbTamperPayload.BackColor = Color.Transparent;
            mgbTamperPayload.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbTamperPayload.Font = new Font("Segoe UI", 9F);
            mgbTamperPayload.ForeColor = Color.FromArgb(225, 232, 238);
            mgbTamperPayload.ImageAlign = ContentAlignment.MiddleLeft;
            mgbTamperPayload.Location = new Point(19, 225);
            mgbTamperPayload.Name = "mgbTamperPayload";
            mgbTamperPayload.Padding = new Padding(10, 0, 10, 0);
            mgbTamperPayload.Size = new Size(254, 32);
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
            mgbTamperAuthTag.Location = new Point(19, 187);
            mgbTamperAuthTag.Name = "mgbTamperAuthTag";
            mgbTamperAuthTag.Padding = new Padding(10, 0, 10, 0);
            mgbTamperAuthTag.Size = new Size(254, 32);
            mgbTamperAuthTag.TabIndex = 5;
            mgbTamperAuthTag.Text = "TamperAuthTag";
            mgbTamperAuthTag.UseVisualStyleBackColor = false;
            // 
            // mgbTamperReplayToken
            // 
            mgbTamperReplayToken.BackColor = Color.Transparent;
            mgbTamperReplayToken.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbTamperReplayToken.Font = new Font("Segoe UI", 9F);
            mgbTamperReplayToken.ForeColor = Color.FromArgb(225, 232, 238);
            mgbTamperReplayToken.ImageAlign = ContentAlignment.MiddleLeft;
            mgbTamperReplayToken.Location = new Point(19, 149);
            mgbTamperReplayToken.Name = "mgbTamperReplayToken";
            mgbTamperReplayToken.Padding = new Padding(10, 0, 10, 0);
            mgbTamperReplayToken.Size = new Size(254, 32);
            mgbTamperReplayToken.TabIndex = 4;
            mgbTamperReplayToken.Text = "Tamper Replay Token";
            mgbTamperReplayToken.UseVisualStyleBackColor = false;
            // 
            // mgbReplayLastFrame
            // 
            mgbReplayLastFrame.BackColor = Color.Transparent;
            mgbReplayLastFrame.Font = new Font("Segoe UI", 9F);
            mgbReplayLastFrame.ForeColor = Color.FromArgb(225, 232, 238);
            mgbReplayLastFrame.ImageAlign = ContentAlignment.MiddleLeft;
            mgbReplayLastFrame.Location = new Point(19, 111);
            mgbReplayLastFrame.Name = "mgbReplayLastFrame";
            mgbReplayLastFrame.Padding = new Padding(10, 0, 10, 0);
            mgbReplayLastFrame.Size = new Size(254, 32);
            mgbReplayLastFrame.TabIndex = 3;
            mgbReplayLastFrame.Text = "Replay Last Frame";
            mgbReplayLastFrame.UseVisualStyleBackColor = false;
            // 
            // mgbReceive
            // 
            mgbReceive.BackColor = Color.Transparent;
            mgbReceive.ButtonColor = Color.FromArgb(0, 192, 0);
            mgbReceive.Font = new Font("Segoe UI", 9F);
            mgbReceive.ForeColor = Color.FromArgb(225, 232, 238);
            mgbReceive.ImageAlign = ContentAlignment.MiddleLeft;
            mgbReceive.Location = new Point(19, 73);
            mgbReceive.Name = "mgbReceive";
            mgbReceive.Padding = new Padding(10, 0, 10, 0);
            mgbReceive.Size = new Size(254, 32);
            mgbReceive.TabIndex = 2;
            mgbReceive.Text = "Receive";
            mgbReceive.UseVisualStyleBackColor = false;
            // 
            // mgbSend
            // 
            mgbSend.BackColor = Color.Transparent;
            mgbSend.ButtonColor = Color.FromArgb(0, 192, 0);
            mgbSend.Font = new Font("Segoe UI", 9F);
            mgbSend.ForeColor = Color.FromArgb(225, 232, 238);
            mgbSend.ImageAlign = ContentAlignment.MiddleLeft;
            mgbSend.Location = new Point(19, 35);
            mgbSend.Name = "mgbSend";
            mgbSend.Padding = new Padding(10, 0, 10, 0);
            mgbSend.Size = new Size(254, 32);
            mgbSend.TabIndex = 1;
            mgbSend.Text = "Send";
            mgbSend.UseVisualStyleBackColor = false;
            // 
            // mercuryGlassPanel3
            // 
            mercuryGlassPanel3.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel3.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel3.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel3.Controls.Add(mgbLoadFrameFromFile);
            mercuryGlassPanel3.Controls.Add(mgbSaveLastFrame);
            mercuryGlassPanel3.Controls.Add(mgbViewBinaryFrame);
            mercuryGlassPanel3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel3.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel3.Location = new Point(1336, 476);
            mercuryGlassPanel3.Name = "mercuryGlassPanel3";
            mercuryGlassPanel3.Padding = new Padding(16);
            mercuryGlassPanel3.Size = new Size(292, 161);
            mercuryGlassPanel3.TabIndex = 4;
            mercuryGlassPanel3.Title = "Frame Tools";
            // 
            // mgbLoadFrameFromFile
            // 
            mgbLoadFrameFromFile.BackColor = Color.Transparent;
            mgbLoadFrameFromFile.Font = new Font("Segoe UI", 9F);
            mgbLoadFrameFromFile.ForeColor = Color.FromArgb(225, 232, 238);
            mgbLoadFrameFromFile.ImageAlign = ContentAlignment.MiddleLeft;
            mgbLoadFrameFromFile.Location = new Point(19, 114);
            mgbLoadFrameFromFile.Name = "mgbLoadFrameFromFile";
            mgbLoadFrameFromFile.Padding = new Padding(10, 0, 10, 0);
            mgbLoadFrameFromFile.Size = new Size(254, 36);
            mgbLoadFrameFromFile.TabIndex = 2;
            mgbLoadFrameFromFile.Text = "Load Frame from File";
            mgbLoadFrameFromFile.UseVisualStyleBackColor = false;
            // 
            // mgbSaveLastFrame
            // 
            mgbSaveLastFrame.BackColor = Color.Transparent;
            mgbSaveLastFrame.Font = new Font("Segoe UI", 9F);
            mgbSaveLastFrame.ForeColor = Color.FromArgb(225, 232, 238);
            mgbSaveLastFrame.ImageAlign = ContentAlignment.MiddleLeft;
            mgbSaveLastFrame.Location = new Point(19, 76);
            mgbSaveLastFrame.Name = "mgbSaveLastFrame";
            mgbSaveLastFrame.Padding = new Padding(10, 0, 10, 0);
            mgbSaveLastFrame.Size = new Size(254, 32);
            mgbSaveLastFrame.TabIndex = 1;
            mgbSaveLastFrame.Text = "Save Last Frame";
            mgbSaveLastFrame.UseVisualStyleBackColor = false;
            // 
            // mgbViewBinaryFrame
            // 
            mgbViewBinaryFrame.BackColor = Color.Transparent;
            mgbViewBinaryFrame.Font = new Font("Segoe UI", 9F);
            mgbViewBinaryFrame.ForeColor = Color.FromArgb(225, 232, 238);
            mgbViewBinaryFrame.ImageAlign = ContentAlignment.MiddleLeft;
            mgbViewBinaryFrame.Location = new Point(19, 38);
            mgbViewBinaryFrame.Name = "mgbViewBinaryFrame";
            mgbViewBinaryFrame.Padding = new Padding(10, 0, 10, 0);
            mgbViewBinaryFrame.Size = new Size(254, 32);
            mgbViewBinaryFrame.TabIndex = 0;
            mgbViewBinaryFrame.Text = "View Binary Frame";
            mgbViewBinaryFrame.UseVisualStyleBackColor = false;
            // 
            // mercuryGlassPanel4
            // 
            mercuryGlassPanel4.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel4.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel4.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel4.Controls.Add(label2);
            mercuryGlassPanel4.Controls.Add(lblTransportCon);
            mercuryGlassPanel4.Controls.Add(mgbDisconnect);
            mercuryGlassPanel4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel4.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel4.Location = new Point(1336, 643);
            mercuryGlassPanel4.Name = "mercuryGlassPanel4";
            mercuryGlassPanel4.Padding = new Padding(16);
            mercuryGlassPanel4.Size = new Size(292, 108);
            mercuryGlassPanel4.TabIndex = 6;
            mercuryGlassPanel4.Title = "CONNECTION";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(0, 192, 0);
            label2.Location = new Point(145, 35);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 2;
            label2.Text = "Connected";
            // 
            // lblTransportCon
            // 
            lblTransportCon.AutoSize = true;
            lblTransportCon.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTransportCon.Location = new Point(19, 35);
            lblTransportCon.Name = "lblTransportCon";
            lblTransportCon.Size = new Size(120, 15);
            lblTransportCon.TabIndex = 1;
            lblTransportCon.Text = "In-Memory Transport";
            // 
            // mgbDisconnect
            // 
            mgbDisconnect.BackColor = Color.Transparent;
            mgbDisconnect.ButtonColor = Color.FromArgb(192, 0, 0);
            mgbDisconnect.Enabled = false;
            mgbDisconnect.Font = new Font("Segoe UI", 9F);
            mgbDisconnect.ForeColor = Color.Red;
            mgbDisconnect.ImageAlign = ContentAlignment.MiddleLeft;
            mgbDisconnect.Location = new Point(19, 63);
            mgbDisconnect.Name = "mgbDisconnect";
            mgbDisconnect.Padding = new Padding(10, 0, 10, 0);
            mgbDisconnect.Size = new Size(254, 32);
            mgbDisconnect.TabIndex = 0;
            mgbDisconnect.Text = "Disconnect";
            mgbDisconnect.UseVisualStyleBackColor = false;
            // 
            // mercuryGlassPanel5
            // 
            mercuryGlassPanel5.AlignTitle = ContentAlignment.TopLeft;
            mercuryGlassPanel5.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel5.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel5.Controls.Add(mgbSwapEndpoints);
            mercuryGlassPanel5.Controls.Add(mgbRemoteEndpoint);
            mercuryGlassPanel5.Controls.Add(mgbLocalEndpoint);
            mercuryGlassPanel5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mercuryGlassPanel5.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel5.Location = new Point(12, 388);
            mercuryGlassPanel5.Name = "mercuryGlassPanel5";
            mercuryGlassPanel5.Padding = new Padding(16);
            mercuryGlassPanel5.Size = new Size(292, 175);
            mercuryGlassPanel5.TabIndex = 7;
            mercuryGlassPanel5.Title = "ENDPOINTS";
            // 
            // mgbSwapEndpoints
            // 
            mgbSwapEndpoints.BackColor = Color.Transparent;
            mgbSwapEndpoints.ButtonColor = Color.Gray;
            mgbSwapEndpoints.Font = new Font("Segoe UI", 9F);
            mgbSwapEndpoints.ForeColor = Color.FromArgb(225, 232, 238);
            mgbSwapEndpoints.ImageAlign = ContentAlignment.MiddleLeft;
            mgbSwapEndpoints.Location = new Point(10, 124);
            mgbSwapEndpoints.Name = "mgbSwapEndpoints";
            mgbSwapEndpoints.Padding = new Padding(10, 0, 10, 0);
            mgbSwapEndpoints.Size = new Size(267, 36);
            mgbSwapEndpoints.TabIndex = 2;
            mgbSwapEndpoints.Text = "Swap Endpoints";
            mgbSwapEndpoints.UseVisualStyleBackColor = false;
            // 
            // mgbRemoteEndpoint
            // 
            mgbRemoteEndpoint.BackColor = Color.Transparent;
            mgbRemoteEndpoint.ButtonColor = Color.Gray;
            mgbRemoteEndpoint.Font = new Font("Segoe UI", 9F);
            mgbRemoteEndpoint.ForeColor = Color.FromArgb(225, 232, 238);
            mgbRemoteEndpoint.ImageAlign = ContentAlignment.MiddleLeft;
            mgbRemoteEndpoint.Location = new Point(10, 82);
            mgbRemoteEndpoint.Name = "mgbRemoteEndpoint";
            mgbRemoteEndpoint.Padding = new Padding(10, 0, 10, 0);
            mgbRemoteEndpoint.Size = new Size(267, 36);
            mgbRemoteEndpoint.TabIndex = 1;
            mgbRemoteEndpoint.Text = "Remote Endpoint (Bravo)";
            mgbRemoteEndpoint.UseVisualStyleBackColor = false;
            // 
            // mgbLocalEndpoint
            // 
            mgbLocalEndpoint.BackColor = Color.Transparent;
            mgbLocalEndpoint.ButtonColor = Color.Gray;
            mgbLocalEndpoint.Font = new Font("Segoe UI", 9F);
            mgbLocalEndpoint.ForeColor = Color.FromArgb(225, 232, 238);
            mgbLocalEndpoint.ImageAlign = ContentAlignment.MiddleLeft;
            mgbLocalEndpoint.Location = new Point(10, 40);
            mgbLocalEndpoint.Name = "mgbLocalEndpoint";
            mgbLocalEndpoint.Padding = new Padding(10, 0, 10, 0);
            mgbLocalEndpoint.Size = new Size(267, 36);
            mgbLocalEndpoint.TabIndex = 0;
            mgbLocalEndpoint.Text = "Local Endpoint (Alpha)";
            mgbLocalEndpoint.UseVisualStyleBackColor = false;
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
            textBox1.Text = "Mercury is a secure communications framework that ensures no unprotected data is ever sent over any transport.\n\nBuilt for flexibility. Designed for trust.\n\nArchitected for the mission.";
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
            lblStatusConnection.ForeColor = Color.FromArgb(0, 192, 0);
            lblStatusConnection.Location = new Point(19, 36);
            lblStatusConnection.Name = "lblStatusConnection";
            lblStatusConnection.Size = new Size(124, 20);
            lblStatusConnection.TabIndex = 0;
            lblStatusConnection.Text = "Connected";
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
            lblStatusFramesSent.Location = new Point(19, 36);
            lblStatusFramesSent.Name = "lblStatusFramesSent";
            lblStatusFramesSent.Size = new Size(124, 20);
            lblStatusFramesSent.TabIndex = 1;
            lblStatusFramesSent.Text = "14";
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
            lblStatusFramesReceived.Location = new Point(19, 36);
            lblStatusFramesReceived.Name = "lblStatusFramesReceived";
            lblStatusFramesReceived.Size = new Size(124, 20);
            lblStatusFramesReceived.TabIndex = 1;
            lblStatusFramesReceived.Text = "14";
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
            lblStatusAuthFailures.Location = new Point(19, 36);
            lblStatusAuthFailures.Name = "lblStatusAuthFailures";
            lblStatusAuthFailures.Size = new Size(124, 20);
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
            lblStatusReplayAttemps.Location = new Point(19, 36);
            lblStatusReplayAttemps.Name = "lblStatusReplayAttemps";
            lblStatusReplayAttemps.Size = new Size(124, 20);
            lblStatusReplayAttemps.TabIndex = 1;
            lblStatusReplayAttemps.Text = "1";
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
            lblStatusChunkCount.Location = new Point(19, 36);
            lblStatusChunkCount.Name = "lblStatusChunkCount";
            lblStatusChunkCount.Size = new Size(124, 20);
            lblStatusChunkCount.TabIndex = 1;
            lblStatusChunkCount.Text = "3";
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
            lblStatusAverageSize.Location = new Point(11, 36);
            lblStatusAverageSize.Name = "lblStatusAverageSize";
            lblStatusAverageSize.Size = new Size(187, 20);
            lblStatusAverageSize.TabIndex = 1;
            lblStatusAverageSize.Text = "1,024 B";
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
            // mercuryGlassPanel16
            // 
            mercuryGlassPanel16.BackColor = Color.FromArgb(26, 30, 33);
            mercuryGlassPanel16.BorderColor = Color.FromArgb(46, 50, 54);
            mercuryGlassPanel16.ForeColor = Color.FromArgb(225, 232, 238);
            mercuryGlassPanel16.Location = new Point(310, 428);
            mercuryGlassPanel16.Name = "mercuryGlassPanel16";
            mercuryGlassPanel16.Padding = new Padding(16);
            mercuryGlassPanel16.Size = new Size(1020, 323);
            mercuryGlassPanel16.TabIndex = 20;
            mercuryGlassPanel16.Title = "";
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
            Controls.Add(mercuryGlassPanel16);
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
            Controls.Add(mercuryGlassPanel5);
            Controls.Add(mercuryGlassPanel4);
            Controls.Add(mercuryGlassPanel3);
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
            mercuryGlassPanel3.ResumeLayout(false);
            mercuryGlassPanel4.ResumeLayout(false);
            mercuryGlassPanel4.PerformLayout();
            mercuryGlassPanel5.ResumeLayout(false);
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
        private Controls.MercuryGlassPanel mercuryGlassPanel3;
        private Controls.MercuryGlassButton mgbLoadFrameFromFile;
        private Controls.MercuryGlassButton mgbSaveLastFrame;
        private Controls.MercuryGlassButton mgbViewBinaryFrame;
        private Controls.MercuryGlassPanel mercuryGlassPanel4;
        private Controls.MercuryGlassButton mgbDisconnect;
        private Controls.MercuryGlassPanel mercuryGlassPanel5;
        private Controls.MercuryGlassButton mgbSwapEndpoints;
        private Controls.MercuryGlassButton mgbRemoteEndpoint;
        private Controls.MercuryGlassButton mgbLocalEndpoint;
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
        private Controls.MercuryGlassButton mgbChunkedPayload;
        private Controls.MercuryGlassButton mgbLargePayload;
        private Controls.MercuryGlassButton mgbTamperPayload;
        private Controls.MercuryGlassButton mgbTamperAuthTag;
        private Controls.MercuryGlassButton mgbTamperReplayToken;
        private Controls.MercuryGlassButton mgbReplayLastFrame;
        private Controls.MercuryGlassButton mgbReceive;
        private Controls.MercuryGlassButton mgbSend;
        private Controls.MercuryGlassPanel mercuryGlassPanel9;
        private Controls.MercuryGlassPanel mercuryGlassPanel10;
        private Controls.MercuryGlassPanel mercuryGlassPanel11;
        private Controls.MercuryGlassPanel mercuryGlassPanel12;
        private Controls.MercuryGlassPanel mercuryGlassPanel13;
        private Controls.MercuryGlassPanel mercuryGlassPanel14;
        private Controls.MercuryGlassPanel mercuryGlassPanel15;
        private Label lblTransportCon;
        private Label label2;
        private CheckBox ckbChunkEnabled;
        private Label lblChunking;
        private Label lblLogging;
        private Label lblChunkSize;
        private Label lblEnvelopeCodec;
        private Label lblTransport;
        private Controls.MercuryGlassButton mgcEnvelopeInspector;
        private Controls.MercuryGlassButton mgbProtectedPayload;
        private Controls.MercuryGlassButton mgbHeaderMetadata;
        private Controls.MercuryGlassButton mgbFooterMetadata;
        private Controls.MercuryGlassPanel mercuryGlassPanel16;
        private Label lblStatusConnection;
        private Label lblStatusFramesSent;
        private Label lblStatusFramesReceived;
        private Label lblStatusAuthFailures;
        private Label lblStatusReplayAttemps;
        private Label lblStatusChunkCount;
        private Label lblStatusAverageSize;
        private Label lblPayloadSizeSend;
        private Controls.MercuryGlassButton mercuryGlassButton2;
        private Controls.MercuryGlassButton mercuryGlassButton1;
        private TextBox textBox3;
        private TextBox textBox2;
        private Label lblPayloadSizeReceive;
        private Label lblSenderTag;
        private Label lblPayloadReceived;
        private Label lblPayloadSent;
        private Controls.MercuryGlassButton mgbSendPayload;
        private Label lblRecipientTag;
        private Label lblMercury;
        private Label lblSercureCommunicationsFramework;
        private RichTextBox richTextBox1;
    }
}
