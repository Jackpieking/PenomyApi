using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class RoleEntityConfiguration : IEntityConfiguration<PgRole>
{
    public void Configure(EntityTypeBuilder<PgRole> builder)
    {
        builder
            .Property(role => role.Name)
            .HasMaxLength(Role.MetaData.NameLength)
            .IsRequired(false);

        builder
            .Property(role => role.NormalizedName)
            .HasMaxLength(Role.MetaData.NormalizedNameLength)
            .IsRequired(false);

        builder
            .Property(role => role.ConcurrencyStamp)
            .HasMaxLength(Role.MetaData.ConcurrencyStampLength)
            .IsRequired(false);
    }
}
