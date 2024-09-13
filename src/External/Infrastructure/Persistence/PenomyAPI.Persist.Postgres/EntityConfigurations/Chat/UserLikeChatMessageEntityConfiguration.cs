using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class UserLikeChatMessageEntityConfiguration
    : IEntityConfiguration<UserLikeChatMessage>
{
    public void Configure(EntityTypeBuilder<UserLikeChatMessage> builder)
    {
        builder.ToTable("penomy_user_like_chat_message");

        builder.HasKey(userLikeChatMessage => new
        {
            userLikeChatMessage.ChatMessageId,
            userLikeChatMessage.UserId
        });

        builder.Property(userLikeChatMessage => userLikeChatMessage.Value).IsRequired();

        builder
            .Property(report => report.LikedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userLike => userLike.User)
            .WithMany(user => user.UserLikeChatMessages)
            .HasForeignKey(userLike => userLike.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userLike => userLike.LikedChatMessage)
            .WithMany(chatMessage => chatMessage.UserLikes)
            .HasForeignKey(userLike => userLike.ChatMessageId)
            .HasPrincipalKey(chatMessage => chatMessage.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
