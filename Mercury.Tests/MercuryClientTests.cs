using Mercury.Abstractions.Primitives;
using Mercury.Core.Factories;

namespace Mercury.Tests;
/// <summary>
/// Class MercuryClientTests.
/// </summary>
public class MercuryClientTests
{
    /// <summary>
    /// Defines the test method BuildClient_ReturnsClient.
    /// </summary>
    [Fact]
    public void BuildClient_ReturnsClient()
    {
        var client =
            MercuryFactory.Instance.BuildClient();

        Assert.NotNull(client);
    }

    /// <summary>
    /// Defines the test method SendAsync_ThenReceiveAsync_ReturnsPayload.
    /// </summary>
    [Fact]
    public async Task SendAsync_ThenReceiveAsync_ReturnsPayload()
    {
        var client =
            MercuryFactory.Instance.BuildClient();
        var cryptoContext = MercuryFactory.Instance.BuildCryptoContext("Alpha", "Bravo");

        var expected = new byte[] { 1, 2, 3, 4 };

        await client.SendAsync(cryptoContext, new ReadOnlyMemory(expected));

        var result =
            await client.ReceiveAsync();

        Assert.True(result.Success);
        Assert.Equal(expected, result.Payload.ToArray());
    }
}