using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class GuestArtworkViewHistoryEntityConfiguration
    : IEntityConfiguration<GuestArtworkViewHistory>
{
    public void Configure(EntityTypeBuilder<GuestArtworkViewHistory> builder)
    {
        builder.ToTable("penomy_guest_artwork_view_history");

        builder.HasKey(viewHistory => new
        {
            viewHistory.GuestId,
            viewHistory.ArtworkId
        });

        builder.Property(viewHistory => viewHistory.ChapterId).IsRequired();

        builder.Property(viewHistory => viewHistory.ArtworkType).IsRequired();

        builder
            .Property(viewHistory => viewHistory.ViewedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Indexes
        builder.HasIndex(viewHistory => viewHistory.ArtworkType);
        #endregion
    }
}
