namespace Miltha.Tech.Core.Extensions.Tests;

public class StringAndObjectExtensionsTests
{
    [Fact]
    public void ToInt_ShouldParse_WhenValueIsNumeric()
    {
        // Arrange

        // Act
        var result = "42".ToInt();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void ToInt_ShouldUseDefault_WhenValueIsNotNumeric()
    {
        // Arrange

        // Act
        var result = "abc".ToInt(7);

        // Assert
        Assert.Equal(7, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsNullOrWhiteSpace_ShouldBeTrue_WhenNullOrWhiteSpace(string value)
    {
        // Arrange

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void RemoveDiacritics_ShouldStripAccents()
    {
        // Arrange

        // Act
        var result = "coração".RemoveDiacritics();

        // Assert
        Assert.Equal("coracao", result);
    }

    [Fact]
    public void OnlyNumbers_ShouldKeepDigitsOnly()
    {
        // Arrange

        // Act
        var result = "529.982.247-25".OnlyNumbers();

        // Assert
        Assert.Equal("52998224725", result);
    }

    [Fact]
    public void ToEnum_ShouldParse_WhenValueExists()
    {
        // Arrange

        // Act
        var result = "Monday".ToEnum<DayOfWeek>();

        // Assert
        Assert.Equal(DayOfWeek.Monday, result);
    }

    [Fact]
    public void ToObject_ShouldDeserializeJson()
    {
        // Arrange
        var json = "{\"name\":\"Miltha\"}";

        // Act
        var result = json.ToObject<SampleDto>();

        // Assert
        Assert.Equal("Miltha", result.Name);
    }

    [Fact]
    public void ToJson_ShouldSerializeObject()
    {
        // Arrange
        var value = new SampleDto { Name = "Miltha" };

        // Act
        var json = value.ToJson();

        // Assert
        Assert.Contains("Miltha", json);
    }

    [Fact]
    public void ToQueryString_ShouldSerializeQuery()
    {
        // Arrange
        var value = new SampleDto { Name = "Miltha" };

        // Act
        var query = value.ToQueryString();

        // Assert
        Assert.Contains("Miltha", query);
    }

    [Fact]
    public void Base64Encoding_ShouldRoundTrip()
    {
        // Arrange
        const string original = "Miltha.Tech";

        // Act
        var encoded = original.EncodeToBase64();
        var decoded = encoded.DecodeFromBase64();

        // Assert
        Assert.Equal(original, decoded);
    }

    [Fact]
    public void ToIntAsString_ShouldConvertEnum()
    {
        // Arrange

        // Act
        var result = DayOfWeek.Monday.ToIntAsString();

        // Assert
        Assert.Equal("1", result);
    }

    private sealed class SampleDto
    {
        public string Name { get; set; }
    }
}