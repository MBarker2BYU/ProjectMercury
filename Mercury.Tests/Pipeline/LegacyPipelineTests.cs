using Mercury.Abstractions.Enums;
using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;
using Mercury.Tests.Support;

namespace Mercury.Tests.Pipeline;

public sealed class LegacyPipelineTests
{

#if LEGACY_LOOPBACK_TESTS

    internal const string ALPHA = @"Alpha";
    internal const string BRAVO = @"Bravo";

    /// <summary>
    /// Defines the test method BuildClient_ReturnsClient.
    /// </summary>
    [Fact]
    public void BuildClient_ReturnsClient()
    {
        var client = MercuryFactory.Instance.BuildClient(ALPHA);

        Assert.NotNull(client);
    }

    /// <summary>
    /// Defines the test method SendAsync_ThenReceiveAsync_ReturnsPayload.
    /// </summary>
    //[Fact]
    public async Task SendAsync_ThenReceiveAsync_ReturnsPayload()
    {

        var client =
            MercuryFactory.Instance.BuildClient(ALPHA);

        var cryptoContext = MercuryFactory.Instance.BuildCryptoContext(ALPHA, ALPHA);

        var expected = new byte[] { 1, 2, 3, 4 };

        await client.SendAsync(cryptoContext, new ReadOnlyMemory(expected));

        var result = await client.ReceiveAsync();

        Assert.True(result.Success);
        Assert.Equal(expected, result.Payload.ToArray());
    }

    /// <summary>
    /// Defines the test method BuildDependencies_NullProvider_ThrowsArgumentNullException.
    /// </summary>
    [Fact]
    public void BuildDependencies_NullProvider_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => MercuryFactory.Instance.BuildDependencies(
                null!,
                EnvelopeCodec.Binary,
                new QueueTransport()));
    }

#endif

}