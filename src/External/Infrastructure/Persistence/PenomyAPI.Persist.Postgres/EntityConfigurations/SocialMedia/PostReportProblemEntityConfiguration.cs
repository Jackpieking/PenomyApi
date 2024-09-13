using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class PostReportProblemEntityConfiguration : IEntityConfiguration<PostReportProblem>
{
    public void Configure(EntityTypeBuilder<PostReportProblem> builder)
    {
        builder.ToTable("penomy_post_report_problem");

        builder.HasKey(problem => problem.Id);

        builder
            .Property(report => report.Title)
            .HasMaxLength(PostReportProblem.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(report => report.Description)
            .HasMaxLength(PostReportProblem.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(report => report.ProblemSeverity).IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(reportProblem => reportProblem.Creator)
            .WithMany(systemAccount => systemAccount.CreatedPostReportProblems)
            .HasForeignKey(reportProblem => reportProblem.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => report.CreatedBy);

        // builder.HasIndex(report => report.ProblemSeverity);
        #endregion
    }
}
