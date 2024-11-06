using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG8;

public class G8Handler : IFeatureHandler<G8Request, G8Response>
{
    private readonly IG8Repository _g8Repository;

    public G8Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g8Repository = unitOfWork.Value.G8Repository;
    }

    public async Task<G8Response> ExecuteAsync(G8Request request, CancellationToken ct)
    {
        List<ArtworkChapter> chapters;
        int chapterCount = 0;
        if (request.Id == 0 || request.StartPage <= 0 || request.PageSize <= 0)
        {
            return new() { StatusCode = G8ResponseStatusCode.INVALID_REQUEST, IsSuccess = false };
        }
        if (!await _g8Repository.IsArtworkExistAsync(request.Id, ct))
        {
            return new() { StatusCode = G8ResponseStatusCode.NOT_FOUND, IsSuccess = false };
        }
        (chapters, chapterCount) = await _g8Repository.GetArtWorkChapterByIdAsync(
            request.Id,
            request.StartPage,
            request.PageSize,
            ct
        );

        return new()
        {
            ChapterCount = chapterCount,
            Chapters = chapters,
            StatusCode = G8ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
