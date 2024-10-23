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
        if (request.UserId == 0)
        {
            return new G25Response { StatusCode = G25ResponseStatusCode.EMPTY };
        }

        return new G25Response
        {
            Result = await _g25Repository.GetArtworkViewHistories(
                request.UserId,
                request.ArtworkType,
                ct,
                request.PageNum,
                request.ArtNum
            ),
            IsSuccess = true,
            StatusCode = G25ResponseStatusCode.SUCCESS
        };
    }
}
