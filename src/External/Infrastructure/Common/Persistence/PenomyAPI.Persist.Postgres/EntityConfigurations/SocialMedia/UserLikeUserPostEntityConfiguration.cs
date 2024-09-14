using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserLikeUserPostEntityConfiguration : IEntityConfiguration<UserLikeUserPost>
{
    public void Configure(EntityTypeBuilder<UserLikeUserPost> builder)
    {
        builder.ToTable("penomy_user_like_user_post");

        builder.HasKey(userLike => new { userLike.PostId, userLike.UserId });

        builder.Property(userLike => userLike.Value).IsRequired();

        builder
            .Property(userLike => userLike.LikedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userLike => userLike.User)
            .WithMany(user => user.UserLikeUserPosts)
            .HasForeignKey(userLike => userLike.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userLike => userLike.UserPost)
            .WithMany(post => post.UserLikes)
            .HasForeignKey(userLike => userLike.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
