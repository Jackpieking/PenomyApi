using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupJoinRequestEntityConfiguration
    : IEntityConfiguration<SocialGroupJoinRequest>
{
    public void Configure(EntityTypeBuilder<SocialGroupJoinRequest> builder)
    {
        builder.ToTable("penomy_social_group_join_request");

        builder.HasKey(request => new { request.GroupId, request.CreatedBy });

        builder
            .Property(request => request.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ);

        #region Relationships
        builder
            .HasOne(joinRequest => joinRequest.Creator)
            .WithMany(user => user.SocialGroupJoinRequests)
            .HasForeignKey(joinRequest => joinRequest.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(joinRequest => joinRequest.Group)
            .WithMany(socialGroup => socialGroup.SocialGroupJoinRequests)
            .HasForeignKey(joinRequest => joinRequest.GroupId)
            .HasPrincipalKey(socialGroup => socialGroup.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(request => request.CreatedBy);
        #endregion
    }
}
