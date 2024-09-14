using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserPostReportEntityConfiguration : IEntityConfiguration<UserPostReport>
{
    public void Configure(EntityTypeBuilder<UserPostReport> builder)
    {
        builder.ToTable("penomy_user_post_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.PostId).IsRequired();

        builder.Property(report => report.ReportProblemId).IsRequired();

        builder
            .Property(report => report.DetailNote)
            .HasMaxLength(UserPostReport.MetaData.DetailNoteLength)
            .IsRequired();

        builder.Property(report => report.CreatedBy).IsRequired();

        builder
            .Property(report => report.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(report => report.IsResolved).IsRequired();

        #region Relationships
        builder
            .HasOne(report => report.Creator)
            .WithMany(user => user.CreatedUserPostReports)
            .HasForeignKey(report => report.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.ReportProblem)
            .WithMany(problem => problem.UserPostReports)
            .HasForeignKey(report => report.ReportProblemId)
            .HasPrincipalKey(problem => problem.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.UserPost)
            .WithMany(post => post.UserPostReports)
            .HasForeignKey(report => report.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => report.PostId);
        #endregion
    }
}
