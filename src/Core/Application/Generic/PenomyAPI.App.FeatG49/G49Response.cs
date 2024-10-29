using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG49;

public class G49Response : IFeatureResponse
{
    public static readonly G49Response FAILED = new() { AppCode = G49ResponseStatusCode.FAILED };

    public static readonly G49Response NOT_FOUND =
        new() { AppCode = G49ResponseStatusCode.NOT_FOUND };

    public static readonly G49Response SUCCESS = new() { AppCode = G49ResponseStatusCode.SUCCESS };
    public G49ResponseStatusCode AppCode { get; set; }
}
