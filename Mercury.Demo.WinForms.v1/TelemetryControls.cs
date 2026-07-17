using Mercury.Demo.WinForms.v1.Interfaces;

namespace Mercury.Demo.WinForms.v1;

internal class TelemetryControls(Label statusLabel, Label framesSentLabel, Label framesReceivedLabel, Label authFailuresLabel, Label replayAttemptsLabel, 
    Label chunkCountLabel, Label avgPayloadSizelLabel )
{

    public IResult Initialize()
        => Reset();

    public IResult Reset()
    {
        try
        {
            StatusLabel.Text = "Offline";
            StatusLabel.ForeColor = Color.FromArgb(192, 192, 0);

            FrameSentLabel.Text = "0";
            FrameReceivedLabel.Text = "0";
            AuthFailuresLabel.Text = "0";
            ReplayAttemptsLabel.Text = "0";
            ChunkCountLabel.Text = "0";
            AvgPayloadSizeLabel.Text = "0";

            return Result.Successful;
        }
        catch (Exception exception)
        {
            return Result.Failure( exception);
        }
    }

    public Label StatusLabel { get; } = statusLabel;
    public Label FrameSentLabel { get; } = framesSentLabel;
    public Label FrameReceivedLabel { get; } = framesReceivedLabel;
    public Label AuthFailuresLabel { get; } = authFailuresLabel;
    public Label ReplayAttemptsLabel { get; } = replayAttemptsLabel;
    public Label ChunkCountLabel { get; } = chunkCountLabel;
    public Label AvgPayloadSizeLabel { get; } = avgPayloadSizelLabel;

}