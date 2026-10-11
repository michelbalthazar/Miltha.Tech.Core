namespace Miltha.Tech.Core.Http
{
    public static class PropagationKeyHelper
    {
        /// <summary>
        /// The HttpContext Items key used to propagate tenantId across services.
        /// </summary>
        public const string TenantKey = "__tenant_id";

        /// <summary>
        /// The header key used to propagate correlation IDs across services.
        /// </summary>
        public const string CorrelationKey = "X-Correlation-Id";

        /// <summary>
        /// The header key used to propagate request timestamp across services.
        /// </summary>
        public const string RequestAtKey = "X-RequestAt";

        /// <summary>
        /// The header key used to propagate language preference across services.
        /// </summary>
        public const string Language = "Language";

        /// <summary>
        /// The header key used to propagate request RefreshToken across services.
        /// </summary>
        public const string RefreshTokenKey = "X-Refresh-Token";
    }
}