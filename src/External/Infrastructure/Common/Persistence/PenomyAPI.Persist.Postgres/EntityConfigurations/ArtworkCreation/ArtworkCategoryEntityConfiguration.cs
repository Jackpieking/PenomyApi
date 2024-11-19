using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkCategoryEntityConfiguration : IEntityConfiguration<ArtworkCategory>
{
    public void Configure(EntityTypeBuilder<ArtworkCategory> builder)
    {
        builder.ToTable("penomy_artwork_category");

        builder.HasKey(artworkCategory => new
        {
            artworkCategory.ArtworkId,
            artworkCategory.CategoryId
        });

        #region Relationships
        builder
            .HasOne(artworkCategory => artworkCategory.Artwork)
            .WithMany(artwork => artwork.ArtworkCategories)
            .HasForeignKey(artworkCategory => artworkCategory.ArtworkId)
            .HasPrincipalKey(artwork => artwork.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(artworkCategory => artworkCategory.Category)
            .WithMany(category => category.ArtworkCategories)
            .HasForeignKey(artworkCategory => artworkCategory.CategoryId)
            .HasPrincipalKey(category => category.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(artworkCategory => artworkCategory.CategoryId);
        #endregion
    }
}
