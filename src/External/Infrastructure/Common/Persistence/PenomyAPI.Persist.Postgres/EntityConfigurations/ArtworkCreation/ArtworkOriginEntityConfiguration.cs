using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkOriginEntityConfiguration : IEntityConfiguration<ArtworkOrigin>
{
    public void Configure(EntityTypeBuilder<ArtworkOrigin> builder)
    {
        builder.ToTable("penomy_artwork_origin");

        builder.HasKey(origin => origin.Id);

        builder
            .Property(origin => origin.CountryName)
            .HasMaxLength(ArtworkOrigin.MetaData.CountryNameLength)
            .IsRequired();

        builder
            .Property(origin => origin.Label)
            .HasMaxLength(ArtworkOrigin.MetaData.LabelLength)
            .IsRequired();

        builder
            .Property(origin => origin.ImageUrl)
            .HasMaxLength(ArtworkOrigin.MetaData.ImageUrlLength)
            .IsRequired();

        builder.Property(origin => origin.CreatedBy).IsRequired();

        builder
            .Property(origin => origin.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(origin => origin.UpdatedBy).IsRequired();

        builder
            .Property(origin => origin.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(artworkOrigin => artworkOrigin.Creator)
            .WithMany(systemAccount => systemAccount.CreatedOrigins)
            .HasForeignKey(artworkOrigin => artworkOrigin.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artworkOrigin => artworkOrigin.Updater)
            .WithMany(systemAccount => systemAccount.UpdatedOrigins)
            .HasForeignKey(artworkOrigin => artworkOrigin.UpdatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
