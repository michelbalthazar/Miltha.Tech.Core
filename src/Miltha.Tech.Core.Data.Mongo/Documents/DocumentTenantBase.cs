using Miltha.Tech.Core.Validation;

namespace Miltha.Tech.Data.Mongo.Documents
{
    public record DocumentTenantBase : DocumentBase
    {
        [RequiredGuid]
        public Guid TenantId { get; set; }
    }
}