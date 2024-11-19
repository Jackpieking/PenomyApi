using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt1.OtherHandlers.OverviewStatistic;

public sealed class Art1OverviewStatisticResponse : IFeatureResponse
{
    public int TotalComics { get; set; }

    public int TotalAnimations { get; set; }

    public int TotalSeries { get; set; }
}
