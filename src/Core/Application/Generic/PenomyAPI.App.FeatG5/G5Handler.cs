using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG5;

public class G5Handler : IFeatureHandler<G5Request, G5Response>
{
    private readonly IG5Repository _IG5Repository;

    public G5Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _IG5Repository = unitOfWork.Value.FeatG5Repository;
    }

    public async Task<G5Response> ExecuteAsync(G5Request request, CancellationToken ct)
    {
        Artwork result;

        var rq = request.Id;
        if (rq == 0)
        {
            return new G5Response { StatusCode = G5ResponseStatusCode.INVALID_REQUEST };
        }
        result = await _IG5Repository.GetArtWorkDetailByIdAsync(rq, ct);

        return new() { Result = result, StatusCode = G5ResponseStatusCode.SUCCESS };
    }
}
