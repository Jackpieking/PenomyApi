using PenomyAPI.App.Common;

namespace PenomyAPI.App.G43;

public class G43Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G43ResponseStatusCode StatusCode { get; set; }

    public static readonly G43Response FAILED = new()
    {
        IsSuccess = false,
        StatusCode = G43ResponseStatusCode.FAILED,
    };

    public static readonly G43Response INVALID_REQUEST = new()
    {
        IsSuccess = false,
        StatusCode = G43ResponseStatusCode.INVALID_REQUEST
    };

    public static readonly G43Response SUCCESS = new()
    {
        IsSuccess = true,
        StatusCode = G43ResponseStatusCode.SUCCESS,
    };
}
