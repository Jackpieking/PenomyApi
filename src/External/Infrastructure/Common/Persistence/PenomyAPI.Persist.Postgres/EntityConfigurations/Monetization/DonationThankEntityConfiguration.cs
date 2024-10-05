using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class DonationThankEntityConfiguration :
    IEntityConfiguration<DonationThank>
{
    public void Configure(EntityTypeBuilder<DonationThank> builder)
    {
        builder.ToTable("penomy_donation_thank");

        builder.HasKey(donationThank => new
        {
            donationThank.UserDonationTransactionId,
            donationThank.CreatorId
        });

        builder
            .Property(donationThank => donationThank.ThankNote)
            .HasMaxLength(DonationThank.MetaData.ThankNoteLength)
            .IsRequired();

        builder
            .Property(donationThank => donationThank.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(donationThank => donationThank.Creator)
            .WithMany(creator => creator.DonationThanks)
            .HasPrincipalKey(creator => creator.UserId)
            .HasForeignKey(donationThank => donationThank.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(donationThank => donationThank.UserDonationTransaction)
            .WithOne(userDonation => userDonation.DonationThank)
            .HasPrincipalKey<UserDonationTransaction>(userDonation => userDonation.Id)
            .HasForeignKey<DonationThank>(donationThank => donationThank.UserDonationTransactionId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
