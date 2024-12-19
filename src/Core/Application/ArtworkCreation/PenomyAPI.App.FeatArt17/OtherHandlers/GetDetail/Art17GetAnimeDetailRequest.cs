using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;

public class Art17GetAnimeDetailRequest
    : IFeatureRequest<Art17GetAnimeDetailResponse>
{
    public long ArtworkId { get; set; }

    private long CreatorId { get; set; }

    public void SetCreatorId(long creatorId)
    {
        CreatorId = creatorId;
    }

    public long GetCreatorId() => CreatorId;
}
