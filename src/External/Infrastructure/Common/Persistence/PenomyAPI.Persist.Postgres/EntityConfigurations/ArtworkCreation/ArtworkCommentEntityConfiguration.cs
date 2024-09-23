using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.ArtworkCreation;

internal sealed class ArtworkCommentEntityConfiguration : IEntityConfiguration<ArtworkComment>
{
    public void Configure(EntityTypeBuilder<ArtworkComment> builder)
    {
        builder.ToTable("penomy_artwork_comment");

        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.ArtworkId).IsRequired();

        builder.Property(comment => comment.ChapterId).IsRequired();

        builder
            .Property(comment => comment.Content)
            .HasMaxLength(ArtworkComment.MetaData.ContentLength)
            .IsRequired();

        builder.Property(comment => comment.IsDirectlyCommented).IsRequired();

        builder.Property(comment => comment.TotalChildComments).IsRequired();

        builder.Property(comment => comment.TotalLikes).IsRequired();

        builder.Property(comment => comment.CreatedBy).IsRequired();

        builder
            .Property(comment => comment.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(comment => comment.UpdatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(comment => comment.Creator)
            .WithMany(user => user.CreatedArtworkComments)
            .HasForeignKey(comment => comment.CreatedBy)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(comment => new { comment.ArtworkId, comment.ChapterId });

        // builder.HasIndex(comment => comment.TotalLikes);

        // builder.HasIndex(comment => comment.CreatedAt);
        #endregion
    }
}
