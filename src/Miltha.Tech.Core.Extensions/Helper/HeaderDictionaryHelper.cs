using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Helper;

namespace Miltha.Tech.Core.Extensions.Helper
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