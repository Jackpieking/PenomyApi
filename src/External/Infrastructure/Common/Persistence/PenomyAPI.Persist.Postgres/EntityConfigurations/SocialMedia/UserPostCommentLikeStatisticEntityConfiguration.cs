using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserPostCommentLikeStatisticEntityConfiguration
    : IEntityConfiguration<UserPostCommentLikeStatistic>
{
    public void Configure(EntityTypeBuilder<UserPostCommentLikeStatistic> builder)
    {
        builder.ToTable("penomy_user_post_comment_like_statistics");

        builder.HasKey(likeStatistic => new { likeStatistic.CommentId, likeStatistic.ValueId });

        builder.Property(likeStatistic => likeStatistic.Total).IsRequired();

        #region Relationships
        builder
            .HasOne(likeStatistic => likeStatistic.UserPostComment)
            .WithMany(comment => comment.LikeStatistics)
            .HasForeignKey(likeStatistic => likeStatistic.CommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(likeStatistic => likeStatistic.LikeValue)
            .WithMany(likeValue => likeValue.UserPostCommentLikeStatistics)
            .HasForeignKey(likeStatistic => likeStatistic.CommentId)
            .HasPrincipalKey(likeValue => likeValue.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
