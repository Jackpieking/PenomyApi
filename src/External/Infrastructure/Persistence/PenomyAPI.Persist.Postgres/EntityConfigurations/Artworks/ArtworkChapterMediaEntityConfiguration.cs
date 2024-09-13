using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

public sealed class ArtworkChapterMediaEntityConfiguration
    : IEntityConfiguration<ArtworkChapterMedia>
{
    public void Configure(EntityTypeBuilder<ArtworkChapterMedia> builder)
    {
        builder.ToTable("penomy_artwork_chapter_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.ChapterId).IsRequired();

        builder.Property(media => media.UploadOrder).IsRequired();

        builder.Property(media => media.MediaType).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(ArtworkChapterMedia.MetaData.FileNameLength)
            .IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(ArtworkChapterMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(media => media.ArtworkChapter)
            .WithMany(chapter => chapter.ChapterMedias)
            .HasForeignKey(media => media.ChapterId)
            .HasPrincipalKey(chapter => chapter.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.ChapterId);
        #endregion
    }
}
