using Miltha.Tech.Core.Result;

namespace Miltha.Tech.Core.Http
{
    /// <summary>
    /// Standard error contract returned by Miltha.Tech APIs (RFC 9457 / Problem Details aligned).
    /// Built from the domain <see cref="Error"/> so the HTTP error body is consistent across all projects.
    /// </summary>
    public sealed record ApiErrorResponse
    {
        public const string ContentType = "application/problem+json";

        public const string DefaultType = "about:blank";

        /// <summary>
        /// A URI reference that identifies the problem type. Uses <c>about:blank</c> by default;
        /// the machine-readable discriminator is exposed through <see cref="Code"/>.
        /// </summary>
        public string Type { get; init; } = DefaultType;

        /// <summary>
        /// A short, human-readable summary of the problem type.
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// The HTTP status code.
        /// </summary>
        public int Status { get; init; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence.
        /// </summary>
        public string Detail { get; init; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence (request path).
        /// </summary>
        public string Instance { get; init; }

        /// <summary>
        /// Stable, machine-readable error code (e.g. <c>Login.EmailNotConfirmed</c>).
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Correlation identifier of the request (falls back to the framework trace id).
        /// </summary>
        public string TraceId { get; init; }

        /// <summary>
        /// Validation errors grouped by property name. <see langword="null"/> when there are none.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; init; }

        public static ApiErrorResponse From(Error error, string instance = null, string traceId = null)
        {
            ArgumentNullException.ThrowIfNull(error);

            return new ApiErrorResponse
            {
                Type = DefaultType,
                Title = GetTitle(error.Type),
                Status = GetStatusCode(error.Type),
                Detail = error.Message,
                Instance = instance,
                Code = error.Code,
                TraceId = traceId,
                Errors = GroupValidationErrors(error.ValidationErrors)
            };
        }

        public static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => 400,
                ErrorType.Unauthorized => 401,
                ErrorType.Forbidden => 403,
                ErrorType.NotFound => 404,
                ErrorType.Conflict => 409,
                ErrorType.Unexpected => 500,
                _ => 422
            };

        public static string GetTitle(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => "Validation failed",
                ErrorType.Unauthorized => "Unauthorized",
                ErrorType.Forbidden => "Forbidden",
                ErrorType.NotFound => "Not found",
                ErrorType.Conflict => "Conflict",
                ErrorType.Unexpected => "Unexpected error",
                _ => "Unprocessable entity"
            };

        private static IReadOnlyDictionary<string, string[]> GroupValidationErrors(IReadOnlyCollection<ValidationError> validationErrors)
        {
            if (validationErrors == null || validationErrors.Count == 0)
                return null;

            return validationErrors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(group => group.Key, group => group.Select(error => error.Message).ToArray());
        }
    }
}