using Mercury.Abstractions.Cryptograph;
using Mercury.Provider.ChaCha20;

namespace Mercury.Tests.CryptoProviders;

public sealed class ChaCha20CryptoProviderTests : CryptoProviderContractTests
{
    protected override string ExpectedProviderName => "chacha20-poly1305";

    protected override ICryptoProvider CreateProvider(
        ISymmetricKeyProvider keyProvider)
        => new ChaCha20CryptoProvider(keyProvider);
}
