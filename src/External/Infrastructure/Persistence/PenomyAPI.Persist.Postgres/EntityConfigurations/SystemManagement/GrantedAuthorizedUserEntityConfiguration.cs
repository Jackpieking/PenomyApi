using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SystemManagement;

internal sealed class GrantedAuthorizedUserEntityConfiguration
    : IEntityConfiguration<GrantedAuthorizedUser>
{
    public void Configure(EntityTypeBuilder<GrantedAuthorizedUser> builder)
    {
        builder.ToTable("penomy_granted_authorized_user");

        builder.HasKey(grantedAuthorizedUser => new
        {
            grantedAuthorizedUser.GrantedTo,
            grantedAuthorizedUser.AuthorizedUserId,
            grantedAuthorizedUser.GrantedBy
        });

        builder
            .Property(grantedAuthorizedUser => grantedAuthorizedUser.GrantedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(grantedAuthorizedUser => grantedAuthorizedUser.IsActive).IsRequired();

        #region Relationships
        builder
            .HasOne(grantedAuthorizedUser => grantedAuthorizedUser.AuthorizedUserAccount)
            .WithMany(authorizedUserAccount => authorizedUserAccount.GrantedTickets)
            .HasForeignKey(grantedAuthorizedUser => grantedAuthorizedUser.AuthorizedUserId)
            .HasPrincipalKey(authorizedUserAccount => authorizedUserAccount.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(grantedAuthorizedUser => grantedAuthorizedUser.GrantedSystemAccount)
            .WithMany(authorizedUserAccount => authorizedUserAccount.GrantedAuthorizedUsers)
            .HasForeignKey(grantedAuthorizedUser => grantedAuthorizedUser.GrantedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(grantedAuthorizedUser => grantedAuthorizedUser.ReceivedSystemAccount)
            .WithMany(authorizedUserAccount => authorizedUserAccount.ReceivedAuthorizedUsers)
            .HasForeignKey(grantedAuthorizedUser => grantedAuthorizedUser.GrantedTo)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(grantedAuthorizedUser => grantedAuthorizedUser.GrantedBy);
        #endregion
    }
}
