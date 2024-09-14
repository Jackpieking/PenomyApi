using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Generic;

internal sealed class BanTypeEntityConfiguration : IEntityConfiguration<BanType>
{
    public void Configure(EntityTypeBuilder<BanType> builder)
    {
        builder.ToTable("penomy_ban_type");

        builder.HasKey(banType => banType.Id);

        builder
            .Property(banType => banType.Title)
            .HasMaxLength(BanType.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(banType => banType.Code)
            .HasMaxLength(BanType.MetaData.CodeLength)
            .IsRequired();

        builder
            .Property(banType => banType.Description)
            .HasMaxLength(BanType.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(banType => banType.CreatedBy).IsRequired();

        builder
            .Property(banType => banType.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(banType => banType.Creator)
            .WithMany(systemAccount => systemAccount.CreatedBanTypes)
            .HasForeignKey(banType => banType.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(banType => banType.Code);

        // builder.HasIndex(banType => banType.CreatedBy);
        #endregion
    }
}
