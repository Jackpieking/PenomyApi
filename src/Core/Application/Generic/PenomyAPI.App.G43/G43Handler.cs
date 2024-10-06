using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

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
            await _g43Repository.FollowArtwork(
            request.userId,
            request.artworkId,
            request.ArtworkType,
            ct
            );
        }
        catch
        {
            return new G43Response { IsSuccess = false, StatusCode = G43ResponseStatusCode.FAILED };
        }

        return new G43Response { IsSuccess = true, StatusCode = G43ResponseStatusCode.SUCCESS };
    }
}
