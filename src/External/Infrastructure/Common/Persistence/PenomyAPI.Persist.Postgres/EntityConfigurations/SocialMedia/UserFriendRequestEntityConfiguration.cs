using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserFriendRequestEntityConfiguration : IEntityConfiguration<UserFriendRequest>
{
    public void Configure(EntityTypeBuilder<UserFriendRequest> builder)
    {
        builder.ToTable("penomy_user_friend_request");

        builder.HasKey(userFriendRequest => new
        {
            userFriendRequest.CreatedBy,
            userFriendRequest.FriendId
        });

        builder
            .Property(request => request.RequestStatus)
            .IsRequired();

        builder
            .Property(userFriendRequest => userFriendRequest.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(userFriendRequest => userFriendRequest.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Indexes
        // builder.HasIndex(userFriendRequest => userFriendRequest.FriendId);
        #endregion
    }
}
