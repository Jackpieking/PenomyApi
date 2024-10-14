using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class RevenueProgramEntityConfiguration :
    IEntityConfiguration<RevenueProgram>
{
    public void Configure(EntityTypeBuilder<RevenueProgram> builder)
    {
        builder.ToTable("penomy_revenue_program");

        builder.HasKey(revenueProgram => revenueProgram.Id);

        builder
            .Property(revenueProgram => revenueProgram.Title)
            .HasMaxLength(RevenueProgram.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.Description)
            .HasMaxLength(RevenueProgram.MetaData.DescriptionLength)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.MinTotalViewsToApply)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.MinTotalFollowersToApply)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.MinTotalFollowersToApply)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.CreatedBy)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.UpdatedBy)
            .IsRequired();

        builder
            .Property(revenueProgram => revenueProgram.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(revenueProgram => revenueProgram.Creator)
            .WithMany(systemAccount => systemAccount.CreatedAdRevenuePrograms)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(revenueProgram => revenueProgram.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(revenueProgram => revenueProgram.Updater)
            .WithMany(systemAccount => systemAccount.UpdatedAdRevenuePrograms)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(revenueProgram => revenueProgram.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
