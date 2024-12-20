using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostCommentEntityConfiguration : IEntityConfiguration<GroupPostComment>
{
    public void Configure(EntityTypeBuilder<GroupPostComment> builder)
    {
        builder.ToTable("penomy_group_post_comment");

        builder.HasKey(comment => comment.Id);

        builder
            .Property(comment => comment.Content)
            .HasMaxLength(GroupPostComment.MetaData.ContentLength)
            .IsRequired();

        builder.Property(comment => comment.PostId).IsRequired();

        builder.Property(comment => comment.TotalChildComments).IsRequired();

        builder.Property(comment => comment.IsDirectlyCommented).IsRequired();

        builder.Property(comment => comment.CreatedBy).IsRequired();

        builder
            .Property(comment => comment.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(comment => comment.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(comment => comment.IsRemoved).IsRequired();

        #region Relationships
        builder
            .HasOne(comment => comment.Creator)
            .WithMany(user => user.CreatedGroupPostComments)
            .HasForeignKey(comment => comment.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(comment => comment.GroupPost)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(comment => new
        // {
        //     comment.PostId,
        //     comment.IsDirectlyCommented,
        //     comment.CreatedBy
        // });
        #endregion
    }
}
