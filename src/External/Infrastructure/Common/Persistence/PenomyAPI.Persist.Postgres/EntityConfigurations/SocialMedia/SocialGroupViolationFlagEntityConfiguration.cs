using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class SocialGroupViolationFlagEntityConfiguration
    : IEntityConfiguration<SocialGroupViolationFlag>
{
    public void Configure(EntityTypeBuilder<SocialGroupViolationFlag> builder)
    {
        builder.ToTable("penomy_social_group_violation_flag");

        builder.HasKey(violationFlag => violationFlag.Id);

        builder.Property(violationFlag => violationFlag.GroupId).IsRequired();

        builder
            .Property(violationFlag => violationFlag.Title)
            .HasMaxLength(SocialGroupViolationFlag.MetaData.TitleLength)
            .IsRequired();

        builder
            .Property(violationFlag => violationFlag.Description)
            .HasMaxLength(SocialGroupViolationFlag.MetaData.DescriptionLength)
            .IsRequired();
        builder
            .Property(violationFlag => violationFlag.CreatedBy)
            .IsRequired();

        builder
            .Property(violationFlag => violationFlag.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder.Property(violationFlag => violationFlag.IsResolved).IsRequired();

        builder
            .Property(violationFlag => violationFlag.ResolveNote)
            .HasMaxLength(SocialGroupViolationFlag.MetaData.ResolveNoteLength)
            .IsRequired();

        builder
            .Property(violationFlag => violationFlag.ResolvedBy)
            .IsRequired();

        builder
            .Property(violationFlag => violationFlag.ResolvedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(violationFlag => violationFlag.Creator)
            .WithMany(creator => creator.CreatedSocialGroupViolationFlags)
            .HasPrincipalKey(creator => creator.Id)
            .HasForeignKey(violationFlag => violationFlag.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(violationFlag => violationFlag.Resolver)
            .WithMany(resolver => resolver.ResolvedSocialGroupViolationFlags)
            .HasPrincipalKey(resolver => resolver.UserId)
            .HasForeignKey(violationFlag => violationFlag.ResolvedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
