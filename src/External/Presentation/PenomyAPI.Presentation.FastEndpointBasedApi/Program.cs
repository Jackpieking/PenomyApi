using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using PenomyAPI.BuildingBlock.FeatRegister;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddAppDependency();
services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

if (app.Environment.IsStaging()) { }

if (app.Environment.IsProduction()) { }

app.UseFastEndpoints();

await app.RunAsync();
