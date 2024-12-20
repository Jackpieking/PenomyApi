using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkMetaDataEntityConfiguration : IEntityConfiguration<ArtworkMetaData>
{
    public void Configure(EntityTypeBuilder<ArtworkMetaData> builder)
    {
        builder.ToTable("penomy_artwork_metadata");

        builder.HasKey(metadata => metadata.ArtworkId);

        builder.Property(metadata => metadata.TotalViews).IsRequired();

        builder.Property(metadata => metadata.TotalComments).IsRequired();

        builder.Property(metadata => metadata.TotalFollowers).IsRequired();

        builder.Property(metadata => metadata.TotalFavorites).IsRequired();

        builder.Property(metadata => metadata.TotalStarRates).IsRequired();

        builder.Property(metadata => metadata.TotalUsersRated).IsRequired();

        builder.Property(metadata => metadata.AverageStarRate).IsRequired();

        builder.Property(metadata => metadata.HasFanGroup).IsRequired();

        builder.Property(metadata => metadata.HasAdRevenueEnabled).IsRequired();

        #region Relationships
        builder
            .HasOne(metadata => metadata.Artwork)
            .WithOne(artwork => artwork.ArtworkMetaData)
            .HasForeignKey<ArtworkMetaData>(metadata => metadata.ArtworkId)
            .HasPrincipalKey<Artwork>(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
