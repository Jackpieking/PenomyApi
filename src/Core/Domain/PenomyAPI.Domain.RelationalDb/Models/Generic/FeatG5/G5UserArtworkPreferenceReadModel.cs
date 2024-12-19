namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;

public sealed class G5UserArtworkPreferenceReadModel
{
    public long FirstChapterId { get; set; }

    public long LastReadChapterId { get; set; }

    public bool IsUserFavorite { get; set; }

    public bool HasFollowed { get; set; }
}
