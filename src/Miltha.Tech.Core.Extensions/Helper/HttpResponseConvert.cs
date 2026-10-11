using Microsoft.AspNetCore.Mvc;
using Miltha.Tech.Core.Result;
using System.Net;
using System.Text.Json;

namespace Miltha.Tech.Core.Extensions.Helper
{
    public static class HttpResponseConvert
    {
        private static async Task<Result<HttpResponseMessage>> ResponseValidate(HttpResponseMessage response)
        {
            if (response is null)
                return Result<HttpResponseMessage>.Failure(Errors.Validation("Http.Response.Null", "The HTTP response is null"));

            var status = response.StatusCode;
            var responseData = await response.Content.ReadAsStringAsync();

            if (status != HttpStatusCode.Accepted && status != HttpStatusCode.OK && status != HttpStatusCode.NoContent && status != HttpStatusCode.Created)
            {
                if (!string.IsNullOrWhiteSpace(responseData) && responseData.Contains("title") && responseData.Contains("status") && responseData.Contains("errors"))
                {
                    var error = JsonSerializer.Deserialize<ValidationProblemDetails>(responseData, JsonOptions());

                    return Result<HttpResponseMessage>.Failure(ToError(status, error?.Errors.FirstOrDefault().Value.FirstOrDefault() ?? "The HTTP status code of the response was not expected"));
                }
                else
                    return Result<HttpResponseMessage>.Failure(ToError(status, responseData ?? "The HTTP status code of the response was not expected"));
            }

            return Result<HttpResponseMessage>.Success(response);
        }

        public static async Task<Result<T>> ResponseReadAsStringAsync<T>(this HttpResponseMessage response)
        {
            var validate = await ResponseValidate(response);

            if (validate.IsFailure)
                return Result<T>.Failure(validate.Error);

            var responseResult = await response.Content.ReadAsStringAsync();

            return !string.IsNullOrWhiteSpace(responseResult) ?
                Result<T>.Success(JsonSerializer.Deserialize<T>(responseResult, JsonOptions())!) : Result<T>.Success(default!);
        }

        private static JsonSerializerOptions JsonOptions() =>
            new(JsonSerializerDefaults.Web);

        private static Error ToError(HttpStatusCode statusCode, string message) =>
            statusCode switch
            {
                HttpStatusCode.Unauthorized => Errors.Unauthorized($"Http.{statusCode}", message),
                HttpStatusCode.Forbidden => Errors.Forbidden($"Http.{statusCode}", message),
                HttpStatusCode.NotFound => Errors.NotFound($"Http.{statusCode}", message),
                HttpStatusCode.Conflict => Errors.Conflict($"Http.{statusCode}", message),
                HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity => Errors.Validation($"Http.{statusCode}", message),
                _ => Errors.Failure($"Http.{statusCode}", message)
            };
    }
}