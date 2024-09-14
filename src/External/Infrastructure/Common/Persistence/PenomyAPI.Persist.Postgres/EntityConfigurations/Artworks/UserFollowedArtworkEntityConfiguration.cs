using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class UserFollowedArtworkEntityConfiguration
    : IEntityConfiguration<UserFollowedArtwork>
{
    public void Configure(EntityTypeBuilder<UserFollowedArtwork> builder)
    {
        builder.ToTable("penomy_user_followed_artwork");

        builder.HasKey(userFollowed => new { userFollowed.UserId, userFollowed.ArtworkId });

        builder
            .Property(userFollow => userFollow.StartedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userFollowedArtwork => userFollowedArtwork.FollowedArtwork)
            .WithMany(artwork => artwork.UserFollowedArtworks)
            .HasForeignKey(userFollowedArtwork => userFollowedArtwork.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
