using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.App.Common.UserIdentity;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.UserIdentity;

internal sealed class UserEntityConfiguration : IEntityConfiguration<PgUser>
{
    public void Configure(EntityTypeBuilder<PgUser> builder)
    {
        builder
            .Property(user => user.UserName)
            .HasMaxLength(User.MetaData.UserNameLength)
            .IsRequired(false);

        builder
            .Property(user => user.NormalizedUserName)
            .HasMaxLength(User.MetaData.UserNameLength)
            .IsRequired(false);

        builder
            .Property(user => user.Email)
            .HasMaxLength(User.MetaData.EmailLength)
            .IsRequired(false);

        builder
            .Property(user => user.NormalizedEmail)
            .HasMaxLength(User.MetaData.EmailLength)
            .IsRequired(false);

        builder
            .Property(user => user.PasswordHash)
            .HasMaxLength(User.MetaData.PasswordHashLength)
            .IsRequired(false);

        builder
            .Property(user => user.PhoneNumber)
            .HasMaxLength(User.MetaData.PhoneNumberLength)
            .IsRequired(false);

        builder
            .Property(user => user.ConcurrencyStamp)
            .HasMaxLength(User.MetaData.ConcurrencyStampLength)
            .IsRequired(false);

        builder
            .Property(user => user.SecurityStamp)
            .HasMaxLength(User.MetaData.SecurityStampLength)
            .IsRequired(false);
    }
}
