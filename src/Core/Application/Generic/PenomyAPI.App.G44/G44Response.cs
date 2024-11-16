using PenomyAPI.App.Common;

namespace PenomyAPI.App.G44;

public class G44Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G44ResponseStatusCode StatusCode { get; set; }

    public static readonly G44Response SUCCESS = new()
    {
        IsSuccess = true,
        StatusCode = G44ResponseStatusCode.SUCCESS,
    };

    public static readonly G44Response INVALID_REQUEST = new()
    {
        IsSuccess = false,
        StatusCode = G44ResponseStatusCode.INVALID_REQUEST,
    };

    public static readonly G44Response FAILED = new()
    {
        IsSuccess = false,
        StatusCode = G44ResponseStatusCode.FAILED,
    };
}
