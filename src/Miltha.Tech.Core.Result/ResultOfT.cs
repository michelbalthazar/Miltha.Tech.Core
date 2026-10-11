namespace Miltha.Tech.Core.Result;

/// <summary>
/// Represents the outcome of an operation that produces a value on success, following the Result pattern.
/// </summary>
/// <typeparam name="TValue">The type of the value produced when the operation succeeds.</typeparam>
/// <remarks>
/// <para>
/// A <see cref="Result{TValue}"/> is always in one of two mutually exclusive states: success or failure.
/// A successful result carries a <see cref="Value"/>; a failed result always carries an <see cref="Error"/>
/// and never exposes <see cref="Value"/>.
/// </para>
/// <para>
/// <b>Complete flow:</b>
/// </para>
/// <list type="number">
/// <item><description>
/// Create a result with <see cref="Success(TValue)"/> when the operation produces a value, or with
/// <see cref="Failure"/> when it does not.
/// </description></item>
/// <item><description>
/// Inspect the state through <see cref="IsSuccess"/>, <see cref="IsFailure"/>, <see cref="Value"/>, or <see cref="Error"/>.
/// </description></item>
/// <item><description>
/// Transform the value without unwrapping the result using <see cref="Map{TNext}"/>.
/// </description></item>
/// <item><description>
/// Chain steps that can themselves fail using <see cref="Bind{TNext}"/>.
/// </description></item>
/// <item><description>
/// Run side effects on success with <see cref="Tap"/> and enforce rules with <see cref="Ensure"/>.
/// </description></item>
/// <item><description>
/// Drop the value when it is no longer needed with <see cref="ToResult"/>.
/// </description></item>
/// <item><description>
/// Finish the flow with <see cref="Match{TResult}"/> to convert both branches into a single value.
/// </description></item>
/// </list>
/// <para>
/// Once a step fails, every subsequent operation short-circuits, so the value is never accessed and the
/// first <see cref="Error"/> produced in the chain is preserved.
/// </para>
/// </remarks>
public readonly record struct Result<TValue>
{
    private readonly Error _error;
    private readonly TValue _value;

    private Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
        _error = Error.None;
    }

    private Result(Error error)
    {
        if (error == Error.None)
            throw new InvalidOperationException("A failed result must contain an error.");

        IsSuccess = false;
        _value = default;
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
    /// Gets the value produced when the operation succeeded.
    /// </summary>
    /// <value>The value carried by a successful result.</value>
    /// <exception cref="InvalidOperationException">The result is a failure and therefore has no value.</exception>
    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    /// <summary>
    /// Gets the error associated with a failed result.
    /// </summary>
    /// <value>
    /// The <see cref="Result.Error"/> produced by the failure, or <see cref="Error.None"/> when the result is successful.
    /// </value>
    public Error Error => IsFailure ? _error ?? Errors.Unexpected() : Error.None;

    /// <summary>
    /// Creates a successful result that carries the specified value.
    /// </summary>
    /// <param name="value">The value produced by the operation.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> carrying <paramref name="value"/>.</returns>
    public static Result<TValue> Success(TValue value) =>
        new(value);

    /// <summary>
    /// Creates a failed result that carries the specified error.
    /// </summary>
    /// <param name="error">The error describing the failure. Cannot be <see langword="null"/> or <see cref="Error.None"/>.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> carrying <paramref name="error"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="error"/> is <see cref="Error.None"/>.</exception>
    public static Result<TValue> Failure(Error error) =>
        new(error ?? throw new ArgumentNullException(nameof(error)));

    /// <summary>
    /// Converts a value into a successful result.
    /// </summary>
    /// <param name="value">The value produced by the operation.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> carrying <paramref name="value"/>.</returns>
    public static implicit operator Result<TValue>(TValue value) =>
        Success(value);

    /// <summary>
    /// Projects both branches of the result into a single value.
    /// </summary>
    /// <typeparam name="TResult">The type of the projected value.</typeparam>
    /// <param name="onSuccess">The function invoked with the value when the result is successful.</param>
    /// <param name="onFailure">The function invoked with the <see cref="Error"/> when the result is a failure.</param>
    /// <returns>The value returned by either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
    /// <remarks>
    /// Use this method to leave the Result pipeline and produce a concrete outcome,
    /// such as an HTTP response, a DTO, or a log message.
    /// </remarks>
    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);

    /// <summary>
    /// Transforms the successful value into another value, preserving any failure.
    /// </summary>
    /// <typeparam name="TNext">The type of the transformed value.</typeparam>
    /// <param name="map">The transformation applied to the value on success.</param>
    /// <returns>
    /// A successful <see cref="Result{TNext}"/> carrying the transformed value when the current result is successful;
    /// otherwise, a failed <see cref="Result{TNext}"/> carrying the current error.
    /// </returns>
    /// <remarks>
    /// Use this method when the transformation itself cannot fail. If it can fail, use <see cref="Bind{TNext}"/>.
    /// </remarks>
    public Result<TNext> Map<TNext>(Func<TValue, TNext> map) =>
        IsSuccess ? Result<TNext>.Success(map(Value)) : Result<TNext>.Failure(Error);

    /// <summary>
    /// Chains another operation that also returns a <see cref="Result{TNext}"/>, but only when the current result is successful.
    /// </summary>
    /// <typeparam name="TNext">The type of the value produced by the chained operation.</typeparam>
    /// <param name="bind">The operation to invoke on success.</param>
    /// <returns>
    /// The result of <paramref name="bind"/> when the current result is successful; otherwise, a failed
    /// <see cref="Result{TNext}"/> carrying the current error.
    /// </returns>
    /// <remarks>
    /// Use this method when the next step in the flow can itself fail.
    /// The difference from <see cref="Map{TNext}"/> is that <c>Map</c> returns a value while <c>Bind</c> returns a result.
    /// </remarks>
    public Result<TNext> Bind<TNext>(Func<TValue, Result<TNext>> bind) =>
        IsSuccess ? bind(Value) : Result<TNext>.Failure(Error);

    /// <summary>
    /// Executes a side effect with the successful value, without changing the outcome.
    /// </summary>
    /// <param name="action">The action to execute with the value on success. It is not executed when the result is a failure.</param>
    /// <returns>The current result, unchanged.</returns>
    /// <remarks>
    /// Use this method for logging, metrics, auditing, or other side effects that must not alter the flow.
    /// </remarks>
    public Result<TValue> Tap(Action<TValue> action)
    {
        if (IsSuccess)
            action(Value);

        return this;
    }

    /// <summary>
    /// Validates a condition against the successful value and converts the result into a failure when it is not met.
    /// </summary>
    /// <param name="predicate">The condition that must be satisfied. It is not evaluated when the result is already a failure.</param>
    /// <param name="error">The error used when <paramref name="predicate"/> returns <see langword="false"/>.</param>
    /// <returns>
    /// The current result when it is already a failure or when <paramref name="predicate"/> returns <see langword="true"/>;
    /// otherwise, a failed result carrying <paramref name="error"/>.
    /// </returns>
    public Result<TValue> Ensure(Func<TValue, bool> predicate, Error error) =>
        IsFailure || predicate(Value) ? this : Failure(error);

    /// <summary>
    /// Discards the successful value and keeps only the success or failure state.
    /// </summary>
    /// <returns>
    /// <see cref="Result.Success"/> when the current result is successful; otherwise, a failed <see cref="Result"/>
    /// carrying the current error.
    /// </returns>
    /// <remarks>
    /// Use this method when the caller only needs to know whether the operation succeeded,
    /// not which value it produced.
    /// </remarks>
    public Result ToResult() =>
        IsSuccess ? Result.Success() : Result.Failure(Error);
}
