using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.GetGuestTracking;

public sealed class G25GetGuestTrackingHandler
    : IFeatureHandler<G25GetGuestTrackingRequest, G25GetGuestTrackingResponse>
{
    private readonly IG25Repository _g25Repository;

    public G25GetGuestTrackingHandler(
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25GetGuestTrackingResponse> ExecuteAsync(
        G25GetGuestTrackingRequest request,
        CancellationToken ct)
    {
        var guestTracking = await _g25Repository.GetGuestTrackingByIdAsync(request.GuestId, ct);

        if (Equals(guestTracking, null))
        {
            return G25GetGuestTrackingResponse.GUEST_ID_NOT_FOUND;
        }

        return G25GetGuestTrackingResponse.SUCCESS(guestTracking);
    }
}
