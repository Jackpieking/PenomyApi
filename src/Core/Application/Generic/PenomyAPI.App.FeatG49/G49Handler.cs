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
        try
        {
            if (!await _g49Repository.IsArtworkExistsAsync(request.ArtworkId, ct)) return G49Response.NOT_FOUND;
            var isSuccess =
                await _g49Repository.RateArtworkAsync(request.UserId, request.ArtworkId, request.StarRate, ct);
            return isSuccess ? G49Response.SUCCESS : G49Response.FAILED;
        }
        catch
        {
            return G49Response.FAILED;
        }
    }
}
