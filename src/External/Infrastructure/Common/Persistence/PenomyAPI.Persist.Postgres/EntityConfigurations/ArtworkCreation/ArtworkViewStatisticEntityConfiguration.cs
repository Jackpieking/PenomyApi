using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkViewStatisticEntityConfiguration
    : IEntityConfiguration<ArtworkViewStatistic>
{
    public void Configure(EntityTypeBuilder<ArtworkViewStatistic> builder)
    {
        builder.ToTable("penomy_artwork_view_statistic");

        builder.HasKey(viewStatistic => viewStatistic.Id);

        builder.Property(viewStatistic => viewStatistic.ArtworkId).IsRequired();

        builder.Property(viewStatistic => viewStatistic.TotalViews).IsRequired();

        builder
            .Property(viewStatistic => viewStatistic.From)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(viewStatistic => viewStatistic.To)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Indexes
        builder
            .HasIndex(viewStatistic => viewStatistic.ArtworkId)
            .IsCreatedConcurrently(true);
        #endregion
    }
}
