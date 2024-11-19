using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkEntityConfiguration : IEntityConfiguration<Artwork>
{
    public void Configure(EntityTypeBuilder<Artwork> builder)
    {
        builder.ToTable("penomy_artwork");

        builder.HasKey(artwork => artwork.Id);

        builder
            .Property(artwork => artwork.Title)
            .HasMaxLength(Artwork.MetaData.TitleLength)
            .IsRequired();

        builder.Property(artwork => artwork.PublicLevel).IsRequired();

        builder
            .Property(artwork => artwork.AuthorName)
            .HasMaxLength(Artwork.MetaData.AuthorNameLength)
            .IsRequired();

        builder.Property(artwork => artwork.HasSeries).IsRequired();

        builder
            .Property(artwork => artwork.ThumbnailUrl)
            .HasMaxLength(Artwork.MetaData.ThumbnailUrlLength)
            .IsRequired();

        builder
            .Property(artwork => artwork.Introduction)
            .HasMaxLength(Artwork.MetaData.IntroductionLength)
            .IsRequired();

        builder.Property(artwork => artwork.LastChapterUploadOrder).IsRequired();

        builder.Property(artwork => artwork.FixedTotalChapters).IsRequired();

        builder.Property(artwork => artwork.ArtworkStatus).IsRequired();

        builder.Property(artwork => artwork.ArtworkType).IsRequired();

        builder.Property(artwork => artwork.ArtworkOriginId).IsRequired();

        builder.Property(artwork => artwork.AllowComment).IsRequired();

        builder.Property(artwork => artwork.CreatedBy).IsRequired();

        builder
            .Property(artwork => artwork.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(artwork => artwork.IsCreatedByAuthorizedUser).IsRequired();

        builder.Property(artwork => artwork.UpdatedBy).IsRequired();

        builder
            .Property(artwork => artwork.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(artwork => artwork.TemporarilyRemovedBy).IsRequired();

        builder
            .Property(artwork => artwork.TemporarilyRemovedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(artwork => artwork.IsTemporarilyRemoved).IsRequired();

        builder.Property(artwork => artwork.IsTakenDown).IsRequired();

        #region Relationships
        builder
            .HasOne(artwork => artwork.Origin)
            .WithMany(origin => origin.Artworks)
            .HasForeignKey(artwork => artwork.ArtworkOriginId)
            .HasPrincipalKey(origin => origin.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artwork => artwork.Creator)
            .WithMany(creator => creator.CreatedArtworks)
            .HasForeignKey(artwork => artwork.CreatedBy)
            .HasPrincipalKey(creator => creator.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artwork => artwork.Updater)
            .WithMany(updater => updater.UpdatedArtworks)
            .HasForeignKey(artwork => artwork.UpdatedBy)
            .HasPrincipalKey(updater => updater.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artwork => artwork.Remover)
            .WithMany(remover => remover.TemporarilyRemovedArtworks)
            .HasForeignKey(artwork => artwork.TemporarilyRemovedBy)
            .HasPrincipalKey(remover => remover.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(artwork => artwork.CreatedBy);

        // builder.HasIndex(artwork => artwork.ArtworkOriginId);
        #endregion
    }
}
