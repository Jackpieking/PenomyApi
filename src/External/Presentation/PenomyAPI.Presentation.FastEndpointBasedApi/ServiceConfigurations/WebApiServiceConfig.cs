﻿using System;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Password;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.ServiceConfigurations;

public static class WebApiServiceConfig
{
    internal static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureNSwag(configuration);

        services.ConfigureCors(configuration);

        services.ConfigureLogging(configuration);

        services.AddGlobalExceptionHandler();

        services.AddAppDefinedServices(configuration);

        services.ConfigureRequestLimits(configuration);
    }

    private static void ConfigureRequestLimits(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = ArtworkConstraints.MAXIMUM_VIDEO_FILE_SIZE;
        });
    }

    private static void AddAppDefinedServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .MakeSingletonLazy<IServiceScopeFactory>()
            .AddScoped<ICommonCacheHandler, CommonCacheHandler>()
            .AddSingleton<IAppPasswordHasher, AppPasswordHasher>()
            .MakeSingletonLazy<IAppPasswordHasher>();
    }

    private static void AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddScoped<AppGlobalExceptionHandler>();
    }

    private static void ConfigureNSwag(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var swaggerOption = configuration
            .GetRequiredSection(key: "Swagger")
            .GetRequiredSection(key: "NSwag")
            .Get<NSwagOptions>();

        services.SwaggerDocument(documentOption =>
        {
            documentOption.DocumentSettings = setting =>
            {
                setting.PostProcess = document =>
                {
                    document.Info = new()
                    {
                        Version = swaggerOption.Doc.PostProcess.Info.Version,
                        Title = swaggerOption.Doc.PostProcess.Info.Title,
                        Description = swaggerOption.Doc.PostProcess.Info.Description,
                        Contact = new()
                        {
                            Name = swaggerOption.Doc.PostProcess.Info.Contact.Name,
                            Email = swaggerOption.Doc.PostProcess.Info.Contact.Email
                        },
                        License = new()
                        {
                            Name = swaggerOption.Doc.PostProcess.Info.License.Name,
                            Url = new(value: swaggerOption.Doc.PostProcess.Info.License.Url)
                        }
                    };
                };

                setting.AddAuth(
                    schemeName: JwtBearerDefaults.AuthenticationScheme,
                    securityScheme: new OpenApiSecurityScheme()
                    {
                        Type = (OpenApiSecuritySchemeType)
                            Enum.ToObject(
                                enumType: typeof(OpenApiSecuritySchemeType),
                                value: swaggerOption.Doc.Auth.Bearer.Type
                            ),
                        In = (OpenApiSecurityApiKeyLocation)
                            Enum.ToObject(
                                enumType: typeof(OpenApiSecurityApiKeyLocation),
                                value: swaggerOption.Doc.Auth.Bearer.In
                            ),
                        Scheme = swaggerOption.Doc.Auth.Bearer.Scheme,
                        BearerFormat = swaggerOption.Doc.Auth.Bearer.BearerFormat,
                        Description = swaggerOption.Doc.Auth.Bearer.Description,
                    }
                );
            };

            documentOption.EnableJWTBearerAuth = swaggerOption.EnableJWTBearerAuth;
        });
    }

    private static void ConfigureCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var corsOption = configuration
            .GetRequiredSection(key: "Cors")
            .GetRequiredSection(key: "Policy")
            .GetRequiredSection(key: "Default")
            .Get<CorsOptions>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (corsOption.AllowHeaders.Length == default)
                {
                    policy.AllowAnyHeader();
                }
                else
                {
                    policy.WithHeaders(corsOption.AllowHeaders);
                }

                if (corsOption.AllowMethods.Length == default)
                {
                    policy.AllowAnyMethod();
                }
                else
                {
                    policy.WithMethods(corsOption.AllowMethods);
                }

                if (corsOption.AllowOrigins.Length == default)
                {
                    policy.AllowAnyOrigin();
                }
                else
                {
                    policy.WithOrigins(corsOption.AllowOrigins);
                }

                if (corsOption.AllowCredentials)
                {
                    policy.AllowCredentials();
                }
            });
        });
    }

    private static void ConfigureLogging(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddConsole();
        });
    }
}
