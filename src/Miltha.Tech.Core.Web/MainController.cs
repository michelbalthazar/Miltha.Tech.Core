using Microsoft.AspNetCore.Mvc;
using Miltha.Tech.Core.Http;
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
                return ErrorResponse(result.Error);

            return StatusCode((int)HttpStatusCode.OK, result.Value);
        }

        protected IActionResult CreatedResponse<T>(Result<T> result)
        {
            if (result.IsFailure)
                return ErrorResponse(result.Error);

            return StatusCode((int)HttpStatusCode.Created, result.Value);
        }

        protected IActionResult NoContentResponse<T>(Result<T> result)
        {
            if (result.IsFailure)
                return ErrorResponse(result.Error);

            return NoContent();
        }

        protected IActionResult OkResponse(CoreResult result)
        {
            if (result.IsFailure)
                return ErrorResponse(result.Error);

            return Ok();
        }

        protected IActionResult CreatedResponse(CoreResult result)
        {
            if (result.IsFailure)
                return ErrorResponse(result.Error);

            return StatusCode((int)HttpStatusCode.Created);
        }

        protected IActionResult NoContentResponse(CoreResult result)
        {
            if (result.IsFailure)
                return ErrorResponse(result.Error);

            return NoContent();
        }

        private IActionResult ErrorResponse(Error error)
        {
            var traceId = Request.Headers.GetCorrelationId();

            if (string.IsNullOrWhiteSpace(traceId))
                traceId = HttpContext.TraceIdentifier;

            var response = ApiErrorResponse.From(error, Request.Path.Value, traceId);

            return new ObjectResult(response)
            {
                StatusCode = response.Status,
                ContentTypes = { ApiErrorResponse.ContentType }
            };
        }
    }
}