using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class SocialGroupLinkedChatGroupEntityConfiguration
    : IEntityConfiguration<SocialGroupLinkedChatGroup>
{
    public void Configure(EntityTypeBuilder<SocialGroupLinkedChatGroup> builder)
    {
        builder.ToTable("penomy_social_group_linked_chat_group");

        builder.HasKey(link => new { link.SocialGroupId, link.ChatGroupId, });

        #region Relationships
        builder
            .HasOne(link => link.LinkedSocialGroup)
            .WithOne(socialGroup => socialGroup.SocialGroupLinkedChatGroup)
            .HasForeignKey<SocialGroupLinkedChatGroup>(link => link.SocialGroupId)
            .HasPrincipalKey<SocialGroup>(socialGroup => socialGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(link => link.LinkedChatGroup)
            .WithOne(chatGroup => chatGroup.SocialGroupLinkedChatGroup)
            .HasForeignKey<SocialGroupLinkedChatGroup>(link => link.ChatGroupId)
            .HasPrincipalKey<ChatGroup>(chatGroup => chatGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
