using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class RoleClaimEntityConfiguration : IEntityConfiguration<PgRoleClaim>
{
    public void Configure(EntityTypeBuilder<PgRoleClaim> builder)
    {
        builder
            .Property(roleClaim => roleClaim.ClaimType)
            .HasMaxLength(RoleClaim.MetaData.ClaimTypeLength)
            .IsRequired(false);

        builder
            .Property(roleClaim => roleClaim.ClaimValue)
            .HasMaxLength(RoleClaim.MetaData.ClaimValueLength)
            .IsRequired(false);
    }
}
