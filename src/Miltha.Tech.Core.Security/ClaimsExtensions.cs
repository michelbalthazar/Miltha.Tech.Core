namespace Miltha.Tech.Core.Security
{
    /// <summary>
    /// Claim names used by the Miltha.Tech authentication/authorization flows.
    /// </summary>
    public static class ClaimsExtensions
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string Role = "Role";
        public const string ImageUrl = "ImageUrl";
        public const string IpAddress = "IpAddress";
        public const string PhoneNumber = "PhoneNumber";
        public const string Email = "Email";
        public const string TenantId = "tenant_id";
        public const string SessionId = "session_id";
        public const string Permission = "permission";
        public const string DeviceId = "device_id";
        public const string MfaVerified = "mfa_verified";
        public const string MfaTime = "mfa_time";
        public const string TimeZone = "tz";
    }
}