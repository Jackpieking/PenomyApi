using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG48;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G48;

public class G48Handler : IFeatureHandler<G48Request, G48Response>
{
    private readonly IG48Repository _g48Repository;

    public G48Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g48Repository = unitOfWork.Value.G48Repository;
    }

    public async Task<G48Response> ExecuteAsync(G48Request request, CancellationToken ct)
    {
        try
        {
            List<G48FavoriteArtworkReadModel> artworks = await _g48Repository
                .GetFavoriteArtworksByTypeAndUserIdWithPaginationAsync(
                    request.UserId,
                    request.ArtworkType,
                    request.PageNum,
                    request.ArtNum,
                    ct
                    );

            return new G48Response
            {
                IsSuccess = true,
                Result = artworks,
                StatusCode = G48ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new G48Response
            {
                IsSuccess = false,
                StatusCode = G48ResponseStatusCode.FAILED
            };
        }
    }
}
