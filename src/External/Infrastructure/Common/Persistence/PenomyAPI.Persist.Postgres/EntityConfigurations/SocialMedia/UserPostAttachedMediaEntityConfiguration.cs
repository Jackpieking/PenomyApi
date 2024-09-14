using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserPostAttachedMediaEntityConfiguration
    : IEntityConfiguration<UserPostAttachedMedia>
{
    public void Configure(EntityTypeBuilder<UserPostAttachedMedia> builder)
    {
        builder.ToTable("penomy_user_post_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.PostId).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(UserPostAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(media => media.MediaType).IsRequired();

        builder.Property(media => media.UploadOrder).IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(UserPostAttachedMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(attachedMedia => attachedMedia.UserPost)
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
