using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt4;

public class FeatArt4Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public FeatArt4ResponseStatusCode StatusCode { get; set; }
}
