using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class ArtworkReportAttachedMediaEntityConfiguration
    : IEntityConfiguration<ArtworkReportAttachedMedia>
{
    public void Configure(EntityTypeBuilder<ArtworkReportAttachedMedia> builder)
    {
        builder.ToTable("penomy_artwork_report_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.ArtworkReportedId).IsRequired();

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
            .HasOne(attachedMedia => attachedMedia.ArtworkReport)
            .WithMany(artworkReport => artworkReport.AttachedMedias)
            .HasForeignKey(attachedMedia => attachedMedia.ArtworkReportedId)
            .HasPrincipalKey(artworkReport => artworkReport.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.ArtworkReportedId);
        #endregion
    }
}
