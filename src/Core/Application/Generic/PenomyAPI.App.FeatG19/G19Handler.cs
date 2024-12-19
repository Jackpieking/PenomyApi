using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG19;

public class G19Handler : IFeatureHandler<G19Request, G19Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private IG19Repository _g19Repository;
    private IArtworkChapterRepository _chapterRepository;

    public G19Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
    }

    public async Task<G19Response> ExecuteAsync(
        G19Request request,
        CancellationToken ct)
    {
        // Check if the chapter is existed or not
        _chapterRepository = _unitOfWork.ChapterRepository;

        var isChapterExisted = await _chapterRepository.IsChapterBelongedToArtworkByIdAsync(
            request.AnimeId,
            request.ChapterId,
            ct);

        if (!isChapterExisted)
        {
            return G19Response.CHAPTER_IS_NOT_FOUND;
        }

        // Check if the chapter is available to display to user.
        var isAvailableToDisplay = await _chapterRepository.IsChapterAvailableToDisplayByIdAsync(
            request.ChapterId,
            request.UserId);

        if (!isAvailableToDisplay)
        {
            return G19Response.CHAPTER_IS_NOT_FOUND;
        }

        // Get the detail of the chapter.
        _g19Repository = _unitOfWork.G19Repository;

        var chapterDetail = await _g19Repository.GetChapterDetailByIdAsync(
            request.ChapterId,
            ct);

        return G19Response.SUCCESS(chapterDetail);
    }
}
