using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        List<Artwork> result = [];
        try
        {
            if (request.UserId == 0 || request.Limit <= 0)
            {
                return new G14Response
                {
                    StatusCode = G14ResponseStatusCode.INVALID_REQUEST,
                    IsSuccess = false
                };
            }
            List<Category> categories = await _IG14Repository.GetUserFavoritesCategoryIdsAsync(request.UserId, request.Limit, ct);
            foreach (var category in categories)
            {
                if (await _IG14Repository.IsExistCategoryAsync(category.Id, ct))
                {
                    result.AddRange(await _IG14Repository.GetRecommendedAnimeAsync(category.Id, ct));
                }
            }
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
