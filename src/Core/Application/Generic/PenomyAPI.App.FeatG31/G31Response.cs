using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG31;

public sealed class G31Response : IFeatureResponse
{
    public G31ResponseStatusCode StatusCode { get; init; }

    public ResponseBody Body { get; init; }

    public sealed class ResponseBody
    {
        public string AccessToken { get; init; }

        public string RefreshToken { get; init; }

        public UserCredential User { get; init; }

        public sealed class UserCredential
        {
            public long Id { get; init; }

            public string Nickname { get; init; }

            public string AvatarUrl { get; init; }

            public bool RegisterAsCreator { get; init; }
        }
    }
}
