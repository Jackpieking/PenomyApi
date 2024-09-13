using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class ArtworkReportProblemEntityConfiguration
    : IEntityConfiguration<ArtworkReportReason>
{
    public void Configure(EntityTypeBuilder<ArtworkReportReason> builder)
    {
        builder.ToTable("penomy_artwork_report_reason");

        builder.HasKey(reason => reason.Id);

        builder
            .Property(reason => reason.Title)
            .HasMaxLength(ArtworkReportReason.MetaData.TitleLength)
            .IsRequired();

        builder.Property(reason => reason.ProblemSeverity).IsRequired();

        builder
            .Property(reason => reason.Description)
            .HasMaxLength(ArtworkReportReason.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(reason => reason.CreatedBy).IsRequired();

        builder
            .Property(reason => reason.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(reason => reason.Creator)
            .WithMany(systemAccount => systemAccount.CreatedArtworkReportProblems)
            .HasForeignKey(reason => reason.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(problem => problem.CreatedBy);
        #endregion
    }
}
