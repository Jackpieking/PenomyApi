using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM12;

public class SM12Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public long UserPostId { get; set; }

    public string[] ErrorMessages { get; set; }

    public SM12ResponseStatusCode StatusCode { get; set; }
}
