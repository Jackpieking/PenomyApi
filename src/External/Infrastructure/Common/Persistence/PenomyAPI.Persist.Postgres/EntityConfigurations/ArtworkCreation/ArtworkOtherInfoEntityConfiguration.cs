using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkOtherInfoEntityConfiguration : IEntityConfiguration<ArtworkOtherInfo>
{
    public void Configure(EntityTypeBuilder<ArtworkOtherInfo> builder)
    {
        builder.ToTable("penomy_artwork_other_info");

        builder.HasKey(artworkOtherInfo => new
        {
            artworkOtherInfo.ArtworkId,
            artworkOtherInfo.OtherInfoId
        });

        builder
            .Property(artworkOtherInfo => artworkOtherInfo.Value)
            .HasMaxLength(ArtworkOtherInfo.MetaData.ValueLength)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(artworkOtherInfo => artworkOtherInfo.Artwork)
            .WithMany(artwork => artwork.ArtworkOtherInfos)
            .HasForeignKey(artworkOtherInfo => artworkOtherInfo.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artworkOtherInfo => artworkOtherInfo.OtherInfo)
            .WithMany(artwork => artwork.ArtworkOtherInfos)
            .HasForeignKey(artworkOtherInfo => artworkOtherInfo.OtherInfoId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(category => category.CreatedBy);

        // builder.HasIndex(category => category.UpdatedBy);
        #endregion
    }
}
