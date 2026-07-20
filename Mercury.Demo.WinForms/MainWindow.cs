using Mercury.Demo.WinForms.Controllers;
using Mercury.Demo.WinForms.Controls;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Presentation;
using System.Drawing.Drawing2D;
using Mercury.Abstractions.Primitives;

namespace Mercury.Demo.WinForms
{
    public partial class MainWindow : Form
    {

        #region Events

        private async void btnApplyConfiguration_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (m_DemoController == null)
                    return;

                SetBusy(true);

                
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {

            try
            {
                if (m_DemoController == null)
                    return;

                SetBusy(true);


              
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                
                
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnReplayAttack_Click(object sender, EventArgs e)
        {

            try
            {
                if (m_DemoController == null)
                    return;

                SetBusy(true);

               
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnTamperAttack_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_DemoController == null)
                    return;

                SetBusy(true);

                
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnWrongKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_DemoController == null)
                    return;

                SetBusy(true);

               
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbEventLog.Clear();

            AppendLog(new DemoLogEntry("INFO", "Event log cleared"));
        }

        private  void tglChunking_CheckedChanged(object sender, EventArgs e)
        {
            cboChunkSize.Enabled = tglChunking.Checked;
            lblChunkSize.Enabled = tglChunking.Checked;
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                SetBusy(true);

                m_DemoController = new DemoController(AppendLog);

                BindConfigurationOptions();

                var configuration = BuildConfiguration();

                var result = await m_DemoController
                    .ConfigureAsync(configuration);

                ApplyConfiguration(result.Configuration, result.IsConnected);
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (m_IsClosing)
                    return;

                var dialog = new MercuryDialog();
                var dialogResult = dialog.ShowDialog();
                
                if (dialogResult != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

                e.Cancel = true;
                m_IsClosing = true;

                if (m_DemoController != null)
                {
                    await m_DemoController.DisposeAsync();

                    m_DemoController = null;
                }

                Close();
            }
            catch (Exception exception)
            {
                m_IsClosing = false;
                DisplayError(exception.Message);
            }
        }

        #endregion

        #region Methods

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion
        
        private void RenderBorder(Graphics graphics)
        {

            var bounds = ClientRectangle;

            bounds.Width -= 1;
            bounds.Height -= 1;

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var smoothingMode = graphics.SmoothingMode;
            var pixelOffsetMode = graphics.PixelOffsetMode;

            try
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var borderPen = new Pen(Color.FromArgb(34, 61, 84));
                graphics.DrawRectangle(borderPen, bounds);

            }
            finally
            {
                graphics.SmoothingMode = smoothingMode;
                graphics.PixelOffsetMode = pixelOffsetMode;
            }

        }

        private void ApplyConfiguration(DemoConfiguration configuration, bool connected)
        {
            lblProviderValue.Text = configuration.CryptoProvider;
            lblTransportValue.Text = configuration.Transport;

            //lblConnectionValue.Text =
            //    configuration.Transport.ToUpperInvariant();

            //lblConnectionValue.ForeColor = connected
            //    ? MercuryTheme.SuccessColor
            //    : MercuryTheme.FailureColor;

            lblTransportState.Text = connected
                ? "CONNECTED"
                : "OFFLINE";

            lblTransportState.ForeColor = connected
                ? MercuryTheme.SuccessColor
                : MercuryTheme.FailureColor;

            lblSystemState.Text = connected
                ? "OPERATIONAL"
                : "OFFLINE";

            lblSystemState.ForeColor = connected
                ? MercuryTheme.SuccessColor
                : MercuryTheme.FailureColor;

            lblFlowTransport.Text =
                configuration.Transport.ToUpperInvariant();

            lblFlowProvider.Text =
                configuration.CryptoProvider.ToUpperInvariant();

            //lblConfigurationState.Text = "CONFIGURATION APPLIED";
            //lblConfigurationState.ForeColor =
            //    MercuryTheme.SuccessColor;

            lblIntegrityState.Text = "READY";
            lblIntegrityState.ForeColor =
                MercuryTheme.MutedColor;

            lblReplayState.Text = "ACTIVE";
            lblReplayState.ForeColor =
                MercuryTheme.SuccessColor;

            lblTamperState.Text = "CLEAN";
            lblTamperState.ForeColor =
                MercuryTheme.SuccessColor;

            SetStatusIndicator(picProviderCheck, connected);
            SetStatusIndicator(picTransportCheck, connected);
            SetStatusIndicator(picIntegrityCheck, connected);
            SetStatusIndicator(picReplayCheck, connected);
            SetStatusIndicator(picTamperCheck, connected);
        }

        private void SetBusy(bool busy)
        {
            btnApplyConfiguration.Enabled = !busy;
            btnSend.Enabled = !busy;
            btnSendFile.Enabled = !busy;
            btnReplayAttack.Enabled = !busy;
            btnTamperAttack.Enabled = !busy;
            btnWrongKey.Enabled = !busy;

            UseWaitCursor = busy;
        }

        private void BindConfigurationOptions()
        {
            BindItems(cboCryptoProvider, DemoController.CryptoProviders);

            BindItems(cboTransport, DemoController.Transports.Cast<object>());

            BindItems(cboEnvelopeCodec, DemoController.EnvelopeCodecs.Cast<object>());

            BindItems(cboLogging, DemoController.LoggingLevels.Cast<object>());

            var chunkSizes = DemoController.ChunkSizes
                .Select(bytes => new ChunkSizeOption(bytes))
                .Cast<object>();

            BindItems(cboChunkSize, chunkSizes);

            cboCryptoProvider.Text = DemoConstants.AES_GCM;
            cboTransport.Text = DemoConstants.IN_MEMORY_TRANSPORT;
            cboEnvelopeCodec.Text = DemoConstants.BINARY_CODEC;
            cboLogging.Text = DemoConstants.VERBOSE_LOGGING;
            cboChunkSize.Text = "64 KB";

            tglChunking.Checked = true;
            cboChunkSize.Enabled = true;
            lblChunkSize.Enabled = true;
        }

        private DemoConfiguration BuildConfiguration()
        {
            var chunkSize = cboChunkSize.SelectedItem is ChunkSizeOption option
                ? option.Bytes
                : DemoConstants.DEFAULT_CHUNK_SIZE;

            return new DemoConfiguration(cboCryptoProvider.Text, cboTransport.Text, 
                cboEnvelopeCodec.Text, tglChunking.Checked, chunkSize, cboLogging.Text);
        }

        private void AppendLog(DemoLogEntry logEntry)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => AppendLog(logEntry));
                return;
            }

            var levelColor = logEntry.Level switch
            {
                "ERROR" => MercuryTheme.FailureColor,
                "WARN" => MercuryTheme.WarningColor,
                "TRACE" => MercuryTheme.MutedColor,
                _ => MercuryTheme.SuccessColor
            };

            rtbEventLog.SelectionStart = rtbEventLog.TextLength;
            rtbEventLog.SelectionLength = 0;

            rtbEventLog.SelectionColor = MercuryTheme.MutedColor;
            rtbEventLog.AppendText($"{logEntry.Timestamp:HH:mm:ss.fff}  ");

            rtbEventLog.SelectionColor = levelColor;
            rtbEventLog.AppendText($"[{logEntry.Level}] ");

            rtbEventLog.SelectionColor = MercuryTheme.ForeColor;
            rtbEventLog.AppendText(logEntry.Entry + Environment.NewLine);

            rtbEventLog.SelectionStart = rtbEventLog.TextLength;
            rtbEventLog.ScrollToCaret();
        }

        private void DisplayError(string message)
        {
            lblReceiveResult.Text = $"FAILED: {message}";
            lblReceiveResult.ForeColor = MercuryTheme.FailureColor;
            lblReceiveResult.BackColor = Color.FromArgb(35, MercuryTheme.FailureColor);

            MessageBox.Show(this, message, "Mercury Demo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //private void DisplayResult(DemoExchangeResult result)
        //{
        //    var isProtectedExchange =
        //        result.Scenario == DemoScenario.ProtectedExchange;

        //    var accepted =
        //        isProtectedExchange && result.Success;

        //    txtReceiveHeader.Text = result.Header;

        //    txtReceivePayload.Text = accepted
        //        ? result.RestoredPayload
        //        : result.LogEntry;

        //    txtReceiveFooter.Text = result.Footer;

        //    lblReceiveResult.Text = result.Scenario switch
        //    {
        //        DemoScenario.ProtectedExchange when result.Success =>
        //            $"RECEIVED OK  |  {result.Duration.TotalMilliseconds:N1} ms",

        //        DemoScenario.ReplayAttack when result.Success =>
        //            "REPLAY BLOCKED",

        //        DemoScenario.TamperAttack when result.Success =>
        //            "TAMPERED FRAME REJECTED",

        //        DemoScenario.WrongKey when result.Success =>
        //            "WRONG KEY REJECTED",

        //        _ =>
        //            $"FAILED: {result.LogEntry}"
        //    };

        //    lblReceiveResult.ForeColor = result.Success
        //        ? MercuryTheme.SuccessColor
        //        : MercuryTheme.FailureColor;

        //    lblReceiveResult.BackColor = Color.FromArgb(
        //        32,
        //        result.Success
        //            ? MercuryTheme.SuccessColor
        //            : MercuryTheme.FailureColor);

        //    lblIntegrityState.Text = result.Success
        //        ? "VERIFIED"
        //        : "FAILED";

        //    lblIntegrityState.ForeColor = result.Success
        //        ? MercuryTheme.SuccessColor
        //        : MercuryTheme.FailureColor;

        //    lblLastCheck.Text = $"LAST CHECK: {DateTimeOffset.UtcNow:HH:mm:ss} UTC";

        //    lblTamperState.Text = result.Scenario == DemoScenario.TamperAttack
        //            ? result.Success
        //                ? "DETECTED"
        //                : "FAILED"
        //            : "CLEAN";

        //    lblTamperState.ForeColor = result.Success
        //        ? MercuryTheme.SuccessColor
        //        : MercuryTheme.FailureColor;

        //    //lblThreatValue.Text = result.Success
        //    //    ? "LOW"
        //    //    : "ELEVATED";

        //    //lblThreatValue.ForeColor = result.Success
        //    //    ? MercuryTheme.SuccessColor
        //    //    : MercuryTheme.WarningColor;

        //    //lblFlowProtected.Text = result.Success
        //    //    ? "PROTECTED"
        //    //    : "FAILED";

        //    //lblFlowProtected.ForeColor = result.Success
        //    //    ? MercuryTheme.SuccessColor
        //    //    : MercuryTheme.FailureColor;

        //    //lblFlowTransit.Text = result.RawPayloadVisible
        //    //    ? "RAW PAYLOAD VISIBLE"
        //    //    : "PROTECTED IN TRANSIT";

        //    //lblFlowTransit.ForeColor = result.RawPayloadVisible
        //    //    ? MercuryTheme.FailureColor
        //    //    : MercuryTheme.SuccessColor;

        //    //lblFlowReceived.Text = result.Scenario switch
        //    //{
        //    //    DemoScenario.ProtectedExchange when result.Success =>
        //    //        "DELIVERED",

        //    //    DemoScenario.ReplayAttack when result.Success =>
        //    //        "BLOCKED",

        //    //    DemoScenario.TamperAttack when result.Success =>
        //    //        "BLOCKED",

        //    //DemoScenario.WrongKey when result.Success =>
        //    //"BLOCKED",

        //    //    _ =>
        //    //        "FAILED"
        //    //};

        //    //lblFlowReceived.ForeColor = result.Success
        //    //    ? MercuryTheme.SuccessColor
        //    //    : MercuryTheme.FailureColor;

        //    DisplayEnvelope(result.Envelope, result.RawPayloadVisible);

        //    SetStatusIndicator(picTamperCheck, result.Scenario != DemoScenario.TamperAttack || result.Success);
        //}

        private void DisplayEnvelope(EnvelopeInspection envelope, bool rawPayloadVisible)
        {
            lblEnvelopeVersionValue.Text = envelope.Version;
            lblEnvelopeIdValue.Text = envelope.EnvelopeId;
            //lblEnvelopeTimestampValue.Text = envelope.Timestamp;
            //lblSenderKeyValue.Text = envelope.SenderKeyId;
            //lblRecipientKeyValue.Text = envelope.RecipientKeyId;
            lblAlgorithmValue.Text = envelope.Algorithm;
            lblReplayTokenValue.Text = envelope.ReplayToken;

            lblProtectedSizeValue.Text =
                $"{envelope.ProtectedPayloadSize:N0} bytes";

            lblFrameSizeValue.Text =
                $"{envelope.TotalFrameSize:N0} bytes";

            lblRawPayloadValue.Text =
                rawPayloadVisible ? "YES" : "NO";

            lblRawPayloadValue.ForeColor =
                rawPayloadVisible
                    ? MercuryTheme.FailureColor
                    : MercuryTheme.SuccessColor;

            txtHeaderMetadata.Text =
                envelope.HeaderMetadata;

            txtFooterMetadata.Text =
                envelope.FooterMetadata;

            rtbHexPreview.Text =
                envelope.HexPreview;
        }

        //private DemoExchangeRequest BuildExchangeRequest()
        //{
        //    return new DemoExchangeRequest(txtSendHeader.Text, txtSendPayload.Text, txtSendFooter.Text);
        //}

        private void ShowReceivedImage(ReadOnlyMemory payload, string fileName)
        {
            if (payload.IsEmpty)
                throw new InvalidOperationException("Mercury did not return an image payload.");

            using var stream = new MemoryStream(payload.ToArray(), writable: false);

            using var sourceImage = Image.FromStream(stream, useEmbeddedColorManagement: true,
                validateImageData: true);

            var receivedImage = new Bitmap(sourceImage);

            using var window = new FileReceivedDialog(receivedImage, fileName);

            window.ShowDialog(this);
        }

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RenderBorder(e.Graphics);
        }

        #endregion

        #region Static

        private static string FormatFileSize(long byteCount)
        {
            const double kilobyte = 1024;
            const double megabyte = kilobyte * 1024;

            if (byteCount >= megabyte)
                return $"{byteCount / megabyte:N2} MB";

            if (byteCount >= kilobyte)
                return $"{byteCount / kilobyte:N2} KB";

            return $"{byteCount:N0} bytes";
        }

        private static void BindItems(MercuryGlassComboBox comboBox, IEnumerable<object> items, int defaultIndex = 0)
        {
            comboBox.Items.Clear();

            foreach (var item in items)
                comboBox.Items.Add(item);

            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = defaultIndex;
        }

        private static void SetStatusIndicator(PictureBox pictureBox, bool success)
        {
            pictureBox.BackColor = success
                ? Color.FromArgb(32, MercuryTheme.SuccessColor)
                : Color.FromArgb(32, MercuryTheme.FailureColor);
        }

        #endregion

        #endregion

        #region Properties and Fields

        private DemoController? m_DemoController;
        private bool m_IsClosing;

        #endregion
    }
}
