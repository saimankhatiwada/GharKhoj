# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/GharKhoj.Api/GharKhoj.Api.csproj", "src/GharKhoj.Api/"]
COPY ["src/GharKhoj.Application/GharKhoj.Application.csproj", "src/GharKhoj.Application/"]
COPY ["src/GharKhoj.Domain/GharKhoj.Domain.csproj", "src/GharKhoj.Domain/"]
COPY ["src/GharKhoj.Infrastructure/GharKhoj.Infrastructure.csproj", "src/GharKhoj.Infrastructure/"]
RUN dotnet restore "./src/GharKhoj.Api/GharKhoj.Api.csproj"
COPY . .
WORKDIR "/src/src/GharKhoj.Api"
RUN dotnet build "./GharKhoj.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GharKhoj.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GharKhoj.Api.dll"]