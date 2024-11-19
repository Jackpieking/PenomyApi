using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt1.OtherHandlers.OverviewStatistic;

public sealed class Art1OverviewStatisticHandler
    : IFeatureHandler<Art1OverviewStatisticRequest, Art1OverviewStatisticResponse>
{
    private readonly IArt1Repository _repository;

    public Art1OverviewStatisticHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.Art1Repository;
    }

    public async Task<Art1OverviewStatisticResponse> ExecuteAsync(
        Art1OverviewStatisticRequest request,
        CancellationToken ct)
    {
        var overviewStatistic = await _repository.GetOverviewStatisticByCreatorIdAsync(
            request.CreatorId,
            ct);

        return new()
        {
            TotalComics = overviewStatistic.TotalComics,
            TotalAnimations = overviewStatistic.TotalAnimations,
            TotalSeries = overviewStatistic.TotalSeries,
        };
    }
}
