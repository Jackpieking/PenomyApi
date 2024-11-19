using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class DonationItemEntityConfiguration :
    IEntityConfiguration<DonationItem>
{
    public void Configure(EntityTypeBuilder<DonationItem> builder)
    {
        builder.ToTable("penomy_donation_item");

        builder.HasKey(donationItem => donationItem.Id);

        builder
            .Property(donationItem => donationItem.Name)
            .HasMaxLength(DonationItem.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(donationItem => donationItem.Description)
            .HasMaxLength(DonationItem.MetaData.DescriptionLength)
            .IsRequired();

        builder
            .Property(donationItem => donationItem.Price)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: DonationItem.MetaData.PricePrecision,
                    scale: DonationItem.MetaData.PriceScale
                )
            )
            .IsRequired();

        builder
            .Property(donationItem => donationItem.AllowDonatorToSetPrice)
            .IsRequired();

        builder
            .Property(donationItem => donationItem.CreatorReceivedPercentage)
            .IsRequired();

        builder.Property(donationItem => donationItem.CreatedBy).IsRequired();

        builder
            .Property(donationItem => donationItem.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(donationItem => donationItem.UpdatedBy).IsRequired();

        builder
            .Property(donationItem => donationItem.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(donationItem => donationItem.Creator)
            .WithMany(problem => problem.CreatedDonationItems)
            .HasPrincipalKey(problem => problem.Id)
            .HasForeignKey(donationItem => donationItem.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(donationItem => donationItem.Updater)
            .WithMany(systemAccount => systemAccount.UpdatedDonationItems)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(donationItem => donationItem.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
