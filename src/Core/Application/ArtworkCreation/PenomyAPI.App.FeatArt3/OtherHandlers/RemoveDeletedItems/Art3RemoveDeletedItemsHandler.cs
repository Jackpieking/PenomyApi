using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RemoveDeletedItems;

public class Art3RemoveDeletedItemsHandler
    : IFeatureHandler<Art3RemoveDeletedItemsRequest, Art3RemoveDeletedItemsResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt3Repository _art3Repository;

    public Art3RemoveDeletedItemsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art3RemoveDeletedItemsResponse> ExecuteAsync(
        Art3RemoveDeletedItemsRequest request,
        CancellationToken ct)
    {
        _art3Repository = _unitOfWork.Value.Art3Repository;

        var artworkIdsAreValidToDelete = await _art3Repository.AreArtworksTemporarilyRemovedAsync(
            request.DeletedArtworkIds,
            request.CreatorId,
            ct
        );

        if (!artworkIdsAreValidToDelete)
        {
            return Art3RemoveDeletedItemsResponse.CREATOR_HAS_NO_PERMISSION;
        }

        var deleteResult = await _art3Repository.PermanentlyRemoveArtworksByIdsAsync(
            request.DeletedArtworkIds,
            ct);

        if (!deleteResult)
        {
            return Art3RemoveDeletedItemsResponse.DATABASE_ERROR;
        }

        return Art3RemoveDeletedItemsResponse.SUCCESS;
    }
}
