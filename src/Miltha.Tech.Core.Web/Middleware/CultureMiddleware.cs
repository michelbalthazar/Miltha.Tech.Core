using System.Globalization;
using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Helper;

namespace Miltha.Tech.Core.Web.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureHeader = context.Request.Headers[PropagationKeyHelper.Language].ToString();

            var culture = cultureHeader switch
            {
                "pt" => new CultureInfo("pt-BR"),
                "en" => new CultureInfo("en-US"),
                "es" => new CultureInfo("es-ES"),
                _ => new CultureInfo("pt-BR")
            };

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }
    }
}