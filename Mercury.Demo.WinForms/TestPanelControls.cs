
using Mercury.Demo.WinForms.Controls;

namespace Mercury.Demo.WinForms;

internal sealed class TestPanelControls(MercuryGlassButton replayLastFrameButton, MercuryGlassButton tamperReplayTokenButton, 
    MercuryGlassButton tamperAuthButton, MercuryGlassButton tamperPayloadButton, MercuryGlassButton clearLogButton)
{
    public (bool success, Exception? exception) Initialize()
    {
        try
        {




            return (true, null);
        }
        catch (Exception exception)
        {
            return (false, exception);
        }
    }

   
    public MercuryGlassButton ReplayLastFrameButton { get; } = replayLastFrameButton;
    public MercuryGlassButton TamperReplayTokenButton { get; } = tamperReplayTokenButton;
    public MercuryGlassButton TamperAuthButton { get; } = tamperAuthButton;
    public MercuryGlassButton TamperPayloadButton { get; } = tamperPayloadButton;
    public MercuryGlassButton ClearLogButton { get; } = clearLogButton;

}