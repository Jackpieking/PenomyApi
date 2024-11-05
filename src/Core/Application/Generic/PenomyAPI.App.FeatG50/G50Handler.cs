using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG50;

public class G50Handler : IFeatureHandler<G50Request, G50Response>
{
    private readonly IG50Repository _g50Repository;

    public G50Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g50Repository = unitOfWork.Value.G50Repository;
    }

    public async Task<G50Response> ExecuteAsync(G50Request request, CancellationToken ct)
    {
        try
        {
            var isSuccess =
                await _g50Repository.RevokeStarForArtworkAsync(request.GetUserId(), request.ArtworkId, ct);
            return isSuccess ? G50Response.SUCCESS : G50Response.FAILED;
        }
        catch
        {
            return G50Response.FAILED;
        }
    }
}
