using PenomyAPI.App.Common;
using PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;

public class G25RemoveUserHistoryItemHandler
    : IFeatureHandler<G25RemoveGuestHistoryItemRequest, G25RemoveGuestHistoryItemReponse>
{
    private readonly IG25Repository _g25Repository;

    public G25RemoveUserHistoryItemHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25RemoveGuestHistoryItemReponse> ExecuteAsync(
        G25RemoveGuestHistoryItemRequest request,
        CancellationToken ct)
    {
        var isGuestIdExisted = await _g25Repository.IsGuestIdExistedAsync(
            request.GuestId,
            ct);

        if (!isGuestIdExisted)
        {
            return G25RemoveGuestHistoryItemReponse.GUEST_ID_NOT_FOUND;
        }

        var removeSuccess = await _g25Repository.RemoveGuestViewHistoryItemAsync(
            request.GuestId,
            request.ArtworkId,
            ct);

        if (removeSuccess)
        {
            return G25RemoveGuestHistoryItemReponse.SUCCESS;
        }

        return G25RemoveGuestHistoryItemReponse.DATABASE_ERROR;
    }
}
