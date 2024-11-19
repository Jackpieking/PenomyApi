using PenomyAPI.App.Common;

namespace PenomyAPI.App.G61;

public class G61Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public G61ResponseStatusCode StatusCode { get; set; }

    public static G61Response INVALID_REQUEST = new()
    {
        IsSuccess = false,
        StatusCode = G61ResponseStatusCode.INVALID_REQUEST,
    };
}
