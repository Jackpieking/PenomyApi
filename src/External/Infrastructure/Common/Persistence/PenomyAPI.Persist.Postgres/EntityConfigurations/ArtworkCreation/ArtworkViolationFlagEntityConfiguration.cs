using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkViolationFlagEntityConfiguration
    : IEntityConfiguration<ArtworkViolationFlag>
{
    public void Configure(EntityTypeBuilder<ArtworkViolationFlag> builder)
    {
        builder.ToTable("penomy_artwork_violation_flag");

        builder.HasKey(violationFlag => violationFlag.Id);

        builder.Property(violationFlag => violationFlag.ViolationFlagTypeId).IsRequired();

        builder.Property(violationFlag => violationFlag.ArtworkId).IsRequired();

        builder
            .Property(violationFlag => violationFlag.DetailNote)
            .HasMaxLength(ArtworkViolationFlag.MetaData.DetailNoteLength)
            .IsRequired();

        builder.Property(violationFlag => violationFlag.CreatedBy).IsRequired();

        builder
            .Property(violationFlag => violationFlag.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(violationFlag => violationFlag.ResolveStatus).IsRequired();

        builder
            .Property(violationFlag => violationFlag.ResolveNote)
            .HasMaxLength(ArtworkViolationFlag.MetaData.ResolveNoteLength)
            .IsRequired();

        builder.Property(violationFlag => violationFlag.ResolvedBy).IsRequired();

        builder
            .Property(violationFlag => violationFlag.ResolvedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(violationFlag => violationFlag.ViolationFlagType)
            .WithMany(flagType => flagType.ArtworkViolationFlags)
            .HasForeignKey(violationFlag => violationFlag.ViolationFlagTypeId)
            .HasPrincipalKey(flagType => flagType.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(violationFlag => violationFlag.ViolatedArtwork)
            .WithMany(artwork => artwork.ViolationFlags)
            .HasForeignKey(violationFlag => violationFlag.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(violationFlag => violationFlag.Creator)
            .WithMany(systemAccount => systemAccount.CreatedArtworkViolationFlags)
            .HasForeignKey(violationFlag => violationFlag.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(violationFlag => violationFlag.Resolver)
            .WithMany(creator => creator.ResolvedViolationFlags)
            .HasForeignKey(violationFlag => violationFlag.ResolvedBy)
            .HasPrincipalKey(creator => creator.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(report => report.ArtworkId);
        #endregion
    }
}
