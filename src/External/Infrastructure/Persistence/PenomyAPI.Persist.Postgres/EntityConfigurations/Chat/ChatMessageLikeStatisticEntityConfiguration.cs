using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatMessageLikeStatisticEntityConfiguration
    : IEntityConfiguration<ChatMessageLikeStatistic>
{
    public void Configure(EntityTypeBuilder<ChatMessageLikeStatistic> builder)
    {
        builder.ToTable("penomy_chat_message_like_statistic");

        builder.HasKey(likeStatistic => new { likeStatistic.ChatMessageId, likeStatistic.Value });

        #region Relationships
        builder
            .HasOne(likeStatistic => likeStatistic.ChatMessage)
            .WithMany(chatMessage => chatMessage.LikeStatistics)
            .HasForeignKey(likeStatistic => likeStatistic.ChatMessageId)
            .HasPrincipalKey(chatMessage => chatMessage.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
