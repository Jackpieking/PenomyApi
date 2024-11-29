using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using PenomyAPI.BuildingBlock.FeatRegister;
using PenomyAPI.BuildingBlock.FeatRegister.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.ServiceConfigurations;
using PenomyAPI.Realtime.SignalR;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

services.AddFastEndpoints().AddSignalR();

var app = builder.Build();

// Add services provider to the FeatureHandlerResolver.
FeatureHandlerResolver.SetProvider(app.Services);

// Seeding the required data for the application to run.
DataSeedingResolver.Resolve(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AppGlobalExceptionHandler>()
        .UseHttpsRedirection()
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

app.MapHub<NotificationHub>(NotificationHub.connectPath);

await app.RunAsync();
