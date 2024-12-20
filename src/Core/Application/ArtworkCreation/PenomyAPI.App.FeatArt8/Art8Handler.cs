using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt8;

public sealed class Art8Handler : IFeatureHandler<Art8Request, Art8Response>
{
    private IArt8Repository _featRepository;
    private IArtworkRepository _artworkRepository;
    private readonly IUnitOfWork _unitOfWork;

    public Art8Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
    }

    public async Task<Art8Response> ExecuteAsync(Art8Request request, CancellationToken cancellationToken)
    {
        // Check if the artwork is existed or not before processing.
        _artworkRepository = _unitOfWork.ArtworkRepository;

        var isArtworkExisted = await _artworkRepository.IsArtworkExistedByIdAsync(
            request.ArtworkId,
            cancellationToken);

        if (!isArtworkExisted)
        {
            return Art8Response.NOT_FOUND;
        }

        // Check if the artwork is already temporarily removed or not before processing.
        var isAlreadyRemoved = await _artworkRepository.IsArtworkTemporarilyRemovedByIdAsync(
            request.ArtworkId,
            cancellationToken);

        if (isAlreadyRemoved)
        {
            return Art8Response.ALREADY_REMOVED;
        }

        // Remove the artwork.
        _featRepository = _unitOfWork.Art8Repository;

        var removedAt = DateTime.UtcNow;

        var result = await _featRepository.TemporarilyRemoveArtworkByIdAsync(
            request.ArtworkId,
            request.RemovedBy,
            removedAt,
            cancellationToken);

        if (result)
        {
            return Art8Response.SUCCESS;
        }

        return Art8Response.DATABASE_ERROR;
    }
}
