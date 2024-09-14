using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatMessageAttachMediaEntityConfiguration
    : IEntityConfiguration<ChatMessageAttachedMedia>
{
    public void Configure(EntityTypeBuilder<ChatMessageAttachedMedia> builder)
    {
        builder.ToTable("penomy_chat_message_attached_media");

        builder.HasKey(attachedMedia => attachedMedia.Id);

        builder.Property(attachedMedia => attachedMedia.ChatMessageId).IsRequired();

        builder
            .Property(attachedMedia => attachedMedia.FileName)
            .HasMaxLength(ChatMessageAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(attachedMedia => attachedMedia.MediaType).IsRequired();

        builder.Property(attachedMedia => attachedMedia.UploadOrder).IsRequired();

        builder
            .Property(attachedMedia => attachedMedia.StorageUrl)
            .HasMaxLength(ChatMessageAttachedMedia.MetaData.StorageUrlLength);

        #region Relationships
        builder
            .HasOne(attachedMedia => attachedMedia.ChatMessage)
            .WithMany(chatMessage => chatMessage.AttachedMedias)
            .HasForeignKey(attachedMedia => attachedMedia.ChatMessageId)
            .HasPrincipalKey(chatMessage => chatMessage.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.ChatMessageId);
        #endregion
    }
}
