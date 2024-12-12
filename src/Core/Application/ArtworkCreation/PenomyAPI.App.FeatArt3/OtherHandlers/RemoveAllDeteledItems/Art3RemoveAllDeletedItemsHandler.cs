using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RemoveAllDeteledItems;

public class Art3RemoveAllDeletedItemsHandler
    : IFeatureHandler<Art3RemoveAllDeletedItemsRequest, Art3RemoveAllDeletedItemsResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt3Repository _art3Repository;

    public Art3RemoveAllDeletedItemsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art3RemoveAllDeletedItemsResponse> ExecuteAsync(
        Art3RemoveAllDeletedItemsRequest request,
        CancellationToken ct)
    {
        _art3Repository = _unitOfWork.Value.Art3Repository;

        var hasAnyDeletedItem = await _art3Repository.HasAnyDeletedItemsWithArtworTypeAsync(
            request.CreatorId,
            request.ArtworkType,
            ct);

        if (!hasAnyDeletedItem)
        {
            return Art3RemoveAllDeletedItemsResponse.NO_DELETED_ITEMS_FOUND;
        }

        var removeResult = await _art3Repository.PermanentlyRemoveAllDeletedItemsByArtworkTypeAsync(
            request.CreatorId,
            request.ArtworkType,
            ct);

        if (removeResult)
        {
            return Art3RemoveAllDeletedItemsResponse.SUCCESS;
        }

        return Art3RemoveAllDeletedItemsResponse.DATABASE_ERROR;
    }
}
