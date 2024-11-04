using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG32.OtherHandlers.VerifyGoogleSignIn;

public sealed class G32VerifyGoogleSignInResponse : IFeatureResponse
{
    public G32VerifyGoogleSignInResponseStatusCode StatusCode { get; init; }

    public ResponseBody Body { get; init; }

    public sealed class ResponseBody
    {
        public string AccessToken { get; init; }

        public string RefreshToken { get; init; }
    }
}
