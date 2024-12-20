using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.CountArtwork;

public class G25CountArtworkResponse : IFeatureResponse
{
    public int TotalArtwork { get; set; }
}
