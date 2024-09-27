using System;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using PenomyAPI.Presentation.FastEndpointBasedApi.Options;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.ServiceConfigurations
{
    public static class WebApiServiceConfig
    {
        internal static void Configure(IServiceCollection services, IConfiguration configuration)
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
                        securityScheme: new NSwag.OpenApiSecurityScheme()
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
    }
}
