using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

public class UserChatGroupActiveHistoryEntityConfiguration
    : IEntityConfiguration<UserChatGroupActiveHistory>
{
    public void Configure(EntityTypeBuilder<UserChatGroupActiveHistory> builder)
    {
        builder.ToTable("penomy_user_chat_group_active_history");

        builder.HasKey(activeHistory => new { activeHistory.UserId, activeHistory.ChatGroupId });

        builder.Property(activeHistory => activeHistory.LastUpdatedAt).IsRequired();

        #region Relationships
        builder
            .HasOne(chatGroupActiveHistory => chatGroupActiveHistory.User)
            .WithMany(user => user.ChatGroupActiveHistories)
            .HasForeignKey(activeHistory => activeHistory.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(chatGroupActiveHistory => chatGroupActiveHistory.ChatGroup)
            .WithMany(chatGroup => chatGroup.ChatGroupActiveHistories)
            .HasForeignKey(activeHistory => activeHistory.ChatGroupId)
            .HasPrincipalKey(chatGroup => chatGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(activeHistory => activeHistory.LastUpdatedAt);
        #endregion
    }
}
