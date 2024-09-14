using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class UserBanEntityConfiguration : IEntityConfiguration<UserBan>
{
    public void Configure(EntityTypeBuilder<UserBan> builder)
    {
        builder.ToTable("penomy_user_ban");

        builder.HasKey(userBan => userBan.Id);

        builder.Property(userBan => userBan.UserId).IsRequired();

        builder.Property(userBan => userBan.BanTypeId).IsRequired();

        builder.Property(userBan => userBan.BannedBy).IsRequired();

        builder.Property(userBan => userBan.IsActive).IsRequired();

        builder
            .Property(userBan => userBan.StartedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(userBan => userBan.EndedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userBan => userBan.BannedUser)
            .WithMany(user => user.UserBans)
            .HasForeignKey(userBan => userBan.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userBan => userBan.BanType)
            .WithMany(banType => banType.UserBans)
            .HasForeignKey(userBan => userBan.BanTypeId)
            .HasPrincipalKey(banType => banType.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userBan => userBan.Creator)
            .WithMany(systemAccount => systemAccount.UserBans)
            .HasForeignKey(userBan => userBan.BannedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(userBan => new { userBan.UserId, userBan.IsActive });
        #endregion
    }
}
