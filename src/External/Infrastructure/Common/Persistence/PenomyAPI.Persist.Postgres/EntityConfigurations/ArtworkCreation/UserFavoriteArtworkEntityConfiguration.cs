using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class UserFavoriteArtworkEntityConfiguration
    : IEntityConfiguration<UserFavoriteArtwork>
{
    public void Configure(EntityTypeBuilder<UserFavoriteArtwork> builder)
    {
        builder.ToTable("penomy_user_favorite_artwork");

        builder.HasKey(userFavorite => new { userFavorite.UserId, userFavorite.ArtworkId });

        builder
            .Property(userFavorite => userFavorite.StartedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(userFavorite => userFavorite.ArtworkType)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userFavorite => userFavorite.FavoriteArtwork)
            .WithMany(artwork => artwork.UserFavoriteArtworks)
            .HasForeignKey(userFavorite => userFavorite.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
