using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class ArtworkStatisticEntityConfiguration : IEntityConfiguration<ArtworkStatistic>
{
    public void Configure(EntityTypeBuilder<ArtworkStatistic> builder)
    {
        builder.ToTable("penomy_artwork_statistic");

        builder.HasKey(statistic => statistic.ArtworkId);

        builder.Property(statistic => statistic.TotalViews).IsRequired();

        builder.Property(statistic => statistic.TotalComments).IsRequired();

        builder.Property(statistic => statistic.TotalFollowers).IsRequired();

        builder.Property(statistic => statistic.TotalFavorites).IsRequired();

        builder.Property(statistic => statistic.TotalStarRates).IsRequired();

        builder.Property(statistic => statistic.TotalUsersRated).IsRequired();

        builder.Property(statistic => statistic.AverageStarRate).IsRequired();

        #region Relationships
        builder
            .HasOne(statistic => statistic.Artwork)
            .WithOne(artwork => artwork.Statistic)
            .HasForeignKey<ArtworkStatistic>(statistic => statistic.ArtworkId)
            .HasPrincipalKey<Artwork>(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
