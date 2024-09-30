using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class CreatorWalletEntityConfiguration
    : IEntityConfiguration<CreatorWallet>
{
    public void Configure(EntityTypeBuilder<CreatorWallet> builder)
    {
        builder.ToTable("penomy_creator_wallet");

        builder.HasKey(wallet => wallet.Id);

        builder.Property(wallet => wallet.CreatorId).IsRequired();

        builder.Property(wallet => wallet.BankId).IsRequired();

        builder
            .Property(wallet => wallet.BankAccountNumber)
            .HasMaxLength(CreatorWallet.MetaData.BankAccountNumberLength)
            .IsRequired();

        builder.Property(wallet => wallet.WalletStatus).IsRequired();

        builder
            .Property(subscriptionPlan => subscriptionPlan.WalletAmount)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: CreatorWallet.MetaData.WalletAmountPrecision,
                    scale: CreatorWallet.MetaData.WalletAmountScale
                )
            )
            .IsRequired();

        builder
            .Property(wallet => wallet.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(wallet => wallet.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(wallet => wallet.Bank)
            .WithMany(problem => problem.CreatorWallets)
            .HasPrincipalKey(problem => problem.Id)
            .HasForeignKey(wallet => wallet.BankId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(wallet => wallet.Creator)
            .WithOne(creator => creator.CreatorWallet)
            .HasPrincipalKey<CreatorProfile>(creator => creator.UserId)
            .HasForeignKey<CreatorWallet>(wallet => wallet.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion;
    }
}
