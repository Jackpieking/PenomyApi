using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupMemberEntityConfiguration : IEntityConfiguration<SocialGroupMember>
{
    public void Configure(EntityTypeBuilder<SocialGroupMember> builder)
    {
        builder.ToTable("penomy_social_group_member");

        builder.HasKey(member => new
        {
            member.GroupId,
            member.MemberId,
            member.RoleId
        });

        builder
            .Property(member => member.JoinedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(socialGroupMember => socialGroupMember.Member)
            .WithMany(member => member.JoinedSocialGroupMembers)
            .HasForeignKey(socialGroupMember => socialGroupMember.MemberId)
            .HasPrincipalKey(member => member.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(socialGroupMember => socialGroupMember.Group)
            .WithMany(group => group.GroupMembers)
            .HasForeignKey(socialGroupMember => socialGroupMember.GroupId)
            .HasPrincipalKey(group => group.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // This index help to search members faster for a specific role in a group.
        // builder.HasIndex(member => new { member.MemberId, member.RoleId });
        #endregion
    }
}
