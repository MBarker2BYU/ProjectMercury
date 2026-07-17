// ***********************************************************************
// Assembly     : Mercury.Core.WinForms
// Author         : Matthew D. Barker
// Created        : 07-14-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-14-2026
// ***********************************************************************
// <copyright file="MercuryMarqueeLabel.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.ComponentModel;
using Timer = System.Windows.Forms.Timer;

namespace Mercury.Demo.WinForms.v1.Controls;

/// <summary>
/// Class MercuryMarqueeLabel.
/// Implements the <see cref="System.Windows.Forms.Label" />
/// </summary>
/// <seealso cref="System.Windows.Forms.Label" />
public class MercuryMarqueeLabel : Label
{
    /// <summary>
    /// Enum MarqueeDirection
    /// </summary>
    public enum MarqueeDirection
    {
        /// <summary>
        /// The left
        /// </summary>
        Left,
        /// <summary>
        /// The right
        /// </summary>
        Right
    }

    /// <summary>
    /// The m timer
    /// </summary>
    private readonly Timer m_Timer = new Timer();
    /// <summary>
    /// The m interval
    /// </summary>
    private int m_Interval = 120;
    /// <summary>
    /// The m direction
    /// </summary>
    private MarqueeDirection m_Direction = MarqueeDirection.Left;

    /// <summary>
    /// Initializes a new instance of the <see cref="MercuryMarqueeLabel"/> class.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the direction.
    /// </summary>
    /// <value>The direction.</value>
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

    /// <summary>
    /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.
    /// </summary>
    protected override void OnCreateControl()
    {
        base.OnCreateControl();

        if (!DesignMode)
        {
            m_Timer.Start();
        }
    }

    /// <summary>
    /// Handles the <see cref="E:TimerTick" /> event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Label" /> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
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