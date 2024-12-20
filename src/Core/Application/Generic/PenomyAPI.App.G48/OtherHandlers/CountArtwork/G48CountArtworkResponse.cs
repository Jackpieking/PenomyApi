using PenomyAPI.App.Common;

namespace PenomyAPI.App.G48.OtherHandlers.CountArtwork;

public class G48CountArtworkResponse : IFeatureResponse
{
    public int TotalArtwork { get; set; }
}
