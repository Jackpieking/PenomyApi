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
            ArtworkType artworkType = await _g43Repository.GetArtworTypeById(request.ArtworkId, ct);

            // Check artwork is existed or not
            if (artworkType == ArtworkType.NotFound)
            {
                return G43Response.FAILED;
            }

            var isAlreadyFollowed = await _g43Repository.CheckFollowedArtwork(
                request.UserId,
                request.ArtworkId, ct);

            if (isAlreadyFollowed)
            {
                return G43Response.INVALID_REQUEST;
            }

            await _g43Repository.FollowArtwork(
                request.UserId,
                request.ArtworkId,
                artworkType,
                ct
            );

            return G43Response.SUCCESS;
        }
        catch
        {
            return G43Response.FAILED;
        }
    }
}
