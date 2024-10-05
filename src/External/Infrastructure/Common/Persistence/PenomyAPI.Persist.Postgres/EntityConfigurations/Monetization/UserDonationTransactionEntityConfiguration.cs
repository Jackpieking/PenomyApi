using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class UserDonationTransactionEntityConfiguration
    : IEntityConfiguration<UserDonationTransaction>
{
    public void Configure(EntityTypeBuilder<UserDonationTransaction> builder)
    {
        builder.ToTable("penomy_user_donation_transaction");

        builder.HasKey(userDonation => userDonation.Id);

        builder.Property(userDonation => userDonation.DonatorId).IsRequired();

        builder.Property(userDonation => userDonation.CreatorId).IsRequired();

        builder.Property(userDonation => userDonation.WalletTransactionId).IsRequired();

        builder
            .Property(userDonation => userDonation.TransactionCode)
            .HasMaxLength(UserDonationTransaction.MetaData.TransactionCodeLength)
            .IsRequired();

        builder.Property(userDonation => userDonation.TransactionStatus).IsRequired();

        builder
            .Property(userDonation => userDonation.TotalDonationAmount)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: UserDonationTransaction.MetaData.DonationAmountPrecision,
                    scale: UserDonationTransaction.MetaData.DonationAmountScale
                )
            )
            .IsRequired();

        builder.Property(userDonation => userDonation.CreatorReceivedPercentage).IsRequired();

        builder.Property(userDonation => userDonation.HasReceivedThankFromCreator).IsRequired();

        builder
            .Property(userDonation => userDonation.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userDonation => userDonation.Donator)
            .WithMany(donator => donator.UserDonationTransactions)
            .HasPrincipalKey(donator => donator.UserId)
            .HasForeignKey(userDonation => userDonation.DonatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userDonation => userDonation.Creator)
            .WithMany(creator => creator.ReceivedUserDonations)
            .HasPrincipalKey(creator => creator.UserId)
            .HasForeignKey(userDonation => userDonation.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
