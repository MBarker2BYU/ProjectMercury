using Mercury.Abstractions;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using Mercury.Provider.AesGcm;
using Mercury.Transport.InMemory;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Mercury.Demo.WinForms
{

    /// <summary>
    /// Class MainWindow.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainWindow : Form
    {
        internal const string ALPHA_NODE = "Alpha Node";
        internal const string BRAVO_NODE = "Bravo Node";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            InitTransport();
        }

        private void InitTransport()
        {
            var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

            var keys = new Dictionary<KeyId, byte[]>
            {
                [ALPHA_NODE] = RandomNumberGenerator.GetBytes(32),
                [BRAVO_NODE] = RandomNumberGenerator.GetBytes(32)
            };

            var keyProvider = new AesKeyProviderDictionary(keys);
            
            var alphaAesGcm = new AesGcmCryptoProvider(keyProvider);
            var bravoAesGcm = new AesGcmCryptoProvider(keyProvider);

            var alphaDependencies = MercuryFactory.Instance.BuildDependencies(alphaAesGcm, EnvelopeCodec.Json, alphaTransport);
            var bravoDependencies = MercuryFactory.Instance.BuildDependencies(bravoAesGcm, EnvelopeCodec.Json, bravoTransport);

            m_AlphaClient = MercuryFactory.Instance.BuildClient(alphaDependencies);
            m_BravoClient = MercuryFactory.Instance.BuildClient(bravoDependencies);

        }

        private IMercuryClient m_AlphaClient = null!;
        private IMercuryClient m_BravoClient = null!;


        private async void button1_Click(object sender, EventArgs e)
        {
            var result = TestClient();

            if (!result.Result.Success)
                throw new Exception(result.Result.Message);

            var value = result.Result.Payload.ToArray()[2];

            Debug.WriteLine($@"{value}");
        }

        private static async Task<IMercuryResult> TestClient()
        {
            var mercuryClient = MercuryFactory.Instance.BuildClient();
            var cryptoContext = MercuryFactory.Instance.BuildCryptoContext(ALPHA_NODE, BRAVO_NODE);

            await mercuryClient.SendAsync(cryptoContext, new ReadOnlyMemory([1, 2, 3]));

            var result = mercuryClient.ReceiveAsync();

            return await result;
        }

        private async void btnAlphaToBravo_Click(object sender, EventArgs e)
        {
            const string message = @"Alpha One to Bravo Actual";

            var cryptoContext = MercuryFactory.Instance.BuildCryptoContext(ALPHA_NODE, BRAVO_NODE);

            await m_AlphaClient.SendAsync(cryptoContext,Encoding.UTF8.GetBytes(message));

            var mercuryResult = await m_BravoClient.ReceiveAsync();

            Debug.WriteLine($"{Encoding.UTF8.GetString(mercuryResult.Payload.ToArray())}");

        }

        private async void btnBravoToAlpha_Click(object sender, EventArgs e)
        {
            const string message = @"Bravo Actual to Alpha One";

            var cryptoContext = MercuryFactory.Instance.BuildCryptoContext( BRAVO_NODE, ALPHA_NODE);

            await m_BravoClient.SendAsync(cryptoContext, Encoding.UTF8.GetBytes(message));

            var mercuryResult = await m_AlphaClient.ReceiveAsync();

            Debug.WriteLine($"{Encoding.UTF8.GetString(mercuryResult.Payload.ToArray())}");
        }
    }
}
