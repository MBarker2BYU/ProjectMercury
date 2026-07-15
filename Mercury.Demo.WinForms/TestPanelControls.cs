
using Mercury.Demo.WinForms.Controls;
using Mercury.Demo.WinForms.Interfaces;

namespace Mercury.Demo.WinForms;

internal sealed class TestPanelControls(MercuryGlassButton replayLastFrameButton, MercuryGlassButton tamperReplayTokenButton, 
    MercuryGlassButton tamperAuthButton, MercuryGlassButton tamperPayloadButton, MercuryGlassButton clearLogButton)
{
    public IResult Initialize()
        => Reset();

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


    public MercuryGlassButton ReplayLastFrameButton { get; } = replayLastFrameButton;
    public MercuryGlassButton TamperReplayTokenButton { get; } = tamperReplayTokenButton;
    public MercuryGlassButton TamperAuthButton { get; } = tamperAuthButton;
    public MercuryGlassButton TamperPayloadButton { get; } = tamperPayloadButton;
    public MercuryGlassButton ClearLogButton { get; } = clearLogButton;

}