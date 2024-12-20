using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class UserProfileReportProblemEntityConfiguration :
    IEntityConfiguration<UserProfileReportProblem>
{
    public void Configure(EntityTypeBuilder<UserProfileReportProblem> builder)
    {
        builder.ToTable("penomy_user_profile_report_problem");

        builder.HasKey(problem => problem.Id);

        builder
            .Property(problem => problem.Title)
            .HasMaxLength(UserProfileReportProblem.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(problem => problem.Description)
            .HasMaxLength(UserProfileReportProblem.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(problem => problem.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(problem => problem.Creator)
            .WithMany(systemAccount => systemAccount.CreatedUserProfileReportProblems)
            .HasForeignKey(problem => problem.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(problem => problem.CreatedBy);
        #endregion
    }
}
