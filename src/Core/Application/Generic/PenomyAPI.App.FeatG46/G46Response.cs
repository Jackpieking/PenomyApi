using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG46;

public class G46Response : IFeatureResponse
{
    public G46ResponseStatusCode AppCode { get; set; }

    public static readonly G46Response FAILED = new()
    {
        AppCode = G46ResponseStatusCode.FAILED,
    };

    public static readonly G46Response INVALID_REQUEST = new()
    {
        AppCode = G46ResponseStatusCode.INVALID_REQUEST,
    };

    public static readonly G46Response EXISTED = new()
    {
        AppCode = G46ResponseStatusCode.EXISTED,
    };
    public static readonly G46Response NOT_FOUND = new()
    {
        AppCode = G46ResponseStatusCode.NOT_FOUND,
    };
    public static readonly G46Response SUCCESS = new()
    {
        AppCode = G46ResponseStatusCode.SUCCESS,
    };
}
