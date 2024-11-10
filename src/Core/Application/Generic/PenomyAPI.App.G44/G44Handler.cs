using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G44;

public class G44Handler : IFeatureHandler<G44Request, G44Response>
{
    private readonly IG44Repository _g44Repository;

    public G44Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g44Repository = unitOfWork.Value.G44Repository;
    }

    public async Task<G44Response> ExecuteAsync(G44Request request, CancellationToken ct)
    {
        try
        {
            await _g44Repository.UnFollowArtwork(
                request.UserId,
                request.ArtworkId,
                request.ArtworkType,
                ct
            );
        }
        catch
        {
            return new G44Response { IsSuccess = false, StatusCode = G44ResponseStatusCode.INVALID_REQUEST };
        }

        return new G44Response { IsSuccess = true, StatusCode = G44ResponseStatusCode.SUCCESS };
    }
}
