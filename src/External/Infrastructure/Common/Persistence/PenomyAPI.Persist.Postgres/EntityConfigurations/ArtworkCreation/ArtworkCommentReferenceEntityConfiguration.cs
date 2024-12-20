using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkCommentReferenceEntityConfiguration
    : IEntityConfiguration<ArtworkCommentReference>
{
    public void Configure(EntityTypeBuilder<ArtworkCommentReference> builder)
    {
        builder.ToTable("penomy_artwork_comment_reference");

        builder.HasKey(reference => new { reference.CommentId, reference.ArtworkId });

        #region Relationships
        builder
            .HasOne(commentReference => commentReference.Comment)
            .WithMany(comment => comment.ArtworkCommentReferences)
            .HasForeignKey(commentReference => commentReference.CommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(commentReference => commentReference.ReferencedArtwork)
            .WithMany(artwork => artwork.CommentReferences)
            .HasForeignKey(commentReference => commentReference.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
