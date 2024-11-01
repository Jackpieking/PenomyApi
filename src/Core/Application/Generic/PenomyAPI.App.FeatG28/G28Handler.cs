using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG28;

public class G28Handler : IFeatureHandler<G28Request, G28Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G28Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<G28Response> ExecuteAsync(G28Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G28Repository.GetPaginationDetailAsync(
            UserId: long.Parse(request.GetUserId()),
            ArtworkType: request.ArtworkType,
            PageNumber: request.PageNumber,
            PageSize: 8
        );

        if (result != null)
        {
            return new G28Response { result = result, StatusCode = G28ResponseStatusCode.SUCCESS };
        }
        else
        {
            return new G28Response
            {
                result = result,
                StatusCode = G28ResponseStatusCode.DATABASE_ERROR,
            };
        }
    }
}
