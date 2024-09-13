using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class ArtworkSeriesEntityConfiguration : IEntityConfiguration<ArtworkSeries>
{
    public void Configure(EntityTypeBuilder<ArtworkSeries> builder)
    {
        builder.ToTable("penomy_artwork_series");

        builder.HasKey(artworkSeries => new { artworkSeries.SeriesId, artworkSeries.ArtworkId });

        builder.Property(artworkSeries => artworkSeries.ItemOrder).IsRequired();

        #region Relationships
        builder
            .HasOne(artworkSeries => artworkSeries.Artwork)
            .WithMany(artwork => artwork.ArtworkSeries)
            .HasForeignKey(artworkSeries => artworkSeries.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artworkSeries => artworkSeries.Series)
            .WithMany(series => series.ArtworkSeries)
            .HasForeignKey(artworkSeries => artworkSeries.SeriesId)
            .HasPrincipalKey(series => series.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(artworkSeries => artworkSeries.ArtworkId);
        #endregion
    }
}
