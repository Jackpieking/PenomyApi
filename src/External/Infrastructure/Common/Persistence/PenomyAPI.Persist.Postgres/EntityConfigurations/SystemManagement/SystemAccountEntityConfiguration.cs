using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SystemManagement;

internal sealed class SystemAccountEntityConfiguration : IEntityConfiguration<SystemAccount>
{
    public void Configure(EntityTypeBuilder<SystemAccount> builder)
    {
        builder.ToTable("penomy_system_account");

        builder.HasKey(account => account.Id);

        builder
            .Property(account => account.Email)
            .HasMaxLength(SystemAccount.MetaData.EmailLength)
            .IsRequired();

        builder.Property(account => account.EmailConfirmed).IsRequired();

        builder
            .Property(account => account.PasswordHash)
            .HasMaxLength(SystemAccount.MetaData.PasswordHashLength)
            .IsRequired();

        builder
            .Property(account => account.PhoneNumber)
            .HasMaxLength(SystemAccount.MetaData.PhoneNumberLength)
            .IsRequired();

        builder.Property(account => account.IsActive).IsRequired();

        builder
            .Property(account => account.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Indexes
        // builder.HasIndex(account => account.Email).IsUnique(true);
        #endregion
    }
}
