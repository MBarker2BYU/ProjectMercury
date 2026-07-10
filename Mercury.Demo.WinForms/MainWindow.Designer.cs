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
            btnLoopback = new Button();
            btnAlphaToBravo = new Button();
            btnBravoToAlpha = new Button();
            SuspendLayout();
            // 
            // btnLoopback
            // 
            btnLoopback.Location = new Point(28, 69);
            btnLoopback.Name = "btnLoopback";
            btnLoopback.Size = new Size(75, 23);
            btnLoopback.TabIndex = 0;
            btnLoopback.Text = "Loopback";
            btnLoopback.UseVisualStyleBackColor = true;
            btnLoopback.Click += button1_Click;
            // 
            // btnAlphaToBravo
            // 
            btnAlphaToBravo.Location = new Point(454, 407);
            btnAlphaToBravo.Name = "btnAlphaToBravo";
            btnAlphaToBravo.Size = new Size(75, 23);
            btnAlphaToBravo.TabIndex = 1;
            btnAlphaToBravo.Text = "ToBravo";
            btnAlphaToBravo.UseVisualStyleBackColor = true;
            btnAlphaToBravo.Click += btnAlphaToBravo_Click;
            // 
            // btnBravoToAlpha
            // 
            btnBravoToAlpha.Location = new Point(646, 407);
            btnBravoToAlpha.Name = "btnBravoToAlpha";
            btnBravoToAlpha.Size = new Size(75, 23);
            btnBravoToAlpha.TabIndex = 2;
            btnBravoToAlpha.Text = "ToAlpha";
            btnBravoToAlpha.UseVisualStyleBackColor = true;
            btnBravoToAlpha.Click += btnBravoToAlpha_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(883, 595);
            Controls.Add(btnBravoToAlpha);
            Controls.Add(btnAlphaToBravo);
            Controls.Add(btnLoopback);
            Name = "MainWindow";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnLoopback;
        private Button btnAlphaToBravo;
        private Button btnBravoToAlpha;
    }
}
