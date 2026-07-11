using System.Diagnostics;
using System.Text;
using Mercury.Abstractions;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Transport;
using Mercury.Core.Factories;
using Mercury.Transport.InMemory;

namespace Mercury.Demo.WinForms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            //InitTransport();
        }

        private void InitTransport()
        {
            var (alphaTransport, bravoTransport) = InMemoryDuplexTransport.CreateConnectedPair();

            var alphaDependencies = MercuryFactory.Instance.BuildDependencies(null, alphaTransport);
            var bravoDependencies = MercuryFactory.Instance.BuildDependencies(null, bravoTransport);

            m_AlphaClient = MercuryFactory.Instance.BuildClient(alphaDependencies);
            m_BravoClient = MercuryFactory.Instance.BuildClient(bravoDependencies);

        }

        private IMercuryClient m_AlphaClient;
        private IMercuryClient m_BravoClient;


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

            await mercuryClient.SendAsync(new ReadOnlyMemory([1, 2, 3]));

            var result = mercuryClient.ReceiveAsync();

            return await result;
        }

        private async void btnAlphaToBravo_Click(object sender, EventArgs e)
        {
            //const string message = @"Alpha One to Bravo Actual";

            //await m_AlphaClient.SendAsync(Encoding.UTF8.GetBytes(message));

            //var mercuryResult = await m_BravoClient.ReceiveAsync();

            //Debug.WriteLine($"{Encoding.UTF8.GetString(mercuryResult.Payload.ToArray())}");

        }

        private async void btnBravoToAlpha_Click(object sender, EventArgs e)
        {
            //const string message = @"Bravo Actual to Alpha One";

            //await m_BravoClient.SendAsync(Encoding.UTF8.GetBytes(message));

            //var mercuryResult = await m_AlphaClient.ReceiveAsync();

            //Debug.WriteLine($"{Encoding.UTF8.GetString(mercuryResult.Payload.ToArray())}");
        }
    }
}
