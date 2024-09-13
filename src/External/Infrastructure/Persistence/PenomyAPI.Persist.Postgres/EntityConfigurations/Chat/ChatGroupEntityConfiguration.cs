using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Chat;

internal sealed class ChatGroupEntityConfiguration : IEntityConfiguration<ChatGroup>
{
    public void Configure(EntityTypeBuilder<ChatGroup> builder)
    {
        builder.ToTable("penomy_chat_group");

        builder.HasKey(chatGroup => chatGroup.Id);

        builder
            .Property(chatGroup => chatGroup.GroupName)
            .HasMaxLength(ChatGroup.MetaData.GroupNameLength)
            .IsRequired();

        builder
            .Property(chatGroup => chatGroup.CoverPhotoUrl)
            .HasMaxLength(ChatGroup.MetaData.CoverPhotoUrlLength)
            .IsRequired();

        builder.Property(chatGroup => chatGroup.IsPublic).IsRequired();

        builder.Property(chatGroup => chatGroup.TotalMembers).IsRequired();

        builder.Property(chatGroup => chatGroup.CreatedBy).IsRequired();

        builder
            .Property(chatGroup => chatGroup.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(chatGroup => chatGroup.Creator)
            .WithMany(user => user.CreatedChatGroups)
            .HasForeignKey(chatGroup => chatGroup.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(chatGroup => chatGroup.CreatedBy);

        // builder.HasIndex(chatGroup => chatGroup.IsPublic);
        #endregion
    }
}
