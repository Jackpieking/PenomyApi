using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class GuestTrackingEntityConfiguration
    : IEntityConfiguration<GuestTracking>
{
    public void Configure(EntityTypeBuilder<GuestTracking> builder)
    {
        builder.ToTable("penomy_guest_tracking");

        builder.HasKey(guestTracking => guestTracking.GuestId);

        builder
            .Property(guestTracking => guestTracking.LastActiveAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();
    }
}
