using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Common;

namespace PenomyAPI.App.FeatArt8;

public sealed class Art8Handler : IFeatureHandler<Art8Request, Art8Response>
{
    private readonly IArt8Repository _featRepository;
    private readonly IArtworkRepository _artworkRepository;

    public Art8Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _featRepository = unitOfWork.Value.Art8Repository;
        _artworkRepository = unitOfWork.Value.ArtworkRepository;
    }

    public async Task<Art8Response> ExecuteAsync(Art8Request request, CancellationToken cancellationToken)
    {
        // Check if the artwork is existed or not before processing.
        var isArtworkExisted = await _artworkRepository.IsArtworkExistedByIdAsync(
            request.ArtworkId,
            cancellationToken);

        if (!isArtworkExisted)
        {
            return Art8Response.NOT_FOUND;
        }

        var isAlreadyRemoved = await _featRepository.IsArtworkTemporarilyRemovedByIdAsync(
            request.ArtworkId,
            cancellationToken);

        if (isAlreadyRemoved)
        {
            return Art8Response.ALREADY_REMOVED;
        }

        // Remove the artwork.
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
