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

            mgcCryptoProvider.Items.Add("AES-GCM");
            mgcCryptoProvider.Items.Add("ChaCha20-Poly1305");

            mgcTransport.Items.Add("In-Memory");
            mgcTransport.Items.Add("TCP");

            mgcEnvelopeCodec.Items.Add("Binary");
            mgcEnvelopeCodec.Items.Add("Json");

            mgcChunkSize.Items.Add("1,024 Bytes");
            mgcChunkSize.Items.Add("512 Bytes");
            mgcChunkSize.Items.Add("256 Bytes");
            mgcChunkSize.Items.Add("128 Bytes");

            mgcLogging.Items.Add("Verbose");
            mgcLogging.Items.Add("Simple");

        }
        
    }
}
