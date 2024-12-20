using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class SeriesEntityConfiguration : IEntityConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.ToTable("penomy_series");

        builder.HasKey(series => series.Id);

        builder
            .Property(series => series.Title)
            .HasMaxLength(Series.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(artwork => artwork.Description)
            .HasMaxLength(Series.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(series => series.LastItemOrder).IsRequired();

        builder
            .Property(series => series.ThumbnailUrl)
            .HasMaxLength(Series.MetaData.ThumbnailUrlLength)
            .IsRequired();

        builder.Property(series => series.CreatedBy).IsRequired();

        builder
            .Property(series => series.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(series => series.UpdatedBy).IsRequired();

        builder
            .Property(series => series.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(series => series.TemporarilyRemovedBy).IsRequired();

        builder
            .Property(series => series.TemporarilyRemovedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(series => series.IsTemporarilyRemoved).IsRequired();

        #region Relationships
        builder
            .HasOne(series => series.Creator)
            .WithMany(creator => creator.CreatedSeries)
            .HasForeignKey(series => series.CreatedBy)
            .HasPrincipalKey(creator => creator.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(series => series.Updater)
            .WithMany(updater => updater.UpdatedSeries)
            .HasForeignKey(series => series.UpdatedBy)
            .HasPrincipalKey(updater => updater.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(series => series.Remover)
            .WithMany(remover => remover.TemporarilyRemovedSeries)
            .HasForeignKey(series => series.TemporarilyRemovedBy)
            .HasPrincipalKey(remover => remover.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(series => series.CreatedBy);
        #endregion
    }
}
