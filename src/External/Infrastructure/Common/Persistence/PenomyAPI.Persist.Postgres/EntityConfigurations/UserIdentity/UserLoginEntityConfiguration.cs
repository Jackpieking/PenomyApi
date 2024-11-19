using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class UserLoginEntityConfiguration : IEntityConfiguration<PgUserLogin>
{
    public void Configure(EntityTypeBuilder<PgUserLogin> builder)
    {
        builder
            .Property(userLogin => userLogin.LoginProvider)
            .HasMaxLength(UserLogin.MetaData.LoginProviderLength)
            .IsRequired();

        builder
            .Property(userLogin => userLogin.ProviderKey)
            .HasMaxLength(UserLogin.MetaData.ProviderKeyLength)
            .IsRequired();

        builder
            .Property(userLogin => userLogin.ProviderDisplayName)
            .HasMaxLength(UserLogin.MetaData.ProviderDisplayNameLength)
            .IsRequired(false);
    }
}
