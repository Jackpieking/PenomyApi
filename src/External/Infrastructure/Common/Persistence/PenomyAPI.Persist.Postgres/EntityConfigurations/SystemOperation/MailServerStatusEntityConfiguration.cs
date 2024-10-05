using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SystemOperation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SystemOperation;

internal sealed class MailServerStatusEntityConfiguration
    : IEntityConfiguration<MailServerStatus>
{
    public void Configure(EntityTypeBuilder<MailServerStatus> builder)
    {
        builder.ToTable("penomy_mail_server_status");

        builder.HasKey(mailServerStatus => mailServerStatus.Id);

        builder
            .Property(mailServerStatus => mailServerStatus.MailDomain)
            .HasMaxLength(MailServerStatus.MetaData.MailDomainLength)
            .IsRequired();

        builder
            .Property(mailServerStatus => mailServerStatus.LeftMailQuota)
            .IsRequired();

        builder
            .Property(mailServerStatus => mailServerStatus.ServerStatus)
            .IsRequired();

        #region Indexes
        builder
            .HasIndex(mailServerStatus => mailServerStatus.MailDomain)
            .IsUnique();
        #endregion
    }
}
