using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class AdRevenueProgramEntityConfiguration :
    IEntityConfiguration<AdRevenueProgram>
{
    public void Configure(EntityTypeBuilder<AdRevenueProgram> builder)
    {
        builder.ToTable("penomy_ad_revenue_program");

        builder.HasKey(adRevenueProgram => adRevenueProgram.Id);

        builder
            .Property(adRevenueProgram => adRevenueProgram.Title)
            .HasMaxLength(AdRevenueProgram.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.Description)
            .HasMaxLength(AdRevenueProgram.MetaData.DescriptionLength)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.MinTotalViewsToApply)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.MinTotalFollowersToApply)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.CreatedBy)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.UpdatedBy)
            .IsRequired();

        builder
            .Property(adRevenueProgram => adRevenueProgram.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(adRevenueProgram => adRevenueProgram.Creator)
            .WithMany(systemAccount => systemAccount.CreatedAdRevenuePrograms)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(adRevenueProgram => adRevenueProgram.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(adRevenueProgram => adRevenueProgram.Updater)
            .WithMany(systemAccount => systemAccount.UpdatedAdRevenuePrograms)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(adRevenueProgram => adRevenueProgram.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
