using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SystemOperation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SystemOperation;

internal sealed class RegisterWaitingListEntityConfiguration
    : IEntityConfiguration<RegisterWaitingList>
{
    public void Configure(EntityTypeBuilder<RegisterWaitingList> builder)
    {
        builder.ToTable("penomy_register_waiting_list");

        builder.HasKey(waitingList => waitingList.Id);

        builder
            .Property(waitingList => waitingList.Email)
            .HasMaxLength(RegisterWaitingList.MetaData.EmailLength)
            .IsRequired();

        builder
            .Property(waitingList => waitingList.SendMailStatus)
            .IsRequired();

        builder
            .Property(waitingList => waitingList.LastMailSentAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(waitingList => waitingList.NextMailSentAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Indexes
        builder.HasIndex(waitingList => waitingList.Email).IsUnique();
        #endregion
    }
}
