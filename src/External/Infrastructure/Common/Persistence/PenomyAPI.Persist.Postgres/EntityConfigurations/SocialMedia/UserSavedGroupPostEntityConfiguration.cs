using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserSavedGroupPostEntityConfiguration
    : IEntityConfiguration<UserSavedGroupPost>
{
    public void Configure(EntityTypeBuilder<UserSavedGroupPost> builder)
    {
        builder.ToTable("penomy_user_saved_group_post");

        builder.HasKey(savedPost => new { savedPost.UserId, savedPost.PostId });

        builder
            .Property(savedPost => savedPost.SavedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(savedPost => savedPost.User)
            .WithMany(user => user.SavedGroupPosts)
            .HasForeignKey(savedPost => savedPost.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(savedPost => savedPost.GroupPost)
            .WithMany(post => post.UserSavedGroupPosts)
            .HasForeignKey(savedPost => savedPost.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(savedPost => savedPost.SavedAt);
        #endregion
    }
}
