using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkReportProblemEntityConfiguration
    : IEntityConfiguration<ArtworkReportProblem>
{
    public void Configure(EntityTypeBuilder<ArtworkReportProblem> builder)
    {
        builder.ToTable("penomy_artwork_report_problem");

        builder.HasKey(problem => problem.Id);

        builder
            .Property(problem => problem.Title)
            .HasMaxLength(ArtworkReportProblem.MetaData.TitleLength)
            .IsRequired();

        builder.Property(problem => problem.ProblemSeverity).IsRequired();

        builder
            .Property(problem => problem.Description)
            .HasMaxLength(ArtworkReportProblem.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(problem => problem.CreatedBy).IsRequired();

        builder
            .Property(problem => problem.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(problem => problem.Creator)
            .WithMany(systemAccount => systemAccount.CreatedArtworkReportProblems)
            .HasForeignKey(problem => problem.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(problem => problem.CreatedBy);
        #endregion
    }
}
