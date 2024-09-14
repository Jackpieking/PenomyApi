using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class UserProfileEntityConfiguration : IEntityConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("penomy_user_profile");

        builder.HasKey(profile => profile.UserId);

        builder
            .Property(profile => profile.NickName)
            .HasMaxLength(UserProfile.MetaData.NickNameLength)
            .IsRequired();

        builder.Property(profile => profile.Gender).IsRequired();

        builder
            .Property(profile => profile.AvatarUrl)
            .HasMaxLength(UserProfile.MetaData.AvatarUrlLength)
            .IsRequired();

        builder
            .Property(profile => profile.AboutMe)
            .HasMaxLength(UserProfile.MetaData.AboutMeLength)
            .IsRequired();

        builder.Property(profile => profile.RegisterAsCreator).IsRequired();

        builder.Property(profile => profile.TotalFollowedCreators).IsRequired();

        builder
            .Property(profile => profile.LastActiveAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(profile => profile.RegisteredAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(profile => profile.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();
    }
}
