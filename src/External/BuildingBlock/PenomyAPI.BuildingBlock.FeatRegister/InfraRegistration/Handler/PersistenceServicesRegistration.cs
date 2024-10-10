using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.UnitOfWorks;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

internal sealed class PersistenceServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        AddAppDbContextPool(services, configuration);

        AddAspNetCoreIdentity(services);

        AddAppDefinedServices(services);
    }

    private void AddAppDefinedServices(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>().MakeScopedLazy<IUnitOfWork>();
        services.AddScoped<UserManager<PgUser>>().MakeScopedLazy<UserManager<PgUser>>();
        services.AddScoped<RoleManager<PgRole>>().MakeScopedLazy<RoleManager<PgRole>>();
    }

    private void AddAppDbContextPool(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AppDbContext>(options =>
        {
            options
                .UseNpgsql(
                    configuration.GetConnectionString(name: ""),
                    dbOption =>
                    {
                        dbOption
                            .MigrationsAssembly(assemblyName: "PenomyAPI.Persist.Postgres")
                            .EnableRetryOnFailure(3)
                            .CommandTimeout(30);
                    }
                )
                .EnableSensitiveDataLogging(false)
                .EnableDetailedErrors(false)
                .EnableThreadSafetyChecks(false)
                .EnableServiceProviderCaching(true);
        });
    }

    private void AddAspNetCoreIdentity(IServiceCollection services)
    {
        const string LowerLetters = "abcdefghijklmnopqrstuvwxyz";
        var allowedCharacters = $"{LowerLetters}{LowerLetters.ToUpper()}0123456789-_@+";

        services
            .AddIdentity<PgUser, PgRole>(setupAction: options =>
            {
                // Password configuration.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                // Lockout configuration.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(value: 1);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User's credentials configuration.
                options.User.AllowedUserNameCharacters = $"{allowedCharacters}";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}
