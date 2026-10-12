using Miltha.Tech.Core.Extensions;
using System.Globalization;
using System.Security;
using System.Text;
using System.Text.Json;

namespace Miltha.Tech.Core.Extensions
{
    public static class StringExtensions
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static string DecodeFromBase64(this string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return null;

            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }

        public static string EncodeToBase64(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static bool IsCnpj(this string value) => value != null && BrazilDocumentValidator.IsValidCnpj(value);

        public static bool IsCpf(this string value) => value != null && BrazilDocumentValidator.IsValidCpf(value.OnlyNumbers());

        public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

        public static bool IsValid<T>(this string currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue))
                return false;

            if (!Enum.TryParse(typeof(T), currentValue, out _))
                return false;

            return true;
        }

        public static string OnlyLettersAndNumbers(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return string.Join(string.Empty, value.Where(x => char.IsLetterOrDigit(x)));
        }

        public static string OnlyNumbers(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return string.Join(string.Empty, value.Where(x => char.IsDigit(x)));
        }

        public static string RemoveDiacritics(this string s) =>
                     string.Concat(s.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Normalize(NormalizationForm.FormC);

        public static void SetHeaders(this Dictionary<string, string> headers, HttpClient httpClient)
        {
            if (headers is null || !headers.Any())
                return;

            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Remove(header.Key);
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        public static byte[] ToBytes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return [];

            return Encoding.UTF8.GetBytes(value);
        }

        public static char? ToChar(this string stringValue, int indexFromString)
        {
            if (string.IsNullOrWhiteSpace(stringValue) || stringValue.Length <= indexFromString)
                return null;

            return stringValue[indexFromString];
        }

        public static DateTime? ToDateTime(this string currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue))
                return null;

            if (DateTime.TryParse(currentValue, CultureInfo.CurrentUICulture, out var result))
                return result;

            return null;
        }

        public static T ToEnum<T>(this string currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue))
                return default;

            if (!Enum.TryParse(typeof(T), currentValue, out var result))
                return default;

            if (((Enum)result).IsValid())
                return (T)result;

            return default;
        }

        public static int ToInt(this string value, int defaultIntValue = 0)
        {
            if (int.TryParse(value, out var parsedInt))
                return parsedInt;

            return defaultIntValue;
        }

        public static long ToLong(this string value, long defaultLongValue = 0)
        {
            if (long.TryParse(value, out var parsedLong))
                return parsedLong;

            return defaultLongValue;
        }

        public static string ToMessageFormat(this string message, params string[] values)
        {
            if (message.Count(x => x == '{') == values.Length)
                return string.Format(message, values);

            return message;
        }

public static T ToObject<T>(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return default;

            return JsonSerializer.Deserialize<T>(value, _jsonSerializerOptions);
        }

        /// <summary>
        /// Returns an ASCII-safe string:
        /// - Removes diacritics (é→e, ç→c)
        /// - Keeps printable ASCII (0x20–0x7E)
        /// - Replaces remaining non-ASCII with <paramref name="replacement"/> (or drops if '\0')
        /// - Optionally collapses whitespace to single spaces
        /// </summary>
        /// <param name="input">Source string (nullable allowed)</param>
        /// <param name="collapseWhitespace">Collapse any whitespace (incl. newlines/tabs) to single spaces</param>
        /// <param name="replacement">Replacement for non-ASCII; use '\0' to drop</param>
        /// <returns>Sanitized ASCII string (null → empty)</returns>
        public static string ToSanitizeNonAscii(this string? input, bool collapseWhitespace = true, char replacement = '?')
        {
            if (string.IsNullOrEmpty(input))
                return input ?? string.Empty;

            // 1) Decompose to separate base letters from diacritics (FormD)
            var decomposed = input.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder(decomposed.Length);
            var lastWasSpace = false;

            foreach (var ch in decomposed)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                // Strip combining marks (diacritics)
                if (cat == UnicodeCategory.NonSpacingMark ||
                    cat == UnicodeCategory.SpacingCombiningMark ||
                    cat == UnicodeCategory.EnclosingMark)
                {
                    continue;
                }

                var c = ch;

                if (c <= 0x7F)
                {
                    // ASCII branch
                    if (char.IsControl(c))
                    {
                        // Convert control chars to space if collapsing; allow none by default
                        if (collapseWhitespace)
                        {
                            if (!lastWasSpace)
                            { sb.Append(' '); lastWasSpace = true; }
                        }
                        // else: drop control chars
                        continue;
                    }

                    if (char.IsWhiteSpace(c))
                    {
                        if (collapseWhitespace)
                        {
                            if (!lastWasSpace)
                            { sb.Append(' '); lastWasSpace = true; }
                        }
                        else
                        {
                            sb.Append(c);
                            lastWasSpace = char.IsWhiteSpace(c);
                        }
                    }
                    else
                    {
                        sb.Append(c);
                        lastWasSpace = false;
                    }
                }
                else
                {
                    // Non-ASCII that couldn't be reduced by diacritic removal
                    if (replacement != '\0')
                    {
                        if (collapseWhitespace && replacement == ' ')
                        {
                            if (!lastWasSpace)
                            { sb.Append(' '); lastWasSpace = true; }
                        }
                        else
                        {
                            sb.Append(replacement);
                            lastWasSpace = false;
                        }
                    }
                    // else: drop it
                }
            }

            // 2) Recompose (FormC) & trim
            return sb.ToString().Normalize(NormalizationForm.FormC).Trim();
        }

        public static bool IsValidRedirectUri(this string? redirectUri, IEnumerable<string> allowedUris)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
                return false;

            return allowedUris.Any(x =>
                string.Equals(
                    Normalize(x),
                    Normalize(redirectUri),
                    StringComparison.OrdinalIgnoreCase));
        }

        private static string Normalize(string uri)
        {
            var normalized = uri.Trim().TrimEnd('/');

            if (!normalized.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new SecurityException("Only HTTPS redirect URIs are allowed");

            return normalized;
        }
    }
}