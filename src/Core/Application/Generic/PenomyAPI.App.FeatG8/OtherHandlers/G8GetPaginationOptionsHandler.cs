using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG8.OtherHandlers;

public sealed class G8GetPaginationOptionsHandler
    : IFeatureHandler<G8GetPaginationOptionsRequest, G8GetPaginationOptionsResponse>
{
    private readonly IG8Repository _g8Repository;

    public G8GetPaginationOptionsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g8Repository = unitOfWork.Value.G8Repository;
    }

    public async Task<G8GetPaginationOptionsResponse> ExecuteAsync(
        G8GetPaginationOptionsRequest request,
        CancellationToken ct)
    {
        var isComicExisted = await _g8Repository.IsArtworkExistAsync(
            request.ComicId,
            ct);

        if (!isComicExisted)
        {
            return G8GetPaginationOptionsResponse.COMIC_IS_NOT_FOUND;
        }

        var totalChapters = await _g8Repository.GetTotalChaptersByComicIdAsync(
            request.ComicId,
            ct);

        return G8GetPaginationOptionsResponse.CalculateAndReturn(totalChapters);
    }
}
