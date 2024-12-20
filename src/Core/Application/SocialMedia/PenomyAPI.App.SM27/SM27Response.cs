using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM27;

public class SM27Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public SM27ResponseStatusCode StatusCode { get; set; }
}
