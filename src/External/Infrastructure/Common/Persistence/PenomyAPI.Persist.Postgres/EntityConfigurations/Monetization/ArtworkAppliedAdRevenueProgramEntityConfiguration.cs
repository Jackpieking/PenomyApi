using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Monetization;

internal sealed class ArtworkAppliedAdRevenueProgramEntityConfiguration
    : IEntityConfiguration<ArtworkAppliedAdRevenueProgram>
{
    public void Configure(EntityTypeBuilder<ArtworkAppliedAdRevenueProgram> builder)
    {
        builder.ToTable("penomy_artwork_applied_ad_revenue_program");

        builder.HasKey(appliedToAdRevenueProgram => new
        {
            appliedToAdRevenueProgram.ArtworkId,
            appliedToAdRevenueProgram.AdRevenueProgramId
        });

        builder
            .Property(appliedToAdRevenueProgram => appliedToAdRevenueProgram.AppliedStatus)
            .IsRequired();

        builder
            .Property(appliedToAdRevenueProgram => appliedToAdRevenueProgram.ProposedBy)
            .IsRequired();

        builder
            .Property(appliedToAdRevenueProgram => appliedToAdRevenueProgram.ProposedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(appliedToAdRevenueProgram => appliedToAdRevenueProgram.ApprovedBy)
            .IsRequired();

        builder
            .Property(appliedToAdRevenueProgram => appliedToAdRevenueProgram.ApprovedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(appliedToAdRevenueProgram => appliedToAdRevenueProgram.RejectedNote)
            .HasMaxLength(ArtworkAppliedAdRevenueProgram.MetaData.RejectedNoteLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(appliedToAdRevenueProgram => appliedToAdRevenueProgram.Proposer)
            .WithMany(proposer => proposer.AppliedAdRevenuePrograms)
            .HasPrincipalKey(proposer => proposer.UserId)
            .HasForeignKey(appliedToAdRevenueProgram => appliedToAdRevenueProgram.ProposedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(appliedToAdRevenueProgram => appliedToAdRevenueProgram.Approver)
            .WithMany(systemAccount => systemAccount.ApprovedArtworkAppliedAdRevenuePrograms)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(appliedToAdRevenueProgram => appliedToAdRevenueProgram.ApprovedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
