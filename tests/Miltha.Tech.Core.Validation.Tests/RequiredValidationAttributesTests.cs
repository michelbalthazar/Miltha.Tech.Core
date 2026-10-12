namespace Miltha.Tech.Core.Validation.Tests;

public class RequiredValidationAttributesTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RequiredString_ShouldBeInvalid_WhenNullOrWhiteSpace(object value)
    {
        // Arrange
        var attribute = new RequiredStringAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void RequiredString_ShouldBeValid_WhenHasValue()
    {
        // Arrange
        var attribute = new RequiredStringAttribute();

        // Act
        var valid = attribute.IsValid("Miltha");

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(-1)]
    public void RequiredInt_ShouldBeInvalid_WhenNullOrNegative(object value)
    {
        // Arrange
        var attribute = new RequiredIntAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void RequiredInt_ShouldBeValid_WhenNonNegative(object value)
    {
        // Arrange
        var attribute = new RequiredIntAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void RequiredEnum_ShouldBeInvalid_WhenUndefined()
    {
        // Arrange
        var attribute = new RequiredEnumAttribute();

        // Act
        var valid = attribute.IsValid((DayOfWeek)99);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void RequiredEnum_ShouldBeValid_WhenDefined()
    {
        // Arrange
        var attribute = new RequiredEnumAttribute();

        // Act
        var valid = attribute.IsValid(DayOfWeek.Monday);

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void RequiredCpf_ShouldBeValid_WhenCpfIsValid()
    {
        // Arrange
        var attribute = new RequiredCpfAttribute();

        // Act
        var valid = attribute.IsValid("529.982.247-25");

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void RequiredCpf_ShouldBeInvalid_WhenCpfIsInvalidOrNull()
    {
        // Arrange
        var attribute = new RequiredCpfAttribute();

        // Act
        var invalid = attribute.IsValid("111.111.111-11");
        var nullResult = attribute.IsValid(null);

        // Assert
        Assert.False(invalid);
        Assert.False(nullResult);
    }

    [Fact]
    public void RequiredCnpj_ShouldBeValid_WhenCnpjIsValid()
    {
        // Arrange
        var attribute = new RequiredCnpjAttribute();

        // Act
        var valid = attribute.IsValid("11.222.333/0001-81");

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void RequiredCnpj_ShouldBeInvalid_WhenCnpjIsInvalidOrNull()
    {
        // Arrange
        var attribute = new RequiredCnpjAttribute();

        // Act
        var invalid = attribute.IsValid("11.111.111/1111-11");
        var nullResult = attribute.IsValid(null);

        // Assert
        Assert.False(invalid);
        Assert.False(nullResult);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void RequiredBool_ShouldBeValid_WhenValueIsProvided(object value)
    {
        // Arrange
        var attribute = new RequiredBoolAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void RequiredBool_ShouldBeInvalid_WhenNull()
    {
        // Arrange
        var attribute = new RequiredBoolAttribute();

        // Act
        var valid = attribute.IsValid(null);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void RequiredDate_ShouldBeValid_WhenParsableDate()
    {
        // Arrange
        var attribute = new RequiredDateAttribute();

        // Act
        var valid = attribute.IsValid("2026-01-15");

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("not-a-date")]
    public void RequiredDate_ShouldBeInvalid_WhenNullOrUnparsable(object value)
    {
        // Arrange
        var attribute = new RequiredDateAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void RequiredGuid_ShouldBeValid_WhenNonEmptyGuid()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();

        // Act
        var valid = attribute.IsValid(Guid.NewGuid().ToString());

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void RequiredGuid_ShouldBeInvalid_WhenNullOrEmpty(object value)
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void RequiredObject_ShouldBeValid_WhenValueIsProvided()
    {
        // Arrange
        var attribute = new RequiredObjectAttribute();

        // Act
        var valid = attribute.IsValid(new object());

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void RequiredObject_ShouldBeInvalid_WhenNull()
    {
        // Arrange
        var attribute = new RequiredObjectAttribute();

        // Act
        var valid = attribute.IsValid(null);

        // Assert
        Assert.False(valid);
    }

    [Theory]
    [InlineData("1.5")]
    [InlineData("0")]
    public void RequiredDouble_ShouldBeValid_WhenNonNegative(object value)
    {
        // Arrange
        var attribute = new RequiredDoubleAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("-1")]
    public void RequiredDouble_ShouldBeInvalid_WhenNullOrNegativeOrUnparsable(object value)
    {
        // Arrange
        var attribute = new RequiredDoubleAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }

    [Theory]
    [InlineData("10.5")]
    [InlineData("0")]
    public void RequiredDecimal_ShouldBeValid_WhenNonNegative(object value)
    {
        // Arrange
        var attribute = new RequiredDecimalAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("-0.5")]
    public void RequiredDecimal_ShouldBeInvalid_WhenNullOrNegativeOrUnparsable(object value)
    {
        // Arrange
        var attribute = new RequiredDecimalAttribute();

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void MegaByteLength_ShouldBeInvalid_WhenNull()
    {
        // Arrange
        var attribute = new MegaByteLengthAttribute(1);

        // Act
        var valid = attribute.IsValid(null);

        // Assert
        Assert.False(valid);
    }

    [Fact]
    public void MegaByteLength_ShouldBeValid_WhenWithinLimit()
    {
        // Arrange
        var attribute = new MegaByteLengthAttribute(1);

        // Act
        var valid = attribute.IsValid("small");

        // Assert
        Assert.True(valid);
    }

    [Fact]
    public void MegaByteLength_ShouldBeInvalid_WhenExceedsLimit()
    {
        // Arrange
        var attribute = new MegaByteLengthAttribute(1);
        var value = new string('a', 3_000_000);

        // Act
        var valid = attribute.IsValid(value);

        // Assert
        Assert.False(valid);
    }
}