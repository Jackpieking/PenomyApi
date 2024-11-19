using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkCommentParentChildEntityConfiguration
    : IEntityConfiguration<ArtworkCommentParentChild>
{
    public void Configure(EntityTypeBuilder<ArtworkCommentParentChild> builder)
    {
        builder.ToTable("penomy_artwork_comment_parent_child");

        builder.HasKey(comment => new { comment.ParentCommentId, comment.ChildCommentId });

        #region Relationships
        builder
            .HasOne(ArtworkCommentParentChild => ArtworkCommentParentChild.ParentComment)
            .WithMany(artworkComment => artworkComment.ArtworkCommentParentChilds)
            .HasForeignKey(ArtworkCommentParentChild => ArtworkCommentParentChild.ParentCommentId)
            .HasPrincipalKey(artworkComment => artworkComment.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
