using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkBugReportAttachedMediaEntityConfiguration
    : IEntityConfiguration<ArtworkBugReportAttachedMedia>
{
    public void Configure(EntityTypeBuilder<ArtworkBugReportAttachedMedia> builder)
    {
        builder.ToTable("penomy_artwork_bug_report_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.BugReportId).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(ArtworkBugReportAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(media => media.UploadOrder).IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(ArtworkBugReportAttachedMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(attachedMedia => attachedMedia.BugReport)
            .WithMany(artworkBugReport => artworkBugReport.AttachedMedias)
            .HasForeignKey(attachedMedia => attachedMedia.BugReportId)
            .HasPrincipalKey(artworkBugReport => artworkBugReport.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        //builder.HasIndex(media => media.BugReportId);
        #endregion
    }
}
