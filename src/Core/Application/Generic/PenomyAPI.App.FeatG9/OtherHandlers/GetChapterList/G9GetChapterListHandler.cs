using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG9.OtherHandlers.GetChapterList;

public class G9GetChapterListHandler
    : IFeatureHandler<G9GetChapterListRequest, G9GetChapterListResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArtworkRepository _artworkRepository;
    private IG9Repository _g9Repository;

    public G9GetChapterListHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G9GetChapterListResponse> ExecuteAsync(
        G9GetChapterListRequest request,
        CancellationToken ct
    )
    {
        var unitOfWork = _unitOfWork.Value;

        // Check if the comic id is existed or available to display not.
        _artworkRepository = unitOfWork.ArtworkRepository;

        var isComicExisted = await _artworkRepository.IsArtworkAvailableToDisplayByIdAsync(
            request.ComicId,
            request.UserId,
            ct
        );

        if (!isComicExisted)
        {
            return G9GetChapterListResponse.COMIC_ID_NOT_FOUND;
        }

        // Get the chapter list.
        _g9Repository = unitOfWork.G9Repository;

        var chapterList = await _g9Repository.GetAllChaptersAsyncByComicId(request.ComicId, ct);

        return G9GetChapterListResponse.SUCCESS(chapterList);
    }
}
