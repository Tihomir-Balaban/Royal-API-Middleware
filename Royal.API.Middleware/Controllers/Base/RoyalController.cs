using System.Net;

namespace Royal.API.Middleware.Controllers.Base
{
    public class RoyalController : Controller
    {
        internal RoyalController() : base() { }
        private protected ActionResult<T> HttpStatusCodeResolve<T>(T responseObject, HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(responseObject);
                case HttpStatusCode.BadRequest:
                    return BadRequest(responseObject);
                case HttpStatusCode.NoContent:
                    return NoContent();
                case HttpStatusCode.NotFound:
                    return NotFound(responseObject);
                case HttpStatusCode.InternalServerError:
                default:
                    return StatusCode((int)statusCode);
            }
        }
    }
}
