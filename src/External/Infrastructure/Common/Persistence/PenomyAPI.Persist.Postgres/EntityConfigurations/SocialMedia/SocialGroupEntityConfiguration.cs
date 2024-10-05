using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupEntityConfiguration : IEntityConfiguration<SocialGroup>
{
    public void Configure(EntityTypeBuilder<SocialGroup> builder)
    {
        builder.ToTable("penomy_social_group");

        builder.HasKey(group => group.Id);

        builder
            .Property(group => group.Name)
            .HasMaxLength(SocialGroup.MetaData.NameLength)
            .IsRequired();

        builder.Property(group => group.IsPublic).IsRequired();

        builder
            .Property(group => group.Description)
            .HasMaxLength(SocialGroup.MetaData.DescriptionLength)
            .IsRequired();

        builder
            .Property(group => group.CoverPhotoUrl)
            .HasMaxLength(SocialGroup.MetaData.CoverPhotoUrlLength)
            .IsRequired();

        builder.Property(group => group.TotalMembers).IsRequired();

        builder.Property(group => group.RequireApprovedWhenPost).IsRequired();

        builder.Property(group => group.GroupStatus).IsRequired();

        builder.Property(group => group.CreatedBy).IsRequired();

        builder
            .Property(group => group.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(socialGroup => socialGroup.Creator)
            .WithMany(user => user.CreatedSocialGroups)
            .HasForeignKey(socialGroup => socialGroup.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(group => group.CreatedBy);

        // builder.HasIndex(group => group.IsPublic);
        #endregion
    }
}
