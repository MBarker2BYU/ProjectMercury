using Mercury.Abstractions.Cryptograph;
using Mercury.Provider.AesGcm;

namespace Mercury.Tests.CryptoProviders;

public sealed class AesGcmCryptoProviderTests : CryptoProviderContractTests
{
    protected override string ExpectedProviderName => "aes-gcm-256";

    protected override ICryptoProvider CreateProvider(
        ISymmetricKeyProvider keyProvider)
        => new AesGcmCryptoProvider(keyProvider);
}
