using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG9.OtherHandlers.GetChapterList;

public class G9GetChapterListRequest : IFeatureRequest<G9GetChapterListResponse>
{
    public long ComicId { get; set; }

    public long UserId { get; set; }
}
