using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class UserTokenEntityConfiguration : IEntityConfiguration<PgUserToken>
{
    public void Configure(EntityTypeBuilder<PgUserToken> builder)
    {
        builder
            .Property(userToken => userToken.LoginProvider)
            .HasMaxLength(UserToken.MetaData.LoginProviderLength)
            .IsRequired();

        builder
            .Property(userToken => userToken.Name)
            .HasMaxLength(UserToken.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(userToken => userToken.Value)
            .HasMaxLength(UserToken.MetaData.ValueLength)
            .IsRequired(false);

        builder
            .Property(userToken => userToken.ExpiredAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Indexes
        // builder.HasIndex(userToken => userToken.Id).IsUnique(true);
        #endregion
    }
}
