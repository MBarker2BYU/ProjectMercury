using Mercury.Demo.WinForms.v1.Controls;
using Mercury.Demo.WinForms.v1.Interfaces;

namespace Mercury.Demo.WinForms.v1;

internal sealed class TestPanelControls(MercuryGlassToggleButton replayLastFrameButton, MercuryGlassToggleButton tamperReplayTokenButton,
    MercuryGlassToggleButton tamperAuthButton, MercuryGlassToggleButton tamperPayloadButton, MercuryGlassButton clearLogButton)
{
    public IResult Initialize()
    {

        ReplayLastFrameButton.Checked = false;
        TamperReplayTokenButton.Checked = false;
        TamperAuthButton.Checked = false;
        TamperPayloadButton.Checked = false;

        return Reset();
    }

    public IResult Reset()
    {
        try
        {
            EnableControls(false);

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    public void EnableControls(bool enabled = true)
    {
        ReplayLastFrameButton.Enabled = enabled;
        TamperReplayTokenButton.Enabled = enabled;
        TamperAuthButton.Enabled = enabled;
        TamperPayloadButton.Enabled = enabled;
        ClearLogButton.Enabled = enabled;
    }


    public MercuryGlassToggleButton ReplayLastFrameButton { get; } = replayLastFrameButton;
    public MercuryGlassToggleButton TamperReplayTokenButton { get; } = tamperReplayTokenButton;
    public MercuryGlassToggleButton TamperAuthButton { get; } = tamperAuthButton;
    public MercuryGlassToggleButton TamperPayloadButton { get; } = tamperPayloadButton;
    public MercuryGlassButton ClearLogButton { get; } = clearLogButton;

}