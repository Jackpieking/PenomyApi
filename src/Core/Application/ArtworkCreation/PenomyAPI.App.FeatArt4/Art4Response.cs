using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt4;

public class Art4Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public string[] ErrorMessages { get; set; }

    public Art4ResponseStatusCode StatusCode { get; set; }
}
