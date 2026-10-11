using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Helper;
using Miltha.Tech.Core.Providers.Interfaces;

namespace Miltha.Tech.Core.Providers
{
    public sealed class HttpContextTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? TenantId
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.Items.TryGetValue(PropagationKeyHelper.TenantKey, out var value) ?? false)
                {
                    if (Guid.TryParse(value?.ToString(), out var tenantId))
                        return tenantId == Guid.Empty ? null : tenantId;
                }

                return null;
            }
        }

        public bool HasTenant => TenantId.HasValue && TenantId.Value != Guid.Empty;

        public void SetTenant(Guid tenantId)
        {
            var httpContextAccessor = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("IHttpContextAccessor unavailable");

            httpContextAccessor.Items[PropagationKeyHelper.TenantKey] = tenantId;
        }
    }
}
