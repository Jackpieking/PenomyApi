using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;

public sealed class Art10GetDetailToCreateChapterHandler
    : IFeatureHandler<Art10GetDetailToCreateChapterRequest, Art10GetDetailToCreateChapterResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private IArt10Repository _repository;
    private IArtworkRepository _artworkRepository;

    public Art10GetDetailToCreateChapterHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
    }

    public async Task<Art10GetDetailToCreateChapterResponse> ExecuteAsync(
        Art10GetDetailToCreateChapterRequest request,
        CancellationToken cancellationToken)
    {
        _artworkRepository = _unitOfWork.ArtworkRepository;

        var isComicExisted = await _artworkRepository.IsArtworkExistedByIdAsync(
            request.ComicId,
            cancellationToken);

        if (!isComicExisted)
        {
            return Art10GetDetailToCreateChapterResponse.COMIC_ID_NOT_FOUND;
        }

        // Get the comic detail to support create new chapter and return.
        _repository = _unitOfWork.Art10Repository;

        var comicDetail = await _repository.GetDetailToCreateChapterByComicIdAsync(
            request.ComicId,
            cancellationToken);

        return new Art10GetDetailToCreateChapterResponse
        {
            AppCode = Art10GetDetailToCreateChapterResponseAppCode.SUCCESS,
            ComicDetail = comicDetail
        };
    }
}
