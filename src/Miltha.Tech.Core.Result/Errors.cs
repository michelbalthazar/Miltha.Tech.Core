namespace Miltha.Tech.Core.Result;

public static class Errors
{
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error Validation(string code, string message, IReadOnlyCollection<ValidationError> validationErrors) =>
        new(code, message, ErrorType.Validation, validationErrors);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static Error Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);

    public static Error Unexpected(string code = "Unexpected", string message = "An unexpected error occurred.") =>
        new(code, message, ErrorType.Unexpected);
}
