namespace Miltha.Tech.Core.Providers.Interfaces
{
    public interface ITenantProvider
    {
        Guid? TenantId { get; }

        bool HasTenant { get; }

        void SetTenant(Guid tenantId);
    }
}