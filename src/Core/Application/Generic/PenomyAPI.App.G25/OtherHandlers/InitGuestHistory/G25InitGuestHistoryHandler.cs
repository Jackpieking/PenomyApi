using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.InitGuestHistory;

public sealed class G25InitGuestHistoryHandler
    : IFeatureHandler<G25InitGuestHistoryRequest, G25InitGuestHistoryResponse>
{
    private readonly IG25Repository _g25Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G25InitGuestHistoryHandler(
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IUnitOfWork> unitOfWork)
    {
        _idGenerator = idGenerator;
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25InitGuestHistoryResponse> ExecuteAsync(
        G25InitGuestHistoryRequest request,
        CancellationToken ct)
    {
        var guestId = _idGenerator.Value.Get();

        var initResult = await _g25Repository.InitGuestViewHistoryAsync(guestId, ct);

        if (!initResult)
        {
            return G25InitGuestHistoryResponse.DATABASE_ERROR;
        }

        return G25InitGuestHistoryResponse.SUCCESS(guestId);
    }
}
