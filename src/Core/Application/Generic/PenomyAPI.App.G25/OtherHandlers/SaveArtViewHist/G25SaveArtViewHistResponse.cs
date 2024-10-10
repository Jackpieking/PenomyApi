using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;

public class G25SaveArtViewHistResponse : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public G25SaveArtViewHistResponseStatusCode StatusCode { get; set; }
}
