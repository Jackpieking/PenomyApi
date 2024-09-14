using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class UserReportProblemEntityConfiguration : IEntityConfiguration<UserReportProblem>
{
    public void Configure(EntityTypeBuilder<UserReportProblem> builder)
    {
        builder.ToTable("penomy_user_report_problem");

        builder.HasKey(problem => problem.Id);

        builder
            .Property(problem => problem.Title)
            .HasMaxLength(UserReportProblem.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(problem => problem.Description)
            .HasMaxLength(UserReportProblem.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(problem => problem.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(problem => problem.Creator)
            .WithMany(systemAccount => systemAccount.CreatedUserReportProblems)
            .HasForeignKey(problem => problem.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(problem => problem.CreatedBy);
        #endregion
    }
}
