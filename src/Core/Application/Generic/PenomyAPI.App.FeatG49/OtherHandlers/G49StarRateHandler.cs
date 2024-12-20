using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG49.OtherHandlers;

public class G49StarRateHandler : IFeatureHandler<G49StarRateRequest, G49StarRateResponse>
{
    private readonly IG49Repository _g49Repository;

    public G49StarRateHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g49Repository = unitOfWork.Value.G49Repository;
    }

    public async Task<G49StarRateResponse> ExecuteAsync(G49StarRateRequest request, CancellationToken ct)
    {
        G49StarRateResponse response = new()
        {
            StarRate = await _g49Repository.GetCurrentUserRatingAsync(request.UserId, request.ArtworkId, ct),
            AppCode = G49ResponseStatusCode.SUCCESS
        };
        return response;
    }
}
