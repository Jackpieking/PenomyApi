using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG46;

public class G46Response : IFeatureResponse
{
    public G46ResponseStatusCode AppCode { get; set; }
    public long FavoriteCount { get; set; }
}
