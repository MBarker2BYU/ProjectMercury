using System.Diagnostics;
using Mercury.Abstractions;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;

namespace Mercury.Demo.WinForms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
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

    }
}
