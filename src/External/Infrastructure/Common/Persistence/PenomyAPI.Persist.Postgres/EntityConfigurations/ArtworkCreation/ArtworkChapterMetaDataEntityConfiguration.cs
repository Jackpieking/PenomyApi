using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkChapterMetaDataEntityConfiguration
    : IEntityConfiguration<ArtworkChapterMetaData>
{
    public void Configure(EntityTypeBuilder<ArtworkChapterMetaData> builder)
    {
        builder.ToTable("penomy_artwork_chapter_metadata");

        builder.HasKey(metadata => metadata.ChapterId);

        builder.Property(metadata => metadata.TotalViews).IsRequired();

        builder.Property(metadata => metadata.TotalFavorites).IsRequired();

        builder.Property(metadata => metadata.TotalComments).IsRequired();

        builder.Property(metadata => metadata.HasAdRevenueEnabled).IsRequired();
        #region Relationships
        builder
            .HasOne(metadata => metadata.ArtworkChapter)
            .WithOne(chapter => chapter.ChapterMetaData)
            .HasPrincipalKey<ArtworkChapter>(chapter => chapter.Id)
            .HasForeignKey<ArtworkChapterMetaData>(metadata => metadata.ChapterId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
