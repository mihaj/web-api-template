FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine3.9 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["WebApi/Mjc.Templates.WebApi.csproj", "src/WebApi/"]
COPY ["Core/Mjc.Templates.WebApi.Core.csproj", "src/Core/"]
COPY ["Infrastructure/Mjc.Templates.WebApi.Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/WebApi/Mjc.Templates.WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "Mjc.Templates.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mjc.Templates.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mjc.Templates.WebApi.dll"]
