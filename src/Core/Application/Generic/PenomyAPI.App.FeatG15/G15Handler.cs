using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG15;

public class G15Handler : IFeatureHandler<G15Request, G15Response>
{
    private readonly IG15Repository _IG15Repository;

    public G15Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _IG15Repository = unitOfWork.Value.G15Repository;
    }

    public async Task<G15Response> ExecuteAsync(G15Request request, CancellationToken ct)
    {
        Artwork result;
        try
        {
            var rq = request.Id;
            if (rq == 0)
            {
                return new G15Response
                {
                    StatusCode = G15ResponseStatusCode.INVALID_REQUEST,
                    IsSuccess = false
                };
            }
            if (!await _IG15Repository.IsArtworkExistAsync(rq, ct))
            {
                return new G15Response
                {
                    StatusCode = G15ResponseStatusCode.NOT_FOUND,
                    IsSuccess = false
                };
            }
            result = await _IG15Repository.GetArtWorkDetailByIdAsync(rq, ct);
        }
        catch
        {
            return new G15Response { StatusCode = G15ResponseStatusCode.FAILED, IsSuccess = false };
        }
        return new()
        {
            Result = result,
            StatusCode = G15ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
