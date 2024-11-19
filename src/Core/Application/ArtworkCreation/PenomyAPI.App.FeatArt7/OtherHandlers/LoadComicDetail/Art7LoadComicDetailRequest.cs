using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;

public sealed class Art7LoadComicDetailRequest : IFeatureRequest<Art7LoadComicDetailResponse>
{
    public long ComicId { get; set; }
}
