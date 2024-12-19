using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt20.OtherHandlers.GetDetailToCreateChapter;

public class Art20GetDetailToCreateChapRequest
    : IFeatureRequest<Art20GetDetailToCreateChapResponse>
{
    private long CreatorId { get; set; }

    public long AnimeId { get; set; }

    public void SetCreatorId(long creatorId)
    {
        CreatorId = creatorId;
    }

    public long GetCreatorId() => CreatorId;
}
