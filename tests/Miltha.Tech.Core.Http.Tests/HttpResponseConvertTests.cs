using System.Net;
using Miltha.Tech.Core.Http;
using Miltha.Tech.Core.Result;

namespace Miltha.Tech.Core.Http.Tests;

public class HttpResponseConvertTests
{
    private sealed record SampleDto(int Id, string Name);

    [Fact]
    public async Task ReadAsStringAsync_ShouldReturnSuccess_WhenOkWithJson()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1,\"name\":\"Miltha\"}")
        };

        // Act
        var result = await response.ResponseReadAsStringAsync<SampleDto>();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("Miltha", result.Value.Name);
    }

    [Fact]
    public async Task ReadAsStringAsync_ShouldReturnValidationError_WhenProblemDetails()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"title\":\"One or more validation errors occurred.\",\"status\":400,\"errors\":{\"name\":[\"Name is required\"]}}")
        };

        // Act
        var result = await response.ResponseReadAsStringAsync<SampleDto>();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("Name is required", result.Error.Message);
    }

    [Fact]
    public async Task ReadAsStringAsync_ShouldReturnFailure_WhenNonSuccessPlainText()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("boom")
        };

        // Act
        var result = await response.ResponseReadAsStringAsync<SampleDto>();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.Error.Type);
        Assert.Equal("boom", result.Error.Message);
    }

    [Fact]
    public async Task ReadAsStringAsync_ShouldReturnSuccess_WhenNoContent()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NoContent)
        {
            Content = new StringContent(string.Empty)
        };

        // Act
        var result = await response.ResponseReadAsStringAsync<SampleDto>();

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ReadAsStringAsync_ShouldReturnFailure_WhenResponseIsNull()
    {
        // Arrange

        // Act
        var result = await ((HttpResponseMessage)null).ResponseReadAsStringAsync<SampleDto>();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }
}