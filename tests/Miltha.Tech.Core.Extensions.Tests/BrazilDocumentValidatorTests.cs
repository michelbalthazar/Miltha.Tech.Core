using Miltha.Tech.Core.Extensions;

namespace Miltha.Tech.Core.Extensions.Tests;

public class BrazilDocumentValidatorTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void IsValidCpf_ShouldBeTrue_WhenCpfIsValid(string cpf)
    {
        // Arrange

        // Act
        var valid = BrazilDocumentValidator.IsValidCpf(cpf);

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-00")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValidCpf_ShouldBeFalse_WhenCpfIsInvalid(string cpf)
    {
        // Arrange

        // Act
        var valid = BrazilDocumentValidator.IsValidCpf(cpf);

        // Assert
        Assert.False(valid);
    }

    [Theory]
    [InlineData("11.222.333/0001-81")]
    [InlineData("11222333000181")]
    public void IsValidCnpj_ShouldBeTrue_WhenCnpjIsValid(string cnpj)
    {
        // Arrange

        // Act
        var valid = BrazilDocumentValidator.IsValidCnpj(cnpj);

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData("11.111.111/1111-11")]
    [InlineData("00.000.000/0000-00")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValidCnpj_ShouldBeFalse_WhenCnpjIsInvalid(string cnpj)
    {
        // Arrange

        // Act
        var valid = BrazilDocumentValidator.IsValidCnpj(cnpj);

        // Assert
        Assert.False(valid);
    }

    [Theory]
    [InlineData("12ABC34501DE45")]
    [InlineData("12abc34501de45")]
    public void IsValidCnpj_ShouldAcceptAlphanumericCnpj(string cnpj)
    {
        // Arrange

        // Act
        var valid = BrazilDocumentValidator.IsValidCnpj(cnpj);

        // Assert
        Assert.True(valid);
    }

    [Theory]
    [InlineData("12ABC34501DE44")]
    [InlineData("AAAAAAAAAAAAAA")]
    public void IsValidCnpj_ShouldBeFalse_WhenAlphanumericCnpjIsInvalid(string cnpj)
    {
        // Arrange

        // Act
        var valid = BrazilDocumentValidator.IsValidCnpj(cnpj);

        // Assert
        Assert.False(valid);
    }
}