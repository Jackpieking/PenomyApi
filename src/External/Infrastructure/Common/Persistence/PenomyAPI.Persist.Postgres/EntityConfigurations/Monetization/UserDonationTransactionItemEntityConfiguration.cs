using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal class UserDonationTransactionItemEntityConfiguration
    : IEntityConfiguration<UserDonationTransactionItem>
{
    public void Configure(EntityTypeBuilder<UserDonationTransactionItem> builder)
    {
        builder.ToTable("penomy_user_donation_transaction_item");

        builder.HasKey(donationTransactionItem => new
        {
            donationTransactionItem.DonationTransactionId,
            donationTransactionItem.DonationItemId
        });

        builder
            .Property(donationTransactionItem => donationTransactionItem.ItemPrice)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: UserDonationTransactionItem.MetaData.PricePrecision,
                    scale: UserDonationTransactionItem.MetaData.PriceScale
                )
            )
            .IsRequired();

        builder
            .Property(donationTransactionItem => donationTransactionItem.ItemQuantity)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(donationTransactionItem => donationTransactionItem.DonationTransaction)
            .WithMany(transaction => transaction.TransactionItems)
            .HasPrincipalKey(transaction => transaction.Id)
            .HasForeignKey(donationTransactionItem => donationTransactionItem.DonationTransactionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(donationTransactionItem => donationTransactionItem.DonationItem)
            .WithMany(donationItem => donationItem.DonationTransactionItems)
            .HasPrincipalKey(donationItem => donationItem.Id)
            .HasForeignKey(donationTransactionItem => donationTransactionItem.DonationItemId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
