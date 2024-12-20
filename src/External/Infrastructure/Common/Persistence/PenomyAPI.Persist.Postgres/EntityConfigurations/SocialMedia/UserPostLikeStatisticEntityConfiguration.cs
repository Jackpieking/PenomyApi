using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserPostLikeStatisticEntityConfiguration
    : IEntityConfiguration<UserPostLikeStatistic>
{
    public void Configure(EntityTypeBuilder<UserPostLikeStatistic> builder)
    {
        builder.ToTable("penomy_user_post_like_statistics");

        builder.HasKey(likeStatistic => new { likeStatistic.PostId, likeStatistic.ValueId });

        builder.Property(likeStatistic => likeStatistic.Total).IsRequired();

        #region Relationships
        builder
            .HasOne(likeStatistic => likeStatistic.UserPost)
            .WithMany(post => post.LikeStatistics)
            .HasForeignKey(likeStatistic => likeStatistic.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
