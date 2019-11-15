using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using WebApi.Extensions.HealthChecks;

namespace Mjc.Templates.WebApi.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
            o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = false;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader()
                    , new HeaderApiVersionReader(new string[] { "ver", "X-MjcTemplatesWebApi-version" }));
            });
        }

        public static void ConfigureApplicationInsight(this IServiceCollection services,
            IConfiguration configuration)
        {
            var aiOptions = new ApplicationInsightsServiceOptions
            {
                EnableAdaptiveSampling = false,
                EnableQuickPulseMetricStream = false,
                InstrumentationKey = configuration["MjcTemplatesWebApi:ApplicationInsights:InstrumentationKey"]
            };

            services.AddApplicationInsightsTelemetry(aiOptions);
        }

        public static void ConfigureHealthChecks(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                /*.AddAzureKeyVault(options =>
                    {
                        options
                        .UseKeyVaultUrl($"https://{configuration["MjcTemplatesWebApi:AzureVault:Name"]}.vault.azure.net/")
                        .UseAzureManagedServiceIdentity();
                    },
                    name: "Azure Key Vault",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "azure", "keyvault", "key-vault", "azure-keyvault", "services" })*/
                .AddCheck<DummyHealthCheck>(
                    "Dummy Check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "dummy", "services" })
                .AddApplicationInsightsPublisher(configuration["MjcTemplatesWebApi:ApplicationInsights:InstrumentationKey"]);
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["MjcTemplatesWebApi:AzureRedis:Configuration"];
                options.InstanceName = configuration["MjcTemplatesWebApi:AzureRedis:InstanceName"];
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mjc.Templates.WebApi HTTP API",
                    Version = "v1",
                    Description = "The Mjc.Templates.WebApi Micro service HTTP API."
                });
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .SetIsOriginAllowed((host) => true));
            });
        }
    }
}
