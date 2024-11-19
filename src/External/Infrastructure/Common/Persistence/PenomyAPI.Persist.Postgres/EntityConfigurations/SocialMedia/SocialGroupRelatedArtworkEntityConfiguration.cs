using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupRelatedArtworkEntityConfiguration
    : IEntityConfiguration<SocialGroupRelatedArtwork>
{
    public void Configure(EntityTypeBuilder<SocialGroupRelatedArtwork> builder)
    {
        builder.ToTable("penomy_social_group_related_artwork");

        builder.HasKey(socialGroupRelatedArtwork => new
        {
            socialGroupRelatedArtwork.GroupId,
            socialGroupRelatedArtwork.ArtworkId
        });

        #region Relationships
        builder
            .HasOne(socialGroupRelatedArtwork => socialGroupRelatedArtwork.Group)
            .WithMany(group => group.SocialGroupRelatedArtworks)
            .HasPrincipalKey(group => group.Id)
            .HasForeignKey(socialGroupRelatedArtwork => socialGroupRelatedArtwork.GroupId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(socialGroupRelatedArtwork => socialGroupRelatedArtwork.Artwork)
            .WithMany(artwork => artwork.SocialGroupRelatedArtworks)
            .HasPrincipalKey(artwork => artwork.Id)
            .HasForeignKey(socialGroupRelatedArtwork => socialGroupRelatedArtwork.ArtworkId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
