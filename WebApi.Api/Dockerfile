#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebApi.Api/WebApi.Api.csproj", "WebApi.Api/"]
COPY ["WebApi.Services/WebApi.Services.csproj", "WebApi.Services/"]
COPY ["WebApi.Tools/WebApi.Tools.csproj", "WebApi.Tools/"]
COPY ["WebApi.Models/WebApi.Models.csproj", "WebApi.Models/"]
COPY ["WebApi.Repository/WebApi.Repository.csproj", "WebApi.Repository/"]
COPY ["WebApi.Common/WebApi.Common.csproj", "WebApi.Common/"]
RUN dotnet restore "WebApi.Api/WebApi.Api.csproj"
COPY . .
WORKDIR "/src/WebApi.Api"
RUN dotnet build "WebApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.Api.dll"]