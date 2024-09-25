using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkChapterEntityConfiguration : IEntityConfiguration<ArtworkChapter>
{
    public void Configure(EntityTypeBuilder<ArtworkChapter> builder)
    {
        builder.ToTable("penomy_artwork_chapter");

        builder.HasKey(chapter => chapter.Id);

        builder.Property(chapter => chapter.ArtworkId).IsRequired();

        builder
            .Property(chapter => chapter.Title)
            .HasMaxLength(ArtworkChapter.MetaData.TitleLength)
            .IsRequired();

        builder.Property(chapter => chapter.UploadOrder).IsRequired();

        builder.Property(chapter => chapter.PublicLevel).IsRequired();

        builder
            .Property(chapter => chapter.ThumbnailUrl)
            .HasMaxLength(ArtworkChapter.MetaData.ThumbnailUrlLength)
            .IsRequired();

        builder.Property(chapter => chapter.ChapterStatus).IsRequired();

        builder.Property(chapter => chapter.AllowComment).IsRequired();

        builder.Property(chapter => chapter.TotalViews).IsRequired();

        builder.Property(chapter => chapter.TotalFavorites).IsRequired();

        builder.Property(chapter => chapter.TotalComments).IsRequired();

        builder.Property(chapter => chapter.CreatedBy).IsRequired();

        builder
            .Property(artwork => artwork.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(chapter => chapter.UpdatedBy).IsRequired();

        builder
            .Property(artwork => artwork.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(chapter => chapter.PublishedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(chapter => chapter.TemporarilyRemovedBy).IsRequired();

        builder
            .Property(chapter => chapter.TemporarilyRemovedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(chapter => chapter.IsTemporarilyRemoved).IsRequired();

        #region Relationships
        builder
            .HasOne(chapter => chapter.BelongedArtwork)
            .WithMany(artwork => artwork.Chapters)
            .HasForeignKey(chapter => chapter.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(chapter => chapter.Creator)
            .WithMany(creator => creator.CreatedChapters)
            .HasForeignKey(chapter => chapter.CreatedBy)
            .HasPrincipalKey(creator => creator.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(chapter => chapter.Updater)
            .WithMany(updater => updater.UpdatedChapters)
            .HasForeignKey(chapter => chapter.UpdatedBy)
            .HasPrincipalKey(updater => updater.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(chapter => chapter.Remover)
            .WithMany(remover => remover.TemporarilyRemovedChapters)
            .HasForeignKey(chapter => chapter.TemporarilyRemovedBy)
            .HasPrincipalKey(remover => remover.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(chapter => chapter.ArtworkId);

        // builder.HasIndex(chapter => chapter.CreatedBy);
        #endregion
    }
}
