using Microsoft.AspNetCore.Http;

namespace Miltha.Tech.Core.Http
{
    public static class HeaderDictionaryExtensions
    {
        public static string GetTenantId(this IHeaderDictionary headerDictionary) =>
            headerDictionary?[PropagationKeyHelper.TenantKey].ToString();

        public static string GetCorrelationId(this IHeaderDictionary headerDictionary) =>
            headerDictionary?[PropagationKeyHelper.CorrelationKey].ToString();

        public static string GetRequestAt(this IHeaderDictionary headerDictionary) =>
            headerDictionary?[PropagationKeyHelper.RequestAtKey].ToString();
    }
}