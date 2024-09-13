using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

public class UserSavedUserPostEntityConfiguration : IEntityConfiguration<UserSavedUserPost>
{
    public void Configure(EntityTypeBuilder<UserSavedUserPost> builder)
    {
        builder.ToTable("penomy_user_saved_user_posts");

        builder.HasKey(savedPost => new { savedPost.UserId, savedPost.PostId });

        builder
            .Property(savedPost => savedPost.SavedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(savedPost => savedPost.User)
            .WithMany(user => user.SavedUserPosts)
            .HasForeignKey(savedPost => savedPost.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(savedPost => savedPost.UserPost)
            .WithMany(post => post.UserSavedUserPosts)
            .HasForeignKey(savedPost => savedPost.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(savedPost => savedPost.SavedAt);
        #endregion
    }
}
