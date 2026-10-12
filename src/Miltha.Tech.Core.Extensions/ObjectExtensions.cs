using System.Text;
using System.Text.Json;
using Cysharp.Web;

namespace System
{
    public static class ObjectExtensions
    {
        public static bool Equal<T>(this T value) =>
            EqualityComparer<T>.Default.Equals(value, default);

        public static string ToJson(this object value)
        {
            if (value == null)
                return null;

            return JsonSerializer.Serialize(value);
        }

        public static byte[] ToByte(this object value)
        {
            if (value == null)
                return [];

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
        }

        public static IEnumerable<T> ToListExtended<T>(this T value)
        {
            var result = new List<T>();

            if (value.Equal())
                return result;

            result.Add(value);

            return result;
        }

        public static string ToQueryString(this object value) =>
            WebSerializer.ToQueryString(value);

        public static string ToQueryStringWithUrl(this object value, string urlBase) =>
        WebSerializer.ToQueryString(urlBase, value);
    }
}