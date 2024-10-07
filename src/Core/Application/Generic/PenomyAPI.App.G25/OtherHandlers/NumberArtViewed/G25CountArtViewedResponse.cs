using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.NumberArtViewed
{
    public class G25CountArtViewedResponse : IFeatureResponse
    {
        public bool IsSuccess { get; set; }
        public int ArtCount { get; set; }
        public G25CountArtViewedResponseStatusCode StatusCode { get; set; }
    }
}
