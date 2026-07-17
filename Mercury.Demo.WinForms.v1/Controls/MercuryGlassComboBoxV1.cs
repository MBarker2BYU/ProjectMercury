using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mercury.Demo.WinForms.Controls
{
    [ToolboxItem(true)]
    public partial class MercuryGlassComboBoxV1 : UserControl
    {
        private readonly ContextMenuStrip _menu = new ContextMenuStrip { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };
        private string _selectedText = "AES-GCM";

        public MercuryGlassComboBoxV1()
        {
            InitializeComponent();
            BackColor = Color.Transparent;
            Height = 34;
            Font = new Font("Segoe UI", 10f);
            Cursor = Cursors.Hand;

            _menu.RenderMode = ToolStripRenderMode.Professional;
            _menu.ItemClicked += Menu_ItemClicked;
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                Text = e.ClickedItem.Text;
                Invalidate();
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => _selectedText;
            set { _selectedText = value ?? ""; Invalidate(); }
        }

        public void AddItem(string item)
        {
            if (string.IsNullOrEmpty(item)) return;
            _menu.Items.Add(new ToolStripMenuItem(item) { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White });
        }

        public void ClearItems() => _menu.Items.Clear();

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            OpenDropdown();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            OpenDropdown();
        }

        private void OpenDropdown()
        {
            if (_menu.Items.Count > 0)
            {
                // Make sure the menu is dark too
                _menu.BackColor = Color.FromArgb(45, 45, 48);
                _menu.ForeColor = Color.White;

                // Show it
                _menu.Show(this, new Point(0, this.Height + 1));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var path = GetRoundedRectanglePath(rect, 6))
            {
                using (var brush = new SolidBrush(Color.FromArgb(45, 45, 48)))
                    e.Graphics.FillPath(brush, path);

                using (var pen = new Pen(Color.FromArgb(80, 80, 85), 1.5f))
                    e.Graphics.DrawPath(pen, path);
            }

            var textRect = new Rectangle(12, 0, Width - 40, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, Color.White,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            DrawChevron(e.Graphics);
        }

        private void DrawChevron(Graphics g)
        {
            int x = Width - 24;
            int y = Height / 2 - 1;

            Point[] points = { new Point(x - 6, y - 3), new Point(x, y + 4), new Point(x + 6, y - 3) };

            using (var pen = new Pen(Color.White, 2f))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLines(pen, points);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddLine(r.X + radius, r.Y, r.Right - radius, r.Y);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddLine(r.Right, r.Y + radius, r.Right, r.Bottom - radius);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddLine(r.Right - radius, r.Bottom, r.X + radius, r.Bottom);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}