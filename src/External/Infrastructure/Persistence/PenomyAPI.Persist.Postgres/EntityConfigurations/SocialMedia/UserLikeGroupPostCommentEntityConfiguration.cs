using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserLikeGroupPostCommentEntityConfiguration
    : IEntityConfiguration<UserLikeGroupPostComment>
{
    public void Configure(EntityTypeBuilder<UserLikeGroupPostComment> builder)
    {
        builder.ToTable("penomy_user_like_group_post_comment");

        builder.HasKey(userLike => new { userLike.CommentId, userLike.UserId });

        builder.Property(userLike => userLike.Value).IsRequired();

        builder
            .Property(userLike => userLike.LikedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userLike => userLike.LikedComment)
            .WithMany(comment => comment.UserLikes)
            .HasForeignKey(userLike => userLike.CommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userLike => userLike.User)
            .WithMany(user => user.UserLikeGroupPostComments)
            .HasForeignKey(userLike => userLike.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
