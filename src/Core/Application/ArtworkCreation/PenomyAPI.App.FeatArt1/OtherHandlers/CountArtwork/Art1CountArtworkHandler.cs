using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt1.OtherHandlers.CountArtwork;

public class Art1CountArtworkHandler
    : IFeatureHandler<Art1CountArtworkRequest, Art1CountArtworkResponse>
{
    private readonly IArt1Repository _art1Repository;

    public Art1CountArtworkHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art1Repository = unitOfWork.Value.Art1Repository;
    }

    public async Task<Art1CountArtworkResponse> ExecuteAsync(
        Art1CountArtworkRequest request,
        CancellationToken ct)
    {
        var totalArtworks = await _art1Repository.GetTotalOfArtworksByTypeAndCreatorIdAsync(
            artworkType: request.ArtworkType,
            creatorId: request.CreatorId,
            cancellationToken: ct);

        return new Art1CountArtworkResponse
        {
            TotalArtworks = totalArtworks
        };
    }
}
