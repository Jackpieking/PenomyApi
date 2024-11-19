using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG28.PageCount;

public class G28PageCountResponse : IFeatureResponse
{
    public long result { get; set; }

    public G28PageCountResponseStatusCode StatusCode { get; set; }
}
