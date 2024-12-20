using PenomyAPI.App.Common;

namespace PenomyAPI.App.G61.OtherHandlers.CheckHasFollow;

public sealed class G61CheckHasFollowResponse : IFeatureResponse
{
    public bool HasFollowed { get; set; }

    public G61ResponseStatusCode StatusCode { get; set; }

    public static G61CheckHasFollowResponse FAILED = new G61CheckHasFollowResponse
    {
        StatusCode = G61ResponseStatusCode.FAILED,
    };
}
