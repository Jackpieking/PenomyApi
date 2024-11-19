using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
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
        if (!await _g8Repository.IsArtworkExistAsync(request.Id, ct))
        {
            return G8Response.COMIC_IS_NOT_FOUND;
        }

        var chapters = await _g8Repository.GetChapterByComicIdWithPaginationAsync(
            request.Id,
            request.StartPage,
            request.PageSize,
            ct
        );

        return new()
        {
            ChapterCount = 0,
            Chapters = chapters,
            StatusCode = G8ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
