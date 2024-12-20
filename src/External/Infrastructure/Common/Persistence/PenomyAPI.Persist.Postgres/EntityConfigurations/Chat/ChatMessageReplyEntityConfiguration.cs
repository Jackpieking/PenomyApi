using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatMessageReplyEntityConfiguration : IEntityConfiguration<ChatMessageReply>
{
    public void Configure(EntityTypeBuilder<ChatMessageReply> builder)
    {
        builder.ToTable("penomy_chat_message_reply");

        builder.HasKey(chatMessageReply => new
        {
            chatMessageReply.RootChatMessageId,
            chatMessageReply.RepliedMessageId,
        });
    }
}
