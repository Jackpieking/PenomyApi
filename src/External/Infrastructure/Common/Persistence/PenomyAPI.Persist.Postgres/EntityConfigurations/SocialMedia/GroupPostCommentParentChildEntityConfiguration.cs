using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostCommentParentChildEntityConfiguration
    : IEntityConfiguration<GroupPostCommentParentChild>
{
    public void Configure(EntityTypeBuilder<GroupPostCommentParentChild> builder)
    {
        builder.ToTable("penomy_group_post_comment_parent_child");

        builder.HasKey(comment => new { comment.ParentCommentId, comment.ChildCommentId });

        #region Relationships
        builder
            .HasOne(commentParentChild => commentParentChild.ParentComment)
            .WithMany(comment => comment.GroupPostCommentParentChilds)
            .HasForeignKey(commentParentChild => commentParentChild.ParentCommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
