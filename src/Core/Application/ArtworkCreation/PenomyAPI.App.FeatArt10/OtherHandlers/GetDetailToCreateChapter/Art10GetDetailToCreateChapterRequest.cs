using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;

public sealed class Art10GetDetailToCreateChapterRequest
    : IFeatureRequest<Art10GetDetailToCreateChapterResponse>
{
    public long ComicId { get; set; }

    private long CreatorId { get; set; }

    public void SetCreatorId(long creatorId)
    {
        CreatorId = creatorId;
    }

    public long GetCreatorId() => CreatorId;
}
