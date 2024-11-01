using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG56;

public class G56Request : IFeatureRequest<G56Response>
{
    public long CommentId { get; init; }

    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;
}
