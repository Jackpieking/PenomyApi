using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.UnitOfWorks;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration;

internal class PersistenceServicesRegistration : IServicesRegistration
{
    private const string AivenDevDBConnectionString = "AivenDevDB";
    private const string LocalhostConnectionString = "localhost";

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        // DbContext section.
        services.AddDbContextPool<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(
                name: AivenDevDBConnectionString
            );

            options.UseNpgsql(connectionString);
        });

        // AspNetIdentity configuration section.
        services.AddAspNetIdentityConfiguration();

        // UnitOfWork & IdentityManager section.
        services.AddScoped<IUnitOfWork, UnitOfWork>().MakeScopedLazy<IUnitOfWork>();
        services.AddScoped<UserManager<PgUser>>().MakeScopedLazy<UserManager<PgUser>>();
        services.AddScoped<RoleManager<PgRole>>().MakeScopedLazy<RoleManager<PgRole>>();
    }
}
