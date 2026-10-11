using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Miltha.Tech.Core.Security
{
    /// <summary>
    /// Centralizes the JWT/token configuration used by Miltha.Tech APIs.
    /// </summary>
    public static class JwtConfig
    {
        /// <summary>
        /// Binds <see cref="JwtSettings"/>, applies the <see cref="IdentityOptions"/> password defaults
        /// and registers JWT bearer authentication with the configured signing key.
        /// </summary>
        /// <param name="services">The service collection to register into.</param>
        /// <param name="configuration">The application configuration used to read the <c>JwtSettings</c> section.</param>
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtSettings));
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtSettings.SecurityKey)]));

            services.Configure<JwtSettings>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtSettings.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtSettings.Audience)];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
                options.ExpirationOn = ToInt(jwtAppSettingOptions[nameof(JwtSettings.ExpirationOn)], 0);
                options.RefreshTokenExpiration = ToInt(jwtAppSettingOptions[nameof(JwtSettings.RefreshTokenExpiration)], 0);
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtSettings.Issuer)],
                    ValidateAudience = true,
                    ValidAudience = jwtAppSettingOptions[nameof(JwtSettings.Audience)],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        private static int ToInt(string value, int defaultValue) =>
            int.TryParse(value, out var result) ? result : defaultValue;
    }
}