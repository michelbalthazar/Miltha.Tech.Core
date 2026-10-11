using Miltha.Tech.Core.Result;

namespace Miltha.Tech.Core.Helper
{
    public interface IHttpClienteInternal
    {
        Task<Result<T>> ExecuteGetRequestAsync<T>(string route);

        Task<Result<T>> ExecuteGetRequestAsync<T>(string route, Dictionary<string, string> headers);

        Task<Result<T>> ExecutePostRequestAsync<T>(string route, object content);

        Task<Result<T>> ExecutePostRequestAsync<T>(string route, object content, Dictionary<string, string> headers);

        Task<Result<T>> ExecutePutRequestAsync<T>(string route, object content, Dictionary<string, string> headers);

        Task<Result<T>> ExecutePutRequestAsync<T>(string route, object content);
    }
}
