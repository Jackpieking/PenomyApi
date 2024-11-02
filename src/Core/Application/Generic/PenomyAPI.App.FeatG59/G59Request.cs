using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG59;

public class G59Request : IFeatureRequest<G59Response>
{
    public long ParentCommentId { get; set; }

    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;
}
