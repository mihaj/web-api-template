using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mjc.Templates.WebApi.Core.Dtos;
using Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors;
using Swashbuckle.AspNetCore.Annotations;

namespace Mjc.Templates.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesErrorResponseType(typeof(void))]
    public class ErrorsController : ControllerBase
    {
        /// <summary>
        /// Get API errors
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetErrors))]
        [ProducesResponseType(typeof(List<ErrorDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "List Error codes",
            Description = "This endpoint will list the API error codes.",
            OperationId = "GetErrors",
            Tags = new[] { "General Endpoints" }
        )]
        public IActionResult GetErrors()
        {
            var errors = typeof(ErrorCode).GetFields()
                .Select(field =>
                new ErrorDto
                {
                    Code = ((ErrorCode)field.GetValue(null)).ToCode(),
                    Message = ((ErrorCode)field.GetValue(null)).ToString()
                });

            return Ok(errors);
        }
    }
}
