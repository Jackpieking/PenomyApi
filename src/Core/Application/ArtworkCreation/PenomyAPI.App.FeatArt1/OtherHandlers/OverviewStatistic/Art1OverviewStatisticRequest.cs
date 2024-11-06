using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt1.OtherHandlers.OverviewStatistic;

public sealed class Art1OverviewStatisticRequest
    : IFeatureRequest<Art1OverviewStatisticResponse>
{
    public long CreatorId { get; set; }
}
