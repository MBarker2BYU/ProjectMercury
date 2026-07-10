// ***********************************************************************
// Assembly       : Mercury.Tests
// Author           : Matthew D. Barker
// Created          : 07-02-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-09-2026
// ***********************************************************************
// <copyright file="PrimitiveTests.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Primitives;
using MercuryReadOnlyMemory = Mercury.Abstractions.Primitives.ReadOnlyMemory;

namespace Mercury.Tests;

/// <summary>
/// Class PrimitiveTests. This class cannot be inherited.
/// </summary>
public sealed class PrimitiveTests
{
    /// <summary>
    /// Defines the test method AlgorithmId_Constructor_StoresValue.
    /// </summary>
    [Fact]
    public void AlgorithmId_Constructor_StoresValue()
    {
        var algorithmId = new AlgorithmId("AES-GCM");

        Assert.Equal("AES-GCM", algorithmId.Value);
        Assert.Equal("AES-GCM", algorithmId.ToString());
    }

    /// <summary>
    /// Defines the test method AlgorithmId_ImplicitConversion_StoresValue.
    /// </summary>
    [Fact]
    public void AlgorithmId_ImplicitConversion_StoresValue()
    {
        AlgorithmId algorithmId = "ChaCha20-Poly1305";

        Assert.Equal("ChaCha20-Poly1305", algorithmId.Value);
        Assert.Equal("ChaCha20-Poly1305", algorithmId.ToString());
    }

    /// <summary>
    /// Defines the test method AlgorithmId_ToString_WithNullValue_ReturnsEmptyString.
    /// </summary>
    [Fact]
    public void AlgorithmId_ToString_WithNullValue_ReturnsEmptyString()
    {
        var algorithmId = new AlgorithmId(null!);

        Assert.Equal(string.Empty, algorithmId.ToString());
    }

    /// <summary>
    /// Defines the test method KeyId_Constructor_StoresValue.
    /// </summary>
    [Fact]
    public void KeyId_Constructor_StoresValue()
    {
        var keyId = new KeyId("sender-key-1");

        Assert.Equal("sender-key-1", keyId.Value);
        Assert.Equal("sender-key-1", keyId.ToString());
    }

    /// <summary>
    /// Defines the test method KeyId_ImplicitConversion_StoresValue.
    /// </summary>
    [Fact]
    public void KeyId_ImplicitConversion_StoresValue()
    {
        KeyId keyId = "recipient-key-1";

        Assert.Equal("recipient-key-1", keyId.Value);
        Assert.Equal("recipient-key-1", keyId.ToString());
    }

    /// <summary>
    /// Defines the test method KeyId_ToString_WithNullValue_ReturnsEmptyString.
    /// </summary>
    [Fact]
    public void KeyId_ToString_WithNullValue_ReturnsEmptyString()
    {
        var keyId = new KeyId(null!);

        Assert.Equal(string.Empty, keyId.ToString());
    }

    /// <summary>
    /// Defines the test method FrameworkVersion_Constructor_StoresMajorAndMinor.
    /// </summary>
    [Fact]
    public void FrameworkVersion_Constructor_StoresMajorAndMinor()
    {
        var version = new FrameworkVersion(2, 5);

        Assert.Equal(2, version.Major);
        Assert.Equal(5, version.Minor);
        Assert.Equal("2.5", version.ToString());
    }

    /// <summary>
    /// Defines the test method FrameworkVersion_V1_ReturnsOneZero.
    /// </summary>
    [Fact]
    public void FrameworkVersion_V1_ReturnsOneZero()
    {
        var version = FrameworkVersion.V1;

        Assert.Equal(1, version.Major);
        Assert.Equal(0, version.Minor);
        Assert.Equal("1.0", version.ToString());
    }

    /// <summary>
    /// Defines the test method Metadata_DefaultConstructor_IsEmpty.
    /// </summary>
    [Fact]
    public void Metadata_DefaultConstructor_IsEmpty()
    {
        var metadata = new Metadata();

        Assert.Empty(metadata);
    }

    /// <summary>
    /// Defines the test method Metadata_Constructor_CopiesValidSourceValues.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_Constructor_SkipsBlankKeys.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_Constructor_ConvertsNullValuesToEmptyString.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_Constructor_DefensivelyCopiesSource.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_Add_AddsValue.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_Add_SkipsBlankKey.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_Indexer_MissingKey_ReturnsEmptyString.
    /// </summary>
    [Fact]
    public void Metadata_Indexer_MissingKey_ReturnsEmptyString()
    {
        var metadata = new Metadata();

        Assert.Equal(string.Empty, metadata["missing"]);
    }

    /// <summary>
    /// Defines the test method Metadata_TryGetValue_ExistingKey_ReturnsTrueAndValue.
    /// </summary>
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

    /// <summary>
    /// Defines the test method Metadata_TryGetValue_MissingKey_ReturnsFalseAndEmptyString.
    /// </summary>
    [Fact]
    public void Metadata_TryGetValue_MissingKey_ReturnsFalseAndEmptyString()
    {
        var metadata = new Metadata();

        var found = metadata.TryGetValue("missing", out var value);

        Assert.False(found);
        Assert.Equal(string.Empty, value);
    }

    /// <summary>
    /// Defines the test method Metadata_UsesOrdinalKeyComparison.
    /// </summary>
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

    /// <summary>
    /// Defines the test method ReadOnlyMemory_Empty_HasZeroLength.
    /// </summary>
    [Fact]
    public void ReadOnlyMemory_Empty_HasZeroLength()
    {
        var memory = MercuryReadOnlyMemory.Empty;

        Assert.Equal(0, memory.Length);
        Assert.True(memory.IsEmpty);
        Assert.Empty(memory.ToArray());
    }

    /// <summary>
    /// Defines the test method ReadOnlyMemory_Constructor_CopiesInputArray.
    /// </summary>
    [Fact]
    public void ReadOnlyMemory_Constructor_CopiesInputArray()
    {
        var source = new byte[] { 1, 2, 3 };

        var memory = new MercuryReadOnlyMemory(source);

        source[0] = 99;

        Assert.Equal(new byte[] { 1, 2, 3 }, memory.ToArray());
    }

    /// <summary>
    /// Defines the test method ReadOnlyMemory_ToArray_ReturnsCopy.
    /// </summary>
    [Fact]
    public void ReadOnlyMemory_ToArray_ReturnsCopy()
    {
        var memory = new MercuryReadOnlyMemory([1, 2, 3]);

        var first = memory.ToArray();
        first[0] = 99;

        var second = memory.ToArray();

        Assert.Equal(new byte[] { 1, 2, 3 }, second);
    }

    /// <summary>
    /// Defines the test method ReadOnlyMemory_Slice_ReturnsRequestedBytes.
    /// </summary>
    [Fact]
    public void ReadOnlyMemory_Slice_ReturnsRequestedBytes()
    {
        var memory = new MercuryReadOnlyMemory(new byte[] { 10, 20, 30, 40, 50 });

        var slice = memory.Slice(1, 3);

        Assert.Equal(new byte[] { 20, 30, 40 }, slice.ToArray());
    }

    /// <summary>
    /// Defines the test method ReadOnlyMemory_Slice_WithZeroLength_ReturnsEmpty.
    /// </summary>
    [Fact]
    public void ReadOnlyMemory_Slice_WithZeroLength_ReturnsEmpty()
    {
        var memory = new MercuryReadOnlyMemory(new byte[] { 10, 20, 30 });

        var slice = memory.Slice(1, 0);

        Assert.True(slice.IsEmpty);
        Assert.Empty(slice.ToArray());
    }

    /// <summary>
    /// Defines the test method ReadOnlyMemory_Slice_InvalidRange_Throws.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="length">The length.</param>
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

    /// <summary>
    /// Defines the test method ReadOnlyMemory_ImplicitConversion_FromByteArray_CopiesData.
    /// </summary>
    [Fact]
    public void ReadOnlyMemory_ImplicitConversion_FromByteArray_CopiesData()
    {
        byte[] source = { 7, 8, 9 };

        MercuryReadOnlyMemory memory = source;

        source[0] = 99;

        Assert.Equal(new byte[] { 7, 8, 9 }, memory.ToArray());
    }
}