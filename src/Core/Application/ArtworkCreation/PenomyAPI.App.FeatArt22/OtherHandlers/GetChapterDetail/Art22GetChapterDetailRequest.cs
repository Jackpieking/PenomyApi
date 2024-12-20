using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;

public class Art22GetChapterDetailRequest : IFeatureRequest<Art22GetChapterDetailResponse>
{
    public long ChapterId { get; set; }

    public long CreatorId { get; set; }
}
