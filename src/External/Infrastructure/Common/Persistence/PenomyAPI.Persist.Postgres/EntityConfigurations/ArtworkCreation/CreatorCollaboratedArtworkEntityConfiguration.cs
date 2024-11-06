using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class CreatorCollaboratedArtworkEntityConfiguration
    : IEntityConfiguration<CreatorCollaboratedArtwork>
{
    public void Configure(EntityTypeBuilder<CreatorCollaboratedArtwork> builder)
    {
        builder.ToTable("penomy_creator_collaborated_artwork");

        builder.HasKey(creatorCollaboratedArtwork => new
        {
            creatorCollaboratedArtwork.CreatorId,
            creatorCollaboratedArtwork.ArtworkId,
            creatorCollaboratedArtwork.RoleId,
            creatorCollaboratedArtwork.GrantedBy
        });

        builder
            .Property(chapter => chapter.GrantedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(creatorCollaboratedArtwork => creatorCollaboratedArtwork.GrantedCreator)
            .WithMany(creator => creator.CollaboratedArtworks)
            .HasForeignKey(creatorCollaboratedArtwork => creatorCollaboratedArtwork.CreatorId)
            .HasPrincipalKey(creator => creator.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(creatorCollaboratedArtwork => creatorCollaboratedArtwork.ManagedArtwork)
            .WithMany(artwork => artwork.UserManagedArtworks)
            .HasForeignKey(creatorCollaboratedArtwork => creatorCollaboratedArtwork.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(creatorCollaboratedArtwork => creatorCollaboratedArtwork.PermissionGrantProvider)
            .WithMany(artworkManager => artworkManager.ManagedArtworks)
            .HasForeignKey(creatorCollaboratedArtwork => creatorCollaboratedArtwork.GrantedBy)
            .HasPrincipalKey(artworkManager => artworkManager.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
