using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.CheckDeletedItems;

public class Art3CheckDeletedItemsHandler
    : IFeatureHandler<Art3CheckDeletedItemsRequest, Art3CheckDeletedItemsResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt3Repository _art3Repository;

    public Art3CheckDeletedItemsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art3CheckDeletedItemsResponse> ExecuteAsync(
        Art3CheckDeletedItemsRequest request,
        CancellationToken ct
    )
    {
        var unitOfWork = _unitOfWork.Value;

        _art3Repository = unitOfWork.Art3Repository;

        var result = await _art3Repository.CheckCurrentCreatorDeletedItemsAsync(
            request.CreatorId,
            ct
        );

        return new() { Result = result };
    }
}
