using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace Miltha.Tech.Core.Security
{
    public static class AuthorizationTokenExtensions
    {
        public static Guid GetAuthenticatedUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.GetAuthenticatedWithoutBearer();

            if (token.IsNullOrWhiteSpace())
                return Guid.Empty;

            var id = token.GetJwtSecurityToken().Claims.FirstOrDefault(x => x.Type == ClaimsExtensions.UserId)?.Value;

            return id.IsNullOrWhiteSpace() ? Guid.Empty : new Guid(id);
        }

        public static string GetAuthenticatedUserName(this IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.GetAuthenticatedWithoutBearer();

            if (token.IsNullOrWhiteSpace())
                return string.Empty;

            var id = token.GetJwtSecurityToken().Claims.FirstOrDefault(x => x.Type == ClaimsExtensions.UserName)?.Value;

            return id.IsNullOrWhiteSpace() ? string.Empty : id;
        }

        public static Guid GetAuthenticatedTenant(this IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.GetAuthenticatedWithoutBearer();

            if (token.IsNullOrWhiteSpace())
                return Guid.Empty;

            var id = token.GetJwtSecurityToken().Claims.FirstOrDefault(x => x.Type == ClaimsExtensions.TenantId)?.Value;

            return id.IsNullOrWhiteSpace() ? Guid.Empty : new Guid(id);
        }

        public static JwtSecurityToken GetJwtSecurityToken(this string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public static string GetAuthenticatedWithoutBearer(this IHttpContextAccessor httpContextAccessor) =>
             httpContextAccessor?.HttpContext?.Request?.Headers.Authorization.ToString().Replace("Bearer", string.Empty).Replace("bearer", string.Empty).Trim();
    }
}