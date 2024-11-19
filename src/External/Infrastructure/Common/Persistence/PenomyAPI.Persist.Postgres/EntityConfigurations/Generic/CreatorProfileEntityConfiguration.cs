using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class CreatorProfileEntityConfiguration : IEntityConfiguration<CreatorProfile>
{
    public void Configure(EntityTypeBuilder<CreatorProfile> builder)
    {
        builder.ToTable("penomy_creator_profile");

        builder.HasKey(profile => profile.CreatorId);

        builder.Property(profile => profile.TotalFollowers).IsRequired();

        builder.Property(profile => profile.TotalArtworks).IsRequired();

        builder.Property(profile => profile.ReportedCount).IsRequired();

        builder
            .Property(profile => profile.RegisteredAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(creatorProfile => creatorProfile.ProfileOwner)
            .WithOne(profileOwner => profileOwner.CreatorProfile)
            .HasPrincipalKey<UserProfile>(profileOwner => profileOwner.UserId)
            .HasForeignKey<CreatorProfile>(creatorProfile => creatorProfile.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
