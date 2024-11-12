using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG49.OtherHandlers;

public class G49StarRateResponse : IFeatureResponse
{
    public G49ResponseStatusCode AppCode { get; set; }
    public byte StarRate { get; set; }
}
