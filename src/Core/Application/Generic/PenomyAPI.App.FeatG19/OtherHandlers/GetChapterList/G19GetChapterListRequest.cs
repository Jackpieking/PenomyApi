using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG19.OtherHandlers.GetChapterList;

public class G19GetChapterListRequest : IFeatureRequest<G19GetChapterListResponse>
{
    public long AnimeId { get; set; }

    public long UserId { get; set; }
}
