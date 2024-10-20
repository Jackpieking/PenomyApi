using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG14.OtherHandler;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG14;

public class G14GuestHandler : IFeatureHandler<G14GuestRequest, G14GuestResponse>
{
    private readonly IG14Repository _IG14Repository;

    public G14GuestHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _IG14Repository = unitOfWork.Value.G14Repository;
    }
    public async Task<G14GuestResponse> ExecuteAsync(G14GuestRequest request, CancellationToken ct)
    {
        List<Artwork> result = [];
        try
        {
            if (request.GuestId == 0 || request.Limit <= 0)
            {
                return new G14GuestResponse
                {
                    StatusCode = G14ResponseStatusCode.INVALID_REQUEST,
                    IsSuccess = false
                };
            }
            List<long> categories = await _IG14Repository.GetCategoryIdsFromGuestViewHistoryAsync(request.GuestId, request.Limit, ct);
            foreach (var category in categories)
            {
                if (await _IG14Repository.IsExistCategoryAsync(category, ct))
                {
                    result.AddRange(await _IG14Repository.GetRecommendedAnimeAsync(category, ct));
                }
            }
        }
        catch
        {
            return new G14GuestResponse { StatusCode = G14ResponseStatusCode.FAILED, IsSuccess = false };
        }
        return new()
        {
            Artworks = result,
            StatusCode = G14ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
