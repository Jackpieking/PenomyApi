using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class UserReportEntityConfiguration : IEntityConfiguration<UserReport>
{
    public void Configure(EntityTypeBuilder<UserReport> builder)
    {
        builder.ToTable("penomy_user_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.ReportedUserId).IsRequired();

        builder.Property(report => report.ReportProblemId).IsRequired();

        builder
            .Property(report => report.DetailNote)
            .HasMaxLength(UserReport.MetaData.DetailNoteLength)
            .IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        // This configuration is applied for user from a reporter side.
        builder
            .HasOne(userReport => userReport.Reporter)
            .WithMany(reporter => reporter.CreatedUserReports)
            .HasForeignKey(userReport => userReport.CreatedBy)
            .HasPrincipalKey(reporter => reporter.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // This configuration is applied for user when received the reports.
        builder
            .HasOne(userReport => userReport.ReportedUser)
            .WithMany(user => user.ReceivedUserReports)
            .HasForeignKey(userReport => userReport.ReportedUserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(userReport => userReport.ReportProblem)
            .WithMany(problem => problem.UserReports)
            .HasForeignKey(userReport => userReport.ReportProblemId)
            .HasPrincipalKey(problem => problem.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => report.ReportedUserId);

        // builder.HasIndex(report => report.CreatedBy);
        #endregion
    }
}
