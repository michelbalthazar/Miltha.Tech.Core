namespace Miltha.Tech.Core.Result;

public sealed record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    public Error(string code, string message, ErrorType type = ErrorType.Failure, IReadOnlyCollection<ValidationError> validationErrors = null)
    {
        Code = code;
        Message = message;
        Type = type;
        ValidationErrors = validationErrors ?? [];
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
}
