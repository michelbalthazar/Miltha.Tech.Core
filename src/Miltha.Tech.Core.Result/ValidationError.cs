namespace Miltha.Tech.Core.Result;

public sealed record ValidationError(string PropertyName, string Message, string Code = null);
