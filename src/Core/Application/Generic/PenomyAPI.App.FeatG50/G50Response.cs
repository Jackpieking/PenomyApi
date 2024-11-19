using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG50;

public class G50Response : IFeatureResponse
{
    public static readonly G50Response FAILED = new() { AppCode = G50ResponseStatusCode.FAILED };

    public static readonly G50Response NOT_FOUND =
        new() { AppCode = G50ResponseStatusCode.NOT_FOUND };

    public static readonly G50Response SUCCESS = new() { AppCode = G50ResponseStatusCode.SUCCESS };
    public G50ResponseStatusCode AppCode { get; set; }
}
