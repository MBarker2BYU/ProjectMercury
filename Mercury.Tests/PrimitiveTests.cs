using Mercury.Abstractions.Primitives;
using MercuryReadOnlyMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests;

public sealed class PrimitiveTests
{
    [Fact]
    public void AlgorithmId_Constructor_StoresValue()
    {
        var algorithmId = new AlgorithmId("AES-GCM");

        Assert.Equal("AES-GCM", algorithmId.Value);
        Assert.Equal("AES-GCM", algorithmId.ToString());
    }

    [Fact]
    public void AlgorithmId_ImplicitConversion_StoresValue()
    {
        AlgorithmId algorithmId = "ChaCha20-Poly1305";

        Assert.Equal("ChaCha20-Poly1305", algorithmId.Value);
        Assert.Equal("ChaCha20-Poly1305", algorithmId.ToString());
    }

    [Fact]
    public void AlgorithmId_ToString_WithNullValue_ReturnsEmptyString()
    {
        var algorithmId = new AlgorithmId(null!);

        Assert.Equal(string.Empty, algorithmId.ToString());
    }

    [Fact]
    public void KeyId_Constructor_StoresValue()
    {
        var keyId = new KeyId("sender-key-1");

        Assert.Equal("sender-key-1", keyId.Value);
        Assert.Equal("sender-key-1", keyId.ToString());
    }

    [Fact]
    public void KeyId_ImplicitConversion_StoresValue()
    {
        KeyId keyId = "recipient-key-1";

        Assert.Equal("recipient-key-1", keyId.Value);
        Assert.Equal("recipient-key-1", keyId.ToString());
    }

    [Fact]
    public void KeyId_ToString_WithNullValue_ReturnsEmptyString()
    {
        var keyId = new KeyId(null!);

        Assert.Equal(string.Empty, keyId.ToString());
    }

    [Fact]
    public void FrameworkVersion_Constructor_StoresMajorAndMinor()
    {
        var version = new FrameworkVersion(2, 5);

        Assert.Equal(2, version.Major);
        Assert.Equal(5, version.Minor);
        Assert.Equal("2.5", version.ToString());
    }

    [Fact]
    public void FrameworkVersion_V1_ReturnsOneZero()
    {
        var version = FrameworkVersion.V1;

        Assert.Equal(1, version.Major);
        Assert.Equal(0, version.Minor);
        Assert.Equal("1.0", version.ToString());
    }

    [Fact]
    public void Metadata_DefaultConstructor_IsEmpty()
    {
        var metadata = new Metadata();

        Assert.Empty(metadata);
        Assert.Equal(0, metadata.Count);
    }

    [Fact]
    public void Metadata_Constructor_CopiesValidSourceValues()
    {
        var source = new Dictionary<string, string>
        {
            ["route"] = "alpha",
            ["correlation"] = "123"
        };

        var metadata = new Metadata(source);

        Assert.Equal(2, metadata.Count);
        Assert.Equal("alpha", metadata["route"]);
        Assert.Equal("123", metadata["correlation"]);
    }

    [Fact]
    public void Metadata_Constructor_SkipsBlankKeys()
    {
        var source = new Dictionary<string, string>
        {
            [""] = "empty",
            [" "] = "space",
            ["valid"] = "kept"
        };

        var metadata = new Metadata(source);

        Assert.Single(metadata);
        Assert.True(metadata.ContainsKey("valid"));
        Assert.False(metadata.ContainsKey(""));
        Assert.False(metadata.ContainsKey(" "));
    }

    [Fact]
    public void Metadata_Constructor_ConvertsNullValuesToEmptyString()
    {
        var source = new Dictionary<string, string>
        {
            ["null-value"] = null!
        };

        var metadata = new Metadata(source);

        Assert.Equal(string.Empty, metadata["null-value"]);
    }

    [Fact]
    public void Metadata_Constructor_DefensivelyCopiesSource()
    {
        var source = new Dictionary<string, string>
        {
            ["key"] = "original"
        };

        var metadata = new Metadata(source);

        source["key"] = "changed";

        Assert.Equal("original", metadata["key"]);
    }

    [Fact]
    public void Metadata_Add_AddsValue()
    {
        var metadata = new Metadata
        {
            { "purpose", "test" }
        };

        Assert.True(metadata.ContainsKey("purpose"));
        Assert.Equal("test", metadata["purpose"]);
    }

    [Fact]
    public void Metadata_Add_SkipsBlankKey()
    {
        var metadata = new Metadata
        {
            { "", "empty" },
            { " ", "space" },
            { "valid", "kept" }
        };

        Assert.Single(metadata);
        Assert.Equal("kept", metadata["valid"]);
    }

    [Fact]
    public void Metadata_Indexer_MissingKey_ReturnsEmptyString()
    {
        var metadata = new Metadata();

        Assert.Equal(string.Empty, metadata["missing"]);
    }

    [Fact]
    public void Metadata_TryGetValue_ExistingKey_ReturnsTrueAndValue()
    {
        var metadata = new Metadata(new Dictionary<string, string>
        {
            ["key"] = "value"
        });

        var found = metadata.TryGetValue("key", out var value);

        Assert.True(found);
        Assert.Equal("value", value);
    }

    [Fact]
    public void Metadata_TryGetValue_MissingKey_ReturnsFalseAndEmptyString()
    {
        var metadata = new Metadata();

        var found = metadata.TryGetValue("missing", out var value);

        Assert.False(found);
        Assert.Equal(string.Empty, value);
    }

    [Fact]
    public void Metadata_UsesOrdinalKeyComparison()
    {
        var metadata = new Metadata(new Dictionary<string, string>
        {
            ["Key"] = "upper"
        });

        Assert.True(metadata.ContainsKey("Key"));
        Assert.False(metadata.ContainsKey("key"));
    }

    [Fact]
    public void ReadOnlyMemory_Empty_HasZeroLength()
    {
        var memory = MercuryReadOnlyMemory.Empty;

        Assert.Equal(0, memory.Length);
        Assert.True(memory.IsEmpty);
        Assert.Empty(memory.ToArray());
    }

    [Fact]
    public void ReadOnlyMemory_Constructor_CopiesInputArray()
    {
        var source = new byte[] { 1, 2, 3 };

        var memory = new MercuryReadOnlyMemory(source);

        source[0] = 99;

        Assert.Equal(new byte[] { 1, 2, 3 }, memory.ToArray());
    }
    
    [Fact]
    public void ReadOnlyMemory_ToArray_ReturnsCopy()
    {
        var memory = new MercuryReadOnlyMemory([1, 2, 3]);

        var first = memory.ToArray();
        first[0] = 99;

        var second = memory.ToArray();

        Assert.Equal(new byte[] { 1, 2, 3 }, second);
    }

    [Fact]
    public void ReadOnlyMemory_Slice_ReturnsRequestedBytes()
    {
        var memory = new MercuryReadOnlyMemory(new byte[] { 10, 20, 30, 40, 50 });

        var slice = memory.Slice(1, 3);

        Assert.Equal(new byte[] { 20, 30, 40 }, slice.ToArray());
    }

    [Fact]
    public void ReadOnlyMemory_Slice_WithZeroLength_ReturnsEmpty()
    {
        var memory = new MercuryReadOnlyMemory(new byte[] { 10, 20, 30 });

        var slice = memory.Slice(1, 0);

        Assert.True(slice.IsEmpty);
        Assert.Empty(slice.ToArray());
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(3, 1)]
    [InlineData(2, 2)]
    public void ReadOnlyMemory_Slice_InvalidRange_Throws(int start, int length)
    {
        var memory = new MercuryReadOnlyMemory(new byte[] { 1, 2, 3 });

        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(start, length));
    }

    [Fact]
    public void ReadOnlyMemory_ImplicitConversion_FromByteArray_CopiesData()
    {
        byte[] source = { 7, 8, 9 };

        MercuryReadOnlyMemory memory = source;

        source[0] = 99;

        Assert.Equal(new byte[] { 7, 8, 9 }, memory.ToArray());
    }
}