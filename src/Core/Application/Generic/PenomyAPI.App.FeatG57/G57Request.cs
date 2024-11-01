using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG57;

public class G57Request : IFeatureRequest<G57Response>
{
    public long CommentId { get; init; }

    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

}
