using Mercury.Demo.WinForms.Controllers;
using Mercury.Demo.WinForms.Controls;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Presentation;
using System.Drawing.Drawing2D;
using System.Text;
using Mercury.Abstractions;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Demo.WinForms.Services;

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

                var cryptoProvider = cboCryptoProvider.Text;
                var transport = cboTransport.Text;
                var codec = cboEnvelopeCodec.Text;
                var chunkingEnabled = tglChunking.Enabled;
                
                var logging = cboLogging.Text;

                var demoConfiguration = new DemoConfiguration(cryptoProvider, transport, codec, chunkingEnabled, 64 * 1024, logging);

                await m_DemoController.ConfigureAsync(demoConfiguration);

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

        //Tmp Values
        private bool m_InReplay = false;
        public bool ReplayEnabled = true;
        
        private async Task SendOnceAsync()
        {
            var payload = new ReadOnlyMemory(Encoding.UTF8.GetBytes(txtSendPayload.Text));
            var result = await m_DemoController.SendAsync(payload);

            DisplayResult(result);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {

            try
            {
                if (m_DemoController == null)
                    return;

                SetBusy(true);

                await SendOnceAsync();

                if (MercuryDemoSession.TcpAttackSimulatorProxy != null && MercuryDemoSession.TcpAttackSimulatorProxy.ReplayEnabled && !m_InReplay)
                {
                    m_InReplay = true;
                    await Task.Delay(400);
                    await SendOnceAsync();          // second send

                    // Clear the cached frame in the attack simulator
                    MercuryDemoSession.TcpAttackSimulatorProxy.ClearLastFrame();

                    m_InReplay = false;
                }


                //var payload = new ReadOnlyMemory(Encoding.UTF8.GetBytes(txtSendPayload.Text));

                //var result =
                //    await m_DemoController.SendAsync(payload);

                //DisplayResult(result);

                //if (ReplayEnabled)
                //{
                //    if (!m_InReplay)
                //    {
                //        m_InReplay = true;

                //        await Task.Delay(400); // short pause so it looks like an attack

                //        btnSend_Click(sender, e);

                //        m_InReplay = false;
                //    }
                //}

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
                if (m_DemoController == null)
                    return;

                using var dialog = new OpenFileDialog
                {
                    Title = @"Select an image to send",
                    Filter =
                        @"Image Files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|" +
                       @"*.png;*.jpg;*.jpeg;*.bmp;*.gif",
                    CheckFileExists = true,
                    Multiselect = false,
                    AddExtension = true,
                    RestoreDirectory = true
                };


                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                if (!IsImageFile(dialog.FileName))
                {
                    DisplayError("The selected file is not a supported image.");
                    return;
                }

                SetBusy(true);

                var fileName =
                    Path.GetFileName(dialog.FileName);

                var payload =
                    await File.ReadAllBytesAsync(
                        dialog.FileName);

                AppendLog(new DemoLogEntry("INFO", $"Sending image {fileName} | {FormatFileSize(payload.Length)}"));

                var result =
                    await m_DemoController.SendAsync(
                        new ReadOnlyMemory(payload));

                DisplayFileResult(result, fileName);

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
            lblReceiveResult.Text = @$"FAILED: {message}";
            lblReceiveResult.ForeColor = MercuryTheme.FailureColor;
            lblReceiveResult.BackColor = Color.FromArgb(35, MercuryTheme.FailureColor);

            MessageBox.Show(this, message, @"Mercury Demo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisplayResult(IMercuryResult result)
        {
            if (!result.Success)
            {
                DisplayFailedResult(result);
                return;
            }

            var secureEnvelope = result.ValidatedEnvelope;

            if (secureEnvelope == null)
            {
                DisplayError(
                    "Mercury reported success but did not return a validated SecureEnvelope.");

                return;
            }

            txtReceivePayload.Text =
                Encoding.UTF8.GetString(
                    result.Payload.ToArray());

            txtReceiveHeader.Text =
                FormatMetadata(secureEnvelope.Header.Meta);

            txtReceiveFooter.Text =
                FormatMetadata(secureEnvelope.Footer.Meta);

            lblReceiveResult.Text =
                $"RECEIVED OK | {result.Payload.Length:N0} bytes";

            lblReceiveResult.ForeColor =
                MercuryTheme.SuccessColor;

            lblReceiveResult.BackColor =
                Color.FromArgb(
                    32,
                    MercuryTheme.SuccessColor);

            DisplayValidatedEnvelope(secureEnvelope);

            lblIntegrityState.Text = "VERIFIED";
            lblIntegrityState.ForeColor =
                MercuryTheme.SuccessColor;

            lblReplayState.Text = "ACCEPTED";
            lblReplayState.ForeColor =
                MercuryTheme.SuccessColor;

            lblTamperState.Text = "CLEAN";
            lblTamperState.ForeColor =
                MercuryTheme.SuccessColor;

            lblLastCheck.Text =
                $"LAST CHECK: {DateTimeOffset.UtcNow:HH:mm:ss} UTC";

            SetStatusIndicator(picIntegrityCheck, true);
            SetStatusIndicator(picReplayCheck, true);
            SetStatusIndicator(picTamperCheck, true);
        }

        private void DisplayFileResult(
            IMercuryResult result,
            string fileName)
        {
            if (!result.Success)
            {
                DisplayFailedResult(result);
                return;
            }

            var secureEnvelope =
                result.ValidatedEnvelope;

            if (secureEnvelope == null)
            {
                DisplayError(
                    "Mercury reported success but did not return a validated SecureEnvelope.");

                return;
            }

            txtReceiveHeader.Text =
                FormatMetadata(
                    secureEnvelope.Header.Meta);

            txtReceivePayload.Text =
                @$"{fileName}{Environment.NewLine}" +
                @$"{FormatFileSize(result.Payload.Length)} received";

            txtReceiveFooter.Text =
                FormatMetadata(
                    secureEnvelope.Footer.Meta);

            lblReceiveResult.Text =
                @$"IMAGE RECEIVED OK | {FormatFileSize(result.Payload.Length)}";

            lblReceiveResult.ForeColor =
                MercuryTheme.SuccessColor;

            lblReceiveResult.BackColor =
                Color.FromArgb(
                    32,
                    MercuryTheme.SuccessColor);

            DisplayValidatedEnvelope(
                secureEnvelope);

            lblIntegrityState.Text = @"VERIFIED";

            lblIntegrityState.ForeColor =
                MercuryTheme.SuccessColor;

            lblLastCheck.Text =
                @$"LAST CHECK: {DateTimeOffset.UtcNow:HH:mm:ss} UTC";

            SetStatusIndicator(
                picIntegrityCheck,
                true);

            AppendLog(
                new DemoLogEntry(
                    "INFO",
                    $"File received and authenticated: {fileName}"));

            if (IsImageFile(fileName))
            {
                ShowReceivedImage(
                    result.Payload,
                    fileName);
            }
        }

        private static bool IsImageFile(
            string fileName)
        {
            var extension =
                Path.GetExtension(fileName);

            return extension.Equals(
                       ".png",
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   extension.Equals(
                       ".jpg",
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   extension.Equals(
                       ".jpeg",
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   extension.Equals(
                       ".bmp",
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   extension.Equals(
                       ".gif",
                       StringComparison.OrdinalIgnoreCase);
        }

        private void DisplayValidatedEnvelope(ISecureEnvelope secureEnvelope)
        {
            lblEnvelopeVersionValue.Text =
                secureEnvelope.Version.ToString();

            lblEnvelopeIdValue.Text =
                secureEnvelope.Header.EnvelopeId.ToString();

            lblAlgorithmValue.Text =
                secureEnvelope.Header.Encryption.ToString();

            lblReplayTokenValue.Text =
                Convert.ToHexString(
                    secureEnvelope.Header.ReplayToken.ToArray());

            lblProtectedSizeValue.Text =
                $"{secureEnvelope.Payload.Length:N0} bytes";

            txtHeaderMetadata.Text = FormatMetadata(secureEnvelope.Header.Meta);

            txtFooterMetadata.Text = FormatMetadata(secureEnvelope.Footer.Meta);

            /*
             * These values are not returned by IMercuryResult or
             * ISecureEnvelope. Do not manufacture them.
             */

            lblFrameSizeValue.Text = "NOT AVAILABLE";
            lblRawPayloadValue.Text = "NOT AVAILABLE";
            rtbHexPreview.Clear();

            lblFrameSizeValue.ForeColor =
                MercuryTheme.MutedColor;

            lblRawPayloadValue.ForeColor =
                MercuryTheme.MutedColor;
        }

        private void DisplayFailedResult(
            IMercuryResult result)
        {
            txtReceiveHeader.Clear();
            txtReceivePayload.Clear();
            txtReceiveFooter.Clear();

            ClearEnvelopeDisplay();

            var failureMessage =
                result.Message ??
                result.FailureReason.ToString();

            lblReceiveResult.Text =
                $"FAILED | {failureMessage}";

            lblReceiveResult.ForeColor =
                MercuryTheme.FailureColor;

            lblReceiveResult.BackColor =
                Color.FromArgb(
                    32,
                    MercuryTheme.FailureColor);

            lblIntegrityState.Text = "FAILED";
            lblIntegrityState.ForeColor =
                MercuryTheme.FailureColor;

            lblLastCheck.Text =
                $"LAST CHECK: {DateTimeOffset.UtcNow:HH:mm:ss} UTC";

            SetStatusIndicator(picIntegrityCheck, false);

            AppendLog(
                new DemoLogEntry(
                    "ERROR",
                    failureMessage));
        }

        private void ClearEnvelopeDisplay()
        {
            lblEnvelopeVersionValue.Text = "-";
            lblEnvelopeIdValue.Text = "-";
            lblAlgorithmValue.Text = "-";
            lblReplayTokenValue.Text = "-";
            lblProtectedSizeValue.Text = "-";
            lblFrameSizeValue.Text = "-";
            lblRawPayloadValue.Text = "-";

            txtHeaderMetadata.Clear();
            txtFooterMetadata.Clear();
            rtbHexPreview.Clear();
        }

        private static string FormatMetadata(
            Metadata metadata)
        {
            if (metadata.Count == 0)
                return string.Empty;

            return string.Join(
                Environment.NewLine,
                metadata.Select(
                    item => $"{item.Key}: {item.Value}"));
        }

        //private DemoExchangeRequest BuildExchangeRequest()
        //{
        //    return new DemoExchangeRequest(txtSendHeader.Text, txtSendPayload.Text, txtSendFooter.Text);
        //}

        private void ShowReceivedImage(ReadOnlyMemory payload, string fileName)
        {
            if (payload.IsEmpty)
                throw new InvalidOperationException("Mercury did not return an image payload.");
            
            using var window = new FileReceivedDialog(fileName, payload);

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
