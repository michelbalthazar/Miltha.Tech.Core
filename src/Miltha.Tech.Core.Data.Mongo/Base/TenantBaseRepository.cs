using System.Security;
using Miltha.Tech.Core.Providers.Interfaces;
using Miltha.Tech.Core.Data.Mongo.Documents;
using MongoDB.Driver;

namespace Miltha.Tech.Core.Data.Mongo.Base
{
    public abstract class TenantBaseRepository<TDocument> : BaseRepository<TDocument> where TDocument : DocumentTenantBase
    {
        protected readonly Guid _tenantId;

        protected TenantBaseRepository(IMongoDatabase database, ITenantProvider tenantProvider, string? collectionName = null) : base(database, collectionName)
        {
            if (!tenantProvider.HasTenant)
                throw new SecurityException("Tenant context is required.");

            _tenantId = tenantProvider.TenantId!.Value;
        }

        protected override FilterDefinition<TDocument> BaseFilter =>
            Builders<TDocument>.Filter.And(
                base.BaseFilter,
                Builders<TDocument>.Filter.Eq(x => x.TenantId, _tenantId));

        protected override void PrepareForInsert(TDocument document)
        {
            base.PrepareForInsert(document);
            document.TenantId = _tenantId;
        }
    }
}