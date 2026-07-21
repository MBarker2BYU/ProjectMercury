using System.Drawing.Drawing2D;
using Mercury.Abstractions.Primitives;

namespace Mercury.Demo.WinForms;

public partial class FileReceivedDialog : Form
{
    public FileReceivedDialog(string filename, ReadOnlyMemory payload)
    {
        InitializeComponent();

        ShowImage(filename, payload);
    }

    private void ShowImage(string filename, ReadOnlyMemory payload)
    {
        try
        {
            using var stream = new MemoryStream(payload.ToArray(), writable: false);

            using var image = Image.FromStream(stream, useEmbeddedColorManagement: true,
                    validateImageData: true);

            var receivedImage = new Bitmap(image);

            picReceivedFile.Image = receivedImage;

            lblFileName.Text = @$"{lblFileName.Tag}{filename}";

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

    private void btnClose_Click(object sender, EventArgs e)
    {
        if (picReceivedFile.Image != null)
        {
            picReceivedFile.Image.Dispose();
            picReceivedFile.Image = null;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
    
}

