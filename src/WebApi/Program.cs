using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Mjc.Templates.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine(@"===========================================================================");
            Console.WriteLine(@" _____                     _       _         __                 _          ");
            Console.WriteLine(@"/__   \___ _ __ ___  _ __ | | __ _| |_ ___  / _\ ___ _ ____   _(_) ___ ___ ");
            Console.WriteLine(@"  / /\/ _ \ '_ ` _ \| '_ \| |/ _` | __/ _ \ \ \ / _ \ '__\ \ / / |/ __/ _ \");
            Console.WriteLine(@" / / |  __/ | | | | | |_) | | (_| | ||  __/ _\ \  __/ |   \ V /| | (_|  __/");
            Console.WriteLine(@" \/   \___|_| |_| |_| .__/|_|\__,_|\__\___| \__/\___|_|    \_/ |_|\___\___|");
            Console.WriteLine(@"                    |_|                                                    ");
            Console.WriteLine(@"===========================================================================");
            Console.WriteLine($"Starting...                                                      v{version}");
            Console.WriteLine();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
               WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              var env = hostingContext.HostingEnvironment;

                              //add json config
                              config.AddJsonFile("appsettings.json");

                              //add user secrets if development
                              if (env.IsDevelopment())
                              {
                                  var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                                  if (appAssembly != null)
                                  {
                                      config.AddUserSecrets(appAssembly, optional: false);
                                  }
                              }

                              //add environement variables
                              config.AddEnvironmentVariables();

                              var configuration = config.Build();

                              if (!env.IsDevelopment())
                              {
                                  //add azure key vault providere
                                  if (configuration.GetValue<bool>("MjcTemplatesWebApi:UseAzureVault", true))
                                  {
                                      AzureServiceTokenProvider azureServiceTokenProvider;

                                      if (string.IsNullOrEmpty(configuration["MjcTemplatesWebApi:AzureServiceTokenProviderConnectionString"]))
                                      {
                                          //undefined connection string checks all authentication possibilities
                                          azureServiceTokenProvider = new AzureServiceTokenProvider();
                                      }
                                      else
                                      {
                                          //only checks specified connections string
                                          azureServiceTokenProvider =
                                                    new AzureServiceTokenProvider(configuration["MjcTemplatesWebApi:AzureServiceTokenProviderConnectionString"]);
                                      }

                                      var keyVaultClient = new KeyVaultClient(
                                          new KeyVaultClient.AuthenticationCallback(
                                              azureServiceTokenProvider.KeyVaultTokenCallback));

                                      config.AddAzureKeyVault(
                                          $"https://{configuration["MjcTemplatesWebApi:AzureVault:Name"]}.vault.azure.net/",
                                          keyVaultClient,
                                          new DefaultKeyVaultSecretManager());
                                  }
                              }
                          })
                          .UseStartup<Startup>()
                          .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                                  .ReadFrom.Configuration(hostingContext.Configuration)
                                  .Enrich.FromLogContext()
                                  .WriteTo.Console());
    }
}
