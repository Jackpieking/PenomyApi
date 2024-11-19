using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG58;

public class G58Response : IFeatureResponse
{
    public long CommentId { get; set; }

    public bool IsSuccess { get; set; }

    public G58ResponseStatusCode StatusCode { get; set; }
}
