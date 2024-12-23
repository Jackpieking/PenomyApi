using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserLikeGroupPostEntityConfiguration : IEntityConfiguration<UserLikeGroupPost>
{
    public void Configure(EntityTypeBuilder<UserLikeGroupPost> builder)
    {
        builder.ToTable("penomy_user_like_group_post");

        builder.HasKey(userLike => new { userLike.PostId, userLike.UserId });

        builder.Property(post => post.ValueId).IsRequired();

        builder
            .Property(post => post.LikedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userLike => userLike.GroupPost)
            .WithMany(post => post.UserLikes)
            .HasForeignKey(userLike => userLike.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userLike => userLike.User)
            .WithMany(user => user.UserLikeGroupPosts)
            .HasForeignKey(userLike => userLike.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
