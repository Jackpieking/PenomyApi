using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupReportProblemEntityConfiguration
    : IEntityConfiguration<SocialGroupReportProblem>
{
    public void Configure(EntityTypeBuilder<SocialGroupReportProblem> builder)
    {
        builder.ToTable("penomy_social_group_report_problem");

        builder.HasKey(problem => problem.Id);

        builder
            .Property(problem => problem.Title)
            .HasMaxLength(SocialGroupReportProblem.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(problem => problem.Description)
            .HasMaxLength(SocialGroupReportProblem.MetaData.DescriptionLength)
            .IsRequired();

        builder
            .Property(problem => problem.ProblemSeverity)
            .IsRequired();

        builder
            .Property(problem => problem.CreatedBy)
            .IsRequired();

        builder
            .Property(problem => problem.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(problem => problem.Creator)
            .WithMany(creator => creator.CreatedSocialGroupReportProblems)
            .HasPrincipalKey(creator => creator.Id)
            .HasForeignKey(problem => problem.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
