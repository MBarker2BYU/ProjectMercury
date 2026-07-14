using System.ComponentModel;
using Timer = System.Windows.Forms.Timer;

namespace Mercury.Demo.WinForms.Controls;

public class MercuryMarqueeLabel : Label
{
    public enum MarqueeDirection
    {
        Left,
        Right
    }

    private readonly Timer m_Timer = new Timer();
    private int m_Interval = 120;
    private MarqueeDirection m_Direction = MarqueeDirection.Left;

    public MercuryMarqueeLabel()
    {
        AutoSize = false;
        TextAlign = ContentAlignment.MiddleLeft;
        Font = new Font("Consolas", 10F, FontStyle.Bold);

        m_Timer.Interval = m_Interval;
        m_Timer.Tick += OnTimerTick;
    }

    /// <summary>
    /// Gets or sets the marquee interval.
    /// </summary>
    /// <value>The marquee interval.</value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int MarqueeInterval
    {
        get => m_Interval;
        set
        {
            m_Interval = Math.Max(10, value);
            m_Timer.Interval = m_Interval;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public MarqueeDirection Direction
    {
        get => m_Direction;
        set => m_Direction = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether [marquee enabled].
    /// </summary>
    /// <value><c>true</c> if [marquee enabled]; otherwise, <c>false</c>.</value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool MarqueeEnabled
    {
        get => m_Timer.Enabled;
        set
        {
            if (value)
            {
                m_Timer.Start();
            }
            else
            {
                m_Timer.Stop();
            }
        }
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();

        if (!DesignMode)
        {
            m_Timer.Start();
        }
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Text) || Text.Length < 2)
        {
            return;
        }

        if (m_Direction == MarqueeDirection.Left)
        {
            Text = Text[1..] + Text[0];
        }
        else
        {
            Text = Text[^1] + Text[..^1];
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            m_Timer.Tick -= OnTimerTick;
            m_Timer.Dispose();
        }

        base.Dispose(disposing);
    }
}