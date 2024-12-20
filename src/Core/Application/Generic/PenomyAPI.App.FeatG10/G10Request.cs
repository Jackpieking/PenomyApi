using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG10;

public class G10Request : IFeatureRequest<G10Response>
{
    public string ArtworkId { get; set; }

    public string CommentSection { get; set; }

    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;
}
