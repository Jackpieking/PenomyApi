using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostCommentLikeStatisticEntityConfiguration
    : IEntityConfiguration<GroupPostCommentLikeStatistic>
{
    public void Configure(EntityTypeBuilder<GroupPostCommentLikeStatistic> builder)
    {
        builder.ToTable("penomy_group_post_comment_like_statistic");

        builder.HasKey(likeStatistic => new { likeStatistic.CommentId, likeStatistic.Value });

        builder.Property(likeStatistic => likeStatistic.Total).IsRequired();

        #region Relationships
        builder
            .HasOne(likeStatistic => likeStatistic.GroupPostComment)
            .WithMany(comment => comment.LikeStatistics)
            .HasForeignKey(likeStatistic => likeStatistic.CommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
