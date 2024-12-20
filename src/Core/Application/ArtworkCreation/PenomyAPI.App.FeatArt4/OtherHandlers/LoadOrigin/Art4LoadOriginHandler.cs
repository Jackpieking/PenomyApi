using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadOrigin;

public sealed class Art4LoadOriginHandler
    : IFeatureHandler<Art4LoadOriginRequest, Art4LoadOriginResponse>
{
    private readonly IArt4Repository _art4Repository;

    public Art4LoadOriginHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art4Repository = unitOfWork.Value.Art4Repository;
    }

    public async Task<Art4LoadOriginResponse> ExecuteAsync(
        Art4LoadOriginRequest request,
        CancellationToken ct
    )
    {
        var origins = await _art4Repository.GetAllOriginsAsync(ct);

        return new Art4LoadOriginResponse { Origins = origins, };
    }
}
