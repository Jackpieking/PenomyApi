using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class UserArtworkViewHistoryEntityConfiguration
    : IEntityConfiguration<UserArtworkViewHistory>
{
    public void Configure(EntityTypeBuilder<UserArtworkViewHistory> builder)
    {
        builder.ToTable("penomy_user_artwork_view_history");

        builder.HasKey(viewHistory => new
        {
            viewHistory.UserId,
            viewHistory.ArtworkId,
            viewHistory.ChapterId
        });

        builder
            .Property(viewHistory => viewHistory.ViewedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(viewHistory => viewHistory.ArtworkType)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(viewHistory => viewHistory.Artwork)
            .WithMany(artwork => artwork.UserArtworkViewHistories)
            .HasForeignKey(viewHistory => viewHistory.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(viewHistory => viewHistory.Chapter)
            .WithMany(chapter => chapter.UserChapterViewHistories)
            .HasForeignKey(viewHistory => viewHistory.ChapterId)
            .HasPrincipalKey(chapter => chapter.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.Property(viewHistory => viewHistory.ViewedAt);
        #endregion
    }
}
