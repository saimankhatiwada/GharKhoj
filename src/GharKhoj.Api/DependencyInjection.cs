using GharKhoj.Api.MimeTypes;
using GharKhoj.Api.Services;
using GharKhoj.Api.Swagger;
using GharKhoj.Application;
using GharKhoj.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GharKhoj.Api;

internal static class DependencyInjection
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {

        ConfigureMvcServices(builder);

        ConfigureApplicationAndInfrastructure(builder);

        ConfigureSwagger(builder);

        builder.Services.AddTransient<DataShapingService>();
        builder.Services.AddTransient<LinkService>();

        return builder;
    }

    private static void ConfigureMvcServices(WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            })
            .AddXmlSerializerFormatters();

        builder.Services
            .Configure<MvcOptions>(options =>
            {
                NewtonsoftJsonOutputFormatter formatter = options.OutputFormatters
                    .OfType<NewtonsoftJsonOutputFormatter>()
                    .First();

                formatter.SupportedMediaTypes.Add(CustomMimeTypeNames.Application.JsonV1);
                formatter.SupportedMediaTypes.Add(CustomMimeTypeNames.Application.JsonV2);
                formatter.SupportedMediaTypes.Add(CustomMimeTypeNames.Application.HateoasJson);
                formatter.SupportedMediaTypes.Add(CustomMimeTypeNames.Application.HateoasJsonV1);
                formatter.SupportedMediaTypes.Add(CustomMimeTypeNames.Application.HateoasJsonV2);
            });
    }

    private static void ConfigureApplicationAndInfrastructure(WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
        });

        builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        builder.Services.ConfigureOptions<ConfigureSwaggerUIOptions>();
    }
}
