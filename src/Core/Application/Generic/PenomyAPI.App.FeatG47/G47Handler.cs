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
        try
        {
            if (
                !IsValidRequest(request)
                || !await _g47Repository.IsUserActiveAsync(request.UserId, ct)
            )
                return G47Response.INVALID_REQUEST;
            if (
                !await _g47Repository.IsArtworkExistAsync(request.ArtworkId, ct)
                || !await _g47Repository.IsAlreadyFavoriteAsync(
                    request.UserId,
                    request.ArtworkId,
                    ct
                )
            )
                return G47Response.NOT_FOUND;
            var isSuccess = await _g47Repository.RemoveFromFavoriteAsync(
                request.UserId,
                request.ArtworkId,
                ct
            );
            return isSuccess ? G47Response.SUCCESS : G47Response.FAILED;
        }
        catch
        {
            return G47Response.FAILED;
        }
    }

    private static bool IsValidRequest(G47Request request)
    {
        return request is { UserId: > 0, ArtworkId: > 0 };
    }
}
