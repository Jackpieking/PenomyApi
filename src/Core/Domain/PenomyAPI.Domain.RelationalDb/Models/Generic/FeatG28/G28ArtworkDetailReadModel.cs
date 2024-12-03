using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG28;

public class G28ArtworkDetailReadModel
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public int LastChapterUploadOrder { get; set; }

    public long TotalStarRates { get; set; }

    public long TotalUsersRated { get; set; }

    public double AverageStarRates =>
        ArtworkMetaData.GetAverageStarRate(TotalUsersRated, TotalStarRates);

    public string OriginImageUrl { get; set; }

    public G28ChapterReadModel LatestChapter { get; set; }
}
