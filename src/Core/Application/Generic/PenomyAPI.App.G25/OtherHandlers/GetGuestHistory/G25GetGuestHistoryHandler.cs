using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.GetGuestHistory;

public sealed class G25GetGuestHistoryHandler
    : IFeatureHandler<G25GetGuestHitstoryRequest, G25GetGuestHistoryResponse>
{
    private readonly IG25Repository _g25Repository;

    public G25GetGuestHistoryHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25GetGuestHistoryResponse> ExecuteAsync(G25GetGuestHitstoryRequest request, CancellationToken ct)
    {
        var isGuestIdExisted = await _g25Repository.IsGuestIdExistedAsync(request.GuestId, ct);

        if (!isGuestIdExisted)
        {
            return G25GetGuestHistoryResponse.GUEST_ID_NOT_FOUND;
        }

        // Check the current guest view history has any items or not.
        var isViewHistoryNotEmtpy = await _g25Repository.IsGuestViewHistoryNotEmptyAsync(
            request.GuestId,
            request.ArtworkType,
            ct);

        if (!isViewHistoryNotEmtpy)
        {
            return G25GetGuestHistoryResponse.EMPTY_VIEW_HISTORY;
        }

        // Get the guest view history.
        var guestViewHistory = await _g25Repository.GetGuestViewHistoryByArtworkTypeAsync(
            request.GuestId,
            request.ArtworkType,
            ct);

        return G25GetGuestHistoryResponse.SUCCESS(guestViewHistory);
    }
}
