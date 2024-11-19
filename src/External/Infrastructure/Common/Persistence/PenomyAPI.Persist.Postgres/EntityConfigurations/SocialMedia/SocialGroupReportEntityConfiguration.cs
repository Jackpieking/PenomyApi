using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupReportEntityConfiguration : IEntityConfiguration<SocialGroupReport>
{
    public void Configure(EntityTypeBuilder<SocialGroupReport> builder)
    {
        builder.ToTable("penomy_social_group_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.GroupId).IsRequired();

        builder.Property(report => report.ReportProblemId).IsRequired();

        builder
            .Property(report => report.DetailNote)
            .HasMaxLength(SocialGroupReport.MetaData.DetailNoteLength)
            .IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(report => report.ReportProblem)
            .WithMany(problem => problem.SocialGroupReports)
            .HasPrincipalKey(problem => problem.Id)
            .HasForeignKey(report => report.ReportProblemId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.Reporter)
            .WithMany(reporter => reporter.CreatedSocialGroupReports)
            .HasPrincipalKey(reporter => reporter.UserId)
            .HasForeignKey(report => report.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
