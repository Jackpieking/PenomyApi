using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG28;

public class G28Request : IFeatureRequest<G28Response>
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public long ArtworkType { get; init; }

    public int PageNumber { get; init; }
}
