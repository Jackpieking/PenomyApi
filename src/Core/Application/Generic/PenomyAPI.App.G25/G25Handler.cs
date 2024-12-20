using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25;

public class G25Handler : IFeatureHandler<G25Request, G25Response>
{
    private readonly IG25Repository _g25Repository;

    public G25Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25Response> ExecuteAsync(G25Request request, CancellationToken ct)
    {
        try
        {
            var isUserViewHistoryNotEmpty = await _g25Repository.IsUserViewHistoryNotEmptyAsync(
                request.UserId,
                request.ArtworkType,
                ct);
            
            if (!isUserViewHistoryNotEmpty)
            {
                return G25Response.EMPTY_VIEW_HISTORY;
            }

            var artViewHist = await _g25Repository
                .GetArtworkViewHistByUserIdAndTypeWithPaginationAsync(
                    request.UserId,
                    request.ArtworkType,
                    request.PageNum,
                    request.ArtNum,
                    ct
                );

            return new G25Response
            {
                Result = artViewHist,
                IsSuccess = true,
                StatusCode = G25ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new G25Response
            {
                IsSuccess = false,
                StatusCode = G25ResponseStatusCode.FAILED
            };
        }
    }
}
