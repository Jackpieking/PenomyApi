using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class ArtworkReportEntityConfiguration : IEntityConfiguration<ArtworkReport>
{
    public void Configure(EntityTypeBuilder<ArtworkReport> builder)
    {
        builder.ToTable("penomy_artwork_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.ArtworkId).IsRequired();

        builder.Property(report => report.ChapterId).IsRequired();

        builder.Property(report => report.ReportProblemId).IsRequired();

        builder
            .Property(report => report.DetailNote)
            .HasMaxLength(ArtworkReport.MetaData.DetailNoteLength)
            .IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(report => report.ReportedArtwork)
            .WithMany(artwork => artwork.ArtworkReports)
            .HasForeignKey(report => report.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.ReportedChapter)
            .WithMany(chapter => chapter.ChapterReports)
            .HasForeignKey(report => report.ChapterId)
            .HasPrincipalKey(chapter => chapter.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.ReportedReasons)
            .WithMany(reason => reason.ArtworkReports)
            .HasForeignKey(report => report.ReportProblemId)
            .HasPrincipalKey(reason => reason.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.Creator)
            .WithMany(user => user.CreatedArtworkReports)
            .HasForeignKey(report => report.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => new
        // {
        //     report.ArtworkId,
        //     report.ChapterId,
        //     report.CreatedBy
        // });

        // builder.HasIndex(report => report.CreatedBy);
        #endregion
    }
}
