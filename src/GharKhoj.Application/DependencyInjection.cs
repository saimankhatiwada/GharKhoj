﻿using FluentValidation;
using GharKhoj.Application.Abstracions.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace GharKhoj.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));

            configuration.AddOpenBehavior(typeof(QueryCachingBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}
