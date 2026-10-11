using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Miltha.Tech.Core.Result;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Miltha.Tech.Core.Http
{
    public class HttpClienteInternal : IHttpClienteInternal
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClienteInternal(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<T>> ExecuteGetRequestAsync<T>(string route)
        {
            SetAuthorizationRequest();

            var response = await _httpClient.GetAsync(route);

            return await response.ResponseReadAsStringAsync<T>();
        }

        public async Task<Result<T>> ExecuteGetRequestAsync<T>(string route, Dictionary<string, string> headers)
        {
            SetHeaders(headers);

            var response = await _httpClient.GetAsync(route);

            return await response.ResponseReadAsStringAsync<T>();
        }

        public async Task<Result<T>> ExecutePostRequestAsync<T>(string route, object content)
        {
            SetAuthorizationRequest();

            var response = await _httpClient.PostAsync(route, GetContentRequest(content));

            return await response.ResponseReadAsStringAsync<T>();
        }

        public async Task<Result<T>> ExecutePostRequestAsync<T>(string route, object content, Dictionary<string, string> headers)
        {
            SetHeaders(headers);

            var response = await _httpClient.PostAsync(route, GetContentRequest(content));

            return await response.ResponseReadAsStringAsync<T>();
        }

        public async Task<Result<T>> ExecutePutRequestAsync<T>(string route, object content)
        {
            SetAuthorizationRequest();

            var response = await _httpClient.PutAsync(route, GetContentRequest(content));

            return await response.ResponseReadAsStringAsync<T>();
        }

        public async Task<Result<T>> ExecutePutRequestAsync<T>(string route, object content, Dictionary<string, string> headers)
        {
            SetHeaders(headers);

            var response = await _httpClient.PutAsync(route, GetContentRequest(content));

            return await response.ResponseReadAsStringAsync<T>();
        }

        private void SetAuthorizationRequest()
        {
            var token = StringValues.Empty;

            var headersKey = "Authorization";

            var tryGetValue = _httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headersKey, out token);

            if (tryGetValue.GetValueOrDefault() && !_httpClient.DefaultRequestHeaders.Any(x => x.Key == headersKey))
            {
                _httpClient.DefaultRequestHeaders.Remove(headersKey);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString().Replace("Bearer", string.Empty).Replace("bearer", string.Empty).Trim());
            }
        }

        private void SetHeaders(Dictionary<string, string> headers)
        {
            if (headers is null || !headers.Any())
                return;

            foreach (var header in headers)
            {
                if (!_httpClient.DefaultRequestHeaders.Any(x => x.Key == header.Key))
                {
                    _httpClient.DefaultRequestHeaders.Remove(header.Key);
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        private static StringContent GetContentRequest(object content) =>
            new StringContent(JsonSerializer.Serialize(content, JsonOptions()), Encoding.UTF8, new MediaTypeHeaderValue("application/json"));

        private static JsonSerializerOptions JsonOptions() =>
            new(JsonSerializerDefaults.Web);
    }
}