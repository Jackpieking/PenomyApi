using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using PenomyAPI.BuildingBlock.FeatRegister;
using PenomyAPI.BuildingBlock.FeatRegister.Common;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs1;
using PenomyAPI.Presentation.FastEndpointBasedApi.ServiceConfigurations;
using Typesense;
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

services.AddControllers();
// Add services to the container.
services.AddAppDependency(configuration);
WebApiServiceConfig.Configure(services, configuration);

services.AddFastEndpoints().AddSignalR();

var app = builder.Build();

// Add services provider to the FeatureHandlerResolver.
FeatureHandlerResolver.SetProvider(app.Services);

// Seeding the required data for the application to run.
DataSeedingResolver.Resolve(app.Services);

await using (var scope = app.Services.CreateAsyncScope())
{
    // Init typesense
    var context = scope.TryResolve<AppDbContext>();
    var typesenseClient = scope.TryResolve<ITypesenseClient>();

    Typs1FeatureHandler.LoadRequiredDependencies(context, typesenseClient);
    await Typs1FeatureHandler.ExecuteAsync(CancellationToken.None);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AppGlobalExceptionHandler>()
        .UseHttpsRedirection()
        .UseCors()
        .UseAuthentication()
        .UseAuthorization()
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
    app.UseMiddleware<AppGlobalExceptionHandler>()
        .UseHttpsRedirection()
        .UseCors()
        .UseAuthentication()
        .UseAuthorization()
        .UseFastEndpoints();
}

app.MapHub<NotificationHub>(NotificationHub.connectPath);

await app.RunAsync();
