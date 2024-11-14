using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G43;

public class G43Handler : IFeatureHandler<G43Request, G43Response>
{
    private readonly IG43Repository _g43Repository;

    public G43Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g43Repository = unitOfWork.Value.G43Repository;
    }

    public async Task<G43Response> ExecuteAsync(G43Request request, CancellationToken ct)
    {
        try
        {
            ArtworkType artworkType = await _g43Repository.CheckArtworkExist(request.ArtworkId, ct);

            if (await _g43Repository.CheckFollowedArtwork(request.UserId, request.ArtworkId, ct))
            {
                throw new Exception("Request invalid");
            }

            await _g43Repository.FollowArtwork(
                request.UserId,
                request.ArtworkId,
                artworkType,
                ct
            );
        }
        catch
        {
            return new G43Response { IsSuccess = false, StatusCode = G43ResponseStatusCode.INVALID_REQUEST };
        }

        return new G43Response { IsSuccess = true, StatusCode = G43ResponseStatusCode.SUCCESS };
    }
}
