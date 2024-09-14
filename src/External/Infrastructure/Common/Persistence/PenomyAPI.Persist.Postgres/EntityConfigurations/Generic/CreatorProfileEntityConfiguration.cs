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

        builder.HasKey(profile => profile.UserId);

        builder.Property(profile => profile.TotalFollowers).IsRequired();

        builder.Property(profile => profile.TotalArtworks).IsRequired();

        builder.Property(profile => profile.ReportedCount).IsRequired();

        builder
            .Property(profile => profile.RegisteredAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(profile => profile.ProfileOwner)
            .WithOne(user => user.CreatorProfile)
            .HasForeignKey<CreatorProfile>(profile => profile.UserId)
            .HasPrincipalKey<UserProfile>(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
