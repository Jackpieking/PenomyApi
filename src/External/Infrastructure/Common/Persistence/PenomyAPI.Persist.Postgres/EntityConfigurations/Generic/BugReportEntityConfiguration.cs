using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class BugReportEntityConfiguration : IEntityConfiguration<BugReport>
{
    public void Configure(EntityTypeBuilder<BugReport> builder)
    {
        builder.ToTable("penomy_bug_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.CreatedBy).IsRequired();

        builder.Property(report => report.BugTypeId).IsRequired();

        builder
            .Property(report => report.Title)
            .HasMaxLength(BugReport.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(report => report.Overview)
            .HasMaxLength(BugReport.MetaData.OverviewLength)
            .IsRequired();

        builder
            .Property(report => report.UserDetailNote)
            .HasMaxLength(BugReport.MetaData.UserDetailNoteLength)
            .IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(report => report.ResolveStatus).IsRequired();

        builder
            .Property(report => report.ResolveNote)
            .HasMaxLength(BugReport.MetaData.ResolveNoteLength)
            .IsRequired();

        builder.Property(report => report.ResolvedBy).IsRequired();

        builder
            .Property(report => report.ResolvedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(report => report.Creator)
            .WithMany(bugType => bugType.CreatedBugReports)
            .HasForeignKey(report => report.CreatedBy)
            .HasPrincipalKey(bugType => bugType.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.BugType)
            .WithMany(bugType => bugType.BugReports)
            .HasForeignKey(report => report.BugTypeId)
            .HasPrincipalKey(bugType => bugType.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(bugReport => bugReport.Resolver)
            .WithMany(systemAccount => systemAccount.ResolvedBugReports)
            .HasForeignKey(bugReport => bugReport.ResolvedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => report.CreatedBy);
        #endregion
    }
}
