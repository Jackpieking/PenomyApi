using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG46;

public class G46Handler : IFeatureHandler<G46Request, G46Response>
{
    private readonly IG46Repository _g46Repository;

    public G46Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g46Repository = unitOfWork.Value.G46Repository;
    }

    public async Task<G46Response> ExecuteAsync(G46Request request, CancellationToken ct)
    {
        G46Response response = new();
        try
        {
            if (!IsValidRequest(request) || !await _g46Repository.IsUserActiveAsync(request.UserId, ct))
                response.AppCode = G46ResponseStatusCode.INVALID_REQUEST;
            if (!await _g46Repository.IsArtworkExistAsync(request.ArtworkId, ct))
                response.AppCode = G46ResponseStatusCode.NOT_FOUND;
            if (await _g46Repository.IsAlreadyFavoriteAsync(request.UserId, request.ArtworkId, ct))
                response.AppCode = G46ResponseStatusCode.EXISTED;
            response.FavoriteCount =
                await _g46Repository.AddArtworkFavoriteAsync(request.UserId, request.ArtworkId, ct);
        }
        catch
        {
            response.AppCode = G46ResponseStatusCode.FAILED;
        }

        return response;
    }

    private static bool IsValidRequest(G46Request request)
    {
        return request.UserId > 0 && request.ArtworkId > 0;
    }
}
