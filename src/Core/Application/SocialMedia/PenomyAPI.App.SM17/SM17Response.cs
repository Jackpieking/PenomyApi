using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM17;

public class SM17Response : IFeatureResponse
{
    public bool IsLikeRequest { get; set; }

    public bool IsSuccess { get; set; }

    public SM17ResponseStatusCode StatusCode { get; set; }
}
