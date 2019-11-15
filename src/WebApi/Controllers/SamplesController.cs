using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mjc.Templates.WebApi.Core.Interfaces;
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
    public class SamplesController : ControllerBase
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly IMyHttpFactory _factory;

        public SamplesController(TelemetryClient telemetryClient, IMyHttpFactory factory)
        {
            _telemetryClient = telemetryClient;
            _factory = factory;
        }

        // GET api/values
        [HttpGet(Name = nameof(Get))]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Operation summary",
            Description = "Description",
            OperationId = "Get",
            Tags = new[] { "General" }
        )]
        public ActionResult<IEnumerable<string>> Get()
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>(nameof(Get)))
            {
                return Ok(new string[] { "value1", "value2" });
            }
        }

        // GET api/values/5
        [HttpGet("{keyword}", Name = nameof(GetByKeyword))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Operation summary",
            Description = "Description",
            OperationId = "GetByKeyword",
            Tags = new[] { "General" }
        )]
        public async Task<IActionResult> GetByKeyword(string keyword)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>(nameof(GetByKeyword)))
            {
                //TODO: do not call HTTP clietn directly (create service)
                var result = await _factory.Demo1Async(keyword);
                return Ok(result);
            }
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public void Delete(int id)
        {
        }
    }
}
