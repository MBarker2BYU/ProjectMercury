namespace Mercury.Demo.WinForms.Controls;

public static class ControlExtensions
{
    public static void Clear(this MercuryGlassTextBox textBox)
    {
        textBox.Text = string.Empty;
    }
}