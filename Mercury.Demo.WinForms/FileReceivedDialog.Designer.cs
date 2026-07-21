namespace Mercury.Demo.WinForms
{
    partial class FileReceivedDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnClose = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            picReceivedFile = new PictureBox();
            lblFileName = new Label();
            ((System.ComponentModel.ISupportInitialize)picReceivedFile).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.Font = new Font("Segoe UI", 9F);
            btnClose.ForeColor = Color.FromArgb(225, 232, 238);
            btnClose.ImageAlign = ContentAlignment.MiddleLeft;
            btnClose.Location = new Point(849, 604);
            btnClose.Name = "btnClose";
            btnClose.Padding = new Padding(10, 0, 10, 0);
            btnClose.Size = new Size(140, 36);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // picReceivedFile
            // 
            picReceivedFile.Location = new Point(12, 43);
            picReceivedFile.Name = "picReceivedFile";
            picReceivedFile.Size = new Size(977, 555);
            picReceivedFile.TabIndex = 1;
            picReceivedFile.TabStop = false;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFileName.Location = new Point(12, 9);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(146, 25);
            lblFileName.TabIndex = 2;
            lblFileName.Tag = "Received File : ";
            lblFileName.Text = "Received File : -";
            // 
            // FileReceivedDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 12, 25);
            ClientSize = new Size(1001, 652);
            Controls.Add(lblFileName);
            Controls.Add(picReceivedFile);
            Controls.Add(btnClose);
            DoubleBuffered = true;
            ForeColor = Color.Gainsboro;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FileReceivedDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Close";
            ((System.ComponentModel.ISupportInitialize)picReceivedFile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.MercuryGlassButton btnClose;
        private PictureBox picReceivedFile;
        private Label lblFileName;
    }
}