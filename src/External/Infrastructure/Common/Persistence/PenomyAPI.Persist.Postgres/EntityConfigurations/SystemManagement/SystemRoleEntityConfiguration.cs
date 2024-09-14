using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SystemManagement;

internal sealed class SystemRoleEntityConfiguration : IEntityConfiguration<SystemRole>
{
    public void Configure(EntityTypeBuilder<SystemRole> builder)
    {
        builder.ToTable("penomy_system_role");

        builder.HasKey(role => role.Id);

        builder
            .Property(role => role.Name)
            .HasMaxLength(SystemRole.MetaData.NameLength)
            .IsRequired();

        builder
            .Property(role => role.Code)
            .HasMaxLength(SystemRole.MetaData.CodeLength)
            .IsRequired();

        builder
            .Property(role => role.Description)
            .HasMaxLength(SystemRole.MetaData.DescriptionLength)
            .IsRequired();

        #region Indexes
        // builder.HasIndex(role => role.Code).IsUnique(true);
        #endregion
    }
}
