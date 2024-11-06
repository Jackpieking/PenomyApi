using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG31A;

public sealed class G31ARequest : IFeatureRequest<G31AResponse>
{
    public string RefreshToken { get; init; }

    // This field is assigned by system itself.
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    private string _accessTokenId;

    public string GetAccessTokenId() => _accessTokenId;

    public void SetAccessTokenId(string accessTokenId) => _accessTokenId = accessTokenId;
}
