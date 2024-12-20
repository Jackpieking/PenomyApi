using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class UserRoleEntityConfiguration : IEntityConfiguration<PgUserRole>
{
    public void Configure(EntityTypeBuilder<PgUserRole> builder) { }
}
