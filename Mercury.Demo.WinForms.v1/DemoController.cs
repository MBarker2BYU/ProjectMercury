using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Mercury.Abstractions;
using Mercury.Abstractions.Cryptograph;
using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Abstractions.Shared;
using Mercury.Abstractions.Transport;
using Mercury.Core.Chunking;
using Mercury.Core.Factories;
using Mercury.Demo.WinForms.v1.Interfaces;
using Mercury.Demo.WinForms.v1.Wrappers;
using Mercury.Provider.AesGcm;
using Mercury.Provider.ChaCha20;
using Mercury.Transport.InMemory;
using Mercury.Transport.Tcp;

namespace Mercury.Demo.WinForms.v1;

internal class DemoController(ConfigurationPanelControls configurationPanelControls, CommunicationsControls communicationsControls, TestPanelControls testPanelControls, TelemetryControls telemetryControls)
{
    private readonly ConfigurationPanelControls m_ConfigurationPanelControls  = configurationPanelControls;
    private readonly CommunicationsControls m_CommunicationsControls = communicationsControls;
    private readonly TestPanelControls m_TestPanelControls = testPanelControls;
    private readonly TelemetryControls m_TelemetryControls = telemetryControls;
    
    internal const string ALPHA_NODE = "Alpha Node";
    internal const string BRAVO_NODE = "Bravo Node";

    // Crypto Providers
    internal const string AES_GCM = "AES-GCM";
    internal const string CHA_CHA_20 = "ChaCha20-Poly1305";

    //Transports
    internal const string IN_MEMORY_TRANSPORT = "In-Memory";
    internal const string TCP_TRANSPORT = "TCP";

    //Clients
    private IMercuryClient? m_AlphaClient;
    private IMercuryClient? m_BravoClient;

    //Tamper Demo
    private DemoCaptureTransport? m_AlphaCaptureTransport;

    #region Events

    private async void OnApplyConfiguration(object? sender, EventArgs args)
    {
        try
        {
            await ApplyConfigurationAsync();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Configuration Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AttachEvents()
    {
        DetachEvents();

        m_ConfigurationPanelControls.ApplyConfiguration += OnApplyConfiguration;
        m_CommunicationsControls.SendRequested += OnSendRequested;
    }

    private void DetachEvents()
    {
        m_ConfigurationPanelControls.ApplyConfiguration -= OnApplyConfiguration;
        m_CommunicationsControls.SendRequested -= OnSendRequested;
    }

    #endregion

    private async void OnSendRequested(object? sender, EventArgs eventArgs)
    {
        try
        {
            if (m_AlphaClient == null ||
                m_BravoClient == null)
            {
                throw new InvalidOperationException("Apply a configuration before sending.");
            }

            var senderPayload = m_CommunicationsControls.SenderTextBox.Text;

            if (string.IsNullOrWhiteSpace(senderPayload))
            {
                throw new InvalidOperationException("The sender payload cannot be empty.");
            }

            var payload =
                Encoding.UTF8.GetBytes(senderPayload);

            var payloadSize = Encoding.UTF8.GetByteCount(senderPayload);

            m_CommunicationsControls.SenderPayloadSize.Text =
                $"Payload Size: {payloadSize:N0} bytes";

            var cryptoContext =
                MercuryFactory.Instance.BuildCryptoContext(ALPHA_NODE, BRAVO_NODE);

            IMercuryResult result;

            if (m_ConfigurationPanelControls.ChunkingEnabledCheckBox.Checked)
            {
                var chunkSize = GetChunkSizeBytes();

                await m_AlphaClient.SendChunkedAsync(cryptoContext, payload, chunkSize);

                result =
                    await m_BravoClient.ReceiveChunkedAsync();
            }
            else
            {
                await m_AlphaClient.SendAsync(cryptoContext, payload);

                result = await m_BravoClient.ReceiveAsync();
            }

            if (!result.Success)
            {
                throw new InvalidOperationException(
                    result.Message ??
                    "Mercury failed to receive the payload.");
            }

            m_CommunicationsControls.RecipientPayloadSize.Text =
                    $"Payload Size: {result.Payload.Length:N0} bytes";

            m_CommunicationsControls.RecipientTextBox.Text =
                Encoding.UTF8.GetString(result.Payload.ToArray());
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Communication Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private int GetChunkSizeBytes()
    {
        var selectedValue =
            m_ConfigurationPanelControls.ChunkSizeCombo.Text
                .Replace("KB", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim();

        if (!int.TryParse(selectedValue, out var chunkSizeKilobytes))
        {
            throw new InvalidOperationException(
                "The selected chunk size is invalid.");
        }

        return checked(chunkSizeKilobytes * 1024);
    }

    private async Task ApplyConfigurationAsync()
    {
        var keys =
            new Dictionary<KeyId, byte[]>
            {
                [ALPHA_NODE] =
                    RandomNumberGenerator.GetBytes(32),

                [BRAVO_NODE] =
                    RandomNumberGenerator.GetBytes(32)
            };

        var keyProvider =
            new SymmetricKeyProviderDictionary(keys);

        var alphaCryptoProvider = BuildCryptoProvider(keyProvider);

        var bravoCryptoProvider = BuildCryptoProvider(keyProvider);

        var (alphaTransport, bravoTransport) =  await BuildTransportAsync();

        var envelopeCodec = BuildEnvelopeCodec();

        m_AlphaCaptureTransport = new DemoCaptureTransport(alphaTransport);

        var alphaDependencies = MercuryFactory.Instance.BuildDependencies(
                alphaCryptoProvider, envelopeCodec, m_AlphaCaptureTransport);

        var bravoDependencies =
            MercuryFactory.Instance.BuildDependencies(
                bravoCryptoProvider, envelopeCodec, bravoTransport);

        m_AlphaClient =
            MercuryFactory.Instance.BuildClient(alphaDependencies);

        m_BravoClient =
            MercuryFactory.Instance.BuildClient(bravoDependencies);

        m_TelemetryControls.Reset();

        m_TelemetryControls.StatusLabel.Text = "Connected";
        m_TelemetryControls.StatusLabel.ForeColor = Color.Lime;

        m_CommunicationsControls.EnableControls();
        m_TestPanelControls.EnableControls();
    }

    public IResult Initialize()
    {
        try
        {
            var configInitResults = m_ConfigurationPanelControls.Initialize();

            if (configInitResults is { Success: false, Exception: not null })
                throw configInitResults.Exception;
            
            var commsInitResult = m_CommunicationsControls.Initialize();

            if (commsInitResult is { Success: false, Exception: not null })
                throw commsInitResult.Exception;
            
            var testPanelInitResults = m_TestPanelControls.Initialize();

            if (testPanelInitResults is { Success: false, Exception: not null })
                throw testPanelInitResults.Exception;

            var telemetryInitResults = m_TelemetryControls.Initialize();

            if (telemetryInitResults is { Success: false, Exception: not null })
                throw telemetryInitResults.Exception;

            AttachEvents();

            return Result.Successful;

        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }

    private ICryptoProvider BuildCryptoProvider(SymmetricKeyProviderDictionary keys)
    {
        switch (m_ConfigurationPanelControls.CryptoProviderCombo.Text)
        {
            case AES_GCM:
                return new AesGcmCryptoProvider(keys);
            case CHA_CHA_20:
                return new ChaCha20CryptoProvider(keys);
            default:
                throw new ArgumentException("CryptoProvider");
        }
    }

    private async Task<(ITransport alphaTransport, ITransport bravoTransport)> BuildTransportAsync(CancellationToken cancellationToken = default)
    {
        switch (m_ConfigurationPanelControls.TransportCombo.Text)
        {
            case IN_MEMORY_TRANSPORT:
            {
                return InMemoryDuplexTransport.CreateConnectedPair();
            }

            case TCP_TRANSPORT:
            {
                var listener =
                    new TcpListener(IPAddress.Loopback, 0);

                listener.Start();

                try
                {
                    var localEndpoint =
                        (IPEndPoint)listener.LocalEndpoint;

                    var bravoTransportTask =
                        TcpTransport.AcceptAsync(listener, cancellationToken);

                    var alphaTransportTask =
                        TcpTransport.ConnectAsync(IPAddress.Loopback.ToString(),
                            localEndpoint.Port, cancellationToken);

                    await Task.WhenAll(alphaTransportTask, bravoTransportTask)
                        .ConfigureAwait(false);

                    return (
                        await alphaTransportTask.ConfigureAwait(false),
                        await bravoTransportTask.ConfigureAwait(false));
                }
                finally
                {
                    listener.Stop();
                }
            }

            default:
                throw new ArgumentException("The selected transport is not supported.",
                    nameof(m_ConfigurationPanelControls.TransportCombo));
        }
    }

    private EnvelopeCodec BuildEnvelopeCodec()
    {
        switch (m_ConfigurationPanelControls.EnvelopeCodecCombo.Text)
        {
            case "Binary":
                return EnvelopeCodec.Binary;

            case "Json":
                return EnvelopeCodec.Json;

            default:
                throw new ArgumentException("Unsupported envelope codec.");
        }
    }
}