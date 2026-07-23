
// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="MainWindow.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.Controllers;
using Mercury.Demo.WinForms.Controls;
using Mercury.Demo.WinForms.Demo;
using Mercury.Demo.WinForms.Presentation;
using System.Drawing.Drawing2D;
using System.Text;
using Mercury.Abstractions;
using Mercury.Abstractions.Envelope;
using Mercury.Abstractions.Primitives;
using Mercury.Demo.WinForms.Enums;

namespace Mercury.Demo.WinForms
{
    /// <summary>
    /// Class MainWindow.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainWindow : Form
    {

        #region Events

        /// <summary>
        /// Handles the Click event of the btnApplyConfiguration control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void btnApplyConfiguration_Click(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            await m_DemoController
                .ApplyConfigurationAsync();

        }

        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            await m_DemoController
                .SendPayloadAsync();
        }

        /// <summary>
        /// Handles the Click event of the btnSendFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            await m_DemoController
                .SendFileAsync();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglReplayAttack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tglReplayAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            m_DemoController.ReplayAttackChanged(
                tglReplayAttack.Checked);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglTamperAttack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tglTamperAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            m_DemoController.TamperAttackChanged(
                tglTamperAttack.Checked);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglWrongKeyAttack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tglWrongKeyAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            m_DemoController.WrongKeyAttackChanged(
                tglWrongKeyAttack.Checked);
        }

        /// <summary>
        /// Handles the Click event of the btnClearLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            m_DemoController.ClearEventLog();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglChunking control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tglChunking_CheckedChanged(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            m_DemoController.ChunkingChanged(tglChunking.Checked);
        }

        /// <summary>
        /// Handles the Load event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MainWindow_Load(object sender, EventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            await m_DemoController!.InitializeAsync();
        }

        /// <summary>
        /// Handles the FormClosing event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private async void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_DemoController == null)
                throw new Exception($"{nameof(DemoController)} is null.");

            await m_DemoController
                .FormClosingAsync(e);
        }

        #endregion

        #region Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            m_DemoController = new DemoController(this);
            EventLog = rtbEventLog;
        }

        #endregion

        /// <summary>
        /// Selects the image file.
        /// </summary>
        /// <returns>System.Nullable&lt;System.String&gt;.</returns>
        internal string? SelectImageFile()
        {
            using var dialog = new OpenFileDialog();
            dialog.Title = @"Select an image to send";
            dialog.Filter = @"Image Files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|" +
                            @"*.png;*.jpg;*.jpeg;*.bmp;*.gif";
            dialog.CheckFileExists = true;
            dialog.Multiselect = false;
            dialog.AddExtension = true;
            dialog.RestoreDirectory = true;

            return dialog.ShowDialog(this) == DialogResult.OK
                ? dialog.FileName
                : null;
        }

        /// <summary>
        /// Renders the border.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
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

        /// <summary>
        /// Sets the busy.
        /// </summary>
        /// <param name="busy">if set to <c>true</c> [busy].</param>
        internal void SetBusy(bool busy)
        {
            btnApplyConfiguration.Enabled = !busy;
            btnSend.Enabled = !busy;
            btnSendFile.Enabled = !busy;

            UseWaitCursor = busy;
        }

        /// <summary>
        /// Displays the error.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void DisplayError(string message)
        {
            lblReceiveResult.Text = @$"FAILED: {message}";
            lblReceiveResult.ForeColor = MercuryTheme.FailureColor;
            lblReceiveResult.BackColor = Color.FromArgb(35, MercuryTheme.FailureColor);

            MessageBox.Show(this, message, @"Mercury Demo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Gets the send payload.
        /// </summary>
        /// <returns>System.String.</returns>
        internal string GetSendPayload()
        {
            return txtSendPayload.Text;
        }

        

        

        private void DisplaySuccessfulSecurityState()
        {
            SetState(
                lblIntegrityState,
                "VERIFIED",
                MercuryTheme.SuccessColor);

            SetState(
                lblReplayState,
                "ACCEPTED",
                MercuryTheme.SuccessColor);

            SetState(
                lblTamperState,
                "CLEAN",
                MercuryTheme.SuccessColor);

            SetStatusIndicator(
                picIntegrityCheck,
                true);

            SetStatusIndicator(
                picReplayCheck,
                true);

            SetStatusIndicator(
                picTamperCheck,
                true);

            UpdateLastSecurityCheck();
        }

        /// <summary>
        /// Determines whether [is image file] [the specified file name].
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if [is image file] [the specified file name]; otherwise, <c>false</c>.</returns>
        private static bool IsImageFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return extension.Equals(".png", StringComparison.OrdinalIgnoreCase)
                   || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                   || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
                   || extension.Equals(".bmp", StringComparison.OrdinalIgnoreCase)
                   || extension.Equals(".gif", StringComparison.OrdinalIgnoreCase);
        }

        internal void DisplaySuccessfulPayload(string recoveredPayload, int payloadLength, DemoEnvelopeDisplay envelope)
        {
            txtReceivePayload.Text =
                recoveredPayload;
            
            ApplyEnvelopeDisplay(envelope);

            SetReceiveResult(
                $"RECEIVED OK | {payloadLength:N0} bytes", MercuryTheme.SuccessColor);

            DisplaySuccessfulSecurityState();

            DisplayExchangeFlow(success: true, DemoAttackMode.None);
        }

        internal void DisplaySuccessfulFile(string fileName, ReadOnlyMemory payload, DemoEnvelopeDisplay envelope)
        {
           
            txtReceivePayload.Text =
                @$"{fileName}{Environment.NewLine}" +
                @$"{FormatFileSize(payload.Length)} received";

            

            ApplyEnvelopeDisplay(envelope);

            SetReceiveResult(
                @$"IMAGE RECEIVED OK | {FormatFileSize(payload.Length)}",
                MercuryTheme.SuccessColor);

            DisplaySuccessfulSecurityState();

            DisplayExchangeFlow(
                success: true,
                DemoAttackMode.None);

            if (IsImageFile(fileName))
            {
                ShowReceivedImage(payload, fileName);
            }
        }

        private void ApplyEnvelopeDisplay(
            DemoEnvelopeDisplay envelope)
        {
            lblEnvelopeVersionValue.Text =
                envelope.Version;

            lblEnvelopeIdValue.Text =
                envelope.EnvelopeId;

            lblAlgorithmValue.Text =
                envelope.Algorithm;

            lblReplayTokenValue.Text =
                envelope.ReplayToken;

            lblProtectedSizeValue.Text =
                envelope.ProtectedPayloadSize;

            lblFrameSizeValue.Text =
                envelope.TotalFrameSize;

            lblRawPayloadValue.Text =
                envelope.RawPayloadVisible;

           rtbHexPreview.Text =
                envelope.HexPreview;

            rtbHexPreview.ForeColor =
                MercuryTheme.SuccessColor;

            rtbHexPreview.SelectionStart = 0;
            rtbHexPreview.SelectionLength = 0;

            lblFrameSizeValue.ForeColor =
                MercuryTheme.MutedColor;

            lblRawPayloadValue.ForeColor =
                MercuryTheme.SuccessColor;
        }

        internal void DisplayFailure(
     string failureMessage,
     DemoAttackMode attackMode)
        {
            txtReceivePayload.Clear();
            
            ClearEnvelopeDisplay();

            SetReceiveResult(
                $"FAILED | {failureMessage}",
                MercuryTheme.FailureColor);

            switch (attackMode)
            {
                case DemoAttackMode.Tamper:
                    SetState(
                        lblIntegrityState,
                        "FAILED",
                        MercuryTheme.FailureColor);

                    SetState(
                        lblReplayState,
                        "ACTIVE",
                        MercuryTheme.SuccessColor);

                    SetState(
                        lblTamperState,
                        "DETECTED",
                        MercuryTheme.FailureColor);

                    SetStatusIndicator(
                        picIntegrityCheck,
                        false);

                    SetStatusIndicator(
                        picReplayCheck,
                        true);

                    SetStatusIndicator(
                        picTamperCheck,
                        false);

                    break;

                case DemoAttackMode.Replay:
                    SetState(
                        lblIntegrityState,
                        "VERIFIED",
                        MercuryTheme.SuccessColor);

                    SetState(
                        lblReplayState,
                        "BLOCKED",
                        MercuryTheme.FailureColor);

                    SetState(
                        lblTamperState,
                        "CLEAN",
                        MercuryTheme.SuccessColor);

                    SetStatusIndicator(
                        picIntegrityCheck,
                        true);

                    SetStatusIndicator(
                        picReplayCheck,
                        false);

                    SetStatusIndicator(
                        picTamperCheck,
                        true);

                    break;

                case DemoAttackMode.WrongKey:
                    SetState(
                        lblIntegrityState,
                        "AUTH FAILED",
                        MercuryTheme.FailureColor);

                    SetState(
                        lblReplayState,
                        "ACTIVE",
                        MercuryTheme.SuccessColor);

                    SetState(
                        lblTamperState,
                        "CLEAN",
                        MercuryTheme.SuccessColor);

                    SetStatusIndicator(
                        picIntegrityCheck,
                        false);

                    SetStatusIndicator(
                        picReplayCheck,
                        true);

                    SetStatusIndicator(
                        picTamperCheck,
                        true);

                    break;

                default:
                    SetState(
                        lblIntegrityState,
                        "FAILED",
                        MercuryTheme.FailureColor);

                    SetState(
                        lblReplayState,
                        "UNKNOWN",
                        MercuryTheme.WarningColor);

                    SetState(
                        lblTamperState,
                        "UNKNOWN",
                        MercuryTheme.WarningColor);

                    SetStatusIndicator(
                        picIntegrityCheck,
                        false);

                    SetStatusIndicator(
                        picReplayCheck,
                        false);

                    SetStatusIndicator(
                        picTamperCheck,
                        false);

                    break;
            }

            UpdateLastSecurityCheck();

            DisplayExchangeFlow(
                success: false,
                attackMode);
        }

        private void SetReceiveResult(string message, Color color)
        {
            lblReceiveResult.Text = message;
            lblReceiveResult.ForeColor = color;
            lblReceiveResult.BackColor = Color.FromArgb(32, color);
        }

        private static void SetState(Label label, string state, Color color)
        {
            label.Text = state;
            label.ForeColor = color;
        }

        private void UpdateLastSecurityCheck()
        {
            lblLastCheck.Text = @$"LAST CHECK: {DateTimeOffset.UtcNow:HH:mm:ss} UTC";
        }

        private void DisplayExchangeFlow(bool success, DemoAttackMode attackMode)
        {
            ResetExchangeFlow();

            if (success)
                return;

            /*
             * The frame reached the validation boundary but was not allowed
             * to continue to the Mercury receive side.
             */
            lblFlowArrow5.Image = Properties.Resources.Stop;

            lblFlowArrow5.SizeMode = PictureBoxSizeMode.Zoom;

            lblValidateEnveleope.ForeColor = MercuryTheme.FailureColor;

            lblFlowMercuryReceive.ForeColor = MercuryTheme.MutedColor;

            lblFlowMercuryReceive.Text = @"RECEIVE BLOCKED";

            switch (attackMode)
            {
                case DemoAttackMode.Tamper:
                    lblFlowTransport.ForeColor = MercuryTheme.WarningColor;

                    lblValidateEnveleope.Text = @"TAMPER DETECTED";

                    break;

                case DemoAttackMode.Replay:
                    lblValidateEnveleope.Text = @"REPLAY BLOCKED";

                    break;

                case DemoAttackMode.WrongKey:
                    lblFlowProvider.ForeColor = MercuryTheme.WarningColor;

                    lblValidateEnveleope.Text = @"AUTH FAILED";

                    break;

                default:
                    lblValidateEnveleope.Text = @"FRAME REJECTED";

                    break;
            }
        }

        private void ResetExchangeFlow()
        {
            var arrows = new[]
            {
                lblFlowArrow1,
                lblFlowArrow2,
                lblFlowArrow3,
                lblFlowArrow4,
                lblFlowArrow5
            };

            foreach (var arrow in arrows)
            {
                arrow.Image = Properties.Resources.GreenArrow;

                arrow.SizeMode = PictureBoxSizeMode.Zoom;
            }

            lblFlowProvider.ForeColor = MercuryTheme.ForeColor;

            lblFlowTransport.ForeColor = MercuryTheme.ForeColor;

            lblValidateEnveleope.ForeColor = MercuryTheme.ForeColor;

            lblFlowMercuryReceive.ForeColor = MercuryTheme.ForeColor;

            lblValidateEnveleope.Text =@"VALIDATE ENVELOPE";

            lblFlowMercuryReceive.Text = @"MERCURY RECEIVE SIDE";
        }

        /// <summary>
        /// Clears the envelope display.
        /// </summary>
        private void ClearEnvelopeDisplay()
        {
            lblEnvelopeVersionValue.Text = @"-";
            lblEnvelopeIdValue.Text = @"-";
            lblAlgorithmValue.Text = @"-";
            lblReplayTokenValue.Text = @"-";
            lblProtectedSizeValue.Text = @"-";
            lblFrameSizeValue.Text = @"-";
            lblRawPayloadValue.Text = @"-";
            
            rtbHexPreview.Clear();
        }

        /// <summary>
        /// Formats the metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>System.String.</returns>
        private static string FormatMetadata(Metadata metadata)
        {
            if (metadata.Count == 0)
                return string.Empty;

            return string.Join(Environment.NewLine,
                metadata.Select(item => $"{item.Key}: {item.Value}"));
        }

        /// <summary>
        /// Shows the received image.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="InvalidOperationException">Mercury did not return an image payload.</exception>
        private void ShowReceivedImage(ReadOnlyMemory payload, string fileName)
        {
            if (payload.IsEmpty)
                throw new InvalidOperationException(@"Mercury did not return an image payload.");

            using var window = new FileReceivedDialog(fileName, payload);

            window.ShowDialog(this);
        }

        /// <summary>
        /// Runs the on UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal void RunOnUiThread(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                Invoke(action);
                return;
            }

            action();
        }

        /// <summary>
        /// Runs the on UI thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function">The function.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException">MainWindow</exception>
        internal T RunOnUiThread<T>(
            Func<T> function)
        {
            ArgumentNullException.ThrowIfNull(function);

            if (IsDisposed || Disposing)
                throw new ObjectDisposedException(nameof(MainWindow));

            if (InvokeRequired)
                return (T)Invoke(function)!;

            return function();
        }

        /// <summary>
        /// Sets the attack selection.
        /// </summary>
        /// <param name="tamper">if set to <c>true</c> [tamper].</param>
        /// <param name="replay">if set to <c>true</c> [replay].</param>
        /// <param name="wrongKey">if set to <c>true</c> [wrong key].</param>
        internal void SetAttackSelection(bool tamper, bool replay, bool wrongKey)
        {
            tglTamperAttack.Checked = tamper;
            tglReplayAttack.Checked = replay;
            tglWrongKeyAttack.Checked = wrongKey;
        }

        #region Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RenderBorder(e.Graphics);
        }

        #endregion

        #region Static

        /// <summary>
        /// Formats the size of the file.
        /// </summary>
        /// <param name="byteCount">The byte count.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Sets the status indicator.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        private static void SetStatusIndicator(PictureBox pictureBox, bool success)
        {
            pictureBox.BackColor = success
                ? Color.FromArgb(32, MercuryTheme.SuccessColor)
                : Color.FromArgb(32, MercuryTheme.FailureColor);
        }

        /// <summary>
        /// Confirms the close.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool ConfirmClose()
        {
            using var dialog = new MercuryDialog();

            return dialog.ShowDialog(this) == DialogResult.Yes;
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        internal void CloseWindow()
            => Close();

        #endregion

        #endregion

        #region Properties and Fields

        /// <summary>
        /// The m demo controller
        /// </summary>
        private readonly DemoController? m_DemoController;

        /// <summary>
        /// Gets the event log.
        /// </summary>
        /// <value>The event log.</value>
        internal RichTextBox EventLog { get; }

        #endregion
    }
}