using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class UserFollowedCreatorEntityConfiguration
    : IEntityConfiguration<UserFollowedCreator>
{
    public void Configure(EntityTypeBuilder<UserFollowedCreator> builder)
    {
        builder.ToTable("penomy_user_followed_creator");

        builder.HasKey(userFollowedCreator => new
        {
            userFollowedCreator.UserId,
            userFollowedCreator.CreatorId
        });

        builder
            .Property(userFollowedCreator => userFollowedCreator.StartedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        // This configuration is applied for user side.
        builder
            .HasOne(userFollow => userFollow.User)
            .WithMany(user => user.FollowedCreators)
            .HasForeignKey(userFollow => userFollow.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // This configuration is applied for creator side.
        builder
            .HasOne(userFollow => userFollow.Creator)
            .WithMany(creator => creator.Followers)
            .HasForeignKey(userFollow => userFollow.CreatorId)
            .HasPrincipalKey(creator => creator.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(userFollowedCreator => userFollowedCreator.CreatorId);
        #endregion
    }
}
