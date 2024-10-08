using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class OtherInfoEntityConfiguration : IEntityConfiguration<OtherInfo>
{
    public void Configure(EntityTypeBuilder<OtherInfo> builder)
    {
        builder.ToTable("penomy_other_info");

        builder.HasKey(otherInfo => otherInfo.Id);

        builder
            .Property(otherInfo => otherInfo.Name)
            .HasMaxLength(OtherInfo.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(otherInfo => otherInfo.Description)
            .HasMaxLength(OtherInfo.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(otherInfo => otherInfo.CreatedBy).IsRequired();

        builder
            .Property(otherInfo => otherInfo.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(category => category.Creator)
            .WithMany(systemAccount => systemAccount.CreatedOtherInfos)
            .HasForeignKey(category => category.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(category => category.CreatedBy);

        // builder.HasIndex(category => category.UpdatedBy);
        #endregion
    }
}
