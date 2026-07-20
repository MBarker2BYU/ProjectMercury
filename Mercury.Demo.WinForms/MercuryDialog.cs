using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Mercury.Demo.WinForms
{
    public partial class MercuryDialog : Form
    {
        public MercuryDialog()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RenderBorder(e.Graphics);
        }

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

    }
}
