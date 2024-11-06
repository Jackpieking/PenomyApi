using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG47;

public class G47Handler : IFeatureHandler<G47Request, G47Response>
{
    private readonly IG47Repository _g47Repository;

    public G47Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g47Repository = unitOfWork.Value.G47Repository;
    }

    public async Task<G47Response> ExecuteAsync(G47Request request, CancellationToken ct)
    {
        G47Response response = new();
        try
        {
            if (
                !IsValidRequest(request)
                || !await _g47Repository.IsUserActiveAsync(request.UserId, ct)
            )
                response.AppCode = G47ResponseStatusCode.INVALID_REQUEST;
            if (
                !await _g47Repository.IsArtworkExistAsync(request.ArtworkId, ct)
                || !await _g47Repository.IsAlreadyFavoriteAsync(
                    request.UserId,
                    request.ArtworkId,
                    ct
                )
            )
                response.AppCode = G47ResponseStatusCode.NOT_FOUND;
            response.FavoriteCount = await _g47Repository.RemoveFromFavoriteAsync(
                request.UserId,
                request.ArtworkId,
                ct
            );
        }
        catch
        {
            response.AppCode = G47ResponseStatusCode.FAILED;
        }

        return response;
    }

    private static bool IsValidRequest(G47Request request)
    {
        return request is { UserId: > 0, ArtworkId: > 0 };
    }
}
