using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class CreatorWalletTransactionEntityConfiguration
    : IEntityConfiguration<CreatorWalletTransaction>
{
    public void Configure(EntityTypeBuilder<CreatorWalletTransaction> builder)
    {
        builder.ToTable("penomy_creator_wallet_transaction");

        builder.HasKey(walletTransaction => walletTransaction.Id);

        builder
            .Property(walletTransaction => walletTransaction.WalletId)
            .IsRequired();

        builder
            .Property(walletTransaction => walletTransaction.TransactionTypeId)
            .IsRequired();

        builder
            .Property(walletTransaction => walletTransaction.TransactionCode)
            .HasMaxLength(CreatorWalletTransaction.MetaData.TransactionCodeLength)
            .IsRequired();

        builder
            .Property(walletTransaction => walletTransaction.TransactionStatus)
            .IsRequired();

        builder
            .Property(walletTransaction => walletTransaction.TransactionAmount)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: CreatorWalletTransaction.MetaData.TransactionAmountPrecision,
                    scale: CreatorWalletTransaction.MetaData.TransactionAmountScale)
            )
            .IsRequired();

        builder
            .Property(walletTransaction => walletTransaction.TransactionMetaData)
            .HasMaxLength(CreatorWalletTransaction.MetaData.TransactionMetaDataLength)
            .IsRequired();

        builder
            .Property(walletTransaction => walletTransaction.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(walletTransaction => walletTransaction.CreatorWallet)
            .WithMany(wallet => wallet.WalletTransactions)
            .HasPrincipalKey(wallet => wallet.Id)
            .HasForeignKey(walletTransaction => walletTransaction.WalletId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(walletTransaction => walletTransaction.TransactionType)
            .WithMany(transactionType => transactionType.WalletTransactions)
            .HasPrincipalKey(transactionType => transactionType.Id)
            .HasForeignKey(walletTransaction => walletTransaction.TransactionTypeId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
