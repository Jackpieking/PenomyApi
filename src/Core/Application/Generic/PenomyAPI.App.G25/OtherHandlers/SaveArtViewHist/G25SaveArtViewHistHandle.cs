using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;

public class G25SaveArtViewHistHandle
    : IFeatureHandler<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse>
{
    private readonly IG25Repository _g25Repository;

    public G25SaveArtViewHistHandle(Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25SaveArtViewHistResponse> ExecuteAsync(
        G25SaveArtViewHistRequest request,
        CancellationToken ct
    )
    {
        try
        {
            await _g25Repository.AddUserArtworkViewHistAsync(
            request.UserId,
            request.ArtworkId,
            request.ChapterId,
            request.ArtworkType,
            limitChapter: 5,
            ct
            );
        }
        catch
        {
            return new G25SaveArtViewHistResponse
            {
                IsSuccess = false,
                StatusCode = G25ResponseStatusCode.FAILED
            };
        }

        return new G25SaveArtViewHistResponse
        {
            IsSuccess = true,
            StatusCode = G25ResponseStatusCode.SUCCESS
        };
    }
}
