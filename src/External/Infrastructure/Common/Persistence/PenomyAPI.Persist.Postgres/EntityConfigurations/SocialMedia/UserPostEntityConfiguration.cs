using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserPostEntityConfiguration : IEntityConfiguration<UserPost>
{
    public void Configure(EntityTypeBuilder<UserPost> builder)
    {
        builder.ToTable("penomy_user_post");

        builder.HasKey(post => post.Id);

        builder
            .Property(post => post.Content)
            .HasMaxLength(UserPost.MetaData.ContentLength)
            .IsRequired();

        builder.Property(post => post.TotalLikes).IsRequired();

        builder.Property(post => post.PublicLevel).IsRequired();

        builder.Property(post => post.AllowComment).IsRequired();

        builder.Property(post => post.CreatedBy).IsRequired();

        builder
            .Property(post => post.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(post => post.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userPost => userPost.Creator)
            .WithMany(user => user.CreatedUserPosts)
            .HasForeignKey(userPost => userPost.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        //builder.HasIndex(post => new { post.CreatedBy, post.PublicLevel });
        #endregion
    }
}
