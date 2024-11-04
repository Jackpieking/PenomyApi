using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG32.OtherHandlers.VerifyGoogleSignIn;

public sealed class G32VerifyGoogleSignInRequest : IFeatureRequest<G32VerifyGoogleSignInResponse>
{
    public long UserId { get; init; }

    public string NickName { get; init; }

    public string Email { get; init; }
}
