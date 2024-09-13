using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Artworks;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Artworks;

internal sealed class UserLikeArtworkCommentEntityConfiguration
    : IEntityConfiguration<UserLikeArtworkComment>
{
    public void Configure(EntityTypeBuilder<UserLikeArtworkComment> builder)
    {
        builder.ToTable("penomy_user_like_artwork_comment");

        builder.HasKey(userLike => new { userLike.CommentId, userLike.UserId });

        builder
            .Property(userLike => userLike.LikedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();
    }
}
