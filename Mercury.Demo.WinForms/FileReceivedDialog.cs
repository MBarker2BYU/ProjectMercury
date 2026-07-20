using System.Drawing.Drawing2D;

namespace Mercury.Demo.WinForms
{
    public partial class FileReceivedDialog : Form
    {
        public FileReceivedDialog(Image image, string filename)
        {
            InitializeComponent();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
