using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkViolationFlagTypeEntityConfiguration
    : IEntityConfiguration<ArtworkViolationFlagType>
{
    public void Configure(EntityTypeBuilder<ArtworkViolationFlagType> builder)
    {
        builder.ToTable("penomy_artwork_violation_flag_type");

        builder.HasKey(flagType => flagType.Id);

        builder
            .Property(flagType => flagType.Name)
            .HasMaxLength(ArtworkViolationFlagType.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(flagType => flagType.Description)
            .HasMaxLength(ArtworkViolationFlagType.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(flagType => flagType.CreatedBy).IsRequired();

        builder
            .Property(flagType => flagType.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(flagType => flagType.CreatedBy).IsRequired();

        builder
            .Property(flagType => flagType.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(violationFlagType => violationFlagType.Creator)
            .WithMany(systemAccount => systemAccount.CreatedArtworkViolationFlagTypes)
            .HasForeignKey(violationFlagType => violationFlagType.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(flagType => flagType.CreatedBy);
        #endregion
    }
}
