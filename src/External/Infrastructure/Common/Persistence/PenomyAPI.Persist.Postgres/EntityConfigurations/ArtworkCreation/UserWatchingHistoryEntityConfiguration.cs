using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class UserWatchingHistoryEntityConfiguration
    : IEntityConfiguration<UserWatchingHistory>
{
    public void Configure(EntityTypeBuilder<UserWatchingHistory> builder)
    {
        builder.ToTable("penomy_user_watching_history");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.UserId).IsRequired();

        builder.Property(report => report.ArtworkId).IsRequired();

        builder.Property(report => report.ChapterId).IsRequired();

        builder
            .Property(report => report.WatchedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        #endregion
    }
}
