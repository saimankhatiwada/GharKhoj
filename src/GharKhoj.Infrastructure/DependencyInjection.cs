using Asp.Versioning;
using Azure;
using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Caching;
using GharKhoj.Application.Abstracions.Clock;
using GharKhoj.Application.Abstracions.Data;
using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Properties;
using GharKhoj.Infrastructure.Authentication;
using GharKhoj.Infrastructure.Authorization;
using GharKhoj.Infrastructure.Caching;
using GharKhoj.Infrastructure.Clock;
using GharKhoj.Infrastructure.Data;
using GharKhoj.Infrastructure.Outbox;
using GharKhoj.Infrastructure.Repositories;
using GharKhoj.Infrastructure.Sorting;
using GharKhoj.Infrastructure.Sorting.Mappings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using StackExchange.Redis;
using AuthenticationOptions = GharKhoj.Infrastructure.Authentication.AuthenticationOptions;
using AuthenticationService = GharKhoj.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = GharKhoj.Application.Abstracions.Authentication.IAuthenticationService;

namespace GharKhoj.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        AddDatabaseProviders(services, configuration);

        AddCachingServices(services, configuration);

        AddAuthenticationServices(services, configuration);

        AddAuthorizationServices(services);

        AddApiVersioningServices(services);

        AddOutboxServices(services, configuration);

        AddOpenTelemetryServices(services, environment);

        AddSortingServices(services);

        return services;
    }

    private static void AddDatabaseProviders(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(connectionString)
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }

    private static void AddCachingServices(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Cache") ??
                                          throw new ArgumentNullException(nameof(configuration));

        IConnectionMultiplexer redisConnectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

        services.AddSingleton(redisConnectionMultiplexer);

        services.AddStackExchangeRedisCache(options => options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer));

        //services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddAuthenticationServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection(KeycloakOptions.Name));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
        {
            KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        })
        .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddAuthorizationServices(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();

        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void AddApiVersioningServices(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new DefaultApiVersionSelector(options);

                options.ApiVersionReader = ApiVersionReader.Combine(
                    new MediaTypeApiVersionReader(),
                    new MediaTypeApiVersionReaderBuilder()
                        .Template("application/vnd.gharkhoj.hateoas.{version}+json")
                        .Build());
            })
            .AddMvc()
            .AddApiExplorer();
    }

    private static void AddOutboxServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.Name));

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
    }

    private static void AddOpenTelemetryServices(IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(environment.ApplicationName))
                .WithTracing(tracing => tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddSqlClientInstrumentation())
                .WithMetrics(metrics => metrics
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation())
                .UseOtlpExporter();
    }

    private static void AddSortingServices(IServiceCollection services)
    {
        services.AddTransient<SortMappingProvider>();

        services.AddSingleton<ISortMappingDefinition, SortMappingDefinition<Property>>(_ =>
            PropertySortMapping.SortMapping);
    }
}
