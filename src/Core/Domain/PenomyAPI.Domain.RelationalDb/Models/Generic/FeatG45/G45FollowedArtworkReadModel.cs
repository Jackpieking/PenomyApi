using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG45;

public sealed class G45FollowedArtworkReadModel
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public int LastChapterUploadOrder { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public long TotalStarRates { get; set; }

    public long TotalUsersRated { get; set; }

    public double AverageStarRates
        => ArtworkMetaData.GetAverageStarRate(TotalUsersRated, TotalStarRates);

    public string OriginImageUrl { get; set; }

    public long CreatorId { get; set; }

    public string CreatorName { get; set; }

    public string CreatorAvatarUrl { get; set; }

    [NotMapped]
    public G45ChapterReadModel Chapter { get; set; }
}
