using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserPostCommentParentChildEntityConfiguration
    : IEntityConfiguration<UserPostCommentParentChild>
{
    public void Configure(EntityTypeBuilder<UserPostCommentParentChild> builder)
    {
        builder.ToTable("penomy_user_post_comment_parent_child");

        builder.HasKey(comment => new { comment.ParentCommentId, comment.ChildCommentId });

        #region Relationships
        builder
            .HasOne(commentParentChild => commentParentChild.ParentComment)
            .WithMany(comment => comment.UserPostCommentParentChilds)
            .HasForeignKey(commentParentChild => commentParentChild.ParentCommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
