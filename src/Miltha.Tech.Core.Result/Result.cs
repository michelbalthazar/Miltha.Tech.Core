namespace Miltha.Tech.Core.Result;

/// <summary>
/// Represents the outcome of an operation that does not produce a value, following the Result pattern.
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="Result"/> is always in one of two mutually exclusive states: success or failure.
/// A successful result carries no payload; a failed result always carries an <see cref="Result.Error"/>.
/// </para>
/// <para>
/// <b>Complete flow:</b>
/// </para>
/// <list type="number">
/// <item><description>
/// Create a result with <see cref="Success"/> when the operation completes successfully, or with
/// <see cref="Failure"/> when it does not.
/// </description></item>
/// <item><description>
/// Inspect the state through <see cref="IsSuccess"/>, <see cref="IsFailure"/>, or <see cref="Error"/>.
/// </description></item>
/// <item><description>
/// Transform or compose the outcome without unwrapping it using <see cref="Bind(Func{Result})"/>,
/// <see cref="Tap"/>, and <see cref="Ensure"/>.
/// </description></item>
/// <item><description>
/// Finish the flow with <see cref="Match{TResult}"/> to convert both branches into a single value,
/// for example an HTTP response or a DTO.
/// </description></item>
/// </list>
/// <para>
/// Operations that must remain in the failure short-circuit path never execute their callbacks,
/// so the first <see cref="Error"/> produced in the chain is preserved.
/// </para>
/// </remarks>
public readonly record struct Result
{
    private readonly Error _error;

    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("A successful result cannot contain an error.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("A failed result must contain an error.");

        IsSuccess = isSuccess;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    /// <value><see langword="true"/> when the operation succeeded; otherwise, <see langword="false"/>.</value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <value><see langword="true"/> when the operation failed; otherwise, <see langword="false"/>.</value>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error associated with a failed result.
    /// </summary>
    /// <value>
    /// The <see cref="Result.Error"/> produced by the failure, or <see cref="Error.None"/> when the result is successful.
    /// </value>
    public Error Error => IsFailure ? _error ?? Errors.Unexpected() : Error.None;

    /// <summary>
    /// Creates a successful result that carries no error.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>.</returns>
    public static Result Success() =>
        new(true, Error.None);

    /// <summary>
    /// Creates a failed result that carries the specified error.
    /// </summary>
    /// <param name="error">The error describing the failure. Cannot be <see langword="null"/> or <see cref="Error.None"/>.</param>
    /// <returns>A failed <see cref="Result"/> carrying <paramref name="error"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="error"/> is <see cref="Error.None"/>.</exception>
    public static Result Failure(Error error) =>
        new(false, error ?? throw new ArgumentNullException(nameof(error)));

    /// <summary>
    /// Projects both branches of the result into a single value.
    /// </summary>
    /// <typeparam name="TResult">The type of the projected value.</typeparam>
    /// <param name="onSuccess">The function invoked when the result is successful.</param>
    /// <param name="onFailure">The function invoked with the <see cref="Error"/> when the result is a failure.</param>
    /// <returns>The value returned by either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
    /// <remarks>
    /// Use this method to leave the Result pipeline and produce a concrete outcome,
    /// such as an HTTP status code, a DTO, or a log message.
    /// </remarks>
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error);

    /// <summary>
    /// Chains another operation that also returns a <see cref="Result"/>, but only when the current result is successful.
    /// </summary>
    /// <param name="bind">The operation to invoke on success.</param>
    /// <returns>
    /// The result of <paramref name="bind"/> when the current result is successful; otherwise, the current failure is preserved.
    /// </returns>
    /// <remarks>
    /// Use this overload to continue a flow of operations that do not produce a value.
    /// </remarks>
    public Result Bind(Func<Result> bind) =>
        IsSuccess ? bind() : this;

    /// <summary>
    /// Chains another operation that returns a <see cref="Result{TValue}"/>, but only when the current result is successful.
    /// </summary>
    /// <typeparam name="TValue">The type of the value produced by the chained operation.</typeparam>
    /// <param name="bind">The operation to invoke on success.</param>
    /// <returns>
    /// The result of <paramref name="bind"/> when the current result is successful; otherwise, a failed
    /// <see cref="Result{TValue}"/> carrying the current error.
    /// </returns>
    /// <remarks>
    /// Use this overload when the next step in the flow produces a value.
    /// </remarks>
    public Result<TValue> Bind<TValue>(Func<Result<TValue>> bind) =>
        IsSuccess ? bind() : Result<TValue>.Failure(Error);

    /// <summary>
    /// Executes a side effect when the result is successful, without changing the outcome.
    /// </summary>
    /// <param name="action">The action to execute on success. It is not executed when the result is a failure.</param>
    /// <returns>The current result, unchanged.</returns>
    /// <remarks>
    /// Use this method for logging, metrics, auditing, or other side effects that must not alter the flow.
    /// </remarks>
    public Result Tap(Action action)
    {
        if (IsSuccess)
            action();

        return this;
    }

    /// <summary>
    /// Validates a condition and converts the result into a failure when the condition is not met.
    /// </summary>
    /// <param name="predicate">The condition that must be satisfied. It is not evaluated when the result is already a failure.</param>
    /// <param name="error">The error used when <paramref name="predicate"/> returns <see langword="false"/>.</param>
    /// <returns>
    /// The current result when it is already a failure or when <paramref name="predicate"/> returns <see langword="true"/>;
    /// otherwise, a failed result carrying <paramref name="error"/>.
    /// </returns>
    public Result Ensure(Func<bool> predicate, Error error) =>
        IsFailure || predicate() ? this : Failure(error);
}
