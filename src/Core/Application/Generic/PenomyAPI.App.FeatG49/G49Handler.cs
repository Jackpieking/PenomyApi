using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG49;

public class G49Handler : IFeatureHandler<G49Request, G49Response>
{
    private readonly IG49Repository _g49Repository;

    public G49Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g49Repository = unitOfWork.Value.G49Repository;
    }

    public async Task<G49Response> ExecuteAsync(G49Request request, CancellationToken ct)
    {
        G49Response response = new();
        try
        {
            if (!await _g49Repository.IsArtworkExistsAsync(request.ArtworkId, ct))
                response.AppCode
                    = G49ResponseStatusCode.NOT_FOUND;
            var starRates =
                await _g49Repository.RateArtworkAsync(request.UserId, request.ArtworkId, request.StarRate, ct);
            response.StarRate = starRates;
            response.AppCode = starRates > 0 ? G49ResponseStatusCode.SUCCESS : G49ResponseStatusCode.FAILED;
        }
        catch
        {
            response.AppCode = G49ResponseStatusCode.FAILED;
        }

        return response;
    }
}
