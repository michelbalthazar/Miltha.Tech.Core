using Miltha.Tech.Core.Http;
using Miltha.Tech.Core.Result;

namespace Miltha.Tech.Core.Http.Tests
{
    public class ApiErrorResponseTests
    {
        [Theory]
        [InlineData(ErrorType.Validation, 400, "Validation failed")]
        [InlineData(ErrorType.Unauthorized, 401, "Unauthorized")]
        [InlineData(ErrorType.Forbidden, 403, "Forbidden")]
        [InlineData(ErrorType.NotFound, 404, "Not found")]
        [InlineData(ErrorType.Conflict, 409, "Conflict")]
        [InlineData(ErrorType.Unexpected, 500, "Unexpected error")]
        [InlineData(ErrorType.Failure, 422, "Unprocessable entity")]
        public void From_ShouldMapErrorType_ToStatusAndTitle(ErrorType type, int expectedStatus, string expectedTitle)
        {
            // Arrange
            var error = new Error("Sample.Code", "Sample message", type);

            // Act
            var response = ApiErrorResponse.From(error, "/api/x", "trace-1");

            // Assert
            Assert.Equal(expectedStatus, response.Status);
            Assert.Equal(expectedTitle, response.Title);
        }

        [Fact]
        public void From_ShouldMapCoreFields()
        {
            // Arrange
            var error = Errors.NotFound("Device.NotFound", "Device was not found.");

            // Act
            var response = ApiErrorResponse.From(error, "/api/devices", "trace-9");

            // Assert
            Assert.Equal("about:blank", response.Type);
            Assert.Equal("Device.NotFound", response.Code);
            Assert.Equal("Device was not found.", response.Detail);
            Assert.Equal("/api/devices", response.Instance);
            Assert.Equal("trace-9", response.TraceId);
            Assert.Null(response.Errors);
        }

        [Fact]
        public void From_ShouldGroupValidationErrors_ByPropertyName()
        {
            // Arrange
            var error = Errors.Validation("User.Invalid", "Validation failed.", [
                new ValidationError("Email", "E-mail is required."),
                new ValidationError("Email", "E-mail is invalid."),
                new ValidationError("Password", "Too short.")
            ]);

            // Act
            var response = ApiErrorResponse.From(error);

            // Assert
            Assert.NotNull(response.Errors);
            Assert.Equal(2, response.Errors!.Count);
            Assert.Equal(["E-mail is required.", "E-mail is invalid."], response.Errors["Email"]);
            Assert.Equal(["Too short."], response.Errors["Password"]);
        }

        [Fact]
        public void From_ShouldThrow_WhenErrorIsNull()
        {
            // Arrange

            // Act
            var action = () => ApiErrorResponse.From(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}