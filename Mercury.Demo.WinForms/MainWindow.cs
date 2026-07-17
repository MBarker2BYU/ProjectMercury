using Mercury.Demo.WinForms.Controls;

namespace Mercury.Demo.WinForms
{

    /// <summary>
    /// Class MainWindow.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainWindow : Form
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            var initializeInterfaceResults = InitializeInterface();

            if (initializeInterfaceResults is { success: false, exception: not null })
                throw initializeInterfaceResults.exception;
        }

        private (bool success, Exception? exception) InitializeInterface()
        {
            try
            {

                m_DemoController = new DemoController(
                    new ConfigurationPanelControls(mgcCryptoProvider, mgcTransport, mgcEnvelopeCodec, chkChunkEnabled, mgcChunkSize, mgcLogging, mgbApplyConfiguration),
                    new CommunicationsControls(txtSender, mgbClearSender, lblPayloadSizeSend, mmlSender, txtRecipient, mgbClearRecipient, lblPayloadSizeReceive, mmlRecipient, mgbSendPayload),
                    new TestPanelControls(mgbReplayLastFrame, mgbTamperReplayToken, mgbTamperAuthTag, mgbTamperPayload, mgbClearLog),
                    new TelemetryControls(lblStatusConnection, lblStatusFramesSent, lblStatusFramesReceived, lblStatusAuthFailures, lblStatusReplayAttemps, lblStatusChunkCount, lblStatusAverageSize));

                var demoInitResults = m_DemoController.Initialize();

                if (demoInitResults is { Success: false, Exception: not null })
                    throw demoInitResults.Exception;

                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }


        private DemoController m_DemoController;
    }


}
