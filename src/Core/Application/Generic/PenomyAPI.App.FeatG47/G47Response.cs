using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG47;

public class G47Response : IFeatureResponse
{
    public G47ResponseStatusCode AppCode { get; set; }

    public static readonly G47Response FAILED = new() { AppCode = G47ResponseStatusCode.FAILED, };

    public static readonly G47Response INVALID_REQUEST =
        new() { AppCode = G47ResponseStatusCode.INVALID_REQUEST, };

    public static readonly G47Response NOT_FOUND =
        new() { AppCode = G47ResponseStatusCode.NOT_FOUND, };
    public static readonly G47Response SUCCESS = new() { AppCode = G47ResponseStatusCode.SUCCESS, };
}
