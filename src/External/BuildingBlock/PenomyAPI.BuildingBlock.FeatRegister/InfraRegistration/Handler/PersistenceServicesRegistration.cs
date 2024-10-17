using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.UnitOfWorks;
using PenomyAPI.Persist.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

internal sealed class PersistenceServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        AddAppDbContextPool(services, configuration);

        AddAspNetCoreIdentity(services, configuration);

        AddCaching(services, configuration);

        AddAppDefinedServices(services);
    }

    private static void AddAppDefinedServices(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>().MakeScopedLazy<IUnitOfWork>();
        services.AddScoped<UserManager<PgUser>>().MakeScopedLazy<UserManager<PgUser>>();
        services.AddScoped<RoleManager<PgRole>>().MakeScopedLazy<RoleManager<PgRole>>();
        services.AddScoped<ICacheHandler, CacheHandler>().MakeScopedLazy<ICacheHandler>();
        services.MakeSingletonLazy<IFusionCache>();
        services.MakeSingletonLazy<IFusionCacheSerializer>();
    }

    private static void AddAppDbContextPool(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContextPool<AppDbContext>(config =>
        {
            var configOption = configuration
                .GetRequiredSection("Database")
                .GetRequiredSection("PenomyDev")
                .Get<AppDbContextOptions>();

            config
                .UseNpgsql(
                    configOption.ConnectionString,
                    dbOption =>
                    {
                        dbOption
                            .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                            .EnableRetryOnFailure(configOption.EnableRetryOnFailure)
                            .CommandTimeout(configOption.CommandTimeOutInSecond);
                    }
                )
                .EnableSensitiveDataLogging(configOption.EnableSensitiveDataLogging)
                .EnableDetailedErrors(configOption.EnableDetailedErrors)
                .EnableThreadSafetyChecks(configOption.EnableThreadSafetyChecks)
                .EnableServiceProviderCaching(configOption.EnableServiceProviderCaching);
        });
    }

    private static void AddAspNetCoreIdentity(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddIdentity<PgUser, PgRole>(setupAction: config =>
            {
                var configOption = configuration
                    .GetRequiredSection("AspNetCoreIdentity")
                    .Get<AspNetCoreIdentityOptions>();

                // Password configuration.
                config.Password.RequireDigit = configOption.Password.RequireDigit;
                config.Password.RequireLowercase = configOption.Password.RequireLowercase;
                config.Password.RequireNonAlphanumeric = configOption
                    .Password
                    .RequireNonAlphanumeric;
                config.Password.RequireUppercase = configOption.Password.RequireUppercase;
                config.Password.RequiredLength = configOption.Password.RequiredLength;
                config.Password.RequiredUniqueChars = configOption.Password.RequiredUniqueChars;

                // Lockout configuration.
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(
                    value: configOption.Lockout.DefaultLockoutTimeSpanInSecond
                );
                config.Lockout.MaxFailedAccessAttempts = configOption
                    .Lockout
                    .MaxFailedAccessAttempts;
                config.Lockout.AllowedForNewUsers = configOption.Lockout.AllowedForNewUsers;

                // User's credentials configuration.
                config.User.AllowedUserNameCharacters = config.User.AllowedUserNameCharacters;
                config.User.RequireUniqueEmail = config.User.RequireUniqueEmail;

                config.SignIn.RequireConfirmedEmail = configOption.SignIn.RequireConfirmedEmail;
                config.SignIn.RequireConfirmedPhoneNumber = configOption
                    .SignIn
                    .RequireConfirmedPhoneNumber;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddStackExchangeRedisCache(options =>
            {
                var configOption = configuration
                    .GetRequiredSection("Cache")
                    .GetRequiredSection("Redis")
                    .Get<RedisOptions>();

                options.Configuration = configOption.ConnectionString;
            })
            .AddMemoryCache()
            .AddFusionCacheProtoBufNetSerializer()
            .AddFusionCache()
            .TryWithAutoSetup();
    }
}
