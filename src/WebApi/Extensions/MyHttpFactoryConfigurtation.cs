using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mjc.Templates.WebApi.Core.Interfaces;
using Mjc.Templates.WebApi.Infrastructure;
using Polly;
using Polly.Extensions.Http;

namespace WebApi.Extensions
{
    public static class MyHttpFactoryConfigurtation
    {
        public static IHttpClientBuilder ConfigureMyHttpClient(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.AddHttpClient<IMyHttpFactory, MyHttpFactory>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(30));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitterer = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                //.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))  // exponential back-off plus some jitter
                                                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
        }
    }
}
