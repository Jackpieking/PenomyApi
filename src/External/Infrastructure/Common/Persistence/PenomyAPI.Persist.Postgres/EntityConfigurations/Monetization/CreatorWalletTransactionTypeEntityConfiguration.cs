using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class CreatorWalletTransactionTypeEntityConfiguration
    : IEntityConfiguration<CreatorWalletTransactionType>
{
    public void Configure(EntityTypeBuilder<CreatorWalletTransactionType> builder)
    {
        builder.ToTable("penomy_creator_wallet_transaction_type");

        builder.HasKey(transactionType => transactionType.Id);

        builder
            .Property(transactionType => transactionType.Name)
            .HasMaxLength(CreatorWalletTransactionType.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(transactionType => transactionType.Description)
            .HasMaxLength(CreatorWalletTransactionType.MetaData.DescriptionLength)
            .IsRequired();

        builder
            .Property(transactionType => transactionType.CreatedBy)
            .IsRequired();

        builder
            .Property(transactionType => transactionType.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        #endregion
    }
}
