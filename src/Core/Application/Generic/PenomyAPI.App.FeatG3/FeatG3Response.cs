using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public object Result { get; set; }

    public FeatG3ResponseStatusCode StatusCode { get; set; }
}
