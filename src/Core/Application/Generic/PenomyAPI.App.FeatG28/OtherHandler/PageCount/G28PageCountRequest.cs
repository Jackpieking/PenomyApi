using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG28.PageCount;

public class G28PageCountRequest : IFeatureRequest<G28PageCountResponse>
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public long ArtworkType { get; init; }
}
