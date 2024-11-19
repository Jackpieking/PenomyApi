using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class UserRatingArtworkEntityConfiguration : IEntityConfiguration<UserRatingArtwork>
{
    public void Configure(EntityTypeBuilder<UserRatingArtwork> builder)
    {
        builder.ToTable("penomy_user_rating_artwork");

        builder.HasKey(userRating => new { userRating.UserId, userRating.ArtworkId });

        builder.Property(userRating => userRating.StarRates).IsRequired();

        builder
            .Property(userRating => userRating.RatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userRating => userRating.RatedArtwork)
            .WithMany(artwork => artwork.UserRatingArtworks)
            .HasForeignKey(userRating => userRating.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
