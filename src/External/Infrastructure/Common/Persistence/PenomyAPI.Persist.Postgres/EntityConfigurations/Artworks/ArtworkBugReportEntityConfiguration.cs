using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class ArtworkBugReportEntityConfiguration : IEntityConfiguration<ArtworkBugReport>
{
    public void Configure(EntityTypeBuilder<ArtworkBugReport> builder)
    {
        builder.ToTable("penomy_artwork_bug_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.CreatedBy).IsRequired();

        builder.Property(report => report.BugTypeId).IsRequired();

        builder.Property(report => report.ArtworkId).IsRequired();

        builder
            .Property(report => report.Title)
            .HasMaxLength(ArtworkBugReport.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(report => report.Overview)
            .HasMaxLength(ArtworkBugReport.MetaData.OverviewLength)
            .IsRequired();

        builder
            .Property(report => report.UserDetailNote)
            .HasMaxLength(ArtworkBugReport.MetaData.UserDetailNoteLength)
            .IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(report => report.IsResolved).IsRequired();

        builder
            .Property(report => report.ResolveNote)
            .HasMaxLength(ArtworkBugReport.MetaData.ResolveNoteLength)
            .IsRequired();

        builder.Property(report => report.ResolvedBy).IsRequired();

        builder
            .Property(report => report.ResolvedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(bugReport => bugReport.Creator)
            .WithMany(user => user.CreatedArtworkBugReports)
            .HasForeignKey(bugReport => bugReport.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(bugReport => bugReport.Artwork)
            .WithMany(artwork => artwork.ArtworkBugReports)
            .HasForeignKey(bugReport => bugReport.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(bugReport => bugReport.BugType)
            .WithMany(bugType => bugType.BugReports)
            .HasForeignKey(bugReport => bugReport.BugTypeId)
            .HasPrincipalKey(bugType => bugType.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(bugReport => bugReport.Resolver)
            .WithMany(systemAccount => systemAccount.ResolvedArtworkBugReports)
            .HasForeignKey(bugReport => bugReport.ResolvedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        //builder.HasIndex(report => report.CreatedBy);

        //builder.HasIndex(report => report.ArtworkId);
        #endregion
    }
}
