using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RestoreDeletedItems;

public class Art3RestoreDeletedItemsHandler
    : IFeatureHandler<Art3RestoreDeletedItemsRequest, Art3RestoreDeletedItemResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt3Repository _art3Repository;

    public Art3RestoreDeletedItemsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art3RestoreDeletedItemResponse> ExecuteAsync(Art3RestoreDeletedItemsRequest request, CancellationToken ct)
    {
        _art3Repository = _unitOfWork.Value.Art3Repository;

        var artworksAreTemporarilyRemoved = await _art3Repository.AreArtworksTemporarilyRemovedAsync(
            request.ArtworkIds,
            request.CreatorId,
            ct);

        if (!artworksAreTemporarilyRemoved)
        {
            return Art3RestoreDeletedItemResponse.CREATOR_HAS_NO_PERMISSION;
        }

        var restoreResult = await _art3Repository.RestoreDeletedArtworksByIdsAsync(
            request.ArtworkIds,
            ct);

        if (restoreResult)
        {
            return Art3RestoreDeletedItemResponse.SUCCESS;
        }

        return Art3RestoreDeletedItemResponse.DATABASE_ERROR;
    }
}
