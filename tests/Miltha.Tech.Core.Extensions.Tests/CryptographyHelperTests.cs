using Miltha.Tech.Core.Helper.Cryptography;

namespace Miltha.Tech.Core.Extensions.Tests;

public class CryptographyHelperTests
{
    [Fact]
    public void GetEncryptedString_ShouldBeDeterministic_WhenInputIsTheSame()
    {
        // Arrange
        var key = "secret";
        var value = "data";

        // Act
        var first = CryptographyHelper.GetEncryptedString(key, value);
        var second = CryptographyHelper.GetEncryptedString(key, value);

        // Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void GetEncryptedString_ShouldDiffer_WhenKeyChanges()
    {
        // Arrange

        // Act
        var first = CryptographyHelper.GetEncryptedString("key-a", "value");
        var second = CryptographyHelper.GetEncryptedString("key-b", "value");

        // Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void GetEncryptedString_ShouldReturnSha1UpperHex()
    {
        // Arrange

        // Act
        var hash = CryptographyHelper.GetEncryptedString("key", "value");

        // Assert
        Assert.Equal(40, hash.Length);

        foreach (var c in hash)
            Assert.True((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F'), $"Unexpected character '{c}'.");
    }
}