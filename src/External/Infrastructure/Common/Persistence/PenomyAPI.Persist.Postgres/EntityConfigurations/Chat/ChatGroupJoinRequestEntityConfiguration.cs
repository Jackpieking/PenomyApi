using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatGroupJoinRequestEntityConfiguration
    : IEntityConfiguration<ChatGroupJoinRequest>
{
    public void Configure(EntityTypeBuilder<ChatGroupJoinRequest> builder)
    {
        builder.ToTable("penomy_chat_group_join_request");

        builder.HasKey(chatGroupJoinRequest => new
        {
            chatGroupJoinRequest.ChatGroupId,
            chatGroupJoinRequest.CreatedBy
        });

        builder
            .Property(chatGroupJoinRequest => chatGroupJoinRequest.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(joinRequest => joinRequest.ChatGroup)
            .WithMany(chatGroup => chatGroup.ChatGroupJoinRequests)
            .HasForeignKey(joinRequest => joinRequest.ChatGroupId)
            .HasPrincipalKey(chatGroup => chatGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(joinRequest => joinRequest.Creator)
            .WithMany(user => user.ChatGroupJoinRequests)
            .HasForeignKey(joinRequest => joinRequest.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(request => request.CreatedBy);
        #endregion
    }
}
