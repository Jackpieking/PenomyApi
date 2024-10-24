using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG19;

public class G19Handler : IFeatureHandler<G19Request, G19Response>
{
    private readonly IG19Repository _g19Repository;

    public G19Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g19Repository = unitOfWork.Value.G19Repository;
    }

    public async Task<G19Response> ExecuteAsync(G19Request request, CancellationToken ct)
    {
        List<ArtworkChapter> chapters;
        int chapterCount = 0;
        if (request.Id == 0 || request.StartPage <= 0 || request.PageSize <= 0)
        {
            return new() { StatusCode = G19ResponseStatusCode.INVALID_REQUEST, IsSuccess = false };
        }
        if (!await _g19Repository.IsArtworkExistAsync(request.Id, ct))
        {
            return new() { StatusCode = G19ResponseStatusCode.NOT_FOUND, IsSuccess = false };
        }
        (chapters, chapterCount) = await _g19Repository.GetArtWorkChapterByIdAsync(
            request.Id,
            request.StartPage,
            request.PageSize,
            ct
        );

        return new()
        {
            ChapterCount = chapterCount,
            Chapters = chapters,
            StatusCode = G19ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
