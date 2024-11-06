using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG49;

public class G49Response : IFeatureResponse
{
    public G49ResponseStatusCode AppCode { get; set; }
    public double StarRate { get; set; }
}
