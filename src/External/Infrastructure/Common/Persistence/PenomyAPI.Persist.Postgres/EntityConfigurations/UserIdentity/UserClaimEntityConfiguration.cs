using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class UserClaimEntityConfiguration : IEntityConfiguration<PgUserClaim>
{
    public void Configure(EntityTypeBuilder<PgUserClaim> builder)
    {
        builder
            .Property(userClaim => userClaim.ClaimType)
            .HasMaxLength(UserClaim.MetaData.ClaimTypeLength)
            .IsRequired(false);

        builder
            .Property(userClaim => userClaim.ClaimValue)
            .HasMaxLength(UserClaim.MetaData.ClaimValueLength)
            .IsRequired(false);
    }
}
