using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatMessageEntityConfiguration : IEntityConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("penomy_chat_message");

        builder.HasKey(chatMessage => chatMessage.Id);

        builder
            .Property(chatGroup => chatGroup.Content)
            .HasMaxLength(ChatMessage.MetaData.ContentLength)
            .IsUnicode(true);

        builder.Property(chatMessage => chatMessage.MessageType).IsRequired();

        builder.Property(chatMessage => chatMessage.ChatGroupId).IsRequired();

        builder.Property(chatMessage => chatMessage.ReplyToAnotherMessage).IsRequired();

        builder.Property(chatMessage => chatMessage.IsRevoked).IsRequired();

        builder.Property(chatMessage => chatMessage.CreatedBy).IsRequired();

        builder
            .Property(chatGroup => chatGroup.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(chatMessage => chatMessage.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(chatMessage => chatMessage.ChatGroup)
            .WithMany(chatGroup => chatGroup.ChatMessages)
            .HasForeignKey(chatMessage => chatMessage.ChatGroupId)
            .HasPrincipalKey(chatGroup => chatGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(chatMessage => chatMessage.Sender)
            .WithMany(sender => sender.ChatMessages)
            .HasForeignKey(chatMessage => chatMessage.CreatedBy)
            .HasPrincipalKey(sender => sender.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(chatMessage => new { chatMessage.ChatGroupId, chatMessage.CreatedBy });
        #endregion
    }
}
