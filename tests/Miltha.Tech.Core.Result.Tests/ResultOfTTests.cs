namespace Miltha.Tech.Core.Result.Tests;

public class ResultOfTTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResultWithValue()
    {
        // Arrange
        var value = "created";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Value_ShouldThrow_WhenResultIsFailure()
    {
        // Arrange
        var result = Result<string>.Failure(Errors.NotFound("User.NotFound", "User was not found."));

        // Act
        var action = () => result.Value;

        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void Map_ShouldTransformValue_WhenResultIsSuccessful()
    {
        // Arrange
        var result = Result<int>.Success(10);

        // Act
        var mapped = result.Map(value => value.ToString());

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("10", mapped.Value);
    }

    [Fact]
    public void Map_ShouldPreserveError_WhenResultIsFailure()
    {
        // Arrange
        var error = Errors.Conflict("User.Conflict", "User already exists.");
        var result = Result<int>.Failure(error);

        // Act
        var mapped = result.Map(value => value.ToString());

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Bind_ShouldReturnNextResult_WhenResultIsSuccessful()
    {
        // Arrange
        var result = Result<int>.Success(10);

        // Act
        var bound = result.Bind(value => Result<string>.Success(value.ToString()));

        // Assert
        Assert.True(bound.IsSuccess);
        Assert.Equal("10", bound.Value);
    }

    [Fact]
    public void Tap_ShouldExecuteAction_WhenResultIsSuccessful()
    {
        // Arrange
        var result = Result<int>.Success(10);
        var tapped = 0;

        // Act
        result.Tap(value => tapped = value);

        // Assert
        Assert.Equal(10, tapped);
    }

    [Fact]
    public void Ensure_ShouldReturnFailure_WhenPredicateIsFalse()
    {
        // Arrange
        var error = Errors.Validation("Value.Invalid", "Value is invalid.");
        var result = Result<int>.Success(10);

        // Act
        var ensured = result.Ensure(value => value > 20, error);

        // Assert
        Assert.True(ensured.IsFailure);
        Assert.Equal(error, ensured.Error);
    }

    [Fact]
    public void ImplicitOperator_ShouldCreateSuccessfulResult_WhenValueIsProvided()
    {
        // Arrange

        // Act
        Result<string> result = "created";

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("created", result.Value);
    }
}