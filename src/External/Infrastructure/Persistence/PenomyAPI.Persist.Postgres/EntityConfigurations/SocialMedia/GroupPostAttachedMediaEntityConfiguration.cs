using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class GroupPostAttachedMediaEntityConfiguration
    : IEntityConfiguration<GroupPostAttachedMedia>
{
    public void Configure(EntityTypeBuilder<GroupPostAttachedMedia> builder)
    {
        builder.ToTable("penomy_group_post_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.PostId).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(GroupPostAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(media => media.MediaType).IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(GroupPostAttachedMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(attachedMedia => attachedMedia.GroupPost)
            .WithMany(post => post.AttachedMedias)
            .HasForeignKey(attachedMedia => attachedMedia.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.PostId);
        #endregion
    }
}
