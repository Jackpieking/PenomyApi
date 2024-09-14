using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostLikeStatisticEntityConfiguration
    : IEntityConfiguration<GroupPostLikeStatistic>
{
    public void Configure(EntityTypeBuilder<GroupPostLikeStatistic> builder)
    {
        builder.ToTable("penomy_group_post_like_statistic");

        builder.HasKey(likeStatistic => new { likeStatistic.PostId, likeStatistic.Value });

        builder.Property(likeStatistic => likeStatistic.Total).IsRequired();

        #region Relationships
        builder
            .HasOne(likeStatistic => likeStatistic.GroupPost)
            .WithMany(post => post.LikeStatistics)
            .HasForeignKey(likeStatistic => likeStatistic.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
