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
        var isArtworkFavorite = false;
        try
        {
            var rq = request.Id;
            if (rq == 0)
                return new G5Response
                {
                    StatusCode = G5ResponseStatusCode.INVALID_REQUEST,
                    IsSuccess = false
                };
            if (!await _IG5Repository.IsArtworkExistAsync(rq, ct))
                return new G5Response
                {
                    StatusCode = G5ResponseStatusCode.NOT_FOUND,
                    IsSuccess = false
                };
            result = await _IG5Repository.GetArtWorkDetailByIdAsync(rq, ct);
            isArtworkFavorite = await _IG5Repository.IsArtworkFavoriteAsync(request.UserId, request.Id, ct);
        }
        catch
        {
            return new G5Response { StatusCode = G5ResponseStatusCode.FAILED, IsSuccess = false };
        }

        return new G5Response
        {
            Result = result,
            IsArtworkFavorite = isArtworkFavorite,
            StatusCode = G5ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
