namespace Mercury.Demo.WinForms
{
    partial class MercuryDialog
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
            btnYes = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            btnNo = new Mercury.Demo.WinForms.Controls.MercuryGlassButton();
            picLogo = new PictureBox();
            txt = new TextBox();
            lblMotto = new Label();
            lblQuestion = new Label();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // btnYes
            // 
            btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnYes.BackColor = Color.Transparent;
            btnYes.Font = new Font("Segoe UI", 9F);
            btnYes.ForeColor = Color.FromArgb(225, 232, 238);
            btnYes.ImageAlign = ContentAlignment.MiddleLeft;
            btnYes.Location = new Point(194, 214);
            btnYes.Name = "btnYes";
            btnYes.Padding = new Padding(10, 0, 10, 0);
            btnYes.Size = new Size(140, 36);
            btnYes.TabIndex = 0;
            btnYes.Text = "Yes";
            btnYes.UseVisualStyleBackColor = false;
            btnYes.Click += btnYes_Click;
            // 
            // btnNo
            // 
            btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNo.BackColor = Color.Transparent;
            btnNo.Font = new Font("Segoe UI", 9F);
            btnNo.ForeColor = Color.FromArgb(225, 232, 238);
            btnNo.ImageAlign = ContentAlignment.MiddleLeft;
            btnNo.Location = new Point(340, 214);
            btnNo.Name = "btnNo";
            btnNo.Padding = new Padding(10, 0, 10, 0);
            btnNo.Size = new Size(140, 36);
            btnNo.TabIndex = 1;
            btnNo.Text = "No";
            btnNo.UseVisualStyleBackColor = false;
            btnNo.Click += btnNo_Click;
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.MercuryLogo_256x256;
            picLogo.Location = new Point(12, 12);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(175, 175);
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.TabIndex = 2;
            picLogo.TabStop = false;
            // 
            // txt
            // 
            txt.BackColor = Color.FromArgb(0, 12, 25);
            txt.BorderStyle = BorderStyle.None;
            txt.Font = new Font("Segoe UI Semibold", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt.ForeColor = Color.Gainsboro;
            txt.Location = new Point(194, 13);
            txt.Name = "txt";
            txt.Size = new Size(286, 64);
            txt.TabIndex = 3;
            txt.Text = "Mercury";
            txt.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMotto
            // 
            lblMotto.ForeColor = Color.FromArgb(0, 135, 191);
            lblMotto.Location = new Point(194, 80);
            lblMotto.Name = "lblMotto";
            lblMotto.Size = new Size(286, 25);
            lblMotto.TabIndex = 4;
            lblMotto.Text = "MODULAR.  SECURE.  EXTENSIBLE.";
            lblMotto.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblQuestion
            // 
            lblQuestion.AutoSize = true;
            lblQuestion.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQuestion.ForeColor = Color.Gainsboro;
            lblQuestion.Location = new Point(235, 151);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(223, 25);
            lblQuestion.TabIndex = 5;
            lblQuestion.Text = "Exit The Mercury Demo?";
            // 
            // MercuryDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 12, 25);
            ClientSize = new Size(492, 262);
            Controls.Add(lblQuestion);
            Controls.Add(lblMotto);
            Controls.Add(txt);
            Controls.Add(picLogo);
            Controls.Add(btnNo);
            Controls.Add(btnYes);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "MercuryDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MercuryDialog";
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.MercuryGlassButton btnYes;
        private Controls.MercuryGlassButton btnNo;
        private PictureBox picLogo;
        private TextBox txt;
        private Label lblMotto;
        private Label lblQuestion;
    }
}