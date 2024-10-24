using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        try
        {
            if (!IsValidRequest(request) || !await _g46Repository.IsUserActiveAsync(request.UserId, ct))
            {
                return G46Response.INVALID_REQUEST;
            }
            if (!await _g46Repository.IsArtworkExistAsync(request.ArtworkId, ct))
            {
                return G46Response.NOT_FOUND;
            }
            if (await _g46Repository.IsAlreadyFavoriteAsync(request.UserId, request.ArtworkId, ct))
            {
                return G46Response.EXISTED;
            }
            bool isSuccess = await _g46Repository.AddArtworkFavoriteAsync(request.UserId, request.ArtworkId, ct);
            return isSuccess ? G46Response.SUCCESS : G46Response.FAILED;
        }
        catch
        {
            return G46Response.FAILED;
        }
    }

    private static bool IsValidRequest(G46Request request) =>
        request.UserId > 0 && request.ArtworkId > 0;
}
