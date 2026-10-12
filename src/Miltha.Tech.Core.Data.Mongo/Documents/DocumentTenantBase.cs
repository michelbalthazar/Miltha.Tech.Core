using Miltha.Tech.Core.Validation;

namespace Miltha.Tech.Core.Data.Mongo.Documents
{
    public record DocumentTenantBase : DocumentBase
    {
        [RequiredGuid]
        public Guid TenantId { get; set; }
    }
}