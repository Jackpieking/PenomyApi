using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Data.DbContexts;

public sealed class AppDbContext : IdentityDbContext<PgUser, PgRole, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Apply the configuration from the Identity.
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        RemoveAspNetPrefixInIdentityTable(builder);
    }

    private static void RemoveAspNetPrefixInIdentityTable(ModelBuilder builder)
    {
        const string AspNetPrefix = "AspNet";

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            if (tableName.StartsWith(value: AspNetPrefix))
            {
                var newName = $"penomy_identity_{tableName[AspNetPrefix.Length..].ToLower()}";

                entityType.SetTableName(name: newName.Substring(0, newName.Length - 1));
            }
        }
    }
}
