using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.UnitOfWorks;

namespace PenomyAPI.BuildingBlock.FeatRegister;

public static class AppDependencyRegistrationEntry
{
    const string AivenDevDBConnectionString = "AivenDevDB";

    public static void AddAppDependency(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        FeatureHandlerRegistration.Register(services, configuration);

        services.AddDbContextPool<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(
                name: AivenDevDBConnectionString
            );

            options.UseNpgsql(connectionString);
        });

        services.AddCommonInfrastructure();
    }

    private static void AddCommonInfrastructure(this IServiceCollection services)
    {
        // Add dependencies from Common.Infrastructure.
        services.AddAspNetIdentityConfiguration();
        services.AddScoped<IUnitOfWork, UnitOfWork>().MakeScopedLazy<IUnitOfWork>();
    }
}
