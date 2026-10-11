using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miltha.Tech.Core.Security;

namespace Miltha.Tech.Core.Security.Tests;

public class JwtTests
{
    [Fact]
    public void AddJwtConfiguration_ShouldBindJwtSettings_FromConfiguration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["JwtSettings:Issuer"] = "miltha",
                ["JwtSettings:Audience"] = "clients",
                ["JwtSettings:SecurityKey"] = "very-long-secret-key-for-hmac-sha512-signing-0123456789abcdef0123456789",
                ["JwtSettings:ExpirationOn"] = "20",
                ["JwtSettings:RefreshTokenExpiration"] = "30"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddOptions();
        services.AddJwtConfiguration(configuration);

        using var provider = services.BuildServiceProvider();

        // Act
        var settings = provider.GetRequiredService<IOptions<JwtSettings>>().Value;

        // Assert
        Assert.Equal("miltha", settings.Issuer);
        Assert.Equal("clients", settings.Audience);
        Assert.Equal(20, settings.ExpirationOn);
        Assert.Equal(30, settings.RefreshTokenExpiration);
        Assert.NotNull(settings.SigningCredentials);
    }

    [Fact]
    public void JwtSettings_ShouldAllowSettingProperties()
    {
        // Arrange
        var settings = new JwtSettings();

        // Act
        settings.Issuer = "issuer";
        settings.Audience = "audience";
        settings.ExpirationOn = 5;
        settings.RefreshTokenExpiration = 10;

        // Assert
        Assert.Equal("issuer", settings.Issuer);
        Assert.Equal("audience", settings.Audience);
        Assert.Equal(5, settings.ExpirationOn);
        Assert.Equal(10, settings.RefreshTokenExpiration);
    }
}