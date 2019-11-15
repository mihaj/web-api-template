using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mjc.Templates.WebApi.Extensions;
using WebApi.Extensions;
using WebApi.Middleware;

namespace Mjc.Templates.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.ConfigureApplicationInsight(Configuration);

            services.ConfigureControllers();

            services.ConfigureMyHttpClient();

            services.ConfigureApiVersioning();

            services.ConfigureHealthChecks(Configuration);

            services.ConfigureSwagger();

            services.ConfigureRedis(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            var healthCheckOptions = new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (c, r) =>
                {
                    c.Response.ContentType = MediaTypeNames.Application.Json;
                    var result = JsonSerializer.Serialize(
                        new
                        {
                            checks = r.Entries.Select(e =>
                                new
                                {
                                    description = e.Key,
                                    status = e.Value.Status.ToString(),
                                    responseTime = $"{e.Value.Duration.TotalMilliseconds}ms"
                                }),
                            totalResponseTime = r.TotalDuration.TotalMilliseconds
                        });
                    await c.Response.WriteAsync(result);
                }
            };

            app.UseHealthChecks("/hc", healthCheckOptions);

            app.UseHealthChecks("/self", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("services")
            });

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnShutdown);
            appLifetime.ApplicationStopped.Register(OnStopped);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ApplicationInsightsMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapControllers();
                   });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Mjc.Templates.WebApi V1");
            });
        }

        private void OnShutdown()
        {
            for (var i = 5; i > 0; i--)
            {
                Console.WriteLine($"Stopping in {i}s");
                Thread.Sleep(1000);
            }
        }

        private void OnStopped()
        {
            Console.WriteLine("Service stopped.");
        }

        private void OnStarted()
        {
            Console.WriteLine("Service started...");
        }
    }
}
