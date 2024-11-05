using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using PenomyAPI.BuildingBlock.FeatRegister;
using PenomyAPI.BuildingBlock.FeatRegister.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.ServiceConfigurations;

// Global Configuration.
Console.OutputEncoding = Encoding.UTF8;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddAppDependency(configuration);
WebApiServiceConfig.Configure(services, configuration);

services.AddFastEndpoints();

var app = builder.Build();

// Add services provider to the FeatureHandlerResolver.
FeatureHandlerResolver.SetProvider(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AppGlobalExceptionHandler>()
        .UseCors()
        .UseFastEndpoints()
        .UseSwaggerGen()
        .UseSwaggerUi(options =>
        {
            options.Path = string.Empty;
            options.DefaultModelsExpandDepth = -1;
        });
}

if (app.Environment.IsStaging()) { }

if (app.Environment.IsProduction())
{
    app.UseCors().UseFastEndpoints();
}

await app.RunAsync();
