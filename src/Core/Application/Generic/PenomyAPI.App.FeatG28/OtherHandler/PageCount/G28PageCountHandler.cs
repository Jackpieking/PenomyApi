using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG28.PageCount;

public class G28PageCountHandler : IFeatureHandler<G28PageCountRequest, G28PageCountResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G28PageCountHandler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator
    )
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<G28PageCountResponse> ExecuteAsync(
        G28PageCountRequest request,
        CancellationToken ct
    )
    {
        var unitOfWork = _unitOfWork.Value;

        var totalArtworks = await unitOfWork.G28Repository.GetPaginationOptionsByArtworkTypeAsync(
            creatorId: request.CreatorId,
            artworkType: request.ArtworkType
        );

        if (totalArtworks >= 0)
        {
            return G28PageCountResponse.CalculateAndReturn(totalArtworks);
        }
        else
        {
            return new G28PageCountResponse
            {
                TotalArtworks = totalArtworks,
                StatusCode = G28PageCountResponseStatusCode.DATABASE_ERROR,
            };
        }
    }
}
