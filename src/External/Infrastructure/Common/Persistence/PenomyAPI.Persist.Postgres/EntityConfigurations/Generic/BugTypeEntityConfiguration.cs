using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class BugTypeEntityConfiguration : IEntityConfiguration<BugType>
{
    public void Configure(EntityTypeBuilder<BugType> builder)
    {
        builder.ToTable("penomy_bug_type");

        builder.HasKey(bugType => bugType.Id);

        builder
            .Property(bugType => bugType.Title)
            .HasMaxLength(BugType.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(bugType => bugType.Description)
            .HasMaxLength(BugType.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(bugType => bugType.BugSeverity).IsRequired();

        builder.Property(bugType => bugType.CreatedBy).IsRequired();

        builder
            .Property(bugType => bugType.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ);

        #region Relationships
        builder
            .HasOne(bugType => bugType.Creator)
            .WithMany(systemAccount => systemAccount.CreatedBugTypes)
            .HasForeignKey(bugType => bugType.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(bugType => bugType.CreatedBy);
        #endregion
    }
}
