using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkChapterReportAttachedMediaEntityConfiguration
    : IEntityConfiguration<ArtworkChapterReportAttachedMedia>
{
    public void Configure(EntityTypeBuilder<ArtworkChapterReportAttachedMedia> builder)
    {
        builder.ToTable("penomy_artwork_chapter_report_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.ChapterReportedId).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(ArtworkReportAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(media => media.MediaType).IsRequired();

        builder.Property(media => media.UploadOrder).IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(ArtworkReportAttachedMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(attachedMedia => attachedMedia.ChapterReport)
            .WithMany(chapterReport => chapterReport.AttachedMedias)
            .HasForeignKey(attachedMedia => attachedMedia.ChapterReportedId)
            .HasPrincipalKey(chapterReport => chapterReport.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.ArtworkReportedId);
        #endregion
    }
}
