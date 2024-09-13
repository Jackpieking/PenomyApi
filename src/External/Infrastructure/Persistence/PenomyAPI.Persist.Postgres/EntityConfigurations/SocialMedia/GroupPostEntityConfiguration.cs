using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostEntityConfiguration : IEntityConfiguration<GroupPost>
{
    public void Configure(EntityTypeBuilder<GroupPost> builder)
    {
        builder.ToTable("penomy_group_post");

        builder.HasKey(post => post.Id);

        builder
            .Property(post => post.Content)
            .HasMaxLength(GroupPost.MetaData.ContentLength)
            .IsRequired();

        builder.Property(post => post.TotalLikes).IsRequired();

        builder.Property(post => post.GroupId).IsRequired();

        builder.Property(post => post.AllowComment).IsRequired();

        builder.Property(post => post.IsApproved).IsRequired();

        builder.Property(post => post.ApprovedBy).IsRequired();

        builder
            .Property(post => post.ApprovedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

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
            .HasOne(post => post.Creator)
            .WithMany(user => user.CreatedGroupPosts)
            .HasForeignKey(post => post.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(post => post.Group)
            .WithMany(socialGroup => socialGroup.GroupPosts)
            .HasForeignKey(post => post.GroupId)
            .HasPrincipalKey(socialGroup => socialGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(post => post.Approver)
            .WithMany(approver => approver.ApprovedGroupPosts)
            .HasForeignKey(post => post.ApprovedBy)
            .HasPrincipalKey(approver => approver.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(post => new
        // {
        //     post.GroupId,
        //     post.IsApproved,
        //     post.CreatedBy
        // });
        #endregion
    }
}
