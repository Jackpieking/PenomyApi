using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG19.OtherHandlers.GetChapterList;

public class G19GetChapterListHandler
    : IFeatureHandler<G19GetChapterListRequest, G19GetChapterListResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArtworkRepository _artworkRepository;
    private IG19Repository _g19Repository;

    public G19GetChapterListHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G19GetChapterListResponse> ExecuteAsync(
        G19GetChapterListRequest request,
        CancellationToken ct
    )
    {
        var unitOfWork = _unitOfWork.Value;

        // Check if the comic id is existed or available to display not.
        _artworkRepository = unitOfWork.ArtworkRepository;

        var isArtworkExisted = await _artworkRepository.IsArtworkAvailableToDisplayByIdAsync(
            request.AnimeId,
            request.UserId,
            ct
        );

        if (!isArtworkExisted)
        {
            return G19GetChapterListResponse.ANIME_ID_NOT_FOUND;
        }

        // Get the chapter list.
        _g19Repository = unitOfWork.G19Repository;

        var chapterList = await _g19Repository.GetAllChaptersAsyncByAnimeId(request.AnimeId, ct);

        return G19GetChapterListResponse.SUCCESS(chapterList);
    }
}
