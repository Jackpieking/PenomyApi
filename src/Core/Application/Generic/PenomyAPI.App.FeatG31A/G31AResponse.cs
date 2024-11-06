using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG31A;

public sealed class G31AResponse : IFeatureResponse
{
    public G31AResponseStatusCode StatusCode { get; init; }

    public ResponseBody Body { get; init; }

    public sealed class ResponseBody
    {
        public string AccessToken { get; init; }
    }
}
