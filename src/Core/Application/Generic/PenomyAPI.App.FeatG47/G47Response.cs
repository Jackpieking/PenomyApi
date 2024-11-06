using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG47;

public class G47Response : IFeatureResponse
{
    public G47ResponseStatusCode AppCode { get; set; }
    public long FavoriteCount { get; set; }
}
