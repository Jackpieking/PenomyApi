using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class UserManagedArtworkEntityConfiguration
    : IEntityConfiguration<UserManagedArtwork>
{
    public void Configure(EntityTypeBuilder<UserManagedArtwork> builder)
    {
        builder.ToTable("penomy_user_managed_artwork");

        builder.HasKey(userManagedArtwork => new
        {
            userManagedArtwork.UserId,
            userManagedArtwork.ArtworkId,
            userManagedArtwork.RoleId,
            userManagedArtwork.GrantedBy
        });

        builder
            .Property(chapter => chapter.GrantedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userManagedArtwork => userManagedArtwork.GrantedUser)
            .WithMany(user => user.CollaboratedArtworks)
            .HasForeignKey(userManagedArtwork => userManagedArtwork.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userManagedArtwork => userManagedArtwork.ManagedArtwork)
            .WithMany(artwork => artwork.UserManagedArtworks)
            .HasForeignKey(userManagedArtwork => userManagedArtwork.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userManagedArtwork => userManagedArtwork.ArtworkManager)
            .WithMany(artworkManager => artworkManager.ManagedArtworks)
            .HasForeignKey(userManagedArtwork => userManagedArtwork.GrantedBy)
            .HasPrincipalKey(artworkManager => artworkManager.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
