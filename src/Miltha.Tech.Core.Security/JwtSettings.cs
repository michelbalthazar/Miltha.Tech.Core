using Microsoft.IdentityModel.Tokens;

namespace Miltha.Tech.Core.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public int ExpirationOn { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}