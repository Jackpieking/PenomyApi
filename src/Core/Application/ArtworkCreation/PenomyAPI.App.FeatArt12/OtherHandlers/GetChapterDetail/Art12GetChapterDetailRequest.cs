using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;

public sealed class Art12GetChapterDetailRequest
    : IFeatureRequest<Art12GetChapterDetailResponse>
{
    public long ChapterId { get; set; }

    public long CreatorId { get; set; }
}
