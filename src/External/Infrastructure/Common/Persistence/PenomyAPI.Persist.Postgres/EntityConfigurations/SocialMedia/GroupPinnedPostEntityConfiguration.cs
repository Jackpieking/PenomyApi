using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPinnedPostEntityConfiguration : IEntityConfiguration<GroupPinnedPost>
{
    public void Configure(EntityTypeBuilder<GroupPinnedPost> builder)
    {
        builder.ToTable("penomy_group_pinned_post");

        builder.HasKey(groupPinnedPost => new
        {
            groupPinnedPost.GroupId,
            groupPinnedPost.PostId,
            groupPinnedPost.PinnedBy
        });

        builder
            .Property(post => post.PinnedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(pinnedPost => pinnedPost.Group)
            .WithMany(socialGroup => socialGroup.GroupPinnedPosts)
            .HasForeignKey(pinnedPost => pinnedPost.GroupId)
            .HasPrincipalKey(socialGroup => socialGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(pinnedPost => pinnedPost.PinnedGroupPost)
            .WithOne(post => post.GroupPinnedPost)
            .HasForeignKey<GroupPinnedPost>(pinnedPost => pinnedPost.PostId)
            .HasPrincipalKey<GroupPost>(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(pinnedPost => pinnedPost.UserWhoPin)
            .WithMany(user => user.GroupPinnedPosts)
            .HasForeignKey(pinnedPost => pinnedPost.PinnedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
