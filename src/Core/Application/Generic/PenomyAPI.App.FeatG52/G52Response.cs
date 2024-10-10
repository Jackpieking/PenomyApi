using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG52;

public class G52Response : IFeatureResponse
{
    public long CommentId { get; set; }

    public bool IsSuccess { get; set; }

    public G52ResponseStatusCode StatusCode { get; set; }
}
