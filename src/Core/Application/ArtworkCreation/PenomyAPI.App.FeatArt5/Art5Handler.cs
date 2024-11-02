using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt5;

public sealed class Art5Handler : IFeatureHandler<Art5Request, Art5Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private IArt5Repository _repository;
    private IArtworkRepository _artworkRepository;

    public Art5Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
    }

    public async Task<Art5Response> ExecuteAsync(
        Art5Request request,
        CancellationToken cancellationToken)
    {
        // Check if the comic is existed or not before processing.
        _artworkRepository = _unitOfWork.ArtworkRepository;

        var isComicExisted = await _artworkRepository.IsArtworkExistedByIdAsync(
            request.ComicId,
            cancellationToken);

        if (!isComicExisted)
        {
            return Art5Response.COMIC_ID_NOT_FOUND;
        }

        // Check if the comic is temporarily removed or not before processing.
        var isTemporarilyRemoved = await _artworkRepository.IsArtworkTemporarilyRemovedByIdAsync(
            request.ComicId,
            cancellationToken);

        if (isTemporarilyRemoved)
        {
            return Art5Response.COMIC_IS_TEMPORARILY_REMOVED;
        }

        // Check if the current creator is authorized to access to this comic.
        var isAuthorized = await _artworkRepository.IsArtworkBelongedToCreatorAsync(
            request.ComicId,
            request.CreatorId,
            cancellationToken
        );

        if (!isAuthorized)
        {
            return Art5Response.COMIC_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR;
        }

        _repository = _unitOfWork.Art5Repository;

        var comicDetail = await _repository.GetComicDetailByIdAsync(
            request.ComicId,
            cancellationToken);

        return new()
        {
            AppCode = Art5ResponseAppCode.SUCCESS,
            ComicDetail = comicDetail
        };
    }
}
