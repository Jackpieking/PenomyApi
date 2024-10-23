using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt1.OtherHandlers.CountArtwork;

public sealed class Art1CountArtworkResponse : IFeatureResponse
{
    /// <summary>
    ///     The total numbers of artwork after count by condition.
    /// </summary>
    public int TotalArtworks { get; set; }
}
