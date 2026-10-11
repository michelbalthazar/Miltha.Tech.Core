using Microsoft.AspNetCore.Mvc;
using Miltha.Tech.Core.Result;
using CoreResult = Miltha.Tech.Core.Result.Result;
using System.Net;

namespace Miltha.Tech.Core.Web
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected MainController()
        {
        }

        protected IActionResult OkResponse<T>(Result<T> result)
        {
            if (result.IsFailure)
                return StatusCode(GetStatusCode(result.Error), result.Error);

            return StatusCode((int)HttpStatusCode.OK, result.Value);
        }

        protected IActionResult CreatedResponse<T>(Result<T> result)
        {
            if (result.IsFailure)
                return StatusCode(GetStatusCode(result.Error), result.Error);

            return StatusCode((int)HttpStatusCode.Created, result.Value);
        }

protected IActionResult NoContentResponse<T>(Result<T> result)
        {
            if (result.IsFailure)
                return StatusCode(GetStatusCode(result.Error), result.Error);

            return NoContent();
        }

        protected IActionResult OkResponse(CoreResult result)
        {
            if (result.IsFailure)
                return StatusCode(GetStatusCode(result.Error), result.Error);

            return Ok();
        }

        protected IActionResult CreatedResponse(CoreResult result)
        {
            if (result.IsFailure)
                return StatusCode(GetStatusCode(result.Error), result.Error);

            return StatusCode((int)HttpStatusCode.Created);
        }

        protected IActionResult NoContentResponse(CoreResult result)
        {
            if (result.IsFailure)
                return StatusCode(GetStatusCode(result.Error), result.Error);

            return NoContent();
        }

        private static int GetStatusCode(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => (int)HttpStatusCode.BadRequest,
                ErrorType.Unauthorized => (int)HttpStatusCode.Unauthorized,
                ErrorType.Forbidden => (int)HttpStatusCode.Forbidden,
                ErrorType.NotFound => (int)HttpStatusCode.NotFound,
                ErrorType.Conflict => (int)HttpStatusCode.Conflict,
                ErrorType.Unexpected => (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.UnprocessableEntity
            };
    }
}