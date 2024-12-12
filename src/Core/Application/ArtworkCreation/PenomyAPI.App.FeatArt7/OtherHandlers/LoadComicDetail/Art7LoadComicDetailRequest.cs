using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;

public sealed class Art7LoadComicDetailRequest : IFeatureRequest<Art7LoadComicDetailResponse>
{
    public long ComicId { get; set; }

    private long CreatorId { get; set; }

    public void SetCreatorId(long creatorId)
    {
        CreatorId = creatorId;
    }

    public long GetCreatorId() => CreatorId;
}
