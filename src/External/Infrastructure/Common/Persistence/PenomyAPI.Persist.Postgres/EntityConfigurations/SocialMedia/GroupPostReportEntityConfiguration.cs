using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace TestFeatApp.Domain.Entities.SocialMedia;

internal sealed class GroupPostReportEntityConfiguration : IEntityConfiguration<GroupPostReport>
{
    public void Configure(EntityTypeBuilder<GroupPostReport> builder)
    {
        builder.ToTable("penomy_group_post_report");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.PostId).IsRequired();

        builder.Property(report => report.ReportProblemId).IsRequired();

        builder
            .Property(report => report.DetailNote)
            .HasMaxLength(GroupPostReport.MetaData.DetailNoteLength)
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
            .WithMany(user => user.CreatedGroupPostReports)
            .HasForeignKey(report => report.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.ReportProblem)
            .WithMany(problem => problem.GroupPostReports)
            .HasForeignKey(report => report.ReportProblemId)
            .HasPrincipalKey(problem => problem.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(report => report.GroupPost)
            .WithMany(post => post.GroupPostReports)
            .HasForeignKey(report => report.PostId)
            .HasPrincipalKey(post => post.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => report.PostId);
        #endregion
    }
}
