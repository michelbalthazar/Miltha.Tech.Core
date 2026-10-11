using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Http;
using Miltha.Tech.Core.Providers;

namespace Miltha.Tech.Core.MultiTenancy.Tests;

public class HttpContextTenantProviderTests
{
    private static HttpContextTenantProvider CreateProvider(HttpContext context)
    {
        var accessor = new HttpContextAccessor { HttpContext = context };
        return new HttpContextTenantProvider(accessor);
    }

    [Fact]
    public void TenantId_ShouldBeNull_WhenThereIsNoRequestContext()
    {
        // Arrange
        var provider = CreateProvider(null);

        // Act
        var tenantId = provider.TenantId;

        // Assert
        Assert.Null(tenantId);
        Assert.False(provider.HasTenant);
    }

    [Fact]
    public void SetTenant_ShouldMakeTenantAvailable()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var provider = CreateProvider(context);
        var tenantId = Guid.NewGuid();

        // Act
        provider.SetTenant(tenantId);

        // Assert
        Assert.Equal(tenantId, provider.TenantId);
        Assert.True(provider.HasTenant);
        Assert.Equal(tenantId.ToString(), context.Items[PropagationKeyHelper.TenantKey].ToString());
    }

    [Fact]
    public void TenantId_ShouldBeNull_WhenStoredValueIsInvalid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Items[PropagationKeyHelper.TenantKey] = "not-a-guid";
        var provider = CreateProvider(context);

        // Act
        var tenantId = provider.TenantId;

        // Assert
        Assert.Null(tenantId);
        Assert.False(provider.HasTenant);
    }

    [Fact]
    public void SetTenant_ShouldThrow_WhenHttpContextIsUnavailable()
    {
        // Arrange
        var provider = CreateProvider(null);

        // Act
        var action = () => provider.SetTenant(Guid.NewGuid());

        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }
}