using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SystemManagement;

internal sealed class SystemAccountRoleEntityConfiguration : IEntityConfiguration<SystemAccountRole>
{
    public void Configure(EntityTypeBuilder<SystemAccountRole> builder)
    {
        builder.ToTable("penomy_system_account_role");

        builder.HasKey(systemAccountRole => new
        {
            systemAccountRole.SystemAccountId,
            systemAccountRole.RoleId
        });

        #region Relationships
        builder
            .HasOne(accountRole => accountRole.SystemAccount)
            .WithMany(systemAccount => systemAccount.SystemAccountRoles)
            .HasForeignKey(accountRole => accountRole.SystemAccountId)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(accountRole => accountRole.SystemRole)
            .WithMany(role => role.SystemAccountRoles)
            .HasForeignKey(accountRole => accountRole.RoleId)
            .HasPrincipalKey(role => role.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
