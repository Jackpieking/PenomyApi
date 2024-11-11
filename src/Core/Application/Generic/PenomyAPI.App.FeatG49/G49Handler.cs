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
            var result =
                await _g49Repository.RateArtworkAsync(request.UserId, request.ArtworkId, request.StarRate, ct);
            response.TotalStarRate = result.TotalStarRates;
            response.TotalUsersRate = result.TotalUsersRated;
            response.StarRate = result.AverageStarRate;
            response.CurrentUserRate =
                await _g49Repository.GetCurrentUserRatingAsync(request.UserId, request.ArtworkId, ct);
            response.AppCode = G49ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.AppCode = G49ResponseStatusCode.FAILED;
        }

        return response;
    }
}
