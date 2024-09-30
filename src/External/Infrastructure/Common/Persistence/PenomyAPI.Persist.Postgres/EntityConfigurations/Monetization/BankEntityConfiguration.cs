using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class BankEntityConfiguration :
    IEntityConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("penomy_bank");

        builder.HasKey(bank => bank.Id);

        builder.Property(bank => bank.BankId).IsRequired();

        builder
            .Property(bank => bank.Name)
            .HasMaxLength(Bank.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(bank => bank.Code)
            .HasMaxLength(Bank.MetaData.CodeLength)
            .IsRequired();

        builder
            .Property(bank => bank.Bin)
            .HasMaxLength(Bank.MetaData.BinLength)
            .IsRequired();

        builder
            .Property(bank => bank.ShortName)
            .HasMaxLength(Bank.MetaData.ShortNameLength)
            .IsRequired();

        builder
            .Property(bank => bank.LogoUrl)
            .HasMaxLength(Bank.MetaData.LogoUrlLength)
            .IsRequired();

        builder
            .Property(bank => bank.SwiftCode)
            .HasMaxLength(Bank.MetaData.SwiftCodeLength)
            .IsRequired();

        builder
            .Property(bank => bank.CreatedBy)
            .IsRequired();

        builder
            .Property(bank => bank.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(bank => bank.UpdatedBy)
            .IsRequired();

        builder
            .Property(bank => bank.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(bank => bank.Creator)
            .WithMany(systemAccount => systemAccount.CreatedBanks)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(bank => bank.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(bank => bank.Updater)
            .WithMany(systemAccount => systemAccount.UpdatedBanks)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(bank => bank.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
