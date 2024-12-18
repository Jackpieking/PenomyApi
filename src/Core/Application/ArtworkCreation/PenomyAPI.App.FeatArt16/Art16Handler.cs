using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt16;

public class Art16Handler : IFeatureHandler<Art16Request, Art16Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt16Repository _repository;
    private IArtworkRepository _artworkRepository;

    public Art16Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art16Response> ExecuteAsync(
        Art16Request request,
        CancellationToken cancellationToken)
    {
        // Check if the comic is existed or not before processing.
        var unitOfWork = _unitOfWork.Value;

        _artworkRepository = unitOfWork.ArtworkRepository;

        var isAnimeExisted = await _artworkRepository.IsAnimeExistedByIdAsync(
            request.ArtworkId,
            cancellationToken);

        if (!isAnimeExisted)
        {
            return Art16Response.ARTWORK_ID_NOT_FOUND;
        }

        // Check if the comic is temporarily removed or not before processing.
        var isTemporarilyRemoved = await _artworkRepository.IsArtworkTemporarilyRemovedByIdAsync(
            request.ArtworkId,
            cancellationToken);

        if (isTemporarilyRemoved)
        {
            return Art16Response.ARTWORK_IS_TEMPORARILY_REMOVED;
        }

        // Check if the current creator is authorized to access to this comic.
        var isAuthorized = await _artworkRepository.IsArtworkBelongedToCreatorAsync(
            request.ArtworkId,
            request.CreatorId,
            cancellationToken
        );

        if (!isAuthorized)
        {
            return Art16Response.ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR;
        }

        _repository = unitOfWork.Art16Repository;

        var animeDetail = await _repository.GetAnimeDetailByIdAsync(
            request.ArtworkId,
            cancellationToken);

        return Art16Response.SUCCESS(animeDetail);
    }
}
