using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkBugTypeEntityConfiguration : IEntityConfiguration<ArtworkBugType>
{
    public void Configure(EntityTypeBuilder<ArtworkBugType> builder)
    {
        builder.ToTable("penomy_artwork_bug_type");

        builder.HasKey(bugType => bugType.Id);

        builder
            .Property(bugType => bugType.Title)
            .HasMaxLength(ArtworkBugType.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(bugType => bugType.Description)
            .HasMaxLength(ArtworkBugType.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(bugType => bugType.BugSeverity).IsRequired();

        builder.Property(bugType => bugType.CreatedBy).IsRequired();

        builder
            .Property(bugType => bugType.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(bugType => bugType.Creator)
            .WithMany(systemAccount => systemAccount.CreatedArtworkBugTypes)
            .HasForeignKey(bugType => bugType.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(bugType => bugType.CreatedBy);
        #endregion
    }
}
