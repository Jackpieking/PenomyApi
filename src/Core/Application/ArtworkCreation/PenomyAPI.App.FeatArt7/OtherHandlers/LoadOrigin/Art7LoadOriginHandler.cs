using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadOrigin;

public sealed class Art7LoadOriginHandler
    : IFeatureHandler<Art7LoadOriginRequest, Art7LoadOriginResponse>
{
    private readonly IArt4Repository _art4Repository;

    public Art7LoadOriginHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art4Repository = unitOfWork.Value.Art4Repository;
    }

    public async Task<Art7LoadOriginResponse> ExecuteAsync(
        Art7LoadOriginRequest request,
        CancellationToken ct
    )
    {
        var origins = await _art4Repository.GetAllOriginsAsync(ct);

        return new Art7LoadOriginResponse { Origins = origins, };
    }
}
