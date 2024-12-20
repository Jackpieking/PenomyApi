using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.SocialMedia;

internal sealed class UserLikeValueEntityConfiguration
    : IEntityConfiguration<UserLikeValue>
{
    public void Configure(EntityTypeBuilder<UserLikeValue> builder)
    {
        builder.ToTable("penomy_user_like_value");

        builder.HasKey(likeValue => likeValue.Id);

        builder
            .Property(likeValue => likeValue.Name)
            .HasMaxLength(UserLikeValue.MetaData.NameLength)
            .IsRequired();

        builder.Property(likeValue => likeValue.DisplayOrder).IsRequired();

        builder.Property(likeValue => likeValue.ForDefaultDisplay).IsRequired();

        builder
            .Property(likeValue => likeValue.Value)
            .HasMaxLength(UserLikeValue.MetaData.ValueLength)
            .IsRequired();

        builder
            .Property(likeValue => likeValue.EmojiUrl)
            .HasMaxLength(UserLikeValue.MetaData.EmojiUrlLength)
            .IsRequired();

        builder
            .Property(likeValue => likeValue.CreatedBy)
            .IsRequired();

        builder
            .Property(likeValue => likeValue.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(likeValue => likeValue.Creator)
            .WithMany(systemAccount => systemAccount.CreatedUserLikeValues)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .HasForeignKey(likeValue => likeValue.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}
