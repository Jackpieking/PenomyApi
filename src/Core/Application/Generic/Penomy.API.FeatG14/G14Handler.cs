using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG14;

public class G14Handler : IFeatureHandler<G14Request, G14Response>
{
    private readonly IG14Repository _IG14Repository;

    public G14Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _IG14Repository = unitOfWork.Value.G14Repository;
    }
    public async Task<G14Response> ExecuteAsync(G14Request request, CancellationToken ct)
    {
        List<Artwork> result;
        try
        {
            if (request.CategoryId == 0)
            {
                return new G14Response
                {
                    StatusCode = G14ResponseStatusCode.INVALID_REQUEST,
                    IsSuccess = false
                };
            }
            if (!await _IG14Repository.IsExistCategoryAsync(request.CategoryId, ct))
            {
                return new G14Response
                {
                    StatusCode = G14ResponseStatusCode.NOT_FOUND,
                    IsSuccess = false
                };
            }
            result = await _IG14Repository.GetRecommendedAnimeAsync(request.CategoryId, request.Limit, ct);
        }
        catch
        {
            return new G14Response { StatusCode = G14ResponseStatusCode.FAILED, IsSuccess = false };
        }
        return new()
        {
            Artworks = result,
            StatusCode = G14ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
