using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG2;

public sealed class G2Handler : IFeatureHandler<G2Request, G2Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IG2Repository _g2Repository;

    public G2Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G2Response> ExecuteAsync(G2Request request, CancellationToken ct)
    {
        _g2Repository = _unitOfWork.Value.G2Repository;

        try
        {
            var result = await _g2Repository.GetTopRecommendedArtworksByTypeAsync(
                request.ArtworkType,
                ct);

            return G2Response.SUCCESS(result);
        }
        catch (Exception ex)
        {
            return G2Response.DATABASE_ERROR;
        }
    }
}
