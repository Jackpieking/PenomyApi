using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG33;

public sealed class G33Request : IFeatureRequest<G33Response>
{
    private string _refreshTokenId;

    public string GetRefreshTokenId() => _refreshTokenId;

    public void SetRefreshTokenId(string refreshTokenId) => _refreshTokenId = refreshTokenId;
}
