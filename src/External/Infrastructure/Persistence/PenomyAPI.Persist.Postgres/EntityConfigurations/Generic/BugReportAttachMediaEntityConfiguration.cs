using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

public class BugReportAttachMediaEntityConfiguration : IEntityConfiguration<BugReportAttachedMedia>
{
    public void Configure(EntityTypeBuilder<BugReportAttachedMedia> builder)
    {
        builder.ToTable("penomy_bug_report_attached_media");

        builder.HasKey(media => media.Id);

        builder.Property(media => media.BugReportId).IsRequired();

        builder
            .Property(media => media.FileName)
            .HasMaxLength(BugReportAttachedMedia.MetaData.FileNameLength)
            .IsRequired();

        builder.Property(media => media.UploadOrder).IsRequired();

        builder
            .Property(media => media.StorageUrl)
            .HasMaxLength(BugReportAttachedMedia.MetaData.StorageUrlLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(media => media.BugReport)
            .WithMany(bugReport => bugReport.AttachedMedias)
            .HasForeignKey(media => media.BugReportId)
            .HasPrincipalKey(bugReport => bugReport.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(media => media.BugReportId);
        #endregion
    }
}
