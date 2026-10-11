using Miltha.Tech.Core.Result;

namespace Miltha.Tech.Core.Result.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Arrange

        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WhenErrorIsProvided()
    {
        // Arrange
        var error = Errors.NotFound("User.NotFound", "User was not found.");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_ShouldThrow_WhenErrorIsNone()
    {
        // Arrange

        // Act
        var action = () =>
        {
            _ = Result.Failure(Error.None);
        };

        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void Match_ShouldExecuteSuccessCallback_WhenResultIsSuccessful()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var value = result.Match(() => "success", error => error.Message);

        // Assert
        Assert.Equal("success", value);
    }

    [Fact]
    public void Ensure_ShouldReturnFailure_WhenPredicateIsFalse()
    {
        // Arrange
        var error = Errors.Validation("Rule.Invalid", "Rule is invalid.");
        var result = Result.Success();

        // Act
        var ensured = result.Ensure(() => false, error);

        // Assert
        Assert.True(ensured.IsFailure);
        Assert.Equal(error, ensured.Error);
    }
}
