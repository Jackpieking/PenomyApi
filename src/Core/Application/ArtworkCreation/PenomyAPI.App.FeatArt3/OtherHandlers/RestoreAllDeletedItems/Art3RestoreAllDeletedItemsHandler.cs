using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RestoreAllDeletedItems;

public class Art3RestoreAllDeletedItemsHandler
    : IFeatureHandler<Art3RestoreAllDeletedItemsRequest, Art3RestoreAllDeletedItemsResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt3Repository _art3Repository;

    public Art3RestoreAllDeletedItemsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art3RestoreAllDeletedItemsResponse> ExecuteAsync(
        Art3RestoreAllDeletedItemsRequest request,
        CancellationToken ct)
    {
        _art3Repository = _unitOfWork.Value.Art3Repository;

        var hasAnyDeletedItem = await _art3Repository.HasAnyDeletedItemsWithArtworTypeAsync(
            request.CreatorId,
            request.ArtworkType,
            ct);

        if (!hasAnyDeletedItem)
        {
            return Art3RestoreAllDeletedItemsResponse.NO_DELETED_ITEMS_FOUND;
        }

        var restoreResult = await _art3Repository.RestoreAllDeletedItemsByArtworkTypeAsync(
            request.CreatorId,
            request.ArtworkType,
            ct);

        if (restoreResult)
        {
            return Art3RestoreAllDeletedItemsResponse.SUCCESS;
        }

        return Art3RestoreAllDeletedItemsResponse.DATABASE_ERROR;
    }
}
