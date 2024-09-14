using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostCommentAttachedMediaEntityConfiguration
    : IEntityConfiguration<GroupPostCommentAttachedMedia>
{
    public void Configure(EntityTypeBuilder<GroupPostCommentAttachedMedia> builder)
    {
        builder.ToTable("penomy_group_post_comment_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.CommentId).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(GroupPostCommentAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(media => media.MediaType).IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(GroupPostCommentAttachedMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(attachedMedia => attachedMedia.Comment)
            .WithMany(comment => comment.AttachedMedias)
            .HasForeignKey(attachedMedia => attachedMedia.CommentId)
            .HasPrincipalKey(comment => comment.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.CommentId);
        #endregion
    }
}
