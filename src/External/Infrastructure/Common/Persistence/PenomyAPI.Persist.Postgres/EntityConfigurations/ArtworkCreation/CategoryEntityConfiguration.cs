using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class CategoryEntityConfiguration : IEntityConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("penomy_category");

        builder.HasKey(category => category.Id);

        builder
            .Property(category => category.Name)
            .HasMaxLength(Category.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(category => category.Description)
            .HasMaxLength(Category.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(category => category.CreatedBy).IsRequired();

        builder
            .Property(category => category.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(category => category.UpdatedBy).IsRequired();

        builder
            .Property(artwork => artwork.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(category => category.Creator)
            .WithMany(systemAccount => systemAccount.CreatedCategories)
            .HasForeignKey(category => category.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(category => category.Updater)
            .WithMany(systemAccount => systemAccount.UpdatedCategories)
            .HasForeignKey(category => category.UpdatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(category => category.CreatedBy);

        // builder.HasIndex(category => category.UpdatedBy);
        #endregion
    }
}
