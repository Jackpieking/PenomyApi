using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG9;

public sealed class G9Handler
    : IFeatureHandler<G9Request, G9Response>
{
    private readonly IUnitOfWork unitOfWork;
    private IG9Repository _g9Repository;

    public G9Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        this.unitOfWork = unitOfWork.Value;
    }

    public async Task<G9Response> ExecuteAsync(G9Request request, CancellationToken ct)
    {
        var chapterRepository = unitOfWork.ChapterRepository;

        // Check if the chapter is existed or not
        var isChapterExisted = await chapterRepository.IsChapterBelongedToArtworkByIdAsync(
            request.ComicId,
            request.ChapterId,
            ct);

        if (!isChapterExisted)
        {
            return G9Response.CHAPTER_IS_NOT_FOUND;
        }

        // Check if the chapter is available to display to user.
        var isAvailableToDisplay = await chapterRepository.IsChapterAvailableToDisplayByIdAsync(
            request.ChapterId,
            request.UserId);

        if (!isAvailableToDisplay)
        {
            return G9Response.CHAPTER_IS_NOT_FOUND;
        }

        // Get the detail of the chapter.
        _g9Repository = unitOfWork.G9Repository;

        var chapterDetail = await _g9Repository.GetComicChapterDetailByIdAsync(
            request.ComicId,
            request.ChapterId,
            ct);

        chapterDetail.ChapterMedias = chapterDetail.ChapterMedias.OrderBy(
            media => media.UploadOrder);

        return G9Response.SUCCESS(chapterDetail);
    }
}
