using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.CountArtwork;

public class G25CountArtworkHandler
    : IFeatureHandler<G25CountArtworkRequest, G25CountArtworkResponse>
{
    private readonly IG25Repository _g25Repository;

    public G25CountArtworkHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25CountArtworkResponse> ExecuteAsync(
        G25CountArtworkRequest request,
        CancellationToken ct
    )
    {
        int totalArtwork = await _g25Repository.GetTotalOfArtworksByUserIdAndTypeAsync(
                request.UserId,
                request.ArtworkType,
                ct
            );

        return new G25CountArtworkResponse
        {
            TotalArtwork = totalArtwork
        };
    }
}
