# .NET Core Web API Template

Build status: add link from devops

## 1. Description

Template Service is a Web API build on .NET Core 3.0 with supporting libraries.

It integrates Nuget packages:
* **Application Insights** - Gather and send service metrics to the Azure Application Insights
* **Swagger** - JSON documentation
* **API Versioning** - Versioning library
* **Health Checks** - Healthcheck library
* **Polly** - Http Resiliency library
* **Serilog** - Logging library (text, console)

## 2. Secrets.json

When the repository is checkout, you should initialize secret.json file from the Visual Studio. Do that by right clicking WebAPi project and click "Manage user secrets". Then copy and paste json below and populate correct values.

```json
{
  "MjcTemplatesWebApi:AzureVault:Name": "...",
  "MjcTemplatesWebApi:AzureRedis:InstanceName": "***.redis.cache.windows.net",
  "MjcTemplatesWebApi:AzureRedis:Configuration": "redis connection string",
  "MjcTemplatesWebApi:AzureAdB2C:Domain": "***.onmicrosoft.com",
  "MjcTemplatesWebApi:AzureAdB2C:ClientId": "...",
  "MjcTemplatesWebApi:ApplicationInsights:InstrumentationKey": "..."
}
```

## 3. Health Checks
There are two types of health checks:

### 3.1. Liveness health check
It is located at `/self` endpoint of the API and returns 200 - OK with *Healthy* body. 

### 3.2. Readiness health checks basic
It is located at `/ready` endpoint of the API and returns 200 - OK with *Healthy* body. It checks if all the services defined in heatlh checks are up.

### 3.3. Readiness health with details
It is located at `/hc` and returnes a JSON for checks about Azure Key Vault and Swagger JSON file:

```json
{
  "checks": [
    {
      "description": "self",
      "status": "Healthy",
      "responseTime": "0,0029ms"
    },
    {
      "description": "Dummy Check",
      "status": "Healthy",
      "responseTime": "0,0163ms"
    }
  ],
  "totalResponseTime": 12.8834
}
```

## 4. Build pipeline

Azure DevOps build pipeline is defined in `azure_pipelines.yaml` file.

## 5. Kubernetes scripts

Every environment and service has its own `yaml` scripts that inherit some base properties from the base folder scripts.

## 6. To-do

* SQL database sample (Dapper, SQlCLient)
* Service bus sample
* Azure Storage sample
* service generator tool
* Authorization