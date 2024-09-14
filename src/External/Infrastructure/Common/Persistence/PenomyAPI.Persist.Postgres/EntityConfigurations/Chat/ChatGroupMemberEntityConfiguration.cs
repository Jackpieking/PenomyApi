using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatGroupMemberEntityConfiguration : IEntityConfiguration<ChatGroupMember>
{
    public void Configure(EntityTypeBuilder<ChatGroupMember> builder)
    {
        builder.ToTable("penomy_chat_group_member");

        builder.HasKey(chatGroupMember => new
        {
            chatGroupMember.MemberId,
            chatGroupMember.ChatGroupId
        });

        builder.Property(chatGroupMember => chatGroupMember.RoleId).IsRequired();

        builder
            .Property(chatGroupMember => chatGroupMember.JoinedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(chatGroupMember => chatGroupMember.Member)
            .WithMany(user => user.JoinedChatGroupMembers)
            .HasForeignKey(chatGroupMember => chatGroupMember.MemberId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(chatGroupMember => chatGroupMember.ChatGroup)
            .WithMany(chatGroup => chatGroup.ChatGroupMembers)
            .HasForeignKey(chatGroupMember => chatGroupMember.ChatGroupId)
            .HasPrincipalKey(chatGroup => chatGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(chatGroup => chatGroup.ChatGroupId);

        // builder.HasIndex(chatGroup => chatGroup.RoleId);
        #endregion
    }
}
