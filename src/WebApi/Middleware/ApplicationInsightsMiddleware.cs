using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Mjc.Templates.WebApi.Core.Exceptions;
using Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors;

namespace WebApi.Middleware
{
    public class ApplicationInsightsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _tClient;

        public ApplicationInsightsMiddleware(RequestDelegate next, TelemetryClient tClient)
        {
            _next = next;
            _tClient = tClient;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var exceptionProperties = new Dictionary<string, string>
                {
                    { "RequestUrl", context.Request.Path },
                    { "RequestHeaders", JsonSerializer.Serialize(context.Request.Headers.Values) }
                };

                _tClient.TrackException(ex, exceptionProperties);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;

                if (ex.GetType() == typeof(ThirdPartyException))
                {
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new BadRequestError(ex.Message,
                        ErrorCode.VALIDATION_ERROR)));
                }
                else
                {
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new BadRequestError(ex.Message)));
                }
            }
        }
    }
}
